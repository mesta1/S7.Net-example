using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HmiExample.PlcConnectivity
{
    class PlcTags
    {       
        public const string BitVariable = "DB1.DBX0.0";
        public const string IntVariable = "DB1.DBW2";
        public const string DoubleVariable = "DB1.DBD4";
        public const string DIntVariable = "DB1.DBD8";
        public const string DwordVariable = "DB1.DBW12";
    }
}
