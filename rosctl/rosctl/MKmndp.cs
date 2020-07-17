using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace rosctl
{
    class MKmndp
    {
        const ushort TlvTypeMacAddr = 1;
        const ushort TlvTypeIdentity = 5;
        const ushort TlvTypeVersion = 7;
        const ushort TlvTypePlatform = 8;
        const ushort TlvTypeUptime = 10;
        const ushort TlvTypeSoftwareID = 11;
        const ushort TlvTypeBoard = 12;
        const ushort TlvTypeUnpack = 14;
        const ushort TlvTypeIPv6Addr = 15;
        const ushort TlvTypeInterface = 16;
        const ushort TlvTypeUnknown = 17;
        static readonly int Port = 5678;
        static readonly byte[] sendBytes = new byte[] { 0x00, 0x00, 0x00, 0x00 };
        static  UdpClient udpClient;
        static  IPEndPoint IPBroadcast;
        readonly Thread threadSend;
        readonly Thread threadReceive;
        static List<MKInfo> mkInfos = new List<MKInfo>();
        bool sendFlag = true;
        bool receiveFlag = true;
        static readonly string sendName = "Send";
        static readonly string receiveName = "Receive";
        object lockObj = new object();
        public MKmndp()
        {
           
            threadSend = new Thread(new ThreadStart(SendMsg))
            {
                Name = sendName
            };
            threadReceive = new Thread(new ThreadStart(ReceiveMsg))
            {
                Name = receiveName
            };
        }
        public void Start()
        {
            IPBroadcast = new IPEndPoint(IPAddress.Broadcast, Port);
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, Port))            
            {
                EnableBroadcast = true
            };
            //SendMsgThread
            if (threadSend.ThreadState != ThreadState.Running)
            {
                threadSend.Start();
                if (threadSend.ThreadState == ThreadState.Running)
                {
                    //Console.WriteLine("ThreadSend is Running");
                }
            }
            //ReceiveMsgThread
            if (threadReceive.ThreadState != ThreadState.Running)
            {
                threadReceive.Start();
                if (threadReceive.ThreadState == ThreadState.Running)
                {
                    //Console.WriteLine("ThreadReceive is Running");
                }
            }
        }
        private void SendMsg()
        {
            while (sendFlag)
            {
                if (sendFlag)
                {
                    try
                    {
                        udpClient.Send(sendBytes, sendBytes.Length, IPBroadcast);
                        Thread.Sleep(1000);
                    }
                    catch (ObjectDisposedException) { }
                    catch (SocketException) { }
                }
            }
        }
        private void ReceiveMsg()
        {
            while (receiveFlag)
            {
                try
                {
                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    //ReceiveData:0000；
                    if (receiveFlag)
                    {
                        if (receiveBytes.Length > 4)
                        {
                            if (RemoteIpEndPoint.Address.ToString() != IPAddress.Any.ToString())
                            {
                                using MemoryStream memoryStream = new MemoryStream(receiveBytes);
                                using BinaryReader binaryReader = new BinaryReader(memoryStream);
                                string IPAddr = RemoteIpEndPoint.Address.ToString();
                                MKInfo mkInfo = new MKInfo()
                                {
                                    IPAddr = IPAddr
                                };
                                //TLV格式的数据指针偏移4
                                binaryReader.BaseStream.Position = 4;
                                //开始读取TLV格式的数据
                                //递归方法读取二进制流的数据。
                                ReadBytes(binaryReader, ref mkInfo);
                                foreach (MKInfo t in mkInfos)
                                {
                                    if (t.IPAddr == mkInfo.IPAddr)
                                    {
                                        int i = mkInfos.IndexOf(t);
                                        ListRemove lr = new ListRemove(MKInfoRemove);
                                        lr(i);
                                        break;
                                    }
                                }
                                 ListAdd la = new ListAdd(MKInfoAdd);
                                 la(mkInfo);
                            }
                        }
                    }
                }
                catch (ObjectDisposedException) { }
                catch (SocketException) { }
            }
            udpClient.Dispose();
        }
        void ReadBytes(BinaryReader binaryReader, ref MKInfo mikroTikInfo)
        {
            byte[] Type = binaryReader.ReadBytes(2);
            Array.Reverse(Type);
            byte[] Length = binaryReader.ReadBytes(2);
            Array.Reverse(Length);
            ushort Length_Value = BitConverter.ToUInt16(Length);
            byte[] Value = binaryReader.ReadBytes(Length_Value);
            if (BitConverter.ToUInt16(Type) != TlvTypeUnknown)
            {
                switch (BitConverter.ToUInt16(Type))
                {
                    case TlvTypeMacAddr:
                        mikroTikInfo.MacAddr = BitConverter.ToString(Value).Replace("-", ":");
                        break;
                    case TlvTypeIdentity:
                        mikroTikInfo.Identity = Encoding.Default.GetString(Value);
                        break;
                    case TlvTypeVersion:
                        mikroTikInfo.Version = Encoding.Default.GetString(Value);
                        break;
                    case TlvTypePlatform:
                        mikroTikInfo.Platform = Encoding.Default.GetString(Value);
                        break;
                    case TlvTypeUptime:
                        mikroTikInfo.Uptime = TimeSpan.FromSeconds(BitConverter.ToUInt32(Value, 0)).ToString().Replace(".", "d");
                        break;
                    case TlvTypeSoftwareID:
                        mikroTikInfo.SoftwareID = Encoding.Default.GetString(Value);
                        break;
                    case TlvTypeBoard:
                        mikroTikInfo.Board = Encoding.Default.GetString(Value);
                        break;
                    case TlvTypeUnpack:
                        mikroTikInfo.Unpack = Encoding.Default.GetString(Value);
                        break;
                    case TlvTypeIPv6Addr:
                        mikroTikInfo.IPv6Addr = Encoding.Default.GetString(Value);
                        break;
                    case TlvTypeInterface:
                        mikroTikInfo.InterfaceName = Encoding.Default.GetString(Value);
                        break;
                }
                ReadBytes(binaryReader, ref mikroTikInfo);
            }
        }
        delegate void ListRemove(int i);
        private void MKInfoRemove(int i)
        {
            lock (lockObj)
            {
                mkInfos.RemoveAt(i);
            }
        }
        delegate void ListAdd(MKInfo m);
        private void MKInfoAdd(MKInfo m)
        {
            lock (lockObj)
            {
                mkInfos.Add(m);
            }
        }
        public List<MKInfo> GetMKInfos
        {
            get
            {
                List<MKInfo> tempList = new List<MKInfo>();
                lock (lockObj)
                {
                    foreach(MKInfo m in mkInfos)
                    {
                        tempList.Add(m);
                    }
                }
                return tempList;
            }
        }
        public List<string> GetMKInfoIpAddrs
        {
            get
            {
                List<string> tempList = new List<string> ();
                lock(lockObj)
                {
                    foreach(MKInfo s in mikroTikInfos)
                    {
                        tempList.Add(s.IPAddr);
                    }
                }
                return tempList;
            }
        }
        public List<string> GetMKInfoMacAddrs
        {
            get
            {
                List<string> tempList = new List<string> ();
                lock(lockObj)
                {
                    foreach(MKInfo s in mikroTikInfos)
                    {
                        tempList.Add(s.MacAddr);
                    }
                }
                return tempList;
            }
        }
        public void Stop()
        {
            if (threadSend.ThreadState != ThreadState.Aborted)
            {
                sendFlag = false;
                //Console.WriteLine("ThreadSend is Stop");
            }
            if (threadReceive.ThreadState != ThreadState.Aborted)
            {
                receiveFlag = false;
                //Console.WriteLine("ThreadReceive is Stop");
            }
        }
    }
}