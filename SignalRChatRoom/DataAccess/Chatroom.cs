using Niu.Cabinet.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataAccess
{
    public class Chatroom
    {
        static string DB_MediaConnection = ConnectionString.Get("DB_Live", string.Empty);

        #region  select


        #endregion

        #region  update
        public static bool AuditMsg(dynamic model)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {

                var p = new DynamicParameters(model);
                string sql = "update  NeteaseImMsgQueue set niuguAuditTime=getdate(),niuguAuditAdmin=@admin,niuguAuditFlag=1 where  msgid=@msgid";
                return conn.Execute(sql, p) > 0;
            }
        }
        /// <summary>
        /// 更新消息体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpContent(dynamic model)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                var p = new DynamicParameters(model);
                string sql = "update  NeteaseImMsgQueue set ext=@ext,msgid=@newmsgid  where  msgid=@msgid";
                return conn.Execute(sql, p) > 0;
            }
        }

        #endregion

        #region  delete
        public static bool DelMsg(string msgid)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                string sql = @" 
                    if exists( select * from dbo.NeteaseImMsgQueue where msgid=@msgid   )
                    begin
                     delete from  NeteaseImMsgQueue  where  msgid=@msgid
                    end
                    else
                       delete from  NeteaseImMsgQueue  where CHARINDEX(@msgid, ext)>0";
                return conn.Execute(sql, new { msgid }) > 0;
            }
        }

        #endregion

        #region  add
        public static int ImMsgFailureLog(string msgId, long roomId, int code, string desc, int type, string recordTime)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                string sql = "NeteaseImMsgFailureLog_Add";

                var p = new DynamicParameters();

                p.Add("@msgId", msgId, dbType: DbType.AnsiString, size: 50);
                p.Add("@type", type, dbType: DbType.Int32);
                p.Add("@roomId", roomId, dbType: DbType.Int64);
                p.Add("@code", code, dbType: DbType.Int32);
                p.Add("@desc", desc, dbType: DbType.AnsiString, size: 500);
                p.Add("@recordTime", recordTime, dbType: DbType.AnsiString, size: 50);
                int execu = conn.Execute(sql, p, commandType: CommandType.StoredProcedure);
                return execu;
            }
        }

        public static int NeteaseImCustomContent_Add(long liveId, long roomId, string customContent, long updateAdminId, int contentType)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                string sql = "NeteaseImCustomContent_Add";

                var p = new DynamicParameters();

                p.Add("@LiveID", liveId, dbType: DbType.Int64);
                p.Add("@RoomID", roomId, dbType: DbType.Int64);
                p.Add("@CustomContent", customContent, dbType: DbType.AnsiString, size: 500);
                p.Add("@UpdateAdminID", updateAdminId, dbType: DbType.Int64);
                p.Add("@ContentType", contentType, dbType: DbType.Int32);
                int execu = conn.Execute(sql, p, commandType: CommandType.StoredProcedure);
                return execu;
            }
        }

        #endregion

        #region  other


        #endregion

        public static long ChatroomSendMsg(string msgId, int msgType, long roomId, string fromAccId, int resendFlag, string attach, string ext, bool niuguLiveMaster, int niuguRoomType, int niuguAuditFlag, DateTime niuguSendTime, string niuguUserIp = "")
        {
            return ChatroomSendMsg(msgId, msgType, roomId, fromAccId, resendFlag, attach, ext, 0, string.Empty, string.Empty, niuguLiveMaster, niuguRoomType, niuguAuditFlag, niuguSendTime, niuguUserIp);
        }

        public static long ChatroomSendMsg(string msgId, int msgType, long roomId, string fromAccId, int resendFlag, string attach, string ext, long sourceId, string sourceMsgId, string sourceFromAccId, bool niuguLiveMaster, int niuguRoomType, int niuguAuditFlag, DateTime niuguSendTime, string niuguUserIp = "")
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                string sql = "NeteaseImMsgQueue_SendMsg";

                var p = new DynamicParameters();

                p.Add("@msgId", msgId, dbType: DbType.AnsiStringFixedLength, size: 32);
                p.Add("@msgType", msgType, dbType: DbType.Int32);
                p.Add("@roomId", roomId, dbType: DbType.Int64);
                p.Add("@fromAccId", fromAccId, dbType: DbType.AnsiString, size: 50);
                p.Add("@resendFlag", resendFlag, dbType: DbType.Int32);
                p.Add("@attach", attach, dbType: DbType.String, size: 2048);
                p.Add("@ext", ext, dbType: DbType.String, size: 4000);
                p.Add("@sourceId", sourceId, dbType: DbType.Int64);
                p.Add("@sourceMsgId", sourceMsgId, dbType: DbType.AnsiStringFixedLength, size: 32);
                p.Add("@sourceFromAccId", sourceFromAccId, dbType: DbType.AnsiString, size: 50);
                p.Add("@niuguLiveMaster", niuguLiveMaster, dbType: DbType.Boolean);
                p.Add("@niuguRoomType", niuguRoomType, dbType: DbType.Int32);
                p.Add("@niuguAuditFlag", niuguAuditFlag, dbType: DbType.Int32);
                p.Add("@niuguSendTime", niuguSendTime, dbType: DbType.DateTime);
                p.Add("@niuguUserIp", niuguUserIp, dbType: DbType.AnsiString, size: 20);
                p.Add("@id", dbType: DbType.Int64, direction: ParameterDirection.Output);
                conn.Execute(sql, p, commandType: CommandType.StoredProcedure);

                return p.Get<long>("@id");
            }
        }


        public static int ChatroomUpdateMsgExt(long id, string ext, int niuguAuditFlag)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                string sql = "NeteaseImMsgQueue_UpdateExt_New";

                var p = new DynamicParameters();
                p.Add("@id", id, dbType: DbType.Int64);
                p.Add("@ext", ext, dbType: DbType.String, size: 4000);
                p.Add("@niuguAuditFlag", niuguAuditFlag, dbType: DbType.Int32);

                return conn.Execute(sql, p, commandType: CommandType.StoredProcedure);
            }
        }



        public static List<string> ChatroomMsgReplyMe(long roomId, string fromAccId, string sourceFromAccId, long id, int direction, int size = 20, int order = 1)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                List<string> list = new List<string>();
                string sql = string.Empty;
                if (direction == -1)
                {
                    sql = "SELECT TOP(@size) M.ext FROM dbo.NeteaseImMsgQuote AS Q (nolock) LEFT JOIN dbo.NeteaseImMsgQueue AS M (nolock) ON Q.id = M.id WHERE Q.roomId = @roomId AND Q.fromAccId = @fromAccId AND sourceFromAccId = @sourceFromAccId AND Q.id < @id AND M.niuguAuditFlag = 0 ORDER BY Q.id DESC";
                    list = conn.Query<string>(sql, new { roomId = roomId, fromAccId = fromAccId, sourceFromAccId = sourceFromAccId, id = id, size = size }).ToList();
                    if (order == 1) list.Reverse();
                    return list;
                }
                else
                {
                    sql = "SELECT TOP(@size) M.ext FROM dbo.NeteaseImMsgQuote AS Q (nolock) LEFT JOIN dbo.NeteaseImMsgQueue AS M (nolock) ON Q.id = M.id WHERE Q.roomId = @roomId AND Q.fromAccId = @fromAccId AND sourceFromAccId = @sourceFromAccId AND Q.id > @id AND M.niuguAuditFlag = 0 ORDER BY Q.id ASC";
                    list = conn.Query<string>(sql, new { roomId = roomId, fromAccId = fromAccId, sourceFromAccId = sourceFromAccId, id = id, size = size }).ToList();
                    if (order == -1) list.Reverse();
                    return list;
                }
            }
        }
        public static List<dynamic> ChatroomMsgReplyMeDynamic(long roomId, string fromAccId, string sourceFromAccId, long id, int direction, int size = 20, int order = 1)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                List<dynamic> list = new List<dynamic>();
                string sql = string.Empty;
                if (direction == -1)
                {
                    sql = "SELECT TOP(@size) M.id,M.fromAccId,M.sourceFromAccId,M.ext FROM dbo.NeteaseImMsgQuote AS Q (nolock) LEFT JOIN dbo.NeteaseImMsgQueue AS M (nolock) ON Q.id = M.id WHERE Q.roomId = @roomId AND Q.fromAccId = @fromAccId AND Q.sourceFromAccId = @sourceFromAccId AND Q.id < @id AND M.niuguAuditFlag = 0 ORDER BY Q.id DESC";
                    list = conn.Query<dynamic>(sql, new { roomId = roomId, fromAccId = fromAccId, sourceFromAccId = sourceFromAccId, id = id, size = size }).ToList();
                    if (order == 1) list.Reverse();
                    return list;
                }
                else
                {
                    sql = "SELECT TOP(@size) M.id,M.fromAccId,M.sourceFromAccId,M.ext FROM dbo.NeteaseImMsgQuote AS Q (nolock) LEFT JOIN dbo.NeteaseImMsgQueue AS M (nolock) ON Q.id = M.id WHERE Q.roomId = @roomId AND Q.fromAccId = @fromAccId AND Q.sourceFromAccId = @sourceFromAccId AND Q.id > @id AND M.niuguAuditFlag = 0 ORDER BY Q.id ASC";
                    list = conn.Query<dynamic>(sql, new { roomId = roomId, fromAccId = fromAccId, sourceFromAccId = sourceFromAccId, id = id, size = size }).ToList();
                    if (order == -1) list.Reverse();
                    return list;
                }
            }
        }


        public static List<string> ChatroomMsgMaster(long roomId, string fromAccId, long id, int direction, int size = 20, int order = 1)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                List<string> list = new List<string>();
                string sql = string.Empty;
                if (direction == -1)
                {
                    sql = "SELECT TOP(@size) M.ext FROM dbo.NeteaseImMsgMaster AS Q (nolock) LEFT JOIN dbo.NeteaseImMsgQueue AS M (nolock) ON Q.id = M.id WHERE Q.roomId = @roomId AND Q.fromAccId = @fromAccId AND Q.id < @id AND M.niuguAuditFlag = 0 ORDER BY Q.id DESC";
                    list = conn.Query<string>(sql, new { roomId = roomId, fromAccId = fromAccId, id = id, size = size }).ToList();
                    if (order == 1) list.Reverse();
                    return list;
                }
                else
                {
                    sql = "SELECT TOP(@size) M.ext FROM dbo.NeteaseImMsgMaster AS Q (nolock) LEFT JOIN dbo.NeteaseImMsgQueue AS M (nolock) ON Q.id = M.id WHERE Q.roomId = @roomId AND Q.fromAccId = @fromAccId AND Q.id > @id AND M.niuguAuditFlag = 0 ORDER BY Q.id ASC";
                    list = conn.Query<string>(sql, new { roomId = roomId, fromAccId = fromAccId, id = id, size = size }).ToList();
                    if (order == -1) list.Reverse();
                    return list;
                }
            }
        }
        public static List<dynamic> ChatroomMsgMasterDynamic(long roomId, string fromAccId, long id, int direction, int size = 20, int order = 1)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                List<dynamic> list = new List<dynamic>();
                string sql = string.Empty;
                if (direction == -1)
                {
                    sql = "SELECT TOP(@size) M.id,M.fromAccId,M.sourceFromAccId,M.ext FROM dbo.NeteaseImMsgMaster AS Q (nolock) LEFT JOIN dbo.NeteaseImMsgQueue AS M (nolock) ON Q.id = M.id WHERE Q.roomId = @roomId AND Q.fromAccId = @fromAccId AND Q.id < @id AND M.niuguAuditFlag = 0 ORDER BY Q.id DESC";
                    list = conn.Query<dynamic>(sql, new { roomId = roomId, fromAccId = fromAccId, id = id, size = size }).ToList();
                    if (order == 1) list.Reverse();
                    return list;
                }
                else
                {
                    sql = "SELECT TOP(@size) M.id,M.fromAccId,M.sourceFromAccId,M.ext FROM dbo.NeteaseImMsgMaster AS Q (nolock) LEFT JOIN dbo.NeteaseImMsgQueue AS M (nolock) ON Q.id = M.id WHERE Q.roomId = @roomId AND Q.fromAccId = @fromAccId AND Q.id > @id AND M.niuguAuditFlag = 0 ORDER BY Q.id ASC";
                    list = conn.Query<dynamic>(sql, new { roomId = roomId, fromAccId = fromAccId, id = id, size = size }).ToList();
                    if (order == -1) list.Reverse();
                    return list;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="id"></param>
        /// <param name="direction">1:取大于id  -1:小于id</param>
        /// <param name="size"></param>
        /// <param name="order">1:正序 -1:倒序</param>
        /// <returns></returns>
        public static List<string> ChatroomMsgDetail(long roomId, long id, int direction, int size = 20, int order = 1)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                List<string> list = new List<string>();
                string sql = string.Empty;
                if (direction == -1)
                {
                    sql = "SELECT TOP(@size) ext FROM dbo.NeteaseImMsgQueue (nolock) WHERE roomId = @roomId AND id < @id AND niuguAuditFlag = 0 ORDER BY id DESC";
                    list = conn.Query<string>(sql, new { roomId = roomId, id = id, size = size }).ToList();
                    if (order == 1) list.Reverse();
                    return list;
                }
                else
                {
                    sql = "SELECT TOP(@size) ext FROM dbo.NeteaseImMsgQueue (nolock) WHERE roomId = @roomId AND id > @id AND niuguAuditFlag = 0 ORDER BY id ASC";
                    list = conn.Query<string>(sql, new { roomId = roomId, id = id, size = size }).ToList();
                    if (order == -1) list.Reverse();
                    return list;
                }
            }
        }


        public static List<dynamic> ChatroomMsgDetailDynamic(long roomId, long id, int direction, int size = 20, int order = 1, int ismanger = 0)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                string whereId = string.Empty;
                List<dynamic> list = new List<dynamic>();
                string sql = string.Empty;
                if (ismanger == 0)
                    whereId = " and  niuguAuditFlag =1 ";
                if (direction == -1)
                {
                    if (id != 0)
                        whereId += "AND id < @id ";
                    sql = string.Format("SELECT TOP(@size) id,msgid,fromAccId,sourceFromAccId,IsManger,ext,niuguAuditFlag,niuguSendTime as sendtime FROM dbo.NeteaseImMsgQueue (nolock) WHERE roomId = @roomId {0} and CHARINDEX('\"mstype\":4',ext)=0 ORDER BY id DESC", whereId);
                    sql = string.Format("select temp.*,u.name,u.userlog from ( {0}) as  temp join NeteaseImUser u on  temp.fromAccId= u.accid ", sql);
                    list = conn.Query<dynamic>(sql, new { roomId = roomId, id = id, size = size }).ToList();
                    if (order == 1) list.Reverse();
                    return list;
                }
                else
                {
                    if (id != 0)
                        whereId += "  AND id > @id ";
                    sql = string.Format("SELECT TOP(@size) id,msgid,fromAccId,sourceFromAccId,IsManger,ext,niuguAuditFlag,niuguSendTime as sendtime FROM dbo.NeteaseImMsgQueue (nolock) WHERE roomId = @roomId {0} and CHARINDEX('\"mstype\":4',ext)=0  ORDER BY id ASC", whereId);
                    sql = string.Format("select temp.*,u.name,u.userlog from ( {0}) as  temp join NeteaseImUser u on  temp.fromAccId= u.accid ", sql);
                    list = conn.Query<dynamic>(sql, new { roomId = roomId, id = id, size = size }).ToList();
                    if (order == -1) list.Reverse();
                    return list;
                }
            }
        }


        public static List<dynamic> ChatroomMsgDetailLast(long roomId, int size = 200)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                List<dynamic> list = new List<dynamic>();
                string sql = "SELECT TOP(@size) id,fromAccId,sourceFromAccId,attach,ext FROM dbo.NeteaseImMsgQueue (nolock) WHERE roomId = @roomId AND niuguAuditFlag = 0 ORDER BY id Desc";
                list = conn.Query<dynamic>(sql, new { roomId = roomId, size = size }).ToList();
                list.Reverse();
                return list;
            }
        }


        //logID,operate,id,roomId,fromAccId,sourceFromAccId,ext
        public static List<dynamic> ChatroomMsgLogListByLogID(long prevLogID)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                List<dynamic> list = new List<dynamic>();
                string sql = string.Empty;

                if (prevLogID > 0)
                {
                    sql = string.Format("select l.*,q.ext,q.niuguAuditFlag From dbo.NeteaseImMsgQueue_Log as l (nolock) left join dbo.NeteaseImMsgQueue as q (nolock) on l.id=q.id where logId>{0}", prevLogID);
                }
                else
                {
                    sql = "select l.*,q.ext,q.niuguAuditFlag From dbo.NeteaseImMsgQueue_Log as l (nolock) left join dbo.NeteaseImMsgQueue as q (nolock) on l.id=q.id where dateadd(MINUTE,2,l.AddTime)>GETDATE()";
                }

                list = conn.Query<dynamic>(sql, new { prevLogID = prevLogID }).ToList();
                list.Reverse();
                return list;
            }
        }



        public static List<long> ChatroomGetAll()
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                string sql = "SELECT roomId FROM [dbo].[NeteaseImChatroom]";
                return conn.Query<long>(sql).ToList();
            }
        }



        public static List<long> ChatroomGetMasterAll()
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                string sql = "SELECT roomId FROM dbo.NeteaseImChatroom WHERE niuguLastSendTime > DATEADD(dd,-30,GETDATE()) order by niuguLastSendTime DESC";
                return conn.Query<long>(sql).ToList();
            }
        }
        #region 水军功能
        public static bool AddlRobot(string RobotName, long Owner, int roleId)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                string sql = " insert into dbo.NeteaseImRobot values(@RobotName,getdate(),@Owner,@roleId) ";
                return conn.Execute(sql, new { RobotName, Owner, roleId }) > 0;
            }
        }
        #endregion
        /// <summary>
        /// 取全部
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<dynamic> GetRobotList(long id)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                string sql = "SELECT Id,RobotName,roleId FROM dbo.NeteaseImRobot where owner=@Id";
                return conn.Query<dynamic>(sql, new { id });
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelRobot(int Id)
        {
            using (IDbConnection conn = new SqlConnection(DB_MediaConnection))
            {
                string sql = " delete  FROM dbo.NeteaseImRobot where id=@id";
                return conn.Execute(sql, new { Id }) > 0;
            }
        }
    }
}
