using HPSocketCS;
using Infrastructure.Byte;
//using Infrastructure.MsgPack;
using Infrastructure.Serializable;
using SocoolModel.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communicate
{
    public class StdSchedulerFactory
    {
        #region 公共变量
        private static TcpPullClient client = null;
        private string IP = string.Empty;
        private ushort Port;
        //static Task writeTask = default(Task);
        //private static Task sendCMD = default(Task);
        static ManualResetEvent pause = new ManualResetEvent(false);//开始是无信号的
        private static List<ServiceInfoModel> ConnIdGroup = null;
        private const string GetServiceInfoCMDstr = "$GetServiceInfoId";
        private readonly static object m_lock = new object();
        public static Task writeTask = null;
        #endregion

        #region 构造函数

        public StdSchedulerFactory()
        {

        }
        public StdSchedulerFactory(string ip, ushort port)
        {
            IP = ip;
            Port = port;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取连接状态
        /// </summary>
        public bool IsStarted
        {
            get
            {
                bool state = false;
                lock (m_lock)
                {
                    if (client == null)
                    {
                        return false;
                    }
                    else
                    {
                        state = client.State==ServiceState.Started?true:false;
                        return state;
                    }
                }
            }
        }
        #endregion

        #region 连接服务器
        /// <summary>
        /// 连接设备服务器
        /// </summary>
        /// <returns></returns>
        public void GetScheduler()
        {
            if (writeTask == null)
            {
                MonitorClientAccess();
            }
            
            //SendCMD();
            client = new TcpPullClient();
            client.OnPrepareConnect += new TcpClientEvent.OnPrepareConnectEventHandler(OnPrepareConnect);
            client.OnConnect += new TcpClientEvent.OnConnectEventHandler(OnConnect);
            client.OnSend += new TcpClientEvent.OnSendEventHandler(OnSend);
            client.OnReceive += new TcpPullClientEvent.OnReceiveEventHandler(OnReceive);
            client.OnClose += new TcpClientEvent.OnCloseEventHandler(OnClose);
            //建立客户端连接
            if (client.Connect(IP, Port, true))
            {
                client.SocketBufferSize = 4096;
            }
            else
            {
                throw new Exception(string.Format("$Client Start Error -> {0}({1})", client.ErrorMessage, client.ErrorCode));
            }
        }
        #endregion

        #region 客户端事件
        HandleResult OnPrepareConnect(TcpClient sender, IntPtr socket)
        {
            return HandleResult.Ok;
        }
        HandleResult OnConnect(TcpClient sender)
        {
            //连接成功
            Console.WriteLine($@" > [{sender.ConnectionId},OnConnect,IP:{IP}:{Port}]");
            return HandleResult.Ok;
        }

        HandleResult OnClose(TcpClient sender, SocketOperation enOperation, int errorCode)
        {
            try
            {
                if (errorCode == 0)
                    Console.WriteLine($@" > [{sender.ConnectionId},OnClose,IP:{IP}:{Port}]");
                // 连接关闭了
                else
                    // 出错了
                    Console.WriteLine($@" > [{sender.ConnectionId},OnError] -> OP:{enOperation},CODE:{errorCode},Message:{sender.ErrorMessage},IP:{IP}:{Port}");
                if (client.State != ServiceState.Started)
                {
                    ConnIdGroup = null;
                }
                return HandleResult.Ok;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return HandleResult.Ok;
            }
            finally
            {
                pause.Set();
            }
        }

        HandleResult OnSend(TcpClient sender, byte[] bytes)
        {
            // 客户端发数据了
            Console.WriteLine($@" > [{sender.ConnectionId},OnSend] -> ({bytes.Length} bytes)");
            return HandleResult.Ok;
        }

        private bool GetDataLength(ref int reallength)
        {
            IntPtr bufferPtr = Marshal.AllocHGlobal(7);
            try
            {
                //Peek从数据包中窥视数据，不会影响缓存数据的大小
                if (client.Peek(bufferPtr, 7) == FetchResult.Ok)
                {
                    byte[] bufferBytes = new byte[7];
                    Marshal.Copy(bufferPtr, bufferBytes, 0, 7);
                    byte[] len = new byte[4];
                    Array.Copy(bufferBytes, 2, len, 0, 4);
                    int lens = BitConverter.ToInt32(len, 0);
                    reallength = lens;


                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(bufferPtr);//释放申请的内存空间
            }

        }

        HandleResult OnReceive(TcpPullClient sender, int length)
        {
            // 数据到达了
            IntPtr bufferPtr = IntPtr.Zero;
            try
            {
                int reallength = 0;
                if (GetDataLength(ref reallength))
                {
                    while (length >= 7 + reallength)
                    {
                        length = length - reallength - 7;
                        bufferPtr = Marshal.AllocHGlobal(7 + reallength);//分配内存空间
                        if (sender.Fetch(bufferPtr, 7 + reallength) == FetchResult.Ok)//把缓冲区的数据读到分配的内存空间里
                        {
                            byte[] sendBytes = new byte[7 + reallength];
                            Marshal.Copy(bufferPtr, sendBytes, 0, sendBytes.Length);//把内存空间数据读到byte[]里
                            //var i = 0;
                            //foreach (var item in sendBytes)
                            //{
                            //    Console.WriteLine($"{i}:{item}");
                            //    i = i + 1;
                            //}
                            return HandleDeviceMessage(sendBytes, sender.ConnectionId);//解析数据
                        }
                        GetDataLength(ref reallength);
                    }
                }

                //while (length<0)
                //{
                //    length = length - length;
                //}
                //return HandleResult.Ok;
                //bufferPtr = Marshal.AllocHGlobal(length);//分配内存空间
                //if (sender.Fetch(bufferPtr, length) == FetchResult.Ok)//把缓冲区的数据读到分配的内存空间里
                //{
                //    byte[] sendBytes = new byte[length];
                //    Marshal.Copy(bufferPtr, sendBytes, 0, length);//把内存空间数据读到byte[]里
                //    return HandleDeviceMessage(sendBytes, sender.ConnectionId);//解析数据
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return HandleResult.Ok;
            }
            finally
            {
                if (bufferPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(bufferPtr);//释放内存空间
                    bufferPtr = IntPtr.Zero;
                }
            }
            return HandleResult.Ok;
        }
        #endregion

        #region 重连线程

        public void MonitorClientAccess()
        {
            writeTask = new Task((object obj) =>
            {
                while (true)
                {
                    pause.WaitOne();//等待信号到来
                    pause.Reset();//设置无信号
                    Thread.Sleep(5500);
                    try
                    {
                        if (client.State != ServiceState.Started)
                        {
                            client.Connect(IP, Port, true);
                            //Console.WriteLine($"{Name} 重连");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
                , null
                , TaskCreationOptions.LongRunning);//意味着该任务将长时间运行，因此他不是在线程池中执行。
            writeTask.Start();
        }
        #endregion

        #region 数据解析

        private HandleResult HandleDeviceMessage(byte[] HandleData, IntPtr connId)
        {
            byte[] data = new byte[HandleData.Length - 7];
            Array.Copy(HandleData, 7, data, 0, data.Length);
            ConnIdGroup =(List<ServiceInfoModel>)SerializableHelper.BytesToObject(data);
            return HandleResult.Ok;
        }
        #endregion

        #region 获取服务信息
        public List<ServiceInfoModel> GetServiceInfo()
        {
            //if (client != null && client.IsStarted != false)
            //{
            //    byte[] cmd = Encoding.Default.GetBytes(GetServiceInfoCMDstr);
            //    cmd=ByteHelper.PackByte(cmd, Command.CmdGetServiceInfo);
            //    client.Send(cmd, cmd.Length);
            //}
            return ConnIdGroup==null?null:ConnIdGroup.OrderBy(x => x.DeviceType).ToList();
        }
        #endregion

        #region 后台线程发送获取服务信息代码

        public void SendCMD()
        {
            var sendCMD = new Task((object obj) =>
            {
                while (true)
                {
                    byte[] cmd = Encoding.Default.GetBytes(GetServiceInfoCMDstr);
                    client.Send(cmd, cmd.Length);
                    Thread.Sleep(1000);
                }
            }
                , null
                , TaskCreationOptions.LongRunning);//意味着该任务将长时间运行，因此他不是在线程池中执行。
            sendCMD.Start();
        }
        #endregion

        #region 开启/停止服务
        public void OperateService(SendPackModel model, int IsStarted)
        {
            Command operateCommand= IsStarted == 1? Command.CmdOCloseService : Command.CmdOStartService;

            byte[] cmd = ByteHelper.PackByte(SerializableHelper.ObjectToBytes(model), operateCommand);
            if (client != null && client.IsStarted != false)
            {
                if (!client.Send(cmd, cmd.Length))
                {
                    throw new Exception("操作服务失败！");
                }
            }
        }
        #endregion

        #region 断开连接
        public void CloseLink(SendPackModel model)
        {
            byte[] cmd = ByteHelper.PackByte(SerializableHelper.ObjectToBytes(model),Command.CmdCloseLink);
            if (client != null && client.IsStarted != false)
            {
                if (!client.Send(cmd, cmd.Length))
                {
                    throw new Exception("断开连接失败！");
                }
            }
        }
        #endregion
    }
}