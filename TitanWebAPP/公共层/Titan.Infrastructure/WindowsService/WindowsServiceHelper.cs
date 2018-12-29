using System;
using System.ServiceProcess;

namespace Titan.Infrastructure.WindowsService
{
    public class WindowsServiceHelper
    {
        /// <summary>  
        /// 判断是否安装了某个服务  
        /// </summary>  
        /// <param name="serviceName"></param>  
        /// <returns></returns>  

        public static bool ISWindowsServiceInstalled(string serviceName)
        {
            
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        return true;
                    }
                }
                return false;
            }
            
            catch

            { return false; }
        }


        /// <summary>  
        /// 启动某个服务  
        /// </summary>  
        /// <param name="serviceName"></param>  

        public static void StartService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);//

            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();

            }
            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                sc.Start();

            }
            //ServiceController[] services = ServiceController.GetServices();
            //foreach (ServiceController service in services)
            //{
            //    if (service.ServiceName == serviceName)
            //    {
            //        service.Start();
            //        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
            //    }
            //}
        }

        /// <summary>  
        /// 停止某个服务  
        /// </summary>  
        /// <param name="serviceName"></param>  
        public static void StopService(string serviceName)
        {
            ServiceController sc = new ServiceController(serviceName);
            sc.Stop();
            //ServiceController[] services = ServiceController.GetServices();
            //foreach (ServiceController service in services)
            //{
            //    if (service.ServiceName == serviceName)
            //    {
            //        service.Stop();
            //        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
            //    }
            //}
        }


        /// <summary>  
        /// 判断某个服务是否启动  
        /// </summary>  
        /// <param name="serviceName"></param>  
        public static bool ISStart(string serviceName)
        {
            bool result = true;
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        if ((service.Status == ServiceControllerStatus.Stopped)
                            || (service.Status == ServiceControllerStatus.StopPending))
                        {
                            result = false;
                        }
                    }
                }
            }
            catch { }
            return result;
        }

    }
}
