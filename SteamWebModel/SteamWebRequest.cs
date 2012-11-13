using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWebModel
{
    public abstract class SteamWebRequest : ISteamWebRequest
    {
        private List<ISteamWebRequestMethod> methods;

        public string Name { get; private set;  }
        public IEnumerable<ISteamWebRequestMethod> Methods { get { return methods; } }

        public SteamWebRequest(string name)
        {
            Name = name;
            methods = new List<ISteamWebRequestMethod>();
        }

        public void AddMethod(ISteamWebRequestMethod method)
        {
            methods.Add(method);
        }
    }
}
