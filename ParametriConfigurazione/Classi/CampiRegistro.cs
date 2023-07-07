using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ParametriConfigurazione.Classi
{
    public class CampiRegistro
    {
        public int Campo { get; set; }
        public string Descrizione { get; set; }
        public string Proprietà { get; set; }

        public CampiRegistro() 
        {

        }

        public static List<CampiRegistro> LeggiCampiRegistri()
        {
            return new()
            {
                new() {Campo = 1, Descrizione = "Bottiglie Totali", Proprietà = "R"},
                new() {Campo = 2, Descrizione = "Bottiglie Parziali", Proprietà = "R"},
                new() {Campo = 3, Descrizione = "Ore Totali", Proprietà = "R"},
                new() {Campo = 4, Descrizione = "Ore Parziali", Proprietà = "R"},
                new() {Campo = 5, Descrizione = "Bottiglie da produrre", Proprietà = "W"},
                new() {Campo = 6, Descrizione = "Ricetta da caricare", Proprietà = "W"},
                new() {Campo = 7, Descrizione = "Altezza Fronte", Proprietà = "W"},
                new() {Campo = 8, Descrizione = "Posizione Fronte", Proprietà = "W"},
                new() {Campo = 9, Descrizione = "Altezza Retro", Proprietà = "W"},
                new() {Campo = 10, Descrizione = "Posizione Retro", Proprietà = "W"} 
            };
        }
    }
}
