using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;

namespace AppService.Hubs
{
    [HubName("qwSosHub")]
    public class QwSosHub : Hub
    {
        #region 后台调用
        /// <summary>
        /// 推送乔威SOS（用于后台调用主动向前台推送通知）
        /// </summary>
        /// <param name="checkDataId"></param>
        /// <param name="msg"></param>
        public void PushQwSosNotify(Guid checkDataId, string msg)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<QwSosHub>();
            context.Clients.All.qwSosnotify($"{{\"checkDataId\":\"{checkDataId}\",\"msg\":\"{msg}\"}}");
        }

        /// <summary>
        /// 推送乔威SOS处理（用于后台调用主动向前台推送通知）
        /// </summary>
        /// <param name="checkDataId"></param>
        public void PushQwHandleNotify(Guid checkDataId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<QwSosHub>();
            context.Clients.All.qwHandlenotify($"{{\"checkDataId\":\"{checkDataId}\"}}");
        }
        #endregion
    }
}
