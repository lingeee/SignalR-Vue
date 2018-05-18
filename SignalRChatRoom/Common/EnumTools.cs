using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EnumTools
    {
        private static EnumTools _this;
        private static readonly object _locker = new object();

        /// <summary>
        /// 是否管理角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool IsRoleManager(int roleId) { 
            bool result = false ;
            if ((int)RoleEnum.巡管 == roleId || (int)RoleEnum.频道管理 == roleId || (int)RoleEnum.超管 == roleId || (int)RoleEnum.客服 == roleId || (int)RoleEnum.超级讲师 == roleId)
                result = false;
            return result;
        }

        /// <summary>
        /// 是否需要审核
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool IsAudit(int msgType)
        {
            bool result = false;
            if ((int)MsgEnum.普通文本类 == msgType || (int)MsgEnum.图片类 == msgType)
                result = false;
            return result;
        }

        private EnumTools() { }

        public static EnumTools GetEnumTools()
        {
            if (_this == null)
            {
                lock (_locker)
                {
                    if (_this == null)
                    {
                        _this = new EnumTools();
                    }
                }
            }
            return _this;
        }
    }
}
