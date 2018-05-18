using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 消息主体类型
    /// </summary>
    public class MsgContent
    {
        public string msgId { get; set; }//消息id，分页
        public long userId { get; set; }//用户Id
        public string name { get; set; }//用户名
        public long addtime { get; set; }//时间
        public string ismanger { get; set; }//权限
        public string attach { get; set; }//消息主体 如果是图片就是图片地址, 
        public int isRobot { get; set; }
        public string Robotname { get; set; }
        public Ext ext { get; set; }//消息扩展字段  
        public int isAudit { get; set; }//是否审核
        public int roleId { get; set; }
    }
    public class Ext
    {
        public int mstype { get; set; }//内部标识，判断当前信息类型，0-系统消息，1-文本，2-图片,3-私信
        public int width { get; set; }//宽
        public int height { get; set; } //图片高
        public string url { get; set; }//原图大小地址
        public object other { get; set; }//备用字段 如果是私信{from:'',to:''}
    }
}
