using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParametriConfigurazione.Classi
{
    public class YConfigPLC_Registri
    {
        public string DBGruppo { get; set; }
        public int Registro { get; set; }
        public int Campo { get; set; }
        public string RW { get; set; }
    }
}
