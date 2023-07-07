using ParametriConfigurazione.Classi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParametriConfigurazione
{
    public class ImpostazioniJson
    {
        public Impostazioni Impostazioni { get; set; } = new();
    }

    public class Impostazioni
    {
        public ConnessioneSql ConnessioneSql { get; set; } = new();
        public string PathInstallazione { get; set; }
        public string TipoProdotto { get; set; }
        public string LocaleIP { get; set; }
        public int LocalePort { get; set; }
        public List<ConfigurazioneGruppo> ConfigurazioneGruppo { get; set; } = new();
    }

    public class ConnessioneSql
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Utente { get; set; }
        public string Password { get; set; }
    }

    public class ConfigurazioneGruppo
    {
        public string Gruppo { get; set; }
        public Parametri Configurazione { get; set; } = new();
    }

    public class Parametri
    {
        public Bottiglie Bottiglie { get; set; } = new();
        public Fusti Fusti { get; set; } = new();
        public List<StatoColore> StatoColore { get; set; } = new() { 
            new() {Colore = 0, Stato = -1 },
            new() {Colore = 0, Stato = 1 },
            new() {Colore = 0, Stato = 2 },
            new() {Colore = 0, Stato = 3 },
            new() {Colore = 0, Stato = 4 },
            new() {Colore = 0, Stato = 5 }
        };
    }

    public class Bottiglie
    {
        public string PLCIP { get; set; }
        public int PLCPort { get; set; }
        public string PathCSV { get; set; }
        public string PathBAT { get; set; }
        public int ModelloAcquisizione { get; set; }
        public string PathImportExport { get; set; }
        public List<string> FiltroUdm { get; set; } = new();
    }

    public class Fusti
    {
        public string PLCIP { get; set; }
        public int PLCPort { get; set; }
        public string PathScambio { get; set; }
        public string PathCSV { get; set; }
        public string PathBAT { get; set; }
        public int ModelloAcquisizione { get; set; }
        public string PathImportExport { get; set; }
        public string PALLETIP { get; set; }
        public int PALLETPort { get; set; }
        public List<string> FiltroUdm { get; set; } = new();
    }

    public class StatoColore
    {
        public int Stato { get; set; }
        public int Colore { get; set; }
    }

}
