using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace rosctl
{
    class Program
    {
        static readonly Timer Timer= new Timer(Timer_Callback, null, Timeout.Infinite, Timeout.Infinite);
        static readonly Timer TimerMndp = new Timer(Mndp_Callback,null,Timeout.Infinite,Timeout.Infinite);
        static readonly List<string> IpAddrs = new List<string>();
        static readonly MKmndp mndp = new MKmndp();
        static readonly Snmp snmp = new Snmp();
        static readonly MKuser user = new MKuser();
        static readonly MKlogging logging = new MKlogging();
        static string newPassword = "";
        static bool loggingFlag = false;
        static string ntp = "";
        static bool snmpFlag = false;
        static bool auto = false;
        static bool wireless = false;
        static bool ethernet = false;
        static bool resource = false;
        static bool neighbor = false;
        static bool romon = false;
        static bool health = false;
        static bool reboot = false;
        static bool capsman = false;
        static void Main(string[] args)
        {
            if (args.Length > 2)
            {
                if (args[0] == "mndp")
                {
                    ParseArgs(args);
                    mndp.Start();
                    Timer.Change(0, 10000);
                    while (!Console.KeyAvailable)
                    {
                        Thread.Sleep(100);
                    }
                    Timer.Change(Timeout.Infinite, Timeout.Infinite);
                    Timer.Dispose();
                    mndp.Stop();
                }
                else if (args[0].Length > 2)
                {
                    string st = args[0];
                    string[] addrs = st.Split(",");
                    for (int j = 0; j < addrs.Length; j++)
                    {
                        IpAddrs.Add(addrs[j]);
                    }
                    ParseArgs(args);
                    foreach (string s in IpAddrs)
                    {
                        Timer_MK(s);
                    }
                }
            }
            else if(args.Length == 2)
            {
                if(args[0] == "mndp" && args[1].StartsWith("--show"))
                {
                    mndp.Start();
                    TimerMndp.Change(0,5000);
                    while (!Console.KeyAvailable)
                    {
                        Thread.Sleep(100);
                    }
                    TimerMndp.Change(Timeout.Infinite, Timeout.Infinite);
                    TimerMndp.Dispose();
                    mndp.Stop();
                }
            }
            else if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "--help":
                        Help();
                        break;
                    case "--author":
                        Author();
                        break;
                }
            }
            else if (args.Length == 0)
            {
                Help();
            }
        }
        static void ParseArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-u"))
                {
                    string st = args[i].Substring(2);
                    if (st.Length == 0)
                    {
                        int t = i + 1;
                        if (t >= args.Length)
                        {
                            Console.WriteLine("Username Is Null");
                        }
                        else
                        {
                            if (args[t].StartsWith("-"))
                            {
                                Console.WriteLine("Username Is Null");
                            }
                            else
                            {
                                user.Username = args[t];
                            }                            
                        }
                    }
                    else
                    {
                        user.Username = st;
                    }
                }
                else if (args[i].StartsWith("-p"))
                {
                    string st = args[i].Substring(2);
                    if (st.Length == 0)
                    {
                        int t = i + 1;
                        if (t >= args.Length)
                        {
                            Console.WriteLine("Password Is Null");
                        }
                        else
                        {
                            if(args[t].StartsWith("-"))
                            {
                                Console.WriteLine("Password Is Null");
                            }
                            else
                            {
                                user.Password = args[t];
                            }                            
                        }
                    }
                    else
                    {
                        user.Password = st;
                    }
                }
                else if (args[i].StartsWith("--auto"))
                {
                    auto = true;
                }
                else if (args[i].StartsWith("--new"))
                {
                    string st = args[i].Substring(5);
                    if (st.Length == 0)
                    {
                        int t = i + 1;
                        if (t >= args.Length)
                        {
                            Console.WriteLine("NewPassword Is Null");
                        }
                        else
                        {
                            newPassword = args[t];
                        }
                    }
                    else
                    {
                        newPassword = st;
                    }

                }
                else if (args[i].StartsWith("--logging"))
                {
                    string st = args[i].Substring(9);
                    if (st.Length == 0)
                    {
                        int t = i + 1;
                        if (t >= args.Length)
                        {
                            Console.WriteLine("Logging Target Is Null");
                        }
                        else
                        {
                            logging.Remote = args[t];
                        }
                        int p = i + 2;
                        if(p >= args.Length)
                        {
                            logging.Port = "514";
                            Console.WriteLine("Logging Port Is 514");
                        }
                        else
                        {
                            logging.Port = args[p];
                        }
                    }
                    else
                    {
                        logging.Remote = st;
                        logging.Port = "514";
                    }
                    loggingFlag = true;
                }
                else if (args[i].StartsWith("--snmp"))
                {
                    string st = args[i].Substring(6);
                    if (st.Length == 0)
                    {
                        int t = i + 1;
                        if (t >= args.Length)
                        {
                            Console.WriteLine("SNMP Target Is Null");
                        }
                        else
                        {
                            snmp.Target = args[t];
                        }
                        int c = i + 2;
                        if(c >= args.Length)
                        {
                            Console.WriteLine("SNMP Contact Is Null");
                        }
                        else
                        {
                            snmp.Contact = args[c];
                        }
                        int l = i + 3;
                        if(l >= args.Length)
                        {
                            Console.WriteLine("SNMP Location Is Null");
                        }
                        else
                        {
                            snmp.Location = args[l];
                        }
                    }
                    else
                    {
                        snmp.Target = st;
                    }
                    snmpFlag = true;
                }
                else if (args[i].StartsWith("--ntp"))
                {
                    string st = args[i].Substring(5);
                    if (st.Length == 0)
                    {
                        int t = i + 1;
                        if (t >= args.Length)
                        {
                            Console.WriteLine("Ntp Is Null");
                        }
                        else
                        {
                            ntp = args[t];
                        }
                    }
                    else
                    {
                        ntp = st;
                    }
                }
                else if (args[i].StartsWith("--ethernet"))
                {
                    ethernet = true;
                }
                else if (args[i].StartsWith("--resource"))
                {
                    resource = true;
                }
                else if (args[i].StartsWith("--wireless"))
                {
                    wireless = true;
                }
                else if (args[i].StartsWith("--neighbor"))
                {
                    neighbor = true;
                }
                else if (args[i].StartsWith("--romon"))
                {
                    romon = true;
                }
                else if (args[i].StartsWith("--health"))
                {
                    health = true;
                }
                else if(args[i].StartsWith("--reboot"))
                {
                    reboot = true;
                }
                else if(args[i].StartsWith("--capsman"))
                {
                    capsman = true;
                }
            }
        }
        static void Help()
        {
            Console.WriteLine("命令配置：");
            Console.WriteLine("rosctl mndp -u root -p password");
            Console.WriteLine("rosctl mndp -u root -p password --auto");
            Console.WriteLine("rosctl mndp -u root -p password --ethernet");
            Console.WriteLine("rosctl mndp -u root -p password --resource");
            Console.WriteLine("rosctl mndp -u root -p password --wireless");
            Console.WriteLine("rosctl mndp -u root -p password --neighbor");
            Console.WriteLine("rosctl mndp -u root -p password --romon");
            Console.WriteLine("rosctl mndp -u root -p password --new password");
            Console.WriteLine("rosctl mndp -u root -p password --logging 192.168.1.2");
            Console.WriteLine("rosctl mndp -u root -p password --snmp 192.168.1.2");
            Console.WriteLine("rosctl mndp -u root -p password --ntp 192.168.1.2");
            Console.WriteLine("rosctl mndp -u root -p password --new password --loging 192.168.1.2 --snmp 192.168.1.2");
            Console.WriteLine("rosctl 192.168.1.3 -u root -p password --auto");
            Console.WriteLine("rosctl 192.168.1.3 -u root -p password --resource");
            Console.WriteLine("rosctl 192.168.1.3 -u root -p password --ethernet");
            Console.WriteLine("rosctl 192.168.1.3 -u root -p password --wireless");
            Console.WriteLine("rosctl 192.168.1.3 -u root -p password --health");
            Console.WriteLine("rosctl 192.168.1.3 -u root -p password --reboot");
            Console.WriteLine("rosctl 192.168.1.3 -u root -p password --capsman");
            Console.WriteLine("rosctl 192.168.1.3,192.168.112.4 -u root -p password --resource");
            Console.WriteLine("rosctl 192.168.1.3,192.168.112.4,192.168.112.5 -u root -p password --ethernet");
            Console.WriteLine("rosctl 192.168.1.3,192.168.112.4 -u root -p password --wireless");
            Console.WriteLine("rosctl 192.168.1.3,192.168.112.4,192.168.112.5 -u root -p password --health");
            Console.WriteLine("rosctl 192.168.1.3 -u root -p password --new password");
            Console.WriteLine("rosctl 192.168.1.3 -u root -p password --logging 192.168.1.2");
            Console.WriteLine("rosctl 192.168.1.3 -u root -p password --snmp 192.168.1.2");
            Console.WriteLine("rosctl 192.168.1.3 -u root -p password --ntp 192.168.1.2");
            Console.WriteLine("rosctl 192.168.1.3 -u root -p password --new password --logging 192.168.1.2 --snmp 192.168.1.2");
            Console.WriteLine("命令帮助:");
            Console.WriteLine("rosctl --help");
            Console.WriteLine("rosctl --author");
        }
        static void Author()
        {
            Console.WriteLine("Author:LiBing");
            Console.WriteLine("Mobile:18908035651");
            Console.WriteLine("Phone:028-87488587");
        }
        private static void Timer_Callback(object state)
        {
            Console.Clear();
            Console.SetCursorPosition(0,0);
            foreach (string t in mndp.GetMKInfoIpAddrs)
            {
                Timer_MK(t);
            }
        }
        private static void Mndp_Callback(object state)
        {
            Console.Clear();
            Console.SetCursorPosition(0,0);
            List<MKInfo> mikroTikInfos = mndp.GetMKInfos;
            foreach(MKInfo m in mikroTikInfos)
            { 
                Console.WriteLine("IPAddr:{0},MacAddr:{1},Identity:{2},Version:{3},Platform:{4},Uptime:{5},Board:{6}", m.IPAddr, m.MacAddr, m.Identity, m.Version, m.Platform, m.Uptime, m.Board);
            }
        }
        private static void Timer_MK(string IpAddr)
        {
            MK mk = new MK(IpAddr);
            if (mk.Login(user.Username, user.Password))
            {
                if (!string.IsNullOrEmpty(newPassword))
                {
                    mk.Send("/password");
                    mk.Send("=old-password=" + user.Password);
                    mk.Send("=new-password=" + newPassword);
                    mk.Send("=confirm-new-password=" + newPassword);
                    mk.Send(".tag=password", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址:{0},Password", IpAddr);
                        }
                    }
                }
                if (loggingFlag)
                {
                    mk.Send("/system/logging/action/print");
                    mk.Send("?name=log");
                    mk.Send("=.proplist=remote");
                    mk.Send("=.proplist=remote-port");
                    mk.Send(".tag=log-action", true);
                    string remote = "";
                    string port = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            foreach (var d in GetDictionary(s))
                            {
                                if (d.Key == "remote")
                                {
                                    remote = d.Value;
                                }
                                if(d.Key =="remote-port")
                                {
                                    port = d.Value;
                                }
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(remote))
                    {
                        mk.Send("/system/logging/action/add");
                        mk.Send("=name=log");
                        mk.Send("=target=remote");
                        mk.Send("=remote=" + logging.Remote);
                        mk.Send("=remote-port" + logging.Port);
                        mk.Send("=bsd-syslog=yes");
                        mk.Send(".tag=action", true);
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!done"))
                            {
                                Console.WriteLine("IP地址:{0},Action-Add", IpAddr);
                            }
                        }
                        mk.Send("/system/logging/print");
                        mk.Send("?action=log");
                        mk.Send("?topics=warning");
                        mk.Send("?#&");
                        mk.Send("=.proplist=.id");
                        mk.Send(".tag=warning", true);
                        bool warning = true;
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!re"))
                            {
                                warning = false;
                            }
                        }
                        if (warning)
                        {
                            mk.Send("/system/logging/add");
                            mk.Send("=topics=warning");
                            mk.Send("=action=log");
                            mk.Send(".tag=warning", true);
                            foreach (string s in mk.Read())
                            {
                                if (s.StartsWith("!done"))
                                {
                                    Console.WriteLine("IP地址:{0},Logging-warning", IpAddr);
                                }
                            }
                        }
                        mk.Send("/system/logging/print");
                        mk.Send("?action=log");
                        mk.Send("?topics=error");
                        mk.Send("?#&");
                        mk.Send("=.proplist=.id");
                        mk.Send(".tag=error", true);
                        bool error = true;
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!re"))
                            {
                                error = false;
                            }
                        }
                        if (error)
                        {
                            mk.Send("/system/logging/add");
                            mk.Send("=topics=error");
                            mk.Send("=action=log");
                            mk.Send(".tag=error", true);
                            foreach (string s in mk.Read())
                            {
                                if (s.StartsWith("!done"))
                                {
                                    Console.WriteLine("IP地址:{0},Logging-Error", IpAddr);
                                }
                            }
                        }
                        mk.Send("/system/logging/print");
                        mk.Send("?action=log");
                        mk.Send("?topics=info");
                        mk.Send("?#&");
                        mk.Send("=.proplist=.id");
                        mk.Send(".tag=info", true);
                        bool info = true;
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!re"))
                            {
                                info = false;
                            }
                        }
                        if (info)
                        {
                            mk.Send("/system/logging/add");
                            mk.Send("=topics=info");
                            mk.Send("=action=log");
                            mk.Send(".tag=info", true);
                            foreach (string s in mk.Read())
                            {
                                if (s.StartsWith("!done"))
                                {
                                    Console.WriteLine("IP地址:{0},Logging-Info", IpAddr);
                                }
                            }
                        }
                        mk.Send("/system/logging/print");
                        mk.Send("?action=log");
                        mk.Send("?topics=critical");
                        mk.Send("?#&");
                        mk.Send("=.proplist=.id");
                        mk.Send(".tag=critical", true);
                        bool critical = true;
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!re"))
                            {
                                critical = false;
                            }
                        }
                        if (critical)
                        {
                            mk.Send("/system/logging/add");
                            mk.Send("=topics=critical");
                            mk.Send("=action=log");
                            mk.Send(".tag=critical", true);
                            foreach (string s in mk.Read())
                            {
                                if (s.StartsWith("!done"))
                                {
                                    Console.WriteLine("IP地址:{0},Logging-Critical", IpAddr);
                                }
                            }
                        }
                    }
                    else if (remote != logging.Remote  || port != logging.Port)
                    {
                        mk.Send("/system/logging/action/print");
                        mk.Send("?name=log");
                        mk.Send("=.proplist=.id");
                        mk.Send(".tag=log-action-id", true);
                        string remoteid = "";
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!re"))
                            {
                                remoteid = s.Substring(21);
                            }
                        }
                        mk.Send("/system/logging/action/set");
                        mk.Send(remoteid);
                        mk.Send("=remote=" + logging.Remote);
                        mk.Send("=remote-port=" + logging.Port);
                        mk.Send(".tag=log-action-remove", true);
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!done"))
                            {
                                Console.WriteLine("IP地址:{0},Action-Change",IpAddr);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ntp))
                {
                    mk.Send("/system/ntp/client/set");
                    mk.Send("=enabled=yes");
                    mk.Send("=primary-ntp=" + ntp);
                    mk.Send(".tag=ntp", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址:{0},Ntp", IpAddr);
                        }
                    }
                }
                if (snmpFlag)
                {
                    mk.Send("/snmp/set");
                    mk.Send("=enabled=yes");
                    mk.Send("=contact=" + (string.IsNullOrEmpty(snmp.Contact) ? "LiBing" : snmp.Contact));
                    mk.Send("=location=" + (string.IsNullOrEmpty(snmp.Location) ? "18908035651" : snmp.Location));
                    mk.Send("=trap-target=" + (string.IsNullOrEmpty(snmp.Target) ? "192.168.112.2" : snmp.Target));
                    mk.Send("=trap-version=2");
                    mk.Send("=trap-generators=interface");
                    mk.Send(".tag=snmp", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},Snmp", IpAddr);
                        }
                    }
                }
                if (auto)
                {
                    mk.Send("/tool/bandwidth-server/print");
                    mk.Send("=.proplist=enabled");
                    mk.Send(".tag=bandwidth", true);
                    bool bandwidth = false;
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            foreach(var d in GetDictionary(s))
                            {
                                if(d.Key == "enabled")
                                {
                                    if(d.Value == "true")
                                    {
                                        bandwidth = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (bandwidth)
                    {
                        mk.Send("/tool/bandwidth-server/set");
                        mk.Send("=enabled=no");
                        mk.Send("=authenticate=no");
                        mk.Send(".tag=bandwidth", true);
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!done"))
                            {
                                Console.WriteLine("IP地址:{0},Bandwidth", IpAddr);
                            }
                        }
                    }
                    mk.Send("/tool/mac-server/print");
                    mk.Send("=.proplist=allowed-interface-list");
                    mk.Send(".tag=mac-server", true);
                    bool mac_server = false;
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            foreach(var d in GetDictionary(s))
                            {
                                if(d.Key == "allowed-interface-list")
                                {
                                    if(d.Value != "none")
                                    {
                                        mac_server = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (mac_server)
                    {
                        mk.Send("/tool/mac-server/set");
                        mk.Send("=allowed-interface-list=none");
                        mk.Send(".tag=mac-server", true);
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!done"))
                            {
                                Console.WriteLine("IP地址:{0},Mac-Server", IpAddr);
                            }
                        }
                    }
                    mk.Send("/tool/mac-server/mac-winbox/print");
                    mk.Send("=.proplist=allowed-interface-list");
                    mk.Send(".tag=mac-winbox", true);
                    bool mac_winbox = false;
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            foreach(var d in GetDictionary(s))
                            {
                                if(d.Key == "allowed-interface-list")
                                {
                                    if(d.Value != "none")
                                    {
                                        mac_winbox = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (mac_winbox)
                    {
                        mk.Send("/tool/mac-server/mac-winbox/set");
                        mk.Send("=allowed-interface-list=none");
                        mk.Send(".tag=mac-winbox", true);
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!done"))
                            {
                                Console.WriteLine("IP地址:{0},Mac-Winbox", IpAddr);
                            }
                        }
                    }
                    mk.Send("/tool/mac-server/ping/print");
                    mk.Send("=.proplist=enabled");
                    mk.Send(".tag=mac-ping", true);
                    bool mac_ping = false;
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            foreach(var d in GetDictionary(s))
                            {
                                if(d.Key == "enabled")
                                {
                                    if(d.Value == "true")
                                    {
                                        mac_ping = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (mac_ping)
                    {
                        mk.Send("/tool/mac-server/ping/set");
                        mk.Send("=enabled=no");
                        mk.Send(".tag=mac-ping", true);
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!done"))
                            {
                                Console.WriteLine("IP地址:{0},Mac-Ping", IpAddr);
                            }
                        }
                    }
                    //
                    mk.Send("/system/watchdog/set");
                    mk.Send("=watchdog-timer=no");
                    mk.Send(".tag=watchdog", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址:{0},Watchdog", IpAddr);
                        }
                    }
                    mk.Send("/ip/cloud/set");
                    mk.Send("=ddns-enabled=yes");
                    //mk.Send("=update-time=yes");
                    //mk.Send("=ddns-uptime-interval=1m");
                    mk.Send(".tag=cloud", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址:{0},Cloud", IpAddr);
                        }
                    }
                    mk.Send("/ip/cloud/advanced/set");
                    mk.Send("=use-local-address=yes");
                    mk.Send(".tag=cloud-advanced", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址:{0},Advanced", IpAddr);
                        }
                    }
                    mk.Send("/ip/service/print");
                    mk.Send("?name=www-ssl");
                    mk.Send("=.proplist=.id");
                    mk.Send(".tag=service-wwwssl", true);
                    string wwwssl_id = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            wwwssl_id = s.Substring(22);
                        }
                    }
                    mk.Send("/ip/service/set");
                    mk.Send(wwwssl_id);
                    mk.Send("=disabled=yes");
                    mk.Send(".tag=wwwssl", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址:{0},www-ssl", IpAddr);
                        }
                    }
                    //
                    mk.Send("/ip/service/print");
                    mk.Send("?name=www");
                    mk.Send("=.proplist=.id");
                    mk.Send(".tag=service-www", true);
                    string www_id = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            www_id = s.Substring(19);
                        }
                    }
                    mk.Send("/ip/service/set");
                    mk.Send(www_id);
                    mk.Send("=disabled=yes");
                    mk.Send(".tag=www", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址:{0},www", IpAddr);
                        }
                    }
                    //
                    mk.Send("/ip/service/print");
                    mk.Send("?name=telnet");
                    mk.Send("=.proplist=.id");
                    mk.Send(".tag=service-telnet", true);
                    string telnet_id = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            telnet_id = s.Substring(22);
                        }
                    }
                    mk.Send("/ip/service/set");
                    mk.Send(telnet_id);
                    mk.Send("=disabled=yes");
                    mk.Send(".tag=telnet", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址:{0},telnet", IpAddr);
                        }
                    }
                    //
                    mk.Send("/ip/service/print");
                    mk.Send("?name=ssh");
                    mk.Send("=.proplist=.id");
                    mk.Send(".tag=service-ssh", true);
                    string ssh_id= "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            ssh_id = s.Substring(19);
                        }
                    }
                    mk.Send("/ip/service/set");
                    mk.Send(ssh_id);
                    mk.Send("=disabled=yes");
                    mk.Send(".tag=ssh", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址:{0},ssh", IpAddr);
                        }
                    }
                    //
                    mk.Send("/ip/service/print");
                    mk.Send("?name=ftp");
                    mk.Send("=.proplist=.id");
                    mk.Send(".tag=service-ftp", true);
                    string ftp_id = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            ftp_id = s.Substring(19);
                        }
                    }
                    mk.Send("/ip/service/set");
                    mk.Send(ftp_id);
                    mk.Send("=disabled=yes");
                    mk.Send(".tag=ftp", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址:{0},ftp", IpAddr);
                        }
                    }
                    //
                    mk.Send("/ip/service/print");
                    mk.Send("?name=api-ssl");
                    mk.Send("=.proplist=.id");
                    mk.Send(".tag=service-apissl", true);
                    string apissl_id = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            apissl_id = s.Substring(22);
                        }
                    }
                    mk.Send("/ip/service/set");
                    mk.Send(apissl_id);
                    mk.Send("=disabled=yes");
                    mk.Send(".tag=apissl", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址:{0},api-ssl", IpAddr);
                        }
                    }
                }
                if (neighbor)
                {
                    mk.Send("/ip/neighbor/discovery-settings/set");
                    mk.Send("=discover-interface-list=none");
                    mk.Send(".tag=neighbor",true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址:{0},neighbor", IpAddr);
                        }
                    }
                }
                if (ethernet)
                {
                    mk.Send("/interface/ethernet/print");
                    mk.Send("=.proplist=name");
                    mk.Send("=.proplist=speed");
                    mk.Send("=.proplist=rx-bytes");
                    mk.Send("=.proplist=tx-bytes");
                    mk.Send("=.proplist=rx-packet");
                    mk.Send("=.proplist=tx-packet");
                    mk.Send(".tag=ethernet", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            MKethernet mKethernet = new MKethernet();
                            foreach (var d in GetDictionary(s))
                            {
                                GetEthernetInfo(d.Key, d.Value, ref mKethernet);
                            }
                            Console.WriteLine("IP地址:{0},端口:{1},速度:{2},Rx-Bytes:{3},Tx-Bytes:{4}", IpAddr, mKethernet.Name, mKethernet.Speed, mKethernet.Rx_Bytes, mKethernet.Tx_Bytes);
                        }
                    }
                }
                if (wireless)
                {
                    mk.Send("/interface/wireless/registration-table/print");
                    mk.Send("=.proplist=uptime");
                    mk.Send("=.proplist=mac-address");
                    mk.Send("=.proplist=rx-rate");
                    mk.Send("=.proplist=tx-rate");
                    mk.Send("=.proplist=rx-ccq");
                    mk.Send("=.proplist=tx-ccq");
                    mk.Send("=.proplist=signal-to-noise");
                    mk.Send("=.proplist=signal-strength");
                    mk.Send("=.proplist=signal-strength-ch0");
                    mk.Send("=.proplist=signal-strength-ch1");
                    mk.Send("=.proplist=tx-signal-strength");
                    mk.Send("=.proplist=tx-signal-strength-ch0");
                    mk.Send("=.proplist=tx-signal-strength-ch1");
                    mk.Send(".tag=wireless",true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            MKwireless mKwireless = new MKwireless();
                            foreach (var d in GetDictionary(s))
                            {
                                GetWirelessInfo(d.Key, d.Value, ref mKwireless);
                            }
                            Console.WriteLine("IP地址:{0},Mac地址:{1},时间:{2},Rx/Tx-Rate:{3}/{4},Rx/Tx-CCQ:{5}/{6},STN:{7}", IpAddr, mKwireless.MacAddr, mKwireless.Uptime, mKwireless.Rx_Rate, mKwireless.Tx_Rate, mKwireless.Rx_CCQ, mKwireless.Tx_CCQ, mKwireless.Signal_To_Noise);
                        }
                    }
                }
                if (resource)
                {
                    mk.Send("/system/resource/print");
                    mk.Send("=.proplist=uptime");
                    mk.Send("=.proplist=version");
                    mk.Send("=.proplist=cpu-load");
                    mk.Send("=.proplist=free-memory");
                    mk.Send("=.proplist=total-memory");
                    mk.Send("=.proplist=free-hdd-space");
                    mk.Send("=.proplist=total-hdd-space");
                    mk.Send(".tag=resource", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            MKresource mKresource = new MKresource();
                            foreach (var d in GetDictionary(s))
                            {
                                GetResourceInfo(d.Key, d.Value, ref mKresource); 
                            }
                            double free_mem = (Convert.ToDouble( mKresource.Free_Memory)/Convert.ToDouble(mKresource.Total_Memory));
                            double free_hdd = (Convert.ToDouble(mKresource.Free_Hdd_Space)/Convert.ToDouble(mKresource.Total_Hdd_Space));
                            Console.WriteLine("IP地址:{0},运行时间:{1},版本:{2},CPU:{3}%,内存:{4:P},闪存:{5:P}", IpAddr, mKresource.Uptime, mKresource.Version, mKresource.Cpu_Load, free_mem, free_hdd);
                        }
                    }
                }
                if (romon)
                {
                    mk.Send("/tool/romon/print");
                    mk.Send("=.proplist=enabled");
                    mk.Send(".tag=romon", true);
                    bool romonFlag = false;
                    foreach (string s in mk.Read())
                    {                      
                        if (s.StartsWith("!re"))
                        {
                            foreach(var d in GetDictionary(s))
                            {
                                if(d.Key == "enabled")
                                {
                                    if(d.Value == "true")
                                    {
                                        romonFlag = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (romonFlag)
                    {
                        mk.Send("/tool/romon/set");
                        mk.Send("=enabled=yes");
                        mk.Send(".tag=romon", true);
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!done"))
                            {
                                Console.WriteLine("IP地址:{0},RoMon", IpAddr);
                            }
                        }
                    }
                }
                if (health)
                {
                    mk.Send("/system/health/print");
                    mk.Send(".tag=health",true);
                    foreach(string s in mk.Read())
                    {
                        if(s.StartsWith("!re"))
                        {
                            MKhealth mKhealth = new MKhealth();
                            foreach(var d in GetDictionary(s))
                            {
                                GetHealthInfo(d.Key, d.Value, ref mKhealth);
                            }
                            Console.WriteLine("IP地址:{0},电压:{1},温度:{2}", IpAddr, mKhealth.Voltage, mKhealth.Temperature);
                        }
                    }
                }
                if (reboot)
                {
                    mk.Send("/system/reboot");
                    mk.Send(".tag=reboot",true);
                    foreach(string s in mk.Read())
                    {
                        if(s.StartsWith("!done"))
                        {
                            Console.WriteLine("IP地址{0},重启",IpAddr);
                        }
                    }
                }
                if (capsman)
                {
                    mk.Send("/caps-man/registration-table/print");
                    mk.Send("=.proplist=interface");
                    mk.Send("=.proplist=ssid");
                    mk.Send("=.proplist=mac-address");
                    mk.Send("=.proplist=rx-rate");
                    mk.Send("=.proplist=tx-rate");
                    mk.Send("=.proplist=rx-signal");
                    mk.Send("=.proplist=uptime");
                    mk.Send(".tag=capsman",true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            MKcapsman mKcapsman = new MKcapsman();
                            foreach (var d in GetDictionary(s))
                            {
                                GetCapsmanInfo(d.Key, d.Value, ref mKcapsman);
                            }
                            Console.WriteLine("IP地址:{0},SSID:{1},Mac地址:{2},时间:{3},Rx-Rate/Tx-Rate:{4}/{5},",IpAddr,mKcapsman.SSID,mKcapsman.MacAddress,mKcapsman.Uptime,mKcapsman.RxRate,mKcapsman.TxRate);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("IP地址:{0},账号密码错误", IpAddr);
            }
            mk.Close();
            for(int i = 0; i < Console.WindowWidth ;i++)
            {
                Console.Write("-");
            }    
        }
        static void GetCapsmanInfo(string key,string value,ref MKcapsman mKcapsman)
        {
            switch(key)
            {
                case "interface":
                    mKcapsman.Interface = value;
                    break;
                case "ssid":
                    mKcapsman.SSID = value;
                    break;
                case "mac-address":
                    mKcapsman.MacAddress = value;
                    break;
                case "rx-rate":
                    mKcapsman.RxRate = value;
                    break;
                case "tx-rate":
                    mKcapsman.TxRate = value;
                    break;
                case "rx-signal":
                    mKcapsman.RxSignal = value;
                    break;
                case "uptime":
                    mKcapsman.Uptime = value;
                    break;
            }
        }
        static void GetHealthInfo(string key,string value,ref MKhealth mKhealth)
        {
            switch (key)
            {
                case "voltage":
                    mKhealth.Voltage = value;
                    break;
                case "temperature":
                    mKhealth.Temperature = value;
                    break;
            }
        }
        static void GetEthernetInfo(string key,string value,ref MKethernet mKethernet)
        {
            switch (key)
            {
                case "name":
                    mKethernet.Name = value;
                    break;
                case "speed":
                    mKethernet.Speed = value;
                    break;
                case "rx-bytes":
                    mKethernet.Rx_Bytes = value;
                    break;
                case "tx-bytes":
                    mKethernet.Tx_Bytes = value;
                    break;
                case "rx-packet":
                    mKethernet.Rx_Packet = value;
                    break;
                case "tx-packet":
                    mKethernet.Tx_Packet = value;
                    break;
            }
        }
        static void GetWirelessInfo(string key,string value,ref MKwireless mKwireless)
        {
            switch (key)
            {
                case "mac-address":
                    mKwireless.MacAddr = value;
                    break;
                case "uptime":
                    mKwireless.Uptime = value;
                    break;
                case "rx-rate":
                    mKwireless.Rx_Rate = value;
                    break;
                case "tx-rate":
                    mKwireless.Tx_Rate = value;
                    break;
                case "rx-ccq":
                    mKwireless.Rx_CCQ = value;
                    break;
                case "tx-ccq":
                    mKwireless.Tx_CCQ = value;
                    break;
                case "signal-to-noise":
                    mKwireless.Signal_To_Noise = value;
                    break;
                case "signal-strength":
                    mKwireless.Signal_Strength = value;
                    break;
                case "signal-strength-ch0":
                    mKwireless.Signal_Strength_CH0 = value;
                    break;
                case "signal-strength-ch1":
                    mKwireless.Signal_Strength_CH1 = value;
                    break;
                case "tx-signal-strength":
                    mKwireless.Tx_Signal_Strength = value;
                    break;
                case "tx-signal-strength-ch0":
                    mKwireless.Tx_Signal_Strength_CH0 = value;
                    break;
                case "tx-signal-strength-ch1":
                    mKwireless.Tx_Signal_Strength_CH1 = value;
                    break;
            }
        }
        static void GetResourceInfo(string key,string value ,ref MKresource mKresource)
        {
            switch(key)
            {
                case "uptime":
                    mKresource.Uptime = value;
                    break;
                case "version":
                    mKresource.Version = value;
                    break;
                case "cpu-load":
                    mKresource.Cpu_Load = value;
                    break;
                case "free-memory":
                    mKresource.Free_Memory = value;
                    break;
                case "total-memory":
                    mKresource.Total_Memory = value;
                    break;
                case "free-hdd-space":
                    mKresource.Free_Hdd_Space = value;
                    break;
                case "total-hdd-space":
                    mKresource.Total_Hdd_Space = value;
                    break;
            }    
        }
        static Dictionary<string, string> GetDictionary(string data)
        {
            List<string> keys = new List<string>();
            List<string> values = new List<string>();
            string[] datas = data.Split("=");
            for (int i = 0; i < datas.Length; i++)
            {
                if (i % 2 == 1)
                {
                    values.Add(datas[i]);
                }
                else
                {
                    keys.Add(datas[i]);
                }
            }
            Dictionary<string, string> keyValue = new Dictionary<string, string>();
            for (int i = 0; i < keys.Count; i++)
            {
                keyValue.Add(keys[i], values[i]);
            }
            return keyValue;
        }
    }
}