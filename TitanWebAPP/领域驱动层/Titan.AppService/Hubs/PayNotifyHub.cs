using AppService.DomainService;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using HttpCache = Infrastructure.Domain.CacheHelper;

namespace AppService.Hubs
{
    [HubName("payNotifyHub")]
    public class PayNotifyHub : Hub
    {
        #region 构造方法
        private readonly BillDomainSvc _billDomainSvc;

        public PayNotifyHub()
        {
            _billDomainSvc = new BillDomainSvc(new ModelService.BillSvc(new RepositoryCode.ModelRespositoryFactory<Model.DataModel.Bill, Guid>()));
        }
        #endregion

        #region 客户端调用
        /// <summary>
        /// 注册连接ID（用于前台调用连接服务器）
        /// </summary>
        /// <param name="billNo">订单Id</param>
        public void regConnectionId(string billNo)
        {
            CacheHelper.WriteConnectionId(billNo, Context.ConnectionId);
        }
        #endregion

        #region 后台调用
        /// <summary>
        /// 推送通知（用于后台调用主动向前台推送通知）
        /// </summary>
        /// <param name="billNo"></param>
        /// <param name="msg"></param>
        public void PushNotify(string billNo, string msg)
        {
            var connectionId = CacheHelper.ReadConnectionId(billNo);
            if (!string.IsNullOrEmpty(connectionId))
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<PayNotifyHub>();
                context.Clients.Client(connectionId).notify(msg); 
            }
        }
        #endregion

        /// <summary>
        /// 缓存操作
        /// </summary>
        private class CacheHelper
        {
            //key
            private const string CONNECTION_ID = "ConnectionId";

            /// <summary>
            /// 写入Socket连接Id
            /// </summary>
            /// <param name="billNo"></param>
            /// <param name="connectionId"></param>
            public static void WriteConnectionId(string billNo, string connectionId)
            {
                HttpCache.CacheWriter(
                    $"{CONNECTION_ID}_{billNo}",
                    connectionId,
                    10);
            }

            /// <summary>
            /// 读取Socket连接Id
            /// </summary>
            /// <param name="billNo"></param>
            /// <returns></returns>
            public static string ReadConnectionId(string billNo)
            {
                var connIdCache = HttpCache.CacheReader($"{CONNECTION_ID}_{billNo}");
                return (connIdCache ?? "").ToString();
            }
        }
    }
}
