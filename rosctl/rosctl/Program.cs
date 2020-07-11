using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace rosctl
{
    class Program
    {
        static readonly Timer Timer = new Timer(Timer_Callback, null, Timeout.Infinite, Timeout.Infinite);
        static readonly List<string> IpAddrs = new List<string>();
        static MKmndp mndp = new MKmndp();
        static string username = "";
        static string password = "";
        static string newPassword = "";
        static string logging = "";
        static string ntp = "";
        static string snmp = "";
        static bool auto = false;
        static bool wireless = false;
        static bool ethernet = false;
        static bool resource = false;
        static bool neighbor = false;
        static void Main(string[] args)
        {
            if (args.Length > 2)
            {
                if (args[0] == "mndp")
                {
                    for (int i = 1; i < args.Length; i++)
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
                                    username = args[t];
                                }
                            }
                            else
                            {
                                username = st;
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
                                    password = args[t];
                                }
                            }
                            else
                            {
                                password = st;
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
                                    Console.WriteLine("Logging Is Null");
                                }
                                else
                                {
                                    logging = args[t];
                                }
                            }
                            else
                            {
                                logging = st;
                            }

                        }
                        else if (args[i].StartsWith("--snmp"))
                        {
                            string st = args[i].Substring(6);
                            if (st.Length == 0)
                            {
                                int t = i + 1;
                                if (t >= args.Length)
                                {
                                    Console.WriteLine("SNMP Is Null");
                                }
                                else
                                {
                                    snmp = args[t];
                                }
                            }
                            else
                            {
                                snmp = st;
                            }
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
                    }
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
                else
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].StartsWith("-ip"))
                        {
                            string st = args[i].Substring(3);
                            if (st.Length == 0)
                            {
                                int t = i + 1;
                                if (t >= args.Length)
                                {
                                    Console.WriteLine("IpAddr Is Null");
                                }
                                else
                                {
                                    string[] addrs = args[t].Split(",");
                                    for (int j = 0; j < addrs.Length; j++)
                                    {
                                        IpAddrs.Add(addrs[j]);
                                    }
                                }
                            }
                            else
                            {
                                string[] addrs = st.Split(",");
                                for (int j = 0; j < addrs.Length; j++)
                                {
                                    IpAddrs.Add(addrs[j]);
                                }
                            }
                        }
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
                                    username = args[t];
                                }
                            }
                            else
                            {
                                username = st;
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
                                    password = args[t];
                                }
                            }
                            else
                            {
                                password = st;
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
                                    Console.WriteLine("Logging Is Null");
                                }
                                else
                                {
                                    logging = args[t];
                                }
                            }
                            else
                            {
                                logging = st;
                            }

                        }
                        else if (args[i].StartsWith("--snmp"))
                        {
                            string st = args[i].Substring(6);
                            if (st.Length == 0)
                            {
                                int t = i + 1;
                                if (t >= args.Length)
                                {
                                    Console.WriteLine("SNMP Is Null");
                                }
                                else
                                {
                                    snmp = args[t];
                                }
                            }
                            else
                            {
                                snmp = st;
                            }
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
                    }
                    foreach (string s in IpAddrs)
                    {
                        Timer_MK(s);
                    }
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
                    case "--copyright":
                        Copyright();
                        break;
                    case "--version":
                        Version();
                        break;
                }
            }
            else if (args.Length == 0)
            {
                Help();
            }
        }
        static void Help()
        {
            Console.WriteLine("命令配置：");
            Console.WriteLine("ros mndp --show");
            Console.WriteLine("ros mndp -u root -p password");
            Console.WriteLine("ros mndp -u root -p password --auto");
            Console.WriteLine("ros mndp -u root -p password --ethernet");
            Console.WriteLine("ros mndp -u root -p password --resource");
            Console.WriteLine("ros mndp -u root -p password --wireless");
            Console.WriteLine("ros mndp -u root -p password --neighbor");
            Console.WriteLine("ros mndp -u root -p password --new password");
            Console.WriteLine("ros mndp -u root -p password --logging 192.168.1.2");
            Console.WriteLine("ros mndp -u root -p password --snmp 192.168.1.2");
            Console.WriteLine("ros mndp -u root -p password --ntp 192.168.1.2");
            Console.WriteLine("ros mndp -u root -p password --new password --loging 192.168.1.2 --snmp 192.168.1.2");
            Console.WriteLine("ros -ip 192.168.1.3 -u root -p password --auto");
            Console.WriteLine("ros -ip 192.168.1.3 -u root -p password --resource");
            Console.WriteLine("ros -ip 192.168.1.3 -u root -p password --ethernet");
            Console.WriteLine("ros -ip 192.168.1.3 -u root -p password --wireless");
            Console.WriteLine("ros -ip 192.168.1.3 -u root -p password --new password");
            Console.WriteLine("ros -ip 192.168.1.3 -u root -p password --logging 192.168.1.2");
            Console.WriteLine("ros -ip 192.168.1.3 -u root -p password --snmp 192.168.1.2");
            Console.WriteLine("ros -ip 192.168.1.3 -u root -p password --ntp 192.168.1.2");
            Console.WriteLine("ros -ip 192.168.1.3 -u root -p password --new password --logging 192.168.1.2 --snmp 192.168.1.2");
            Console.WriteLine("命令帮助:");
            Console.WriteLine("ros --help");
            Console.WriteLine("ros --author");
            Console.WriteLine("ros --version");
            Console.WriteLine("ros --copyright");
        }
        static void Author()
        {
            Console.WriteLine("Author:LiBing,Phone:18908035651");
        }
        static void Copyright()
        {
            Console.WriteLine("Copyright:LiBing");
        }
        static void Version()
        {
            Console.WriteLine("20200710");
        }
        private static void Timer_Callback(object state)
        {
            foreach (MKInfo t in mndp.GetMKInfos)
            {
                Timer_MK(t.IPAddr);
            }
        }
        private static void Timer_MK(string IpAddr)
        {
            MK mk = new MK(IpAddr);
            if (mk.Login(username, password))
            {
                if (!String.IsNullOrEmpty(newPassword))
                {
                    mk.Send("/password");
                    mk.Send("=old-password=" + password);
                    mk.Send("=new-password=" + newPassword);
                    mk.Send("=confirm-new-password=" + newPassword);
                    mk.Send(".tag=password", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},password", IpAddr);
                        }
                    }
                }
                if (!String.IsNullOrEmpty(logging))
                {
                    mk.Send("/system/logging/action/print");
                    mk.Send("?name=log");
                    mk.Send("=.proplist=remote");
                    mk.Send(".tag=log-action", true);
                    string remote = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            remote = s.Substring(26);
                        }
                    }
                    if (string.IsNullOrEmpty(remote))
                    {
                        mk.Send("/system/logging/action/add");
                        mk.Send("=name=log");
                        mk.Send("=target=remote");
                        mk.Send("=remote=" + logging);
                        mk.Send("=bsd-syslog=yes");
                        mk.Send(".tag=action", true);
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!done"))
                            {
                                Console.WriteLine("IpAddr:{0},action-add", IpAddr);
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
                                    Console.WriteLine("IpAddr:{0},warning", IpAddr);
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
                                    Console.WriteLine("IpAddr:{0},error", IpAddr);
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
                                    Console.WriteLine("IpAddr:{0},info", IpAddr);
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
                                    Console.WriteLine("IpAddr:{0},critical", IpAddr);
                                }
                            }
                        }
                    }
                    else if (remote != logging)
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
                        mk.Send("=remote=" + logging);
                        mk.Send(".tag=log-action-remove", true);
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!done"))
                            {
                                Console.WriteLine("action-remove");
                            }
                        }
                    }
                }
                if (!String.IsNullOrEmpty(ntp))
                {
                    mk.Send("/system/ntp/client/set");
                    mk.Send("=enabled=yes");
                    mk.Send("=mode=unicast");
                    mk.Send("=primary-ntp=" + ntp);
                    mk.Send(".tag=ntp", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},ntp", IpAddr);
                        }
                    }
                }
                if (!String.IsNullOrEmpty(snmp))
                {
                    mk.Send("/snmp/set");
                    mk.Send("=enabled=yes");
                    mk.Send("=contact=LiBing");
                    mk.Send("=location=18908035651");
                    mk.Send("=trap-target=" + snmp);
                    mk.Send("=trap-version=2");
                    mk.Send("=trap-generators=interface");
                    mk.Send(".tag=snmp", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},snmp", IpAddr);
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
                            if (s.Substring(26) == "yes")
                            {
                                bandwidth = true;
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
                                Console.WriteLine("IpAddr:{0},bandwidth", IpAddr);
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
                            if (s.Substring(42) != "none")
                            {
                                mac_server = true;
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
                                Console.WriteLine("IpAddr:{0},mac-server", IpAddr);
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
                            if (s.Substring(42) != "none")
                            {
                                mac_winbox = true;
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
                                Console.WriteLine("IpAddr:{0},mac-winbox", IpAddr);
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
                            if (s.Substring(25) == "yes")
                            {
                                mac_ping = true;
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
                                Console.WriteLine("IpAddr:{0},mac-ping", IpAddr);
                            }
                        }
                    }
                    mk.Send("/tool/romon/print");
                    mk.Send("=.proplist=enabled");
                    mk.Send(".tag=romon", true);
                    bool romon = false;
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            if (s.Substring(22) != "true")
                            {
                                romon = true;
                            }
                        }
                    }
                    if (romon)
                    {
                        mk.Send("/tool/romon/set");
                        mk.Send("=enabled=yes");
                        mk.Send(".tag=romon", true);
                        foreach (string s in mk.Read())
                        {
                            if (s.StartsWith("!done"))
                            {
                                Console.WriteLine("IpAddr:{0},romon", IpAddr);
                            }
                        }
                    }
                    mk.Send("/system/watchdog/set");
                    mk.Send("=watchdog-timer=no");
                    mk.Send(".tag=watchdog", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},watchdog", IpAddr);
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
                            Console.WriteLine("IpAddr:{0},cloud", IpAddr);
                        }
                    }
                    mk.Send("/ip/cloud/advanced/set");
                    mk.Send("=use-local-address=yes");
                    mk.Send(".tag=cloud-advanced", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},advanced", IpAddr);
                        }
                    }
                    mk.Send("/ip/service/print");
                    mk.Send("?name=www-ssl");
                    mk.Send("=.proplist=.id");
                    mk.Send(".tag=service-wwwssl", true);
                    string swwwssl = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            swwwssl = s.Substring(22);
                        }
                    }
                    mk.Send("/ip/service/set");
                    mk.Send(swwwssl);
                    mk.Send("=disabled=yes");
                    mk.Send(".tag=wwwssl", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},www-ssl", IpAddr);
                        }
                    }
                    //
                    mk.Send("/ip/service/print");
                    mk.Send("?name=www");
                    mk.Send("=.proplist=.id");
                    mk.Send(".tag=service-www", true);
                    string swww = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            swww = s.Substring(19);
                        }
                    }
                    mk.Send("/ip/service/set");
                    mk.Send(swww);
                    mk.Send("=disabled=yes");
                    mk.Send(".tag=www", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},www", IpAddr);
                        }
                    }
                    //
                    mk.Send("/ip/service/print");
                    mk.Send("?name=telnet");
                    mk.Send("=.proplist=.id");
                    mk.Send(".tag=service-telnet", true);
                    string stelnet = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            stelnet = s.Substring(22);
                        }
                    }
                    mk.Send("/ip/service/set");
                    mk.Send(stelnet);
                    mk.Send("=disabled=yes");
                    mk.Send(".tag=telnet", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},telnet", IpAddr);
                        }
                    }
                    //
                    mk.Send("/ip/service/print");
                    mk.Send("?name=ssh");
                    mk.Send("=.proplist=.id");
                    mk.Send(".tag=service-ssh", true);
                    string sssh = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            sssh = s.Substring(19);
                        }
                    }
                    mk.Send("/ip/service/set");
                    mk.Send(sssh);
                    mk.Send("=disabled=yes");
                    mk.Send(".tag=ssh", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},ssh", IpAddr);
                        }
                    }
                    //
                    mk.Send("/ip/service/print");
                    mk.Send("?name=ftp");
                    mk.Send("=.proplist=.id");
                    mk.Send(".tag=service-ftp", true);
                    string sftp = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            sftp = s.Substring(19);
                        }
                    }
                    mk.Send("/ip/service/set");
                    mk.Send(sftp);
                    mk.Send("=disabled=yes");
                    mk.Send(".tag=ftp", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},ftp", IpAddr);
                        }
                    }
                    //
                    mk.Send("/ip/service/print");
                    mk.Send("?name=api-ssl");
                    mk.Send("=.proplist=.id");
                    mk.Send(".tag=service-apissl", true);
                    string sapissl = "";
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            sapissl = s.Substring(22);
                        }
                    }
                    mk.Send("/ip/service/set");
                    mk.Send(sapissl);
                    mk.Send("=disabled=yes");
                    mk.Send(".tag=apissl", true);
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},api-ssl", IpAddr);
                        }
                    }
                }
                if (neighbor)
                {
                    mk.Send("/ip/neighbor/discovery-setting/set");
                    mk.Send("=discover-interface-list=none");
                    mk.Send(".tag=neighbor");
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!done"))
                        {
                            Console.WriteLine("IpAddr:{0},neighbor", IpAddr);
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
                        string data = s.Substring(16);
                        foreach (var d in GetDictionary(data))
                        {
                            Console.WriteLine("IPAddr:{0},{1}:{2}", IpAddr, d.Key, d.Value);
                        }
                    }
                }
                if (wireless)
                {
                    mk.Send("/interface/wireless/registeration-table/print");
                    mk.Send("=.proplist=uptime");
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
                    mk.Send(".tag=wireless");
                    foreach (string s in mk.Read())
                    {
                        if (s.StartsWith("!re"))
                        {
                            string data = s.Substring(16);
                            foreach (var d in GetDictionary(data))
                            {
                                Console.WriteLine("IpAddr:{0},{1}:{2}", d.Key, d.Value);
                            }
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
                            string data = s.Substring(16);
                            foreach (var d in GetDictionary(data))
                            {
                                Console.WriteLine("IPAddr:{0},{1}:{2}", IpAddr, d.Key, d.Value);
                            }
                        }
                    }
                }
                for (int i = 0; i < Console.WindowWidth; i++)
                {
                    Console.Write("-");
                }
            }
            else
            {
                Console.WriteLine("IPAddr:{0},Can't Login", IpAddr);
            }
            mk.Close();
        }
        static Dictionary<string, string> GetDictionary(string data)
        {
            List<string> keys = new List<string>();
            List<string> values = new List<string>();
            string[] datas = data.Split("=");
            for (int i = 1; i < datas.Length; i++)
            {
                if (i % 2 == 1)
                {
                    keys.Add(datas[i]);
                }
                else
                {
                    values.Add(datas[i]);
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
        static readonly UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, Port));
        static IPEndPoint IPBroadcast;
        readonly Thread threadSend;
        readonly Thread threadReceive;
        static readonly List<MKInfo> mkInfos = new List<MKInfo>();
        bool sendFlag = true;
        bool receiveFlag = true;
        static readonly string sendName = "Send";
        static readonly string receiveName = "Receive";
        public MKmndp()
        {
            IPBroadcast = new IPEndPoint(IPAddress.Broadcast, Port);
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
                                    if (t.MacAddr == mkInfo.MacAddr)
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
            mkInfos.RemoveAt(i);
        }
        delegate void ListAdd(MKInfo m);
        private void MKInfoAdd(MKInfo m)
        {
            mkInfos.Add(m);
        }
        public List<MKInfo> GetMKInfos
        {
            get
            {
                return mkInfos;
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
    class MKInfo
    {
        public string IPAddr { get; set; }
        public string MacAddr { get; set; }
        public string Identity { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; }
        public string Uptime { get; set; }
        public string SoftwareID { get; set; }
        public string Board { get; set; }
        public string Unpack { get; set; }
        public string IPv6Addr { get; set; }
        public string InterfaceName { get; set; }
    }
    class MK
    {
        readonly Stream connection;
        readonly TcpClient con;
        public MK(string ip)
        {
            con = new TcpClient();
            con.Connect(ip, 8728);
            connection = (Stream)con.GetStream();
        }
        public void Close()
        {
            connection.Close();
            con.Close();
        }
        public bool Login(string username, string password)
        {
            if (!Login_v2(username, password))
            {
                return Login_v1(username, password);
            }
            else
            {
                return true;
            }
        }
        public bool Login_v1(string username, string password)
        {
            Send("/login", true);
            string hash = Read()[0].Split(new string[] { "ret=" }, StringSplitOptions.None)[1];
            Send("/login");
            Send("=name=" + username);
            Send("=response=00" + EncodePassword(password, hash), true);
            if (Read()[0] == "!done")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Login_v2(string username, string password)
        {
            Send("/login");
            Send("=name=" + username);
            Send("=password=" + password, true);
            if (Read()[0] == "!done")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Send(string co)
        {
            Send(co, false);
        }
        public void Send(string co, bool endsentence)
        {
            byte[] bajty = Encoding.ASCII.GetBytes(co.ToCharArray());
            byte[] velikost = EncodeLength(bajty.Length);
            connection.Write(velikost, 0, velikost.Length);
            connection.Write(bajty, 0, bajty.Length);
            if (endsentence)
            {
                connection.WriteByte(0);
            }
        }
        public List<string> Read()
        {
            List<string> output = new List<string>();
            string o = "";
            byte[] tmp = new byte[4];
            long count;
            while (true)
            {
                tmp[3] = (byte)connection.ReadByte();
                if (tmp[3] == 220) tmp[3] = (byte)connection.ReadByte(); 
                //it sometimes happend to me that                 
                //mikrotik send 220 as some kind of "bonus" between words, this fixed things, not sure about it though
                if (tmp[3] == 0)
                {
                    output.Add(o);
                    if (o.Substring(0, 5) == "!done")
                    {
                        break;
                    }
                    else
                    {
                        o = "";
                        continue;
                    }
                }
                else
                {
                    if (tmp[3] < 0x80)
                    {
                        count = tmp[3];
                    }
                    else
                    {
                        if (tmp[3] < 0xC0)
                        {
                            int tmpi = BitConverter.ToInt32(new byte[] { (byte)connection.ReadByte(), tmp[3], 0, 0 }, 0);
                            count = tmpi ^ 0x8000;
                        }
                        else
                        {
                            if (tmp[3] < 0xE0)
                            {
                                tmp[2] = (byte)connection.ReadByte();
                                int tmpi = BitConverter.ToInt32(new byte[] { (byte)connection.ReadByte(), tmp[2], tmp[3], 0 }, 0);
                                count = tmpi ^ 0xC00000;
                            }
                            else
                            {
                                if (tmp[3] < 0xF0)
                                {
                                    tmp[2] = (byte)connection.ReadByte();
                                    tmp[1] = (byte)connection.ReadByte();
                                    int tmpi = BitConverter.ToInt32(new byte[] { (byte)connection.ReadByte(), tmp[1], tmp[2], tmp[3] }, 0);
                                    count = tmpi ^ 0xE0000000;
                                }
                                else
                                {
                                    if (tmp[3] == 0xF0)
                                    {
                                        tmp[3] = (byte)connection.ReadByte();
                                        tmp[2] = (byte)connection.ReadByte();
                                        tmp[1] = (byte)connection.ReadByte();
                                        tmp[0] = (byte)connection.ReadByte();
                                        count = BitConverter.ToInt32(tmp, 0);
                                    }
                                    else
                                    {
                                        //Error in packet reception, unknown length
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < count; i++)
                {
                    o += (Char)connection.ReadByte();
                }
            }
            return output;
        }
        byte[] EncodeLength(int delka)
        {
            if (delka < 0x80)
            {
                byte[] tmp = BitConverter.GetBytes(delka);
                return new byte[1] { tmp[0] };
            }
            if (delka < 0x4000)
            {
                byte[] tmp = BitConverter.GetBytes(delka | 0x8000);
                return new byte[2] { tmp[1], tmp[0] };
            }
            if (delka < 0x200000)
            {
                byte[] tmp = BitConverter.GetBytes(delka | 0xC00000);
                return new byte[3] { tmp[2], tmp[1], tmp[0] };
            }
            if (delka < 0x10000000)
            {
                byte[] tmp = BitConverter.GetBytes(delka | 0xE0000000);
                return new byte[4] { tmp[3], tmp[2], tmp[1], tmp[0] };
            }
            else
            {
                byte[] tmp = BitConverter.GetBytes(delka);
                return new byte[5] { 0xF0, tmp[3], tmp[2], tmp[1], tmp[0] };
            }
        }
        public string EncodePassword(string Password, string hash)
        {
            byte[] hash_byte = new byte[hash.Length / 2];
            for (int i = 0; i <= hash.Length - 2; i += 2)
            {
                hash_byte[i / 2] = Byte.Parse(hash.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
            }
            byte[] heslo = new byte[1 + Password.Length + hash_byte.Length];
            heslo[0] = 0;
            Encoding.ASCII.GetBytes(Password.ToCharArray()).CopyTo(heslo, 1);
            hash_byte.CopyTo(heslo, 1 + Password.Length);

            Byte[] hotovo;
            System.Security.Cryptography.MD5 md5;

            md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            hotovo = md5.ComputeHash(heslo);

            //Convert encoded bytes back to a 'readable' string
            string navrat = "";
            foreach (byte h in hotovo)
            {
                navrat += h.ToString("x2");
            }
            return navrat;
        }
    }
}