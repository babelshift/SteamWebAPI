using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWebModel
{
    public class WebRequestParameter
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public WebRequestParameter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
