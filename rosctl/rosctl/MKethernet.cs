 using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace rosctl
{
    class MKethernet
    {
        public string Name { get; set; }
        public string Speed { get; set; }
        public string Rx_Bytes { get; set; }
        public string Tx_Bytes { get; set; }
        public string Rx_Packet { get; set; }
        public string Tx_Packet { get; set; }
    }
}