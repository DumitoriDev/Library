using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    public class LoginData
    {
        public string Login { get; set; } = "";
        public string ServerName { get; set; } = "";
        public string DataBaseName { get; set; } = "";
    }
}
