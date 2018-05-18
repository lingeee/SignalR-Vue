using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(SignalRChatRoom.Startup))]

namespace SignalRChatRoom
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 连接标识
            app.Map("/signalr", map =>
            {
                //跨域
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableJSONP = true
                };
                //启动配置
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}
