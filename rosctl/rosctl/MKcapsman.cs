using System;
using System.Collections.Generic;
using System.Text;

namespace rosctl
{
    class MKcapsman
    {
        public string Interface { get; set; }
        public string SSID { get; set; }
        public string MacAddress { get; set; }
        public string RxRate { get; set; }
        public string TxRate { get; set; }
        public string RxSignal { get; set; }
        public string Uptime { get; set; }
    }
}