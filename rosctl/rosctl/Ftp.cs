using System;
using System.Collections.Generic;
using System.Text;

namespace rosctl
{
    class Ftp
    {
        public string Address { get; set; }
        public string Username { get; set; } = "root";
        public string Password { get; set; } = "root";
    }
}