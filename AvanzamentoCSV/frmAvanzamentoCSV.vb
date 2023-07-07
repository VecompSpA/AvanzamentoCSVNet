Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel
Imports System.Threading
Imports DevExpress.DataAccess
Imports FunzioniVecomp
Imports System.IO
Imports System.Text
Imports System.Data
Imports SuperSimpleTcp
Imports ParametriConfigurazione.Classi
Imports ParametriConfigurazione
Imports DevExpress.DataAccess.Sql

Public Class frmAvanzamentoCSV

    Dim ListaOrdiniProd As List(Of YOrdiniProduzione)
    Dim ArgomentiLineaCmd As List(Of String)
    Dim OrdineProd As YOrdiniProduzione = Nothing   'rappresenta la riga selezionata (SEL = True)
    'Dim T1 As System.Threading.Thread
    Dim SqlStr As String
    Dim GruppoENO As String
    Dim StazioneENO As String
    'Dim crypt As New Aes256Base64Encrypter()
    Public Impostazioni As ParametriConfigurazione.ImpostazioniJson
    Private CurrentConfigGruppo As ParametriConfigurazione.ConfigurazioneGruppo

    Dim TCPServer As SimpleTcpServer = Nothing
    Dim PalletConnesso As Boolean = False
    Dim StatoPalletProg = 0
    Dim QtaFusti As Double = 0

    Private Sub FrmAvanzamentoCSV_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ApriDB = True
        Me.Prodotto = ProdottoAvvio.ESOLVER
        MyBase.DXForm_Load(sender, e)

        ArgomentiLineaCmd = Environment.GetCommandLineArgs.ToList
        For Each Str As String In ArgomentiLineaCmd
            If Str.Contains("Stazione=") = True Then
                StazioneENO = Str.Substring(Str.LastIndexOf("=") + 1, 2)
            End If
        Next

        Globali.DBGruppo = ""
        GruppoENO = ""

        Impostazioni = ParametriConfigurazione.LetturaScritturaParam.LeggiParametri()

        If Not Impostazioni.Impostazioni.ConnessioneSql.Server Is Nothing Then
            If Impostazioni.Impostazioni.ConnessioneSql.Password <> "" Then
                Globali.DataBaseManager = New DataBaseManager(Impostazioni.Impostazioni.ConnessioneSql.Server, Impostazioni.Impostazioni.ConnessioneSql.Database, Impostazioni.Impostazioni.ConnessioneSql.Utente, Aes256Base64Encrypter.Decrypt(Impostazioni.Impostazioni.ConnessioneSql.Password, "GLUGLU"))
            End If
        End If

        If Globali.DataBaseManager Is Nothing Then
            RichiamaParametri()
        End If

        Globali.DataBaseManager.getConnection.Open()
        GruppoENO = Globali.DataBaseManager.GetValueFromTable("SIStazioni", $"CodWS='{StazioneENO}'", "Gruppo")
        Globali.DBGruppo = GruppoENO

        CurrentConfigGruppo = Impostazioni.Impostazioni.ConfigurazioneGruppo.FirstOrDefault(Function(g) g.Gruppo = Globali.DBGruppo)

        If IsNothing(CurrentConfigGruppo) Then
            RichiamaParametri()
            CurrentConfigGruppo = Impostazioni.Impostazioni.ConfigurazioneGruppo.FirstOrDefault(Function(g) g.Gruppo = Globali.DBGruppo)
        End If

        ParametriConfigurazione.Classi.YOrdiniProduzione.Configurazione = Impostazioni

        CostruisciToolbar()
        Refresh_griglia()

        GridView1.ActiveFilterString = "IsOutlookIntervalYesterday([DataInizioSched]) Or IsOutlookIntervalToday([DataInizioSched])"

        'Avvio la syncro PLC (schedulata ogni 10 secondi)
        Timer1.Start()

        If CurrentConfigGruppo.Configurazione.Fusti.PathCSV.Trim = "" Or CurrentConfigGruppo.Configurazione.Fusti.PathBAT.Trim = "" Then
            BarButtonItemScriviESO.Enabled = False
        End If
        If CurrentConfigGruppo.Configurazione.Fusti.PathCSV.Trim <> "" Then
            If Not System.IO.Directory.Exists(CurrentConfigGruppo.Configurazione.Fusti.PathCSV.Trim) Then
                BarButtonItemScriviESO.Enabled = False
            End If
        End If
        If CurrentConfigGruppo.Configurazione.Fusti.PathBAT.Trim <> "" Then
            If Not System.IO.Directory.Exists(CurrentConfigGruppo.Configurazione.Fusti.PathBAT.Trim) Then
                BarButtonItemScriviESO.Enabled = False
            End If
        End If

        'avvio server tcp se impostato l'indirizzo + porta
        If Impostazioni.Impostazioni.LocaleIP <> "" AndAlso Impostazioni.Impostazioni.LocalePort <> 0 Then
            Try
                TCPServer = New SimpleTcpServer(Impostazioni.Impostazioni.LocaleIP, Impostazioni.Impostazioni.LocalePort)
                TCPServer.Start()
                barInfoPallet.Caption = "Avvio server TCP in corso..."
                AddHandler TCPServer.Events.ClientConnected, AddressOf Events_ClientConnected
                AddHandler TCPServer.Events.ClientDisconnected, AddressOf Events_ClientDisconnected
                AddHandler TCPServer.Events.DataReceived, AddressOf Events_DataReceived
            Catch ex As Exception
                DXControls.DXGestioneErrori.erroreStandard(ex)
            End Try
        End If

    End Sub

    Sub Events_ClientConnected(sender As Object, e As ConnectionEventArgs)
        If e.IpPort.Contains(CurrentConfigGruppo.Configurazione.Fusti.PALLETIP) Then
            Me.Invoke(Sub() barInfoPallet.Caption = $"{e.IpPort}: connesso {Environment.NewLine}")
            Me.Invoke(Sub() barInfoPallet.ItemAppearance.Normal.BackColor = Color.GreenYellow)
            CurrentConfigGruppo.Configurazione.Fusti.PALLETPort = e.IpPort.Split(":")(1)
            PalletConnesso = True
        End If

    End Sub

    Sub Events_ClientDisconnected(sender As Object, e As ConnectionEventArgs)
        Me.Invoke(Sub() barInfoPallet.Caption = $"{e.IpPort}: scollegato {Environment.NewLine}")
        Me.Invoke(Sub() barInfoPallet.ItemAppearance.Normal.BackColor = Color.OrangeRed)
        PalletConnesso = False
    End Sub

    Sub Events_DataReceived(sender As Object, e As DataReceivedEventArgs)
        Dim datiRicevuti As String = Encoding.UTF8.GetString(e.Data).Replace(vbCr, "").Replace(vbLf, "")
        Dim splitDati As String() = datiRicevuti.Split("|")

        If splitDati.Length <> 5 Then
            StatoPalletProg = 0
            QtaFusti = 0
            Me.Invoke(Sub() barInfoPallet2.Caption = $"Dati ricevuti NON coerenti: {datiRicevuti}")
        Else
            StatoPalletProg = splitDati(0)
            Dim NOrdineProd As String = OrdineProd.NumeroDocumento & "." & OrdineProd.NumProgrRiga

            If NOrdineProd <> splitDati(1) Then
                StatoPalletProg = 0
                QtaFusti = 0
                Me.Invoke(Sub() barInfoPallet2.Caption = $"Ordine ricevuto {splitDati(1)} NON coerente con ordine inviato {NOrdineProd}")
            Else
                'se sono qui vuol dire che mi è stato ritornato un esito 2, 3 o 4
                Select Case StatoPalletProg
                    Case 2
                        Me.Invoke(Sub() barInfoPallet2.Caption = $"Ordine {NOrdineProd} <i>confermato</i> da pallettizzatore")
                    Case 3
                        Me.Invoke(Sub() barInfoPallet2.Caption = $"Ordine {NOrdineProd} <i>in corso di esecuzione</i> su pallettizzatore")
                    Case 4
                        Me.Invoke(Sub() barInfoPallet2.Caption = $"Ordine {NOrdineProd} <i>concluso</i> su pallettizzatore - Fusti prodotti <b>{splitDati.Last()}</b>  ")
                        QtaFusti = splitDati.Last()

                        If TCPServer.IsListening Then
                            If PalletConnesso Then
                                Dim StringaInvio As String = datiRicevuti
                                TCPServer.Send($"{CurrentConfigGruppo.Configurazione.Fusti.PALLETIP}:{CurrentConfigGruppo.Configurazione.Fusti.PALLETPort}", StringaInvio + Environment.NewLine)
                                barInfoPallet2.Caption &= " - <b><i>Inviata conferma conclusione a Pallettizzatore con successo</b></i>"
                                StatoPalletProg = 0
                                'barInfoPallet.Caption = ""
                            End If
                        End If

                        Dim NoteInterne As New List(Of String)

                        If OrdineProd.NotaInterna.Split("|").Length = 3 Then
                            NoteInterne.Add(OrdineProd.NotaInterna.Split("|")(0))
                            NoteInterne.Add(OrdineProd.NotaInterna.Split("|")(1))
                            NoteInterne.Add("QTAFUSTI=" & QtaFusti)
                        End If

                        SqlStr = "UPDATE OrdProduzRighe SET Stato = " & OrdineProd.Stato & ", NotaInterna = '" & String.Join("|", NoteInterne.ToArray()) & "'" _
                        & " WHERE DBGruppo = '" & GruppoENO & "' AND IdDocumento = " & OrdineProd.IdDocumento _
                        & " AND IdRiga = " & OrdineProd.IdRiga
                        Globali.DataBaseManager.ExecuteQuery(SqlStr)

                End Select
            End If
        End If

    End Sub

    Sub RichiamaParametri()
        Try
            Dim parametriGen = New ParametriGenerali(GruppoENO)

            parametriGen.TabAvvio = ParametriGenerali.TabImpostazioni.Fusti
            parametriGen.Impostazioni = Impostazioni
            parametriGen.ShowDialog()

            Impostazioni = ParametriConfigurazione.LetturaScritturaParam.LeggiParametri()
            Globali.DataBaseManager = New DataBaseManager(Impostazioni.Impostazioni.ConnessioneSql.Server, Impostazioni.Impostazioni.ConnessioneSql.Database, Impostazioni.Impostazioni.ConnessioneSql.Utente, Aes256Base64Encrypter.Decrypt(Impostazioni.Impostazioni.ConnessioneSql.Password, "GLUGLU"))

        Catch ex As Exception

        Finally
            If Globali.DataBaseManager Is Nothing Then
                End
            Else
                If Globali.DataBaseManager.TestConnection = False Then
                    End
                End If
            End If
        End Try
    End Sub

    Sub CostruisciToolbar()
        RibbonPage1.Groups.Clear()
        DxRibbon1.Pages.Remove(RibbonPage1)

        Me.DxRibbon1.Pages("Azioni").Groups.Insert(2, RibbonPageGroupImpostazioni)
        Me.DxRibbon1.Pages("Azioni").Groups.Insert(3, RibbonPageGroupPLC)
        Me.DxRibbon1.Pages("Azioni").Groups.Insert(4, RibbonPageGroupAltro)
    End Sub

    Sub Refresh_griglia()
        Dim NoteInterne As New List(Of String)

        Dim query = $"Select * From Y{Globali.DBGruppo}OrdiniProduzione Where UnitaMisura IN @UnitaMisura"
        ListaOrdiniProd = DataAccess.DataDapper.ExecQueryRet(Of YOrdiniProduzione, Object)(query, New With {Key .UnitaMisura = CurrentConfigGruppo.Configurazione.Fusti.FiltroUdm})

        OrdineProd = ListaOrdiniProd.FirstOrDefault(Function(o) o.Stato = -1 Or o.Stato = 2)
        If Not IsNothing(OrdineProd) Then
            If OrdineProd.NotaInterna.Trim <> "" Then

                For Each stringa As String In NoteInterne
                    If stringa.Contains("QTAPROD=") Then
                        OrdineProd.QuantitaProdotta = Convert.ToDouble(stringa.Replace("QTAPROD=", ""))
                        If OrdineProd.Capacita > 0 Then
                            OrdineProd.QuantitaProdotta2 = Math.Ceiling(OrdineProd.QuantitaProdotta2 * OrdineProd.Capacita)
                        Else
                            OrdineProd.QuantitaProdotta2 = OrdineProd.QuantitaProdotta
                        End If

                    End If
                    If stringa.Contains("ID=") Then
                        OrdineProd.NotaInterna = stringa.Replace("ID=", "")
                        NoteInterne.Add(stringa)
                    End If
                    If stringa.Contains("QTAFUSTI=") Then
                        'OrdineProd.NotaInterna = stringa.Replace("ID=", "")
                        'NoteInterne.Add(stringa)
                        QtaFusti = stringa.Replace("QTAFUSTI=", "")
                    End If
                Next
            End If
            OrdineProd.Sel = True
            'BarButtonAvviaProd.Enabled = False
            GestioneAbilitazioneToolbar()
            GridView1.Columns("Sel").OptionsColumn.AllowEdit = False
            GridView1.Columns("Sel").AppearanceCell.BackColor = Color.LightGray
        Else
            GridView1.Columns("Sel").OptionsColumn.AllowEdit = True
            GridView1.Columns("Sel").AppearanceCell.BackColor = Nothing
        End If
        DxGrid1.DataSource = ListaOrdiniProd
    End Sub

#Region " PULSANTI TOOLBAR "

    Private Sub BarButtonItemUpdateGrid_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItemUpdateGrid.ItemClick
        Refresh_griglia()
        'RepositoryItemCheckEdit1_CheckedChanged(New Object, New EventArgs)
    End Sub

    Private Sub BarButtonItemScriviESO_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItemScriviESO.ItemClick
        Try
            If IsNothing(OrdineProd) Then
                Dim ListOrdiniProd As List(Of YOrdiniProduzione) = DxGrid1.DataSource
                OrdineProd = ListOrdiniProd.FirstOrDefault(Function(o) o.Sel = True)
                If OrdineProd.Stato <> -1 Then
                    Exit Sub
                End If
            End If

            Dim msg As New XtraInputBoxArgs()
            Dim txt As New DXControls.DXTextEdit

            txt.Properties.Mask.EditMask = "d"
            txt.Properties.Mask.MaskType = Mask.MaskType.Numeric
            txt.Properties.Mask.UseMaskAsDisplayFormat = True

            msg.Editor = txt
            msg.Caption = "Conferma fusti prodotti"
            msg.Prompt = $"Avanzare il documento {OrdineProd.NumeroDocumento} riga {OrdineProd.IdRiga}? {Environment.NewLine}Conferma fusti prodotti"
            msg.DefaultResponse = OrdineProd.QuantitaProdotta

            Dim result = XtraInputBox.Show(msg)

            If Not IsNumeric(result) Then
                Exit Sub
            Else
                OrdineProd.QuantitaProdotta = result
                If OrdineProd.Capacita > 0 Then
                    OrdineProd.QuantitaProdotta2 = Math.Ceiling(OrdineProd.QuantitaProdotta * OrdineProd.Capacita)
                Else
                    OrdineProd.QuantitaProdotta2 = OrdineProd.QuantitaProdotta
                End If
            End If

            'If XtraMessageBox.Show("Avanzare il documento " & OrdineProd.NumeroDocumento & " riga " & OrdineProd.IdRiga & "?", "Avanzamento Produzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            '    Exit Sub
            'End If

            If Not CurrentConfigGruppo.Configurazione.Fusti.PathCSV.EndsWith("\") Then
                CurrentConfigGruppo.Configurazione.Fusti.PathCSV = CurrentConfigGruppo.Configurazione.Fusti.PathCSV & "\"
            End If
            If Not CurrentConfigGruppo.Configurazione.Fusti.PathBAT.EndsWith("\") Then
                CurrentConfigGruppo.Configurazione.Fusti.PathBAT = CurrentConfigGruppo.Configurazione.Fusti.PathBAT & "\"
            End If

            Dim PercorsoFileCSV As String = CurrentConfigGruppo.Configurazione.Fusti.PathCSV
            Dim PercorsoShell As String = CurrentConfigGruppo.Configurazione.Fusti.PathBAT & "F_SW_Produzione.bat"
            Dim NomeFileCSV As String = ""

            Dim txtfileBATCH As String = ""
            Dim DrivePath As String = System.IO.Path.GetPathRoot(Impostazioni.Impostazioni.PathInstallazione)

            DrivePath = DrivePath.Substring(0, DrivePath.Length - 1)

            If File.Exists(PercorsoShell) Then
                File.Delete(PercorsoShell)
            End If

            'If Not System.IO.File.Exists(PercorsoShell) Then
            txtfileBATCH = "@ECHO OFF" & vbNewLine
            txtfileBATCH &= DrivePath & vbNewLine
            txtfileBATCH &= "CD " & Impostazioni.Impostazioni.PathInstallazione & vbNewLine
            txtfileBATCH &= "If EXIST " & PercorsoFileCSV & "F_PROD_*.CSV  " & IO.Path.Combine(Impostazioni.Impostazioni.PathInstallazione, "PROG32\EACQDATW") & " EG079 /MODIMP=" & CurrentConfigGruppo.Configurazione.Fusti.ModelloAcquisizione & " /CREAANA=1 /VISESITO=0 -GR=" & GruppoENO & " -EXM=1 -PHB=" & Impostazioni.Impostazioni.PathInstallazione & " -PR=" & Impostazioni.Impostazioni.TipoProdotto & " -PRP=" & Impostazioni.Impostazioni.TipoProdotto & " -WS=A1 -WSP=IM -O=ADMIN" & vbNewLine
            txtfileBATCH &= "EXIT"

            Try
                Dim filebatch As New System.IO.StreamWriter(PercorsoShell)
                filebatch.Write(txtfileBATCH)
                filebatch.Close()
                filebatch.Dispose()
            Catch ex As Exception
                XtraMessageBox.Show(ex.Message)
                Exit Sub
            End Try
            'End If

            DXSSM.ShowWaitForm()
            DXSSM.SetWaitFormDescription("Creazione file CSV")

            Dim DT As DataTable = DatiAcquisizione()

            NomeFileCSV = "F_PROD_" & GruppoENO & "_" & OrdineProd.IdDocumento & "_" & Format(OrdineProd.DataRegistrazione, "yyyyMMdd") & "_" & OrdineProd.NumRegistraz & "_" & OrdineProd.IdRiga & ".csv"

            Funzioni.Export_table(DT, PercorsoFileCSV & NomeFileCSV, "|")

            '3 step - lancio il bat di importazione dati
            DXSSM.SetWaitFormDescription("Lancio acquisizione dati")
            Dim p As New Process
            p.StartInfo.FileName = PercorsoShell
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            p.StartInfo.CreateNoWindow = True
            p.StartInfo.UseShellExecute = False
            p.Start()
            p.WaitForExit()

            '4 step - controllo i documenti in esolver/enologia
            DXSSM.SetWaitFormDescription("Controllo gli esiti")
            ''Dim FRM As New EsitoEsportazione
            ''For Each Fattura As FattureSelezionate In lstFattureSelezionate
            ''    checkFTE(Fattura)

            ''    FRM.ListBoxControl1.Items.Add("Fattura: " & Fattura.TipoFat & " - " & Fattura.DataFat & " - " & Fattura.NumFat & ":  " & Fattura.LOG)
            ''Next
            Dim Esito As Integer = EsitoExport(NomeFileCSV)
            Dim Messaggio As String = ""

            Select Case Esito
                Case 0 'NON ANCORA ESEGUITO

                Case 1 'ESPORTATO CORRETTAMENTE
                    Messaggio = "Avanzamento acquisito correttamente"
                Case 2 'ESPORTATO CON AVVISI
                    Messaggio = "Avanzamento acquisito  con segnalazioni"
                Case 3 'NON ESPORTATO CON AVVISI
                    Messaggio = "Avanzamento NON acquisito con segnalazioni"
                Case 4 'NON ESPORTATO
                    Messaggio = "Avanzamento NON acquisito"
            End Select

            Thread.Sleep(2000)
            DXSSM.CloseWaitForm()

            If Esito = 1 Or Esito = 2 Then
                'BarButtonAvviaProd.Enabled = True
                OrdineProd.NotaInterna = QtaFusti '""
                SqlStr = "UPDATE OrdProduzRighe SET Stato = 4, NotaInterna = '" & QtaFusti & "'" _
                                & " WHERE DBGruppo = '" & GruppoENO & "' AND IdDocumento = " & OrdineProd.IdDocumento _
                                & " AND IdRiga = " & OrdineProd.IdRiga
                Globali.DataBaseManager.ExecuteQuery(SqlStr)
                OrdineProd = Nothing
                GridView1.Columns("Sel").OptionsColumn.AllowEdit = True
                GridView1.Columns("Sel").AppearanceCell.BackColor = Nothing
                Timer1.Stop()
                Refresh_griglia()
            Else
                If Esito = 4 Then
                    XtraMessageBox.Show("Avanzamento non riuscito. Consultare il log di ENOLOGIA", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    'BarButtonAvviaProd.Enabled = True
                    OrdineProd = Nothing
                    GridView1.Columns("Sel").OptionsColumn.AllowEdit = True
                    GridView1.Columns("Sel").AppearanceCell.BackColor = Nothing
                    Refresh_griglia()
                End If
            End If

            StatoPalletProg = 0
            QtaFusti = 0
            'barInfoPallet.Caption = ""
            barInfoPallet2.Caption = ""

            GestioneAbilitazioneToolbar()

        Catch ex As Exception
            If DXSSM.IsSplashFormVisible Then
                DXSSM.CloseWaitForm()
            End If
            XtraMessageBox.Show(ex.Message)
        End Try
    End Sub

    Function DatiAcquisizione() As DataTable
        Dim DT As New DataTable
        Dim DC As New DataColumn("TipoRecord", System.Type.GetType("System.String"))
        DT.Columns.Add(DC)
        DC = New DataColumn("DataRegistrazione", System.Type.GetType("System.DateTime"))
        DT.Columns.Add(DC)
        DC = New DataColumn("NumRegistrazione", System.Type.GetType("System.Int32"))
        DT.Columns.Add(DC)
        DC = New DataColumn("RifRigaOrdine", System.Type.GetType("System.String"))
        DT.Columns.Add(DC)
        'DC = New DataColumn("CodiceArt", System.Type.GetType("System.String"))
        'DT.Columns.Add(DC)
        'DC = New DataColumn("Variante", System.Type.GetType("System.String"))
        'DT.Columns.Add(DC)
        DC = New DataColumn("Tipo", System.Type.GetType("System.String"))
        DT.Columns.Add(DC)
        DC = New DataColumn("Quantita", System.Type.GetType("System.Double"))
        DT.Columns.Add(DC)
        DC = New DataColumn("Quantita2", System.Type.GetType("System.Double"))
        DT.Columns.Add(DC)
        DC = New DataColumn("Lotto", System.Type.GetType("System.String"))
        DT.Columns.Add(DC)
        'DC = New DataColumn("RigaSaldata", System.Type.GetType("System.Int32"))
        'DT.Columns.Add(DC)
        'DC = New DataColumn("RiferimentoLotto", System.Type.GetType("System.String"))
        'DT.Columns.Add(DC)
        'DC = New DataColumn("DataOriginale", System.Type.GetType("System.DateTime"))
        'DT.Columns.Add(DC)
        'DC = New DataColumn("NumDocOriginale", System.Type.GetType("System.Int32"))
        'DT.Columns.Add(DC)

        Dim DR As DataRow = DT.NewRow()
        DR("TipoRecord") = "TES"
        DR("DataRegistrazione") = Now.Date
        DR("NumRegistrazione") = 1
        DR("RifRigaOrdine") = OrdineProd.RifRigaOrdine
        'DR("CodiceArt") = OrdineProd.CodArt
        'DR("Variante") = OrdineProd.VarianteArt
        DR("Tipo") = "QTA"
        'DR("Quantita") = OrdineProd.QuantitaProdotta
        DR("Quantita") = OrdineProd.QuantitaProdotta
        DR("Quantita2") = OrdineProd.QuantitaProdotta2
        DR("Lotto") = OrdineProd.RifLottoAlfanum
        'DR("RigaSaldata") = 1
        'DR("RiferimentoLotto") = OrdineProd.RifLottoAlfanum
        'DR("DataOriginale") = OrdineProd.DataRegistrazione
        'DR("NumDocOriginale") = OrdineProd.NumRegistraz
        DT.Rows.Add(DR)

        'DR = DT.NewRow
        'DR("TipoRecord") = "RIG"
        'DR("DataRegistrazione") = Now.Date
        'DR("NumRegistrazione") = 1
        'DR("RifRigaOrdine") = OrdineProd.RifRigaOrdine
        ''DR("CodiceArt") = OrdineProd.CodArt
        ''DR("Variante") = OrdineProd.VarianteArt
        'DR("Tipo") = "ORE"
        'DR("Quantita") = OrdineProd.QuantitaProdotta_HH
        'DR("Quantita2") = OrdineProd.QuantitaProdotta_HH
        ''DR("RigaSaldata") = 1
        ''DR("RiferimentoLotto") = OrdineProd.RifLottoAlfanum
        ''DR("DataOriginale") = OrdineProd.DataRegistrazione
        ''DR("NumDocOriginale") = OrdineProd.NumRegistraz
        'DT.Rows.Add(DR)

        Return DT
    End Function

#End Region


#Region " GESTIONE SELEZIONE RIGHE "

    Private Sub RepositoryItemCheckEdit1_CheckedChanged(sender As Object, e As EventArgs) Handles RepositoryItemCheckEdit1.CheckedChanged
        Dim c As CheckEdit = sender
        'Dim l As List(Of cls_YOrdiniProduzione) = DxGrid1.DataSource
        Dim r As YOrdiniProduzione
        r = GridView1.GetFocusedRow

        If c.Checked Then
            If r.Stato = 4 Then
                c.Checked = False
                GestioneAbilitazioneToolbar()
                Exit Sub
            End If
            Dim l As List(Of YOrdiniProduzione) = DxGrid1.DataSource

            'If Not r.Stato = 4 And Not r.Stato = -1 Then
            For Each r In l
                r.Sel = False
            Next
            r = GridView1.GetFocusedRow
            r.Sel = True
            OrdineProd = r
            GridView1.RefreshData()
        Else
            OrdineProd = Nothing
            r.Sel = False
            GridView1.RefreshData()
        End If
        GestioneAbilitazioneToolbar()
    End Sub

#End Region

#Region " COLORAZIONE RIGHE "

    Private Sub GridView1_RowStyle(sender As Object, e As RowStyleEventArgs) Handles GridView1.RowStyle
        If e.RowHandle >= 0 Then
            Dim r As YOrdiniProduzione
            r = GridView1.GetRow(e.RowHandle)
            e.Appearance.BackColor = r.ColoreStato
            If r.Stato = -1 Then
                e.Appearance.BackColor2 = r.ColoreStato
            End If
        End If
    End Sub

#End Region

#Region " SUB DI LETTURA DA ESOLVER "

    Function EsitoExport(ByVal NomeFileExp As String) As Integer
        Dim Risultato As Integer = 0

        SqlStr = "SELECT TOP 1 T1.IdImportazione AS [IDImportazione], T1.GruppoDoc AS [GruppoDocumento], 
                        T1.Des AS [Descrizione], T1.NomeFormato As [Formato], T1.StatoImportazione As [Stato], 
                        T1.GeneraFileMovNonAcq AS [GeneraScarti], T1.FirmaUltVarData As [UltimaModifica], 
                        T1.FirmaUltVarOperatore AS [UltimoOperatore], T1.FirmaCreazData As [DataCreazione], 
                        T1.EseguiImportazione AS [SimulDef], T2.Esito As [Esito], T2.NumProgrFile As [NumProgressivo], 
                        T2.EntitaLette AS [Letti], T2.EntitaImportate As [Importati], T2.EntitaImpAnomalie As [ImportAnomalie], 
                        T2.EntitaNonImportate AS [NonImportati], T2.DataInizioImport As [DataInizio], T2.DataFineImport As [DataFine], 
                        T3.Des AS [Des1],
                        CASE WHEN T1.StatoImportazione<>1 THEN T1.NomeArchivio WHEN T1.StatoImportazione=1 THEN ISNULL(T2.NomeArchImp,'') ELSE '' END AS [File], 
                        ROUND(ISNULL(T2.OraInizioImportaz, 0) / 100, 0, 1) As [OraInizio], ROUND(ISNULL(T2.OraFineImportaz, 0) / 100, 0, 1) AS [OraFine] 
                    FROM ImportazioniDoc As [T1] LEFT OUTER Join ImpLogEsecuzioni As [T2] On (T2.CategImportazione = 2 And T2.IdImportazione = T1.IdImportazione And (T2.DBGruppo = T1.DBGruppo)) LEFT OUTER Join GruppiDoc As [T3] On (T3.GruppoDoc = T1.GruppoDoc)
                    WHERE ((T1.ModelloImpBatch = 0) And (T1.StatoImportazione <> 2)) And (T1.DBGruppo ='" & GruppoENO & "')
                        And CASE WHEN T1.StatoImportazione<>1 THEN T1.NomeArchivio WHEN T1.StatoImportazione=1 THEN ISNULL(T2.NomeArchImp,'') ELSE '' END = '" & NomeFileExp & "'
                        Order By T1.FirmaCreazData DESC, ROUND(ISNULL(T2.OraInizioImportaz, 0) / 100, 0, 1) DESC, T2.Esito "
        Dim DT As DataTable = Globali.DataBaseManager.GetQueryTableManager(SqlStr).Table
        If DT.Rows.Count > 0 Then
            Risultato = DT.Rows(0)("Esito")
        End If

        Return Risultato
    End Function

    Function LeggiRicetta(ByVal CodiceArticolo As String) As Integer
        Dim NRicetta As Integer

        Dim SqlStr As String = "SELECT T1.NumeroProgrVisualizz AS [NumeroProgrVisualizz], T1.CampoObbligatorio AS [CampoObbligatorio], T2.CodiceCampoUtente AS [CodiceCampoUtente], 
                                        T2.Descrizione AS [Descrizione], T2.Posizione AS [Posizione], T2.TipoDato AS [TipoDato], T2.LunghezzaMassima AS [LunghezzaMassima], T2.NumeroDecimali AS [NumeroDecimali], 
                                        T3.ValoreCampo20 AS [ValoreCampo20]
                                FROM CampiUtenteAbil AS [T1] INNER JOIN CampiUtenteAnagr AS [T2] ON (T2.CodiceCampoUtente=T1.CodiceCampoUtente AND (T2.DBGruppo=T1.DBGruppo)) LEFT OUTER JOIN 
                                    CampiUtenteTabValori AS [T3] ON (T3.CodArt='" & CodiceArticolo.Replace("'", "''") & "' AND T3.VarianteArt='' AND (T3.DBGruppo=T1.DBGruppo)) 
                                WHERE ((T1.AmbitoUtilizzo='ART')) AND (T1.DBGruppo='" & GruppoENO & "')
                                ORDER BY [NumeroProgrVisualizz] ASC"
        Dim DT As DataTable = Globali.DataBaseManager.GetQueryTableManager(SqlStr).Table
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("ValoreCampo20") <> "" Then
                NRicetta = DT.Rows(0)("ValoreCampo20")
            End If
        End If
        DT = Nothing

        Return NRicetta
    End Function

    Function LeggiRicettaOrP(ByVal IdDocumento As Integer, ByVal IdRigaDoc As Integer)
        Dim NRicetta As Integer
        SqlStr = "SELECT ValoreCampo20 FROM CampiUtenteTabValori WHERE DBGruppo = '" & GruppoENO & "' AND IdDocumento = " & IdDocumento & " AND IdRigaDoc = " & IdRigaDoc
        Dim DT As DataTable = Globali.DataBaseManager.GetQueryTableManager(SqlStr).Table
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("ValoreCampo20") <> "" Then
                NRicetta = DT.Rows(0)("ValoreCampo20")
            End If
        End If
        DT = Nothing

        Return NRicetta
    End Function

    Function LeggiAltezzaFronteOrP(ByVal IdDocumento As Integer, ByVal IdRigaDoc As Integer)
        Dim AltezzaFronte As Integer
        SqlStr = "SELECT ValoreCampo1 FROM CampiUtenteTabValori WHERE DBGruppo = '" & GruppoENO & "' AND IdDocumento = " & IdDocumento & " AND IdRigaDoc = " & IdRigaDoc
        Dim DT As DataTable = Globali.DataBaseManager.GetQueryTableManager(SqlStr).Table
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("ValoreCampo1") <> "" Then
                AltezzaFronte = DT.Rows(0)("ValoreCampo1")
            End If
        End If
        DT = Nothing

        Return AltezzaFronte
    End Function

    Function LeggiPosizioneFronteOrP(ByVal IdDocumento As Integer, ByVal IdRigaDoc As Integer)
        Dim PosizioneFronte As Integer
        SqlStr = "SELECT ValoreCampo3 FROM CampiUtenteTabValori WHERE DBGruppo = '" & GruppoENO & "' AND IdDocumento = " & IdDocumento & " AND IdRigaDoc = " & IdRigaDoc
        Dim DT As DataTable = Globali.DataBaseManager.GetQueryTableManager(SqlStr).Table
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("ValoreCampo3") <> "" Then
                PosizioneFronte = DT.Rows(0)("ValoreCampo3")
            End If
        End If
        DT = Nothing

        Return PosizioneFronte
    End Function

    Function LeggiAltezzaRetroOrP(ByVal IdDocumento As Integer, ByVal IdRigaDoc As Integer)
        Dim AltezzaRetro As Integer
        SqlStr = "SELECT ValoreCampo4 FROM CampiUtenteTabValori WHERE DBGruppo = '" & GruppoENO & "' AND IdDocumento = " & IdDocumento & " AND IdRigaDoc = " & IdRigaDoc
        Dim DT As DataTable = Globali.DataBaseManager.GetQueryTableManager(SqlStr).Table
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("ValoreCampo4") <> "" Then
                AltezzaRetro = DT.Rows(0)("ValoreCampo4")
            End If
        End If
        DT = Nothing

        Return AltezzaRetro
    End Function

    Function LeggiPosizioneRetroOrP(ByVal IdDocumento As Integer, ByVal IdRigaDoc As Integer)
        Dim PosizioneRetro As Integer
        SqlStr = "SELECT ValoreCampo5 FROM CampiUtenteTabValori WHERE DBGruppo = '" & GruppoENO & "' AND IdDocumento = " & IdDocumento & " AND IdRigaDoc = " & IdRigaDoc
        Dim DT As DataTable = Globali.DataBaseManager.GetQueryTableManager(SqlStr).Table
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("ValoreCampo5") <> "" Then
                PosizioneRetro = DT.Rows(0)("ValoreCampo5")
            End If
        End If
        DT = Nothing

        Return PosizioneRetro
    End Function

    Function LeggiPalletNumeroProgramma(ByVal IdDocumento As Integer, ByVal IdRigaDoc As Integer)
        Dim Var As Integer
        SqlStr = "SELECT ValoreCampo25 FROM CampiUtenteTabValori WHERE DBGruppo = '" & GruppoENO & "' AND IdDocumento = " & IdDocumento & " AND IdRigaDoc = " & IdRigaDoc
        Dim DT As DataTable = Globali.DataBaseManager.GetQueryTableManager(SqlStr).Table
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("ValoreCampo25") <> "" Then
                Var = DT.Rows(0)("ValoreCampo25")
            End If
        End If
        DT = Nothing

        Return Var
    End Function

    Function LeggiPalletStrati(ByVal IdDocumento As Integer, ByVal IdRigaDoc As Integer)
        Dim Var As Integer
        SqlStr = "SELECT ValoreCampo26 FROM CampiUtenteTabValori WHERE DBGruppo = '" & GruppoENO & "' AND IdDocumento = " & IdDocumento & " AND IdRigaDoc = " & IdRigaDoc
        Dim DT As DataTable = Globali.DataBaseManager.GetQueryTableManager(SqlStr).Table
        If DT.Rows.Count > 0 Then
            If DT.Rows(0)("ValoreCampo26") <> "" Then
                Var = DT.Rows(0)("ValoreCampo26")
            End If
        End If
        DT = Nothing

        Return Var
    End Function

#End Region

    Sub LiberaOrdine()
        'OrdineProd.Sel = False
        GridView1.RefreshData()
        GestioneAbilitazioneToolbar()
        'OrdineProd = Nothing
        'fino a quando non avanzano l'ordine non possono lanciare un'altra produzione
        'BarButtonAvviaProd.Enabled = True
    End Sub

    Private Sub BarButtonItemGridColor_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonItemParamGen.ItemClick
        Try
            Dim parametriGen = New ParametriGenerali(GruppoENO)

            parametriGen.TabAvvio = ParametriGenerali.TabImpostazioni.Fusti
            parametriGen.Impostazioni = Impostazioni
            parametriGen.ShowDialog()

            Impostazioni = ParametriConfigurazione.LetturaScritturaParam.LeggiParametri()
            Globali.DataBaseManager = New DataBaseManager(Impostazioni.Impostazioni.ConnessioneSql.Server, Impostazioni.Impostazioni.ConnessioneSql.Database, Impostazioni.Impostazioni.ConnessioneSql.Utente, Aes256Base64Encrypter.Decrypt(Impostazioni.Impostazioni.ConnessioneSql.Password, "GLUGLU"))

        Catch ex As Exception

        Finally
            If Globali.DataBaseManager Is Nothing Then
                End
            Else
                If Globali.DataBaseManager.TestConnection = False Then
                    End
                End If
            End If
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        LeggiFile()
        Timer1.Start()

    End Sub

    Sub GestioneAbilitazioneToolbar()


        If IsNothing(OrdineProd) Then
            Dim ListOrdiniProd As List(Of YOrdiniProduzione) = DxGrid1.DataSource
            OrdineProd = ListOrdiniProd.Find(Function(p) p.Sel = True)
        End If

        If Not IsNothing(OrdineProd) Then
            If OrdineProd.Sel = True Then
                If OrdineProd.Stato <> 2 And OrdineProd.Stato <> -1 Then
                    BarButtonAvviaProd.Enabled = True
                Else
                    BarButtonAvviaProd.Enabled = False
                End If

                If OrdineProd.Stato = 2 Then '-1 Then
                    BarButtonItemScriviESO.Enabled = True
                Else
                    BarButtonItemScriviESO.Enabled = False
                End If

            Else
                BarButtonAvviaProd.Enabled = False
                BarButtonItemScriviESO.Enabled = False
            End If
        Else
            BarButtonAvviaProd.Enabled = False
            BarButtonItemScriviESO.Enabled = False
        End If

    End Sub

    Private Sub BarButtonAvviaProd_ItemClick(sender As Object, e As ItemClickEventArgs) Handles BarButtonAvviaProd.ItemClick

        CreaFileInvioTcp()
        StampaEtichette()
        Timer1.Start()

    End Sub

    Sub CreaFileInvioTcp()
        'Exit Sub

        Dim NOrdineProd As String = OrdineProd.NumeroDocumento & "." & OrdineProd.NumProgrRiga
        Dim NoteInterne As New List(Of String)

        Dim fileIO As New IO.StreamWriter(IO.Path.Combine(CurrentConfigGruppo.Configurazione.Fusti.PathScambio, "ORDINEPRODUZIONE.CSV"))
        Dim sGuid As String = Guid.NewGuid.ToString

        fileIO.WriteLine("List separator=;Decimal symbol=,")
        fileIO.WriteLine("DATI_GESTIONALE DATI_GESTIONALE DATI_GESTIONALE DATI_GESTIONALE ;")
        fileIO.WriteLine("LANGID_410;" & NOrdineProd)
        fileIO.WriteLine("LANGID_40c;" & NOrdineProd)
        fileIO.WriteLine("LANGID_809;" & NOrdineProd)
        fileIO.WriteLine("LANGID_419;" & NOrdineProd)
        fileIO.WriteLine("6;1")
        fileIO.WriteLine("DB_Dati_Operatore_CLIENTE;PRODUZIONE INTERNA")
        fileIO.WriteLine("DB_Dati_Operatore_LOTTO;" & OrdineProd.RifLottoAlfanum)
        fileIO.WriteLine("DB_Dati_Operatore_PRODOTTO;" & OrdineProd.Des)
        fileIO.WriteLine("DB_Dati_Operatore_FORMATO;" & OrdineProd.QtaUmSecondaria / OrdineProd.Quantita)
        fileIO.WriteLine("DB_Dati_Operatore_OPERATORE;Operatore")
        fileIO.WriteLine("DB_Dati_Operatore_SET_FustiDaProdurre;" & OrdineProd.Quantita)
        fileIO.WriteLine("DB_Dati_Operatore_EnableStopAuto;0")
        fileIO.WriteLine("DB_Dati_Operatore_NOTE1;")
        fileIO.WriteLine("DB_Dati_Operatore_NOTE2;" & sGuid)

        ''commentato per test
        OrdineProd.NotaInterna = $"ID={sGuid}|QTAPRODOTTA=0|QTAFUSTI=0"
        OrdineProd.Stato = 2

        NoteInterne.Add("ID=" & sGuid)
        NoteInterne.Add("QTAPRODOTTA=0")
        NoteInterne.Add("QTAFUSTI=0")

        SqlStr = "UPDATE OrdProduzRighe SET Stato = " & OrdineProd.Stato & ", NotaInterna = '" & String.Join("|", NoteInterne.ToArray()) & "'" _
                        & " WHERE DBGruppo = '" & GruppoENO & "' AND IdDocumento = " & OrdineProd.IdDocumento _
                        & " AND IdRiga = " & OrdineProd.IdRiga
        Globali.DataBaseManager.ExecuteQuery(SqlStr)

        If TCPServer.IsListening Then
            If PalletConnesso Then
                Dim StringaInvio As String = $"1|{NOrdineProd}|{LeggiPalletNumeroProgramma(OrdineProd.IdDocumento, OrdineProd.IdRiga)}|{LeggiPalletStrati(OrdineProd.IdDocumento, OrdineProd.IdRiga)}|{OrdineProd.Quantita}"
                TCPServer.Send($"{CurrentConfigGruppo.Configurazione.Fusti.PALLETIP}:{CurrentConfigGruppo.Configurazione.Fusti.PALLETPort}", StringaInvio + Environment.NewLine)
                barInfoPallet2.Caption = "<b><i>Inviato a Pallettizzatore con successo</b></i>"
                StatoPalletProg = 1
            End If
        End If

        GridView1.RefreshData()
        GestioneAbilitazioneToolbar()

        GridView1.Columns("Sel").OptionsColumn.AllowEdit = False
        GridView1.Columns("Sel").AppearanceCell.BackColor = Color.LightGray

        fileIO.Close()

    End Sub

    Sub LeggiFile()

        'Exit Sub

        Dim inStream As IO.FileStream = Nothing
        Dim sr As IO.StreamReader = Nothing
        Dim Contenuto As String = ""
        Dim DT As New DataTable
        Dim DR As DataRow = Nothing
        Dim DRR As DataRow() = Nothing
        Dim Lines As List(Of String)
        Dim FustiProd As Long = 0
        Dim NoteInterne As New List(Of String)

        If IsNothing(OrdineProd) Then
            Exit Sub
        End If
        'BarButtonItemScriviESO.Enabled = True
        For Each file As String In IO.Directory.GetFiles(CurrentConfigGruppo.Configurazione.Fusti.PathScambio, "20_REPORT_PRODUZIONE_*.CSV")
            inStream = New IO.FileStream(file, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
            sr = New IO.StreamReader(inStream, System.Text.Encoding.Default)
            Contenuto &= sr.ReadToEnd
        Next

        If Contenuto <> "" Then
            Lines = Contenuto.Split(vbNewLine).ToList
            For Each col As String In Lines.First.Split(";")
                DT.Columns.Add(col.Replace("""", ""))
            Next
            DT.Columns(0).DataType = GetType(Long)

            Dim tmpArray As List(Of String)

            Lines.RemoveAt(0)
            For Each line As String In Lines

                tmpArray = line.Split(";").ToList()
                If tmpArray.Count > 1 AndAlso IsNumeric(tmpArray.First()) Then
                    DR = DT.NewRow
                    For i As Integer = 0 To tmpArray.Count - 1
                        If i = 0 Then
                            If line.IndexOf(",") > 0 Then
                                DR(i) = tmpArray(i).Substring(0, line.IndexOf(",")).Replace(vbCr, "").Replace(vbLf, "")
                            Else
                                DR(i) = tmpArray(i).Replace(vbCr, "").Replace(vbLf, "")
                            End If

                        Else
                            'If IsNumeric(tmpArray.First) Then
                            DR(i) = tmpArray(i)
                            'DT.Rows.Add(DR)
                            'Else
                            '    DR = Nothing
                            'End If
                        End If
                    Next
                    'If Not IsNothing(DR) Then
                    DT.Rows.Add(DR)
                    'End If
                End If
            Next

            'Funzioni.DataTable2Table(DT, "yprodfusti", False)

            If DT.Select("StateAfter = '1'").Length > 0 Then
                DT = DT.Select("StateAfter = '1'").CopyToDataTable
                If DT.Select("MsgNumber = '747' AND Var1 = '""" & OrdineProd.NotaInterna.Split("|")(0).Replace("ID=", "") & """'").Length > 0 Then
                    DRR = DT.Select("MsgNumber = '747' AND Var1 = '""" & OrdineProd.NotaInterna.Split("|")(0).Replace("ID=", "") & """'")
                    If DT.Select("MsgNumber = '742' AND Time_ms  >= " & DRR(0)("Time_ms") & "").Length > 0 Then
                        Dim Dt_tmp = DT.Select("MsgNumber = '742' AND Time_ms  >= " & DRR(0)("Time_ms") & "").CopyToDataTable()
                        Dim Dw_tmp = Dt_tmp.DefaultView
                        Dw_tmp.Sort = "Time_ms desc"
                        Dt_tmp = Dw_tmp.ToTable()
                        FustiProd = Dt_tmp.Rows(0)("Var1").ToString().Replace("""", "")
                        'FustiProd = DT.Select("MsgNumber = '742' AND Time_ms  <= '" & DRR(0)("Time_ms") & "'").OrderByDescending(Function(d) d("Time_ms"))(0)("Var1")
                        OrdineProd.QuantitaProdotta = FustiProd
                        OrdineProd.QuantitaProdotta2 = FustiProd * (OrdineProd.QtaUmSecondaria / OrdineProd.Quantita)

                        NoteInterne.Add("ID=" & OrdineProd.NotaInterna.Split("|")(0).Replace("ID=", ""))
                        NoteInterne.Add("QTAPROD=" & OrdineProd.QuantitaProdotta)
                        NoteInterne.Add("QTAFUSTI=" & QtaFusti)

                        SqlStr = "UPDATE OrdProduzRighe SET Stato = " & OrdineProd.Stato & ", NotaInterna = '" & String.Join("|", NoteInterne.ToArray()) & "'" _
                        & " WHERE DBGruppo = '" & GruppoENO & "' AND IdDocumento = " & OrdineProd.IdDocumento _
                        & " AND IdRiga = " & OrdineProd.IdRiga
                        Globali.DataBaseManager.ExecuteQuery(SqlStr)
                    End If
                End If
            End If
        End If

    End Sub

    Sub StampaEtichette()

        Dim Str As String = ""
        Dim DT As DataTable = Nothing
        Dim rep As New XtraReport()
        Dim stp As Boolean = False

        Try
            If IsNothing(OrdineProd) Then
                'vuol dire che non l'ho spuntato e quindi assegno quello evidenziato per la stampa
                OrdineProd = CType(GridView1.GetFocusedRow, YOrdiniProduzione)
                stp = True
            End If


            Str = "TRUNCATE TABLE YStpEtichFU"
            Globali.DataBaseManager.ExecuteQuery(Str)

            'For i = 1 To OrdineProd.Quantita
            Str = "INSERT INTO YStpEtichFU 
                    Select A.DBGruppo, A.CodArt, A.DesArt, A.CodMarca, isnull(M.DesMarca,'') as DesMarca, A.ClasseMerceologica, C.Des as DesClasse, 
                        E.CapacitaUnitaria as MagCoeffUm2MagDen, E.GradoAlcolico, R.QtaUmSecondaria, R.Quantita, '" & OrdineProd.RifLottoAlfanum & "' as Lotto,
                        ValoreEtichetta1, ValoreEtichetta2, ValoreEtichetta3, ValoreEtichetta4, Immagine, CodBarre
                    FROM ArtAnagrafica A 
                    INNER JOIN ENArticoli E ON A.DBGruppo = E.DBGruppo AND A.CodArt = E.CodArt 
                    LEFT OUTER JOIN Marche M ON A.DBGruppo = M.DBGruppo AND A.CodMarca = M.CodMarca 
                    LEFT OUTER JOIN ClassiMerceologiche C ON A.DBGruppo = C.DBGruppo AND A.ClasseMerceologica = C.ClasseMerceologica 
                    INNER JOIN OrdProduzRighe R ON A.DBGruppo = R.DBGruppo 
                    LEFT OUTER JOIN (
                        SELECT ESV_ART_SCHEDETEC.DBGruppo, ESV_ART_SCHEDETEC.CodArt, ESV_ART_SCHEDETEC.CodTipoSchedaTecnica, 
                            MAX(DISTINCT CASE WHEN ESV_ART_SCHEDETEC.IdEtichetta = 1 THEN ISNULL(SchedeTecValAmm.DescrContenuto, ESV_ART_SCHEDETEC.ContAlfanumerico) ELSE '' END) AS ValoreEtichetta1, 
                            MAX(DISTINCT CASE WHEN ESV_ART_SCHEDETEC.IdEtichetta = 2 THEN ISNULL(SchedeTecValAmm.DescrContenuto, ESV_ART_SCHEDETEC.ContAlfanumerico) ELSE '' END) AS ValoreEtichetta2, 
                            MAX(DISTINCT CASE WHEN ESV_ART_SCHEDETEC.IdEtichetta = 3 THEN ISNULL(SchedeTecValAmm.DescrContenuto, ESV_ART_SCHEDETEC.ContAlfanumerico) ELSE '' END) AS ValoreEtichetta3, 
                            MAX(DISTINCT CASE WHEN ESV_ART_SCHEDETEC.IdEtichetta = 4 THEN ISNULL(SchedeTecValAmm.DescrContenuto, ESV_ART_SCHEDETEC.ContAlfanumerico) ELSE '' END) AS ValoreEtichetta4,
                            MAX(DISTINCT CASE WHEN ESV_ART_SCHEDETEC.IdEtichetta = 5 THEN ISNULL(SchedeTecValAmm.DescrContenuto, ESV_ART_SCHEDETEC.ContAlfanumerico) ELSE '' END) +
                            MAX(DISTINCT CASE WHEN ESV_ART_SCHEDETEC.IdEtichetta = 6 THEN ISNULL(SchedeTecValAmm.DescrContenuto, ESV_ART_SCHEDETEC.ContAlfanumerico) ELSE '' END) AS Immagine,
                            MAX(DISTINCT CASE WHEN ESV_ART_SCHEDETEC.IdEtichetta = 7 THEN ISNULL(SchedeTecValAmm.DescrContenuto, ESV_ART_SCHEDETEC.ContAlfanumerico) ELSE '' END) AS CodBarre
                        FROM SchedeTecValAmm RIGHT OUTER JOIN
                            ESV_ART_SCHEDETEC ON SchedeTecValAmm.DBGruppo = ESV_ART_SCHEDETEC.DBGruppo AND SchedeTecValAmm.CodTipoSchedaTecnica = ESV_ART_SCHEDETEC.CodTipoSchedaTecnica AND 
                            SchedeTecValAmm.IdEtichetta = ESV_ART_SCHEDETEC.IdEtichetta AND SchedeTecValAmm.ContNumerico = ESV_ART_SCHEDETEC.ContNumerico AND 
                            SchedeTecValAmm.ContAlfanumerico = ESV_ART_SCHEDETEC.ContAlfanumerico
                        WHERE (ESV_ART_SCHEDETEC.CodTipoSchedaTecnica = 'ETIC')
                        GROUP BY ESV_ART_SCHEDETEC.DBGruppo, ESV_ART_SCHEDETEC.CodArt, ESV_ART_SCHEDETEC.CodTipoSchedaTecnica
                    ) ST
                    ON A.DBGruppo = ST.DBGruppo AND A.CodArt = ST.CodArt
                    WHERE A.DBGruppo = '" & GruppoENO & "' AND A.CodArt = '" & OrdineProd.CodArt & "' AND R.IdDocumento = " & OrdineProd.IdDocumento & " AND R.IdRiga = " & OrdineProd.IdRiga

            Globali.DataBaseManager.ExecuteQuery(Str)
            'Next
            Dim NomeRep As String = "EtichetteFU.repx"
            Dim ValoreEtichetta1 As String = Funzioni.NVL(Globali.DataBaseManager.GetValueFromTable("YStpEtichFU", "Where DBGruppo = '" & GruppoENO & "'", "ValoreEtich1"), "")
            Dim ValoreEtichetta3 As String = Funzioni.NVL(Globali.DataBaseManager.GetValueFromTable("YStpEtichFU", "Where DBGruppo = '" & GruppoENO & "'", "ValoreEtich3"), "")
            Dim Immagine As String = Funzioni.NVL(Globali.DataBaseManager.GetValueFromTable("YStpEtichFU", "Where DBGruppo = '" & GruppoENO & "'", "Immagine"), "")
            Dim CodBarre As String = Funzioni.NVL(Globali.DataBaseManager.GetValueFromTable("YStpEtichFU", "Where DBGruppo = '" & GruppoENO & "'", "CodBarre"), "")

            If ValoreEtichetta3.Trim() <> "" Then
                NomeRep = "EtichetteFU_IGT.repx"
            Else
                If Immagine.Trim() <> "" AndAlso File.Exists(Immagine) Then
                    If CodBarre.Trim() <> "" Then
                        NomeRep = "EtichetteFU_logocb.repx"
                    Else
                        If ValoreEtichetta1.Trim() <> "" Then
                            NomeRep = "EtichetteFU_logo2.repx"
                        Else
                            NomeRep = "EtichetteFU_logo.repx"
                        End If
                    End If
                End If
            End If


            Dim report = New XtraReport()
            report.LoadLayout(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NomeRep))

            Dim sqlDataSource = CType(report.DataSource, SqlDataSource)
            sqlDataSource.Connection.ConnectionString = Globali.DataBaseManager.Connection.ConnectionString

            Dim printTool As ReportPrintTool = New ReportPrintTool(report)
            printTool.PreviewForm.WindowState = FormWindowState.Maximized


            printTool.PrinterSettings.Copies = OrdineProd.Quantita
            printTool.ShowRibbonPreviewDialog()
            printTool.SavePrinterSettings(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EtichetteFU"))

            If stp Then OrdineProd = Nothing

        Catch ex As Exception
            XtraMessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub DxRibbon1_ZbbStampa_ItemClick(sender As Object, e As ItemClickEventArgs) Handles DxRibbon1.ZbbStampa_ItemClick

        StampaEtichette()

    End Sub

    Private Sub bbAzzera_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbAzzera.ItemClick
        If Not OrdineProd Is Nothing Then
            If XtraMessageBox.Show("Azzerare la produzione dell'ordine n° " & OrdineProd.RifRigaOrdine & "?", "Azzera produzione", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Exit Sub
            End If

            OrdineProd.Stato = 0
            SqlStr = "UPDATE OrdProduzRighe SET Stato = 0, NotaInterna = ''" _
                        & " WHERE DBGruppo = '" & GruppoENO & "' AND IdDocumento = " & OrdineProd.IdDocumento _
                        & " AND IdRiga = " & OrdineProd.IdRiga
            Globali.DataBaseManager.ExecuteQuery(SqlStr)
            Refresh_griglia()

            barInfoPallet2.Caption = ""
            StatoPalletProg = 0
            QtaFusti = 0
            'barInfoPallet.Caption = ""
        End If
    End Sub

    Private Sub GridView1_CustomColumnDisplayText(sender As Object, e As CustomColumnDisplayTextEventArgs) Handles GridView1.CustomColumnDisplayText
        If e.Column.FieldName = "NotaInterna" Then
            Dim tmp As Double = 0
            e.DisplayText = "0"

            If e.Value = Nothing Then Return

            If e.Value.ToString.Split("|").Length = 3 Then
                tmp = e.Value.ToString.Split("|")(2).Replace("QTAFUSTI=", "")
                e.DisplayText = tmp
                Return
            End If
            Dim conv As Boolean = Double.TryParse(e.Value.ToString, tmp)
            If conv Then
                e.DisplayText = tmp
                Return
            End If
        End If
    End Sub
End Class
