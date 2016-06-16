using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaYSoft.BaseCode.DBClass.Model
{
    public struct ConnectionArg
    {
        public string IP { get; set; }

        public string Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string DBName { get; set; }
    }
}
