using System;
using System.Collections.Generic;
using System.Text;

namespace rosctl
{
    class MKresource
    {
        public string Uptime { get; set; }
        public string Version { get; set; }
        public string Cpu_Load { get; set; }
        public string Free_Memory { get; set; }
        public string Total_Memory { get; set; }
        public string Free_Hdd_Space { get; set; }
        public string Total_Hdd_Space { get; set; }
    }
}