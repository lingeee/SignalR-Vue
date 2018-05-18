using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IModel
{
    #region userToken 实体和接口

    /// <summary>
    /// userToken 接口
    /// </summary>
    public interface ITokenUserInfo
    {
        /// <summary>
        /// token 类型
        /// </summary>
        tokenType tokenType { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        long userId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        string nickName { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        userType type { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        int status { get; set; }

        /// <summary>
        /// 渠道ID
        /// </summary>
        int channelId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        int roleId { get; set; }
        /// <summary>
        /// 角色名字
        /// </summary>
        string roleName { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        int isManage { get; set; }
    }

    /// <summary>
    /// userToken 实体
    /// </summary>
    public class TokenUserInfo : ITokenUserInfo
    {
        /// <summary>
        /// token 类型
        /// </summary>
        public tokenType tokenType { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long userId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string nickName { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public userType type { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 渠道ID
        /// </summary>
        public int channelId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int roleId { get; set; }
        /// <summary>
        /// 角色名字
        /// </summary>
        public string roleName { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public int isManage { get; set; }
    }

    #endregion

    #region 枚举

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum userType
    {
        /// <summary>
        /// pc端用户
        /// </summary>
        pc = 0,
        /// <summary>
        /// wap端用户
        /// </summary>
        wap = 1
    }

    /// <summary>
    /// token 类型 0 匿名用户  1 正式用户  
    /// </summary>
    public enum tokenType
    {
        /// <summary>
        /// 匿名用户
        /// </summary>
        Anonymous = 0,
        /// <summary>
        /// 正式用户
        /// </summary>
        Formal = 1
    }

    #endregion
}
