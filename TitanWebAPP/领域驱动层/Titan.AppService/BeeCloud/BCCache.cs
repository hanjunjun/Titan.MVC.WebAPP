using LitJson;
using System;
using System.Threading;
using System.Web;
using Titan.AppService.DomainService;

namespace Titan.AppService.BeeCloud
{
    public sealed class BCCache
    {
        private BCCache()
        { }

        //public static readonly BCCache Instance = new BCCache();
        public static BCCache Instance
        {
            get
            {
                string cidStr = BeeCloudDomainSvc.CacheHelper.ReadThreadCid(Thread.CurrentThread.ManagedThreadId)//Cache中的CID，跟当前线程有关
                    .ToString();

                Guid cid;
                if (!Guid.TryParse(cidStr, out cid))
                    throw new Exception("CID Invalid");

                JsonData json = BeeCloudDomainSvc.GetBeeCloudConfig(cid);
                JsonData jAppId;
                JsonData jAppSecret;
                JsonData jMasterSecret;
                JsonData jTestSecret;
                JsonData jTestMode;
                string appId = (json.TryGet("AppId", out jAppId) == true ? jAppId ?? "" : "").ToString();
                string appSecret = (json.TryGet("AppSecret", out jAppSecret) == true ? jAppSecret ?? "" : "").ToString();
                string masterSecret = (json.TryGet("MasterSecret", out jMasterSecret) == true ? jMasterSecret ?? "" : "").ToString();
                string testSecret = (json.TryGet("TestSecret", out jTestSecret) == true ? jTestSecret ?? "" : "").ToString();
                string testMode = (json.TryGet("TestMode", out jTestMode) == true ? jTestMode ?? "" : "").ToString();

                return new BCCache()
                {
                    appId = appId,
                    appSecret = appSecret,
                    masterSecret = masterSecret,
                    testSecret = testSecret,
                    testMode = testMode == "1" ? true : false
                };
            }
        }

        public string appId { get; set; }
        public string appSecret { get; set; }
        public string masterSecret { get; set; }
        public string testSecret { get; set; }

        private bool _testMode = false;
        public bool testMode
        {
            get
            {
                return _testMode;
            }
            set
            {
                _testMode = value;
            }
        }

        private int _newworkTimeout = 30000;
        public int networkTimeout
        {
            get
            {
                return _newworkTimeout;
            }
            set
            {
                _newworkTimeout = value;
            }
        }

    }
}
