using System;
using System.Collections.Generic;
using System.Text;

namespace rosctl
{
    class MKwireless
    {
        public string Uptime { get; set; }
        public string Rx_Rate { get; set; }
        public string Tx_Rate { get; set; }
        public string Rx_CCQ { get; set; }
        public string Tx_CCQ { get; set; }
        public string Signal_To_Noise { get; set; }
        public string Signal_Strength { get; set; }
        public string Signal_Strength_CH0 { get; set; }
        public string Signal_Strength_CH1 { get; set; }
        public string Tx_Signal_Strength { get; set; }
        public string Tx_Signal_Strength_CH0 { get; set; }
        public string Tx_Signal_Strength_CH1 { get; set; }
    }
}