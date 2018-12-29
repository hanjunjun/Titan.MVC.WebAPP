/************************************************************************
 * 文件名：BeeCloudDomainSvc
 * 文件功能描述：BeeCloud领域业务逻辑层
 * 作    者：  Weidaicheng
 * 创建日期：2018年2月4日16:35:00
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2017 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
using LitJson;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using HttpCache = Titan.Infrastructure.Domain.CacheHelper;

namespace Titan.AppService.DomainService
{
    public class BeeCloudDomainSvc
    {
        #region 构造方法
        //key
        private const string BEECLOUD_CONFIG = "BeeCloud_Config";
        private const string BEECLOUDCONF = "BEECLOUDCONF";
        //BeeCloud配置缓存失效时间
        private const int BEECLOUD_EXPIRE_MINUTE = 60;

        //lock
        private static object _locker = new object();
        //singleton
        private static BeeCloudDomainSvc _beeCloudDomainSvc;

        //svc

        private BeeCloudDomainSvc()
        { }
        #endregion

        /// <summary>
        /// 从数据库获取BeeCloud配置
        /// </summary>
        /// <param name="gSysCompanyId"></param>
        /// <returns></returns>
        private Dictionary<string, string> getBeeCloudConf(Guid gSysCompanyId)
        {
            //这里改掉了，不报错
            var configuration = new string [1] {"0"};
            if (configuration == null)
                return new Dictionary<string, string>();

            var confDic = new Dictionary<string, string>();
            var confArr = configuration;
            foreach (var item in confArr)
            {
                try
                {
                    var key = item.Split('^')[0];
                    var value = item.Split('^')[1];

                    confDic.Add(key, value);
                }
                catch
                { }
            }

            return confDic;
        }

        /// <summary>
        /// 读取配置（从数据库、config文件）
        /// </summary>
        /// <param name="gSysCompanyId"></param>
        /// <returns></returns>
        private JsonData getBeeCloudConfig(Guid gSysCompanyId)
        {
            var beeCloudConf = getBeeCloudConf(gSysCompanyId);

            string appId;
            string appSecret;
            string masterSecret;
            string testSecret;
            string testMode;
            if(beeCloudConf.Count == 0)
            {
                appId = (ConfigurationManager.AppSettings["AppId"] ?? "").ToString();
                appSecret = (ConfigurationManager.AppSettings["AppSecret"] ?? "").ToString();
                masterSecret = (ConfigurationManager.AppSettings["MasterSecret"] ?? "").ToString();
                testSecret = (ConfigurationManager.AppSettings["TestSecret"] ?? "").ToString();
                testMode = (ConfigurationManager.AppSettings["TestMode"] ?? "").ToString();
            }
            else
            {
                appId = beeCloudConf["AppId"];
                appSecret = beeCloudConf["AppSecret"];
                masterSecret = beeCloudConf["MasterSecret"];
                testSecret = beeCloudConf["TestSecret"];
                testMode = beeCloudConf["TestMode"];
            }

            JsonData json = new JsonData
            {
                ["AppId"] = appId,
                ["AppSecret"] = appSecret,
                ["MasterSecret"] = masterSecret,
                ["TestSecret"] = testSecret,
                ["TestMode"] = testMode
            };

            return json;
        }

        /// <summary>
        /// BeeCloudDomain单例对象
        /// </summary>
        /// <returns></returns>
        public static BeeCloudDomainSvc Instance
        {
            get
            {
                if (_beeCloudDomainSvc == null)
                {
                    lock (_locker)
                    {
                        if (_beeCloudDomainSvc == null)
                        {
                            //new新对象
                            _beeCloudDomainSvc = new BeeCloudDomainSvc();
                            //所有字段
                            var fields = _beeCloudDomainSvc.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                            foreach (var item in fields)
                            {
                                //对svc字段赋值
                                if (item.FieldType.FullName.StartsWith("AppService.ModelService"))
                                {
                                    var fieldInstance = ObjectFactory.GetInstance(Type.GetType(item.FieldType.FullName));
                                    item.SetValue(_beeCloudDomainSvc, fieldInstance);
                                }
                            }
                        }
                    }
                }

                return _beeCloudDomainSvc;
            }
        }

        /// <summary>
        /// 获取BeeCloud配置
        /// </summary>
        /// <param name="gSysCompanyId">机构ID</param>
        /// <returns></returns>
        public static JsonData GetBeeCloudConfig(Guid gSysCompanyId)
        {
            //读取缓存
            var configCache = HttpCache.CacheReader($"{BEECLOUD_CONFIG}_{gSysCompanyId.ToString().ToUpper()}");
            if (configCache == null)
            {
                //缓存不存在
                //查库，设置缓存
                lock (_locker)
                {
                    configCache = HttpCache.CacheReader($"{BEECLOUD_CONFIG}_{gSysCompanyId.ToString().ToUpper()}");
                    if(configCache == null)
                    {
                        var config = Instance.getBeeCloudConfig(gSysCompanyId);
                        HttpCache.CacheWriterSliding(
                            $"{BEECLOUD_CONFIG}_{gSysCompanyId.ToString().ToUpper()}", 
                            config.ToJson(), 
                            BEECLOUD_EXPIRE_MINUTE);
                        return config;
                    }
                    else
                    {
                        return JsonMapper.ToObject(configCache.ToString());
                    }
                }
            }
            else
            {
                //缓存存在
                return JsonMapper.ToObject(configCache.ToString());
            }
        }

        /// <summary>
        /// 刷新BeeCloud缓存
        /// </summary>
        /// <param name="gSysCompanyId">机构ID</param>
        public static void RefreshBeeCloudConfig(Guid gSysCompanyId)
        {
            var config = Instance.getBeeCloudConfig(gSysCompanyId);
            HttpCache.CacheWriterSliding(
                $"{BEECLOUD_CONFIG}_{gSysCompanyId.ToString().ToUpper()}", 
                config.ToJson(), 
                BEECLOUD_EXPIRE_MINUTE);
        }

        /// <summary>
        /// 缓存操作
        /// </summary>
        public class CacheHelper
        {
            //key
            private const string THREAD_CID = "Thread_CID";

            /// <summary>
            /// 写入线程CID
            /// </summary>
            /// <param name="threadId">线程Id</param>
            /// <param name="cid">CID</param>
            public static void WriteThreadCid(int threadId, Guid cid)
            {
                HttpCache.CacheWriter(
                    $"{THREAD_CID}_{threadId}",
                    cid,
                    5);
            }

            /// <summary>
            /// 读取线程CID
            /// </summary>
            /// <param name="threadId">线程Id</param>
            /// <returns>object</returns>
            public static object ReadThreadCid(int threadId)
            {
                return HttpCache.CacheReader($"{THREAD_CID}_{threadId}");
            }
        }
    }
}
