using Common.IModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niu.Cabinet.Logging;
using System.Reflection;
using Niu.Cabinet.Cryptography;
using Niu.Cabinet.Conversion;

namespace Common
{
    public class TokenManager
    {
        #region 属性

        /// <summary>
        /// 分割串儿。
        /// </summary>
        private static char Separator = (char)0x1EEC;

        /// <summary>
        /// 日志记录
        /// </summary>
        private static LogRecord logRecord = null;

        #endregion

        #region 独立直播令牌管理

        #region 验证令牌，生成用户

        /// <summary>
        /// 验证令牌，生成用户
        /// </summary>
        /// <param name="strUserToken">令牌</param>
        /// <param name="user">返回用户对象</param>
        /// <returns>true/false</returns>
        public static bool ValidateUserToken(string userToken, out TokenUserInfo user)
        {
            user = null;
            try
            {
                string[] userInfoArray = null;
                if (!string.IsNullOrEmpty(userToken))
                {
                    userToken = System.Web.HttpUtility.UrlDecode(userToken, System.Text.Encoding.UTF8);

                    userToken = userToken.Replace("-", "+").Replace("_", "/").Replace("*", "=");

                    userToken = NiuCryptoService.DecryptToken(userToken);
                    userInfoArray = userToken.Split(Separator);

                    if (userInfoArray.Length == 9)
                    {
                        user = new TokenUserInfo()
                        {
                            userId = ObjectConvert.ChangeType<long>(userInfoArray[1], 0),
                            nickName = userInfoArray[3],
                            tokenType = (tokenType)ObjectConvert.ChangeType<long>(userInfoArray[0], 0),
                            type = (userType)ObjectConvert.ChangeType<int>(userInfoArray[2], 0),
                            status = ObjectConvert.ChangeType<int>(userInfoArray[4], 0),
                            channelId = ObjectConvert.ChangeType<int>(userInfoArray[5], 0),
                            roleId = ObjectConvert.ChangeType<int>(userInfoArray[6], 0),
                            roleName = userInfoArray[7],
                            isManage = ObjectConvert.ChangeType<int>(userInfoArray[8], 0)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                logRecord.WriteSingleLog("ValidateUserToken.log", string.Format("error:{0}", ex.Message));
            }

            return true;
        }

        #endregion

        #region 用户对象，生成令牌

        /// <summary>
        /// 用户对象，生成令牌
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <param name="UserToken">令牌</param>
        /// <returns>true/false</returns>
        public static bool GenerateUserToken(ITokenUserInfo user, out string userToken)
        {
            userToken = string.Empty;
            if (user != null && user.userId > 0 && !string.IsNullOrEmpty(user.nickName))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(((int)user.tokenType).ToString());
                sb.Append(Separator);

                sb.Append(user.userId.ToString());
                sb.Append(Separator);

                sb.Append(((int)(user.type)).ToString());
                sb.Append(Separator);

                sb.Append(user.nickName.ToString());
                sb.Append(Separator);

                sb.Append(user.status.ToString());
                sb.Append(Separator);

                sb.Append(user.channelId);
                sb.Append(Separator);

                sb.Append(user.roleId.ToString());
                sb.Append(Separator);

                sb.Append(user.roleName);
                sb.Append(Separator);

                sb.Append(user.isManage.ToString());

                userToken = NiuCryptoService.EncryptToken(sb.ToString());

                userToken = userToken.Replace("+", "-").Replace("/", "_").Replace("=", "*");

                userToken = System.Web.HttpUtility.UrlEncode(userToken, System.Text.Encoding.UTF8);

                return true;
            }
            return false;
        }

        #endregion

        #region 返回字符串token

        /// <summary>
        /// 返回字符串token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GenerateUserToken(ITokenUserInfo user)
        {
            string userToken = string.Empty;
            GenerateUserToken(user, out userToken);
            return userToken;
        }

        #endregion

        #endregion
    }
}
