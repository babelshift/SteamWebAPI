using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWebModel
{
    public class Cart
    {
        public long ID { get; set; }
        public bool IsValid { get; set; }
        public long OwnerSteamID { get; set; }
    }
}
