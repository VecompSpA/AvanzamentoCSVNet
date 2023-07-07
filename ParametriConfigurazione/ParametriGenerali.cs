using DevExpress.CodeParser;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using FunzioniVecomp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;

namespace ParametriConfigurazione
{
    public partial class ParametriGenerali : DXControls.DXForm
    {
        public ImpostazioniJson Impostazioni;
        public TabImpostazioni TabAvvio = TabImpostazioni.Bottiglie;

        string Gruppo;

        public enum TabImpostazioni
        {
            Bottiglie,
            Fusti,
        }

        public ParametriGenerali(string gruppo)
        {
            InitializeComponent();
            this.Gruppo = gruppo;
        }

        private void ParametriGenerali_Load(object sender, EventArgs e)
        {
            Impostazioni = LetturaScritturaParam.LeggiParametri();
            txtServer.Text = Impostazioni.Impostazioni.ConnessioneSql.Server;
            txtDatabase.Text = Impostazioni.Impostazioni.ConnessioneSql.Database;
            txtUtente.Text = Impostazioni.Impostazioni.ConnessioneSql.Utente;
            if (!string.IsNullOrEmpty(Impostazioni.Impostazioni.ConnessioneSql.Password))
                txtPassword.Text = Aes256Base64Encrypter.Decrypt(Impostazioni.Impostazioni.ConnessioneSql.Password, "GLUGLU");

            txtGruppo.Text = Gruppo;
            txtGruppo.Properties.ReadOnly = true;

            if (string.IsNullOrEmpty(Gruppo))
            {
                txtGruppo.Text = "";
                txtGruppo.Properties.ReadOnly = false;
                return;
            }

            var current = Impostazioni.Impostazioni.ConfigurazioneGruppo.FirstOrDefault(g => g.Gruppo == Gruppo);
            if (current == null)
                return;

            for (int i = 0; i < current.Configurazione.StatoColore.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        ColorPickEdit1.Color = Color.FromArgb(current.Configurazione.StatoColore[i].Colore);
                        break;

                    case 1:
                        ColorPickEdit2.Color = Color.FromArgb(current.Configurazione.StatoColore[i].Colore);
                        break;

                    case 2:
                        ColorPickEdit3.Color = Color.FromArgb(current.Configurazione.StatoColore[i].Colore);
                        break;

                    case 3:
                        ColorPickEdit4.Color = Color.FromArgb(current.Configurazione.StatoColore[i].Colore);
                        break;

                    case 4:
                        ColorPickEdit5.Color = Color.FromArgb(current.Configurazione.StatoColore[i].Colore);
                        break;

                    case 5:
                        ColorPickEdit6.Color = Color.FromArgb(current.Configurazione.StatoColore[i].Colore);
                        break;

                    default:
                        break;
                }

            }

            txtPathInst_bott.Text = Impostazioni.Impostazioni.PathInstallazione;
            txtPathInst_fusti.Text = Impostazioni.Impostazioni.PathInstallazione;
            txtTipoProd_bott.Text = Impostazioni.Impostazioni.TipoProdotto;
            txtTipoProd_fusti.Text = Impostazioni.Impostazioni.TipoProdotto;
            txtlocaleIP.Text = Impostazioni.Impostazioni.LocaleIP;
            txtlocalePORT.Text = Impostazioni.Impostazioni.LocalePort.ToString();

            // bott
            txtPLC_IP_bott.Text = current.Configurazione.Bottiglie.PLCIP;
            txtPLC_Port_bott.Text = current.Configurazione.Bottiglie.PLCPort.ToString();
            txtPathCSV_bott.Text = current.Configurazione.Bottiglie.PathCSV;
            txtPathBAT_bott.Text = current.Configurazione.Bottiglie.PathBAT;
            txtModImp_bott.Text = current.Configurazione.Bottiglie.ModelloAcquisizione.ToString();
            txtFiltroUdM_bott.Text = string.Join(",", current.Configurazione.Bottiglie.FiltroUdm);

            // fusti
            txtPathScambio_fusti.Text = current.Configurazione.Fusti.PathScambio;
            txtPathCSV_fusti.Text = current.Configurazione.Fusti.PathCSV;
            txtPathBAT_fusti.Text = current.Configurazione.Fusti.PathBAT;
            txtModImp_fusti.Text = current.Configurazione.Fusti.ModelloAcquisizione.ToString();
            txtFiltroUdM_fusti.Text = string.Join(",", current.Configurazione.Fusti.FiltroUdm);

            txtpalletIP.Text = current.Configurazione.Fusti.PALLETIP;
            txtpalletPORT.Text = current.Configurazione.Fusti.PALLETPort.ToString();
            txtPLC_IP_fusti.Text = current.Configurazione.Fusti.PLCIP;
            txtPLC_Port_fusti.Text = current.Configurazione.Fusti.PLCPort.ToString();

            if (TabAvvio == TabImpostazioni.Bottiglie)
                XtraTabControl1.SelectedTabPage = XtraTabPageBott;
            else
                XtraTabControl1.SelectedTabPage = XtraTabPageFusti;
        }

        private void DxRibbon1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.Item.Name == "BBSalva")
            {
                Impostazioni.Impostazioni.ConnessioneSql.Server = txtServer.Text;
                Impostazioni.Impostazioni.ConnessioneSql.Database = txtDatabase.Text;
                Impostazioni.Impostazioni.ConnessioneSql.Utente = txtUtente.Text;
                Impostazioni.Impostazioni.ConnessioneSql.Password = !string.IsNullOrEmpty(txtPassword.Text) ? Aes256Base64Encrypter.Encrypt(txtPassword.Text, "GLUGLU") : "";

                Impostazioni.Impostazioni.PathInstallazione = txtPathInst_bott.Text;
                Impostazioni.Impostazioni.TipoProdotto = txtTipoProd_bott.Text;
                Impostazioni.Impostazioni.LocaleIP = txtlocaleIP.Text;
                Impostazioni.Impostazioni.LocalePort = !string.IsNullOrEmpty(txtlocalePORT.Text) ? int.Parse(txtlocalePORT.Text) : 0;

                if (string.IsNullOrEmpty(txtGruppo.Text))
                    return;

                var current = Impostazioni.Impostazioni.ConfigurazioneGruppo.FirstOrDefault(g => g.Gruppo == txtGruppo.Text);
                if (current == null)
                {
                    current = new();
                    current.Gruppo = txtGruppo.Text;
                    Impostazioni.Impostazioni.ConfigurazioneGruppo.Add(current);
                    current = Impostazioni.Impostazioni.ConfigurazioneGruppo.FirstOrDefault(g => g.Gruppo == txtGruppo.Text);
                }

                // bott
                current.Configurazione.Bottiglie.PLCIP = txtPLC_IP_bott.Text;
                current.Configurazione.Bottiglie.PLCPort = !string.IsNullOrEmpty(txtPLC_Port_bott.Text) ? int.Parse(txtPLC_Port_bott.Text) : 0;
                current.Configurazione.Bottiglie.PathCSV = txtPathCSV_bott.Text;
                current.Configurazione.Bottiglie.PathBAT = txtPathBAT_bott.Text;
                current.Configurazione.Bottiglie.ModelloAcquisizione = !string.IsNullOrEmpty(txtModImp_bott.Text) ? int.Parse(txtModImp_bott.Text) : 0;
                current.Configurazione.Bottiglie.FiltroUdm = txtFiltroUdM_bott.Text.Split(',').ToList();

                // fusti
                current.Configurazione.Fusti.PathScambio = txtPathScambio_fusti.Text;
                current.Configurazione.Fusti.PathCSV = txtPathCSV_fusti.Text;
                current.Configurazione.Fusti.PathBAT = txtPathBAT_fusti.Text;
                current.Configurazione.Fusti.ModelloAcquisizione = !string.IsNullOrEmpty(txtModImp_fusti.Text) ? int.Parse(txtModImp_fusti.Text) : 0;
                current.Configurazione.Fusti.FiltroUdm = txtFiltroUdM_fusti.Text.Split(',').ToList();
                current.Configurazione.Fusti.PALLETIP = txtpalletIP.Text;
                current.Configurazione.Fusti.PALLETPort = !string.IsNullOrEmpty(txtpalletPORT.Text) ? int.Parse(txtpalletPORT.Text) : 0;
                current.Configurazione.Fusti.PLCIP = txtPLC_IP_fusti.Text;
                current.Configurazione.Fusti.PLCPort = !string.IsNullOrEmpty(txtPLC_Port_fusti.Text) ? int.Parse(txtPLC_Port_fusti.Text) : 0;

                current.Configurazione.StatoColore.Clear();
                current.Configurazione.StatoColore.Add(new() { Colore = ColorPickEdit1.Color.ToArgb(), Stato = -1 });
                current.Configurazione.StatoColore.Add(new() { Colore = ColorPickEdit2.Color.ToArgb(), Stato = 1 });
                current.Configurazione.StatoColore.Add(new() { Colore = ColorPickEdit3.Color.ToArgb(), Stato = 2 });
                current.Configurazione.StatoColore.Add(new() { Colore = ColorPickEdit4.Color.ToArgb(), Stato = 3 });
                current.Configurazione.StatoColore.Add(new() { Colore = ColorPickEdit5.Color.ToArgb(), Stato = 4 });
                current.Configurazione.StatoColore.Add(new() { Colore = ColorPickEdit6.Color.ToArgb(), Stato = 5 });

                LetturaScritturaParam.ScriviParametri(Impostazioni);
                XtraMessageBox.Show("Salvataggio eseguito", "Salva", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}