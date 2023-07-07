using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ParametriConfigurazione
{
    public static class LetturaScritturaParam
    {
        public static ImpostazioniJson LeggiParametri()
        {
            ImpostazioniJson ret = new();

            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Impostazioni.json"))) return ret;

            var strConfig = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Impostazioni.json"));
            return JsonSerializer.Deserialize<ImpostazioniJson>(strConfig, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        }

        public static void ScriviParametri(ImpostazioniJson configurazione)
        {
            var strConfig = JsonSerializer.Serialize(configurazione, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true });

            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Impostazioni.json"), strConfig);
        }

    }
}
