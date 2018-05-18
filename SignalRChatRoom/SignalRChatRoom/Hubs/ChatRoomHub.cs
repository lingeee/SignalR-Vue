using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace SignalRChatRoom.Hubs
{
    public class ChatRoomHub : Hub
    {
        /// < summary>
        /// 
        /// </ summary>
        public static Dictionary<string, string> OnLineUserArray = new Dictionary<string, string>();

        /// <summary>
        /// Get's the currently connected Id of the client.
        /// This is unique for each client and is used to identify
        /// a connection.
        /// </summary>
        /// <returns></returns>
        private string GetClientId()
        {
            string clientId = "";

            // clientId passed from application 
            if (Context.QueryString["clientId"] != null)
            {
                clientId = this.Context.QueryString["clientId"];
            }

            if (string.IsNullOrEmpty(clientId.Trim()))
            {
                clientId = Context.ConnectionId;
            }

            return clientId;
        }

        /// <summary>
        /// Sends the update user count to the listening view.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        public void Send(string count)
        {
            // Call the addNewMessageToPage method to update clients.
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatRoomHub>();
            context.Clients.All.updateUsersOnlineCount(count);
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>Class1.cs
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        /// <summary>
        /// 离线
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}