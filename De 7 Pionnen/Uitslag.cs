using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_7_Pionnen
{
    public class Uitslag
    {
        public string Naam { get; set; }
        public string Waarde { get; set; }

        public Uitslag(string n, string w)
        {
            Naam = n;
            Waarde = w;
        }
    }
}
