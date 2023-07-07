using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParametriConfigurazione.Classi
{
    public class YOrdiniProduzione
    {
        public static ImpostazioniJson Configurazione { get; set; }

        public string DBGruppo { get; set; }
        public DateTime DataRegistrazione { get; set; }
        public DateTime DataOriginale { get; set; }
        public string NumDocOriginale { get; set; }
        public decimal PeriodoRifNumeraz { get; set; }
        public int CodTipoDoc { get; set; }
        public int CodSerie { get; set; }
        public int NumRegistraz { get; set; }
        public int IdDocumento { get; set; }
        public int IdRiga { get; set; }
        public int Stato { get; set; }
        public decimal NumProgrRiga { get; set; }
        public string CodArt { get; set; }
        public string VarianteArt { get; set; }
        public string Des { get; set; }
        public string UnitaMisura { get; set; }
        public decimal Quantita { get; set; }
        public decimal QuantitaProdotta { get; set; }
        public decimal QuantitaProdotta_HH { get; set; }
        public string UmSecondaria { get; set; }
        public decimal QtaUmSecondaria { get; set; }
        public decimal QuantitaProdotta2 { get; set; }
        public DateTime DataInizioSched { get; set; }
        public DateTime DataFineSched { get; set; }
        public string CodMagPrincipale { get; set; }
        public string CodAreaMagPrinc { get; set; }
        public string RifLottoAlfanum { get; set;}
        public string NotaInterna { get; set; }
        public decimal Capacita { get; set; }

        public string StatoDescrizione
        {
            get
            {
                return Stato switch
                {
                    -1 => "Concluso - Attesa avanzamento",
                    0 => "Da pianificare",
                    1 => "Pianificato",
                    2 => "Lanciato",
                    3 => "In avanzamento",
                    4 => "Terminato",
                    5 => "Sospeso",
                    _ => "",
                };
            }
        }

        public Color ColoreStato
        {
            get
            {
                return Stato switch
                {
                    -1 => Color.FromArgb(Configurazione.Impostazioni.ConfigurazioneGruppo.FirstOrDefault(g => g.Gruppo == DBGruppo).Configurazione.StatoColore.FirstOrDefault(c => c.Stato == -1).Colore),
                    0 => Color.White,
                    1 => Color.FromArgb(Configurazione.Impostazioni.ConfigurazioneGruppo.FirstOrDefault(g => g.Gruppo == DBGruppo).Configurazione.StatoColore.FirstOrDefault(c => c.Stato == 1).Colore),
                    2 => Color.FromArgb(Configurazione.Impostazioni.ConfigurazioneGruppo.FirstOrDefault(g => g.Gruppo == DBGruppo).Configurazione.StatoColore.FirstOrDefault(c => c.Stato == 2).Colore),
                    3 => Color.FromArgb(Configurazione.Impostazioni.ConfigurazioneGruppo.FirstOrDefault(g => g.Gruppo == DBGruppo).Configurazione.StatoColore.FirstOrDefault(c => c.Stato == 3).Colore),
                    4 => Color.FromArgb(Configurazione.Impostazioni.ConfigurazioneGruppo.FirstOrDefault(g => g.Gruppo == DBGruppo).Configurazione.StatoColore.FirstOrDefault(c => c.Stato == 4).Colore),
                    5 => Color.FromArgb(Configurazione.Impostazioni.ConfigurazioneGruppo.FirstOrDefault(g => g.Gruppo == DBGruppo).Configurazione.StatoColore.FirstOrDefault(c => c.Stato == 5).Colore),
                    _ => Color.White,
                };
            }
        }

        public string NumeroDocumento { get => PeriodoRifNumeraz + "." + CodSerie + "." + NumRegistraz; } 
        public string RifRigaOrdine { get => NumeroDocumento + "." + NumProgrRiga; }
        public bool Sel { get; set; } 
    }
}
