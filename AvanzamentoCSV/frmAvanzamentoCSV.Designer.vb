<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAvanzamentoCSV
    Inherits DXControls.DXForm

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim RepositoryItemRibbonSearchEdit1 As DevExpress.XtraBars.Ribbon.Internal.RepositoryItemRibbonSearchEdit = New DevExpress.XtraBars.Ribbon.Internal.RepositoryItemRibbonSearchEdit()
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(frmAvanzamentoCSV))
        colStatoDescrizione = New DevExpress.XtraGrid.Columns.GridColumn()
        DxRibbon1 = New DXControls.DXRibbon()
        BarStaticItemPLC = New DevExpress.XtraBars.BarStaticItem()
        BarButtonItemScriviESO = New DevExpress.XtraBars.BarButtonItem()
        BarButtonAvviaProd = New DevExpress.XtraBars.BarButtonItem()
        BarButtonItemUpdateGrid = New DevExpress.XtraBars.BarButtonItem()
        BarButtonItemParamGen = New DevExpress.XtraBars.BarButtonItem()
        barInfoPallet = New DevExpress.XtraBars.BarStaticItem()
        barInfoPallet2 = New DevExpress.XtraBars.BarStaticItem()
        bbAzzera = New DevExpress.XtraBars.BarButtonItem()
        RibbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        RibbonPageGroupImpostazioni = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        RibbonPageGroupPLC = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        RibbonPageGroupAltro = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        RibbonStatusBar1 = New DevExpress.XtraBars.Ribbon.RibbonStatusBar()
        DxGrid1 = New DXControls.DXGrid()
        GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        colNotaInterna = New DevExpress.XtraGrid.Columns.GridColumn()
        Sel = New DevExpress.XtraGrid.Columns.GridColumn()
        RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        colNumeroDocumento = New DevExpress.XtraGrid.Columns.GridColumn()
        colNumProgrRiga = New DevExpress.XtraGrid.Columns.GridColumn()
        colCodArt = New DevExpress.XtraGrid.Columns.GridColumn()
        colVarianteArt = New DevExpress.XtraGrid.Columns.GridColumn()
        colDes = New DevExpress.XtraGrid.Columns.GridColumn()
        colUnitaMisura = New DevExpress.XtraGrid.Columns.GridColumn()
        colQuantita = New DevExpress.XtraGrid.Columns.GridColumn()
        colQuantitaProdotta = New DevExpress.XtraGrid.Columns.GridColumn()
        colUmSecondaria = New DevExpress.XtraGrid.Columns.GridColumn()
        colQtaUmSecondaria = New DevExpress.XtraGrid.Columns.GridColumn()
        colQuantitaProdotta2 = New DevExpress.XtraGrid.Columns.GridColumn()
        colDataInizioSched = New DevExpress.XtraGrid.Columns.GridColumn()
        colDataFineSched = New DevExpress.XtraGrid.Columns.GridColumn()
        colCodMagPrincipale = New DevExpress.XtraGrid.Columns.GridColumn()
        colCodAreaMagPrinc = New DevExpress.XtraGrid.Columns.GridColumn()
        colRifLottoAlfanum = New DevExpress.XtraGrid.Columns.GridColumn()
        Timer1 = New Timer(components)
        CType(DxRibbon1, ComponentModel.ISupportInitialize).BeginInit()
        CType(RepositoryItemRibbonSearchEdit1, ComponentModel.ISupportInitialize).BeginInit()
        CType(DxGrid1, ComponentModel.ISupportInitialize).BeginInit()
        CType(GridView1, ComponentModel.ISupportInitialize).BeginInit()
        CType(RepositoryItemCheckEdit1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' colStatoDescrizione
        ' 
        colStatoDescrizione.Caption = "Stato"
        colStatoDescrizione.FieldName = "StatoDescrizione"
        colStatoDescrizione.Name = "colStatoDescrizione"
        colStatoDescrizione.OptionsColumn.AllowEdit = False
        colStatoDescrizione.Visible = True
        colStatoDescrizione.VisibleIndex = 16
        colStatoDescrizione.Width = 195
        ' 
        ' DxRibbon1
        ' 
        DxRibbon1.AllowMinimizeRibbon = False
        DxRibbon1.BBCaricaLayout = Nothing
        DxRibbon1.BBRipristina = Nothing
        DxRibbon1.BBSalvaLayout = Nothing
        DxRibbon1.ExpandCollapseItem.Id = 0
        DxRibbon1.Items.AddRange(New DevExpress.XtraBars.BarItem() {DxRibbon1.ExpandCollapseItem, BarStaticItemPLC, BarButtonItemScriviESO, BarButtonAvviaProd, BarButtonItemUpdateGrid, BarButtonItemParamGen, barInfoPallet, barInfoPallet2, bbAzzera})
        DxRibbon1.Location = New Point(0, 0)
        DxRibbon1.MaxItemId = 15
        DxRibbon1.Name = "DxRibbon1"
        DxRibbon1.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {RibbonPage1})
        DxRibbon1.PMLayout = Nothing
        RepositoryItemRibbonSearchEdit1.AllowFocused = False
        RepositoryItemRibbonSearchEdit1.AutoHeight = False
        EditorButtonImageOptions1.SvgImage = CType(resources.GetObject("EditorButtonImageOptions1.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        RepositoryItemRibbonSearchEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, True, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.Default), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear, "", -1, True, False, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.Default)})
        RepositoryItemRibbonSearchEdit1.NullText = "Search"
        DxRibbon1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {RepositoryItemRibbonSearchEdit1})
        DxRibbon1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office365
        DxRibbon1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False
        DxRibbon1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False
        DxRibbon1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Show
        DxRibbon1.ShowToolbarCustomizeItem = False
        DxRibbon1.Size = New Size(1276, 158)
        DxRibbon1.StatusBar = RibbonStatusBar1
        DxRibbon1.Toolbar.ShowCustomizeItem = False
        DxRibbon1.ZBBAggiungiEnabled = False
        DxRibbon1.ZBBAggiungiSTD = True
        DxRibbon1.ZBBAggiungiVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBAnteprimaEnabled = True
        DxRibbon1.ZBBAnteprimaSTD = True
        DxRibbon1.ZBBAnteprimaVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBApriEnabled = False
        DxRibbon1.ZBBApriVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBAspettoGriglia2Enabled = True
        DxRibbon1.ZBBAspettoGriglia2Visible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBCercaEnabled = True
        DxRibbon1.ZBBCercaSTD = True
        DxRibbon1.ZBBCercaVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBCopiaEnabled = True
        DxRibbon1.ZBBCopiaSTD = True
        DxRibbon1.ZBBCopiaVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBEliminaEnabled = False
        DxRibbon1.ZBBEliminaSTD = True
        DxRibbon1.ZBBEliminaVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBEsciEnabled = True
        DxRibbon1.ZBBEsciSTD = True
        DxRibbon1.ZBBEsciVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBEsportaEnabled = True
        DxRibbon1.ZBBEsportaSTD = True
        DxRibbon1.ZBBEsportaVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBFineEnabled = True
        DxRibbon1.ZBBFineSTD = True
        DxRibbon1.ZBBFineVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBGroupAppuntiEnabled = True
        DxRibbon1.ZBBGroupAppuntiVisible = True
        DxRibbon1.ZBBGroupGestioneEnabled = True
        DxRibbon1.ZBBGroupGestioneVisible = True
        DxRibbon1.ZBBGroupGrigliaEnabled = True
        DxRibbon1.ZBBGroupGrigliaVisible = True
        DxRibbon1.ZBBGroupHelpEnabled = True
        DxRibbon1.ZBBGroupHelpVisible = True
        DxRibbon1.ZBBGroupNavigazioneEnabled = True
        DxRibbon1.ZBBGroupNavigazioneVisible = True
        DxRibbon1.ZBBGroupStampaEnabled = True
        DxRibbon1.ZBBGroupStampaVisible = True
        DxRibbon1.ZBBHelpEnabled = True
        DxRibbon1.ZBBHelpVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBIncollaEnabled = True
        DxRibbon1.ZBBIncollaSTD = True
        DxRibbon1.ZBBIncollaVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBInizioEnabled = True
        DxRibbon1.ZBBInizioSTD = True
        DxRibbon1.ZBBInizioVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBNuovoEnabled = False
        DxRibbon1.ZBBNuovoVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBPrecedenteEnabled = True
        DxRibbon1.ZBBPrecedenteSTD = True
        DxRibbon1.ZBBPrecedenteVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBRipristinaGrigliaEnabled = True
        DxRibbon1.ZBBRipristinaGrigliaVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBSalvaCausesValidation = False
        DxRibbon1.ZBBSalvaEnabled = False
        DxRibbon1.ZBBSalvaVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBStampaCausesValidation = False
        DxRibbon1.ZBBStampaEnabled = True
        DxRibbon1.ZBBStampaVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBSuccessivoEnabled = True
        DxRibbon1.ZBBSuccessivoSTD = True
        DxRibbon1.ZBBSuccessivoVisible = DevExpress.XtraBars.BarItemVisibility.Always
        DxRibbon1.ZBBTagliaEnabled = True
        DxRibbon1.ZBBTagliaSTD = True
        DxRibbon1.ZBBTagliaVisible = DevExpress.XtraBars.BarItemVisibility.Always
        ' 
        ' BarStaticItemPLC
        ' 
        BarStaticItemPLC.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right
        BarStaticItemPLC.Id = 6
        BarStaticItemPLC.Name = "BarStaticItemPLC"
        ' 
        ' BarButtonItemScriviESO
        ' 
        BarButtonItemScriviESO.Caption = "Avanzamento"
        BarButtonItemScriviESO.Id = 8
        BarButtonItemScriviESO.ImageOptions.LargeImage = CType(resources.GetObject("BarButtonItemScriviESO.ImageOptions.LargeImage"), Image)
        BarButtonItemScriviESO.Name = "BarButtonItemScriviESO"
        ' 
        ' BarButtonAvviaProd
        ' 
        BarButtonAvviaProd.Caption = "Avvia"
        BarButtonAvviaProd.Enabled = False
        BarButtonAvviaProd.Id = 4
        BarButtonAvviaProd.ImageOptions.LargeImage = CType(resources.GetObject("BarButtonAvviaProd.ImageOptions.LargeImage"), Image)
        BarButtonAvviaProd.Name = "BarButtonAvviaProd"
        ' 
        ' BarButtonItemUpdateGrid
        ' 
        BarButtonItemUpdateGrid.Caption = "Aggiorna Dati"
        BarButtonItemUpdateGrid.Id = 9
        BarButtonItemUpdateGrid.ImageOptions.Image = CType(resources.GetObject("BarButtonItemUpdateGrid.ImageOptions.Image"), Image)
        BarButtonItemUpdateGrid.ImageOptions.LargeImage = CType(resources.GetObject("BarButtonItemUpdateGrid.ImageOptions.LargeImage"), Image)
        BarButtonItemUpdateGrid.Name = "BarButtonItemUpdateGrid"
        ' 
        ' BarButtonItemParamGen
        ' 
        BarButtonItemParamGen.Caption = "Parametri"
        BarButtonItemParamGen.Id = 10
        BarButtonItemParamGen.ImageOptions.Image = CType(resources.GetObject("BarButtonItemParamGen.ImageOptions.Image"), Image)
        BarButtonItemParamGen.ImageOptions.LargeImage = CType(resources.GetObject("BarButtonItemParamGen.ImageOptions.LargeImage"), Image)
        BarButtonItemParamGen.Name = "BarButtonItemParamGen"
        ' 
        ' barInfoPallet
        ' 
        barInfoPallet.Id = 11
        barInfoPallet.Name = "barInfoPallet"
        ' 
        ' barInfoPallet2
        ' 
        barInfoPallet2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
        barInfoPallet2.Id = 13
        barInfoPallet2.Name = "barInfoPallet2"
        ' 
        ' bbAzzera
        ' 
        bbAzzera.Caption = "Libera Produzione"
        bbAzzera.Id = 14
        bbAzzera.ImageOptions.SvgImage = CType(resources.GetObject("bbAzzera.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        bbAzzera.Name = "bbAzzera"
        ' 
        ' RibbonPage1
        ' 
        RibbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {RibbonPageGroupImpostazioni, RibbonPageGroupPLC, RibbonPageGroupAltro})
        RibbonPage1.Name = "RibbonPage1"
        RibbonPage1.Text = "RibbonPage1"
        ' 
        ' RibbonPageGroupImpostazioni
        ' 
        RibbonPageGroupImpostazioni.AllowTextClipping = False
        RibbonPageGroupImpostazioni.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False
        RibbonPageGroupImpostazioni.ItemLinks.Add(BarButtonItemParamGen)
        RibbonPageGroupImpostazioni.Name = "RibbonPageGroupImpostazioni"
        RibbonPageGroupImpostazioni.Text = "Impostazioni"
        ' 
        ' RibbonPageGroupPLC
        ' 
        RibbonPageGroupPLC.AllowTextClipping = False
        RibbonPageGroupPLC.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False
        RibbonPageGroupPLC.ItemLinks.Add(BarButtonAvviaProd)
        RibbonPageGroupPLC.ItemLinks.Add(BarButtonItemScriviESO)
        RibbonPageGroupPLC.ItemLinks.Add(bbAzzera)
        RibbonPageGroupPLC.Name = "RibbonPageGroupPLC"
        RibbonPageGroupPLC.Text = "PLC"
        ' 
        ' RibbonPageGroupAltro
        ' 
        RibbonPageGroupAltro.AllowTextClipping = False
        RibbonPageGroupAltro.CaptionButtonVisible = DevExpress.Utils.DefaultBoolean.False
        RibbonPageGroupAltro.ItemLinks.Add(BarButtonItemUpdateGrid)
        RibbonPageGroupAltro.Name = "RibbonPageGroupAltro"
        RibbonPageGroupAltro.Text = "Altro"
        ' 
        ' RibbonStatusBar1
        ' 
        RibbonStatusBar1.ItemLinks.Add(BarStaticItemPLC)
        RibbonStatusBar1.ItemLinks.Add(barInfoPallet)
        RibbonStatusBar1.ItemLinks.Add(barInfoPallet2, True)
        RibbonStatusBar1.Location = New Point(0, 696)
        RibbonStatusBar1.Name = "RibbonStatusBar1"
        RibbonStatusBar1.Ribbon = DxRibbon1
        RibbonStatusBar1.Size = New Size(1276, 22)
        ' 
        ' DxGrid1
        ' 
        DxGrid1.Dock = DockStyle.Fill
        DxGrid1.Location = New Point(0, 158)
        DxGrid1.MainView = GridView1
        DxGrid1.MenuManager = DxRibbon1
        DxGrid1.Name = "DxGrid1"
        DxGrid1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {RepositoryItemCheckEdit1})
        DxGrid1.Size = New Size(1276, 538)
        DxGrid1.TabIndex = 2
        DxGrid1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {GridView1})
        DxGrid1.Zribbon = DxRibbon1
        DxGrid1.ZribbonBTNAggiungi = False
        DxGrid1.ZribbonBTNCopia = True
        DxGrid1.ZribbonBTNElimina = False
        DxGrid1.ZribbonBTNFine = True
        DxGrid1.ZribbonBTNIncolla = True
        DxGrid1.ZribbonBTNInizio = True
        DxGrid1.ZribbonBTNPrecedente = True
        DxGrid1.ZribbonBTNSuccessivo = True
        DxGrid1.ZribbonBTNTaglia = False
        ' 
        ' GridView1
        ' 
        GridView1.Appearance.HeaderPanel.Options.UseTextOptions = True
        GridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        GridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        GridView1.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        GridView1.ColumnPanelRowHeight = 50
        GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {colNotaInterna, Sel, colNumeroDocumento, colNumProgrRiga, colCodArt, colVarianteArt, colDes, colUnitaMisura, colQuantita, colQuantitaProdotta, colUmSecondaria, colQtaUmSecondaria, colQuantitaProdotta2, colDataInizioSched, colDataFineSched, colCodMagPrincipale, colCodAreaMagPrinc, colStatoDescrizione, colRifLottoAlfanum})
        GridView1.GridControl = DxGrid1
        GridView1.Name = "GridView1"
        GridView1.OptionsBehavior.AlignGroupSummaryInGroupRow = DevExpress.Utils.DefaultBoolean.True
        GridView1.OptionsMenu.ShowConditionalFormattingItem = True
        GridView1.OptionsMenu.ShowGroupSummaryEditorItem = True
        GridView1.OptionsSelection.EnableAppearanceFocusedRow = False
        GridView1.OptionsView.ColumnAutoWidth = False
        GridView1.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.False
        GridView1.OptionsView.ShowAutoFilterRow = True
        GridView1.OptionsView.ShowFooter = True
        ' 
        ' colNotaInterna
        ' 
        colNotaInterna.Caption = "Qta prodotta (plc pallet)"
        colNotaInterna.FieldName = "NotaInterna"
        colNotaInterna.Name = "colNotaInterna"
        colNotaInterna.Visible = True
        colNotaInterna.VisibleIndex = 12
        ' 
        ' Sel
        ' 
        Sel.Caption = "SEL"
        Sel.ColumnEdit = RepositoryItemCheckEdit1
        Sel.FieldName = "Sel"
        Sel.Name = "Sel"
        Sel.Visible = True
        Sel.VisibleIndex = 0
        Sel.Width = 41
        ' 
        ' RepositoryItemCheckEdit1
        ' 
        RepositoryItemCheckEdit1.AutoHeight = False
        RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        ' 
        ' colNumeroDocumento
        ' 
        colNumeroDocumento.Caption = "Numero Documento"
        colNumeroDocumento.FieldName = "NumeroDocumento"
        colNumeroDocumento.Name = "colNumeroDocumento"
        colNumeroDocumento.OptionsColumn.AllowEdit = False
        colNumeroDocumento.Visible = True
        colNumeroDocumento.VisibleIndex = 1
        colNumeroDocumento.Width = 67
        ' 
        ' colNumProgrRiga
        ' 
        colNumProgrRiga.Caption = "N. Riga"
        colNumProgrRiga.FieldName = "NumProgrRiga"
        colNumProgrRiga.Name = "colNumProgrRiga"
        colNumProgrRiga.OptionsColumn.AllowEdit = False
        colNumProgrRiga.Visible = True
        colNumProgrRiga.VisibleIndex = 2
        colNumProgrRiga.Width = 46
        ' 
        ' colCodArt
        ' 
        colCodArt.Caption = "Articolo"
        colCodArt.FieldName = "CodArt"
        colCodArt.Name = "colCodArt"
        colCodArt.OptionsColumn.AllowEdit = False
        colCodArt.Visible = True
        colCodArt.VisibleIndex = 3
        colCodArt.Width = 96
        ' 
        ' colVarianteArt
        ' 
        colVarianteArt.Caption = "Variante"
        colVarianteArt.FieldName = "VarianteArt"
        colVarianteArt.Name = "colVarianteArt"
        colVarianteArt.OptionsColumn.AllowEdit = False
        colVarianteArt.Width = 82
        ' 
        ' colDes
        ' 
        colDes.Caption = "Descrizione"
        colDes.FieldName = "Des"
        colDes.Name = "colDes"
        colDes.OptionsColumn.AllowEdit = False
        colDes.Visible = True
        colDes.VisibleIndex = 4
        colDes.Width = 257
        ' 
        ' colUnitaMisura
        ' 
        colUnitaMisura.Caption = "UdM"
        colUnitaMisura.FieldName = "UnitaMisura"
        colUnitaMisura.Name = "colUnitaMisura"
        colUnitaMisura.OptionsColumn.AllowEdit = False
        colUnitaMisura.Visible = True
        colUnitaMisura.VisibleIndex = 6
        colUnitaMisura.Width = 46
        ' 
        ' colQuantita
        ' 
        colQuantita.Caption = "Quantità"
        colQuantita.FieldName = "Quantita"
        colQuantita.Name = "colQuantita"
        colQuantita.OptionsColumn.AllowEdit = False
        colQuantita.Visible = True
        colQuantita.VisibleIndex = 7
        colQuantita.Width = 74
        ' 
        ' colQuantitaProdotta
        ' 
        colQuantitaProdotta.FieldName = "QuantitaProdotta"
        colQuantitaProdotta.Name = "colQuantitaProdotta"
        colQuantitaProdotta.OptionsColumn.AllowEdit = False
        colQuantitaProdotta.Visible = True
        colQuantitaProdotta.VisibleIndex = 8
        colQuantitaProdotta.Width = 74
        ' 
        ' colUmSecondaria
        ' 
        colUmSecondaria.Caption = "Udm 2"
        colUmSecondaria.FieldName = "UmSecondaria"
        colUmSecondaria.Name = "colUmSecondaria"
        colUmSecondaria.OptionsColumn.AllowEdit = False
        colUmSecondaria.Visible = True
        colUmSecondaria.VisibleIndex = 9
        ' 
        ' colQtaUmSecondaria
        ' 
        colQtaUmSecondaria.Caption = "Quantità 2"
        colQtaUmSecondaria.FieldName = "QtaUmSecondaria"
        colQtaUmSecondaria.Name = "colQtaUmSecondaria"
        colQtaUmSecondaria.OptionsColumn.AllowEdit = False
        colQtaUmSecondaria.Visible = True
        colQtaUmSecondaria.VisibleIndex = 10
        ' 
        ' colQuantitaProdotta2
        ' 
        colQuantitaProdotta2.Caption = "Quantità Prodotta 2"
        colQuantitaProdotta2.FieldName = "QuantitaProdotta2"
        colQuantitaProdotta2.Name = "colQuantitaProdotta2"
        colQuantitaProdotta2.OptionsColumn.AllowEdit = False
        colQuantitaProdotta2.Visible = True
        colQuantitaProdotta2.VisibleIndex = 11
        ' 
        ' colDataInizioSched
        ' 
        colDataInizioSched.Caption = "Data Inizio Pianificata"
        colDataInizioSched.FieldName = "DataInizioSched"
        colDataInizioSched.Name = "colDataInizioSched"
        colDataInizioSched.OptionsColumn.AllowEdit = False
        colDataInizioSched.Visible = True
        colDataInizioSched.VisibleIndex = 13
        colDataInizioSched.Width = 83
        ' 
        ' colDataFineSched
        ' 
        colDataFineSched.Caption = "Data Fine Pianificata"
        colDataFineSched.FieldName = "DataFineSched"
        colDataFineSched.Name = "colDataFineSched"
        colDataFineSched.OptionsColumn.AllowEdit = False
        colDataFineSched.Visible = True
        colDataFineSched.VisibleIndex = 14
        colDataFineSched.Width = 83
        ' 
        ' colCodMagPrincipale
        ' 
        colCodMagPrincipale.Caption = "Magazzino"
        colCodMagPrincipale.FieldName = "CodMagPrincipale"
        colCodMagPrincipale.Name = "colCodMagPrincipale"
        colCodMagPrincipale.OptionsColumn.AllowEdit = False
        colCodMagPrincipale.Visible = True
        colCodMagPrincipale.VisibleIndex = 15
        colCodMagPrincipale.Width = 68
        ' 
        ' colCodAreaMagPrinc
        ' 
        colCodAreaMagPrinc.Caption = "Area"
        colCodAreaMagPrinc.FieldName = "CodAreaMagPrinc"
        colCodAreaMagPrinc.Name = "colCodAreaMagPrinc"
        colCodAreaMagPrinc.OptionsColumn.AllowEdit = False
        colCodAreaMagPrinc.Width = 41
        ' 
        ' colRifLottoAlfanum
        ' 
        colRifLottoAlfanum.Caption = "Lotto"
        colRifLottoAlfanum.FieldName = "RifLottoAlfanum"
        colRifLottoAlfanum.Name = "colRifLottoAlfanum"
        colRifLottoAlfanum.Visible = True
        colRifLottoAlfanum.VisibleIndex = 5
        colRifLottoAlfanum.Width = 64
        ' 
        ' Timer1
        ' 
        Timer1.Interval = 10000
        ' 
        ' frmAvanzamentoCSV
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1276, 718)
        Controls.Add(DxGrid1)
        Controls.Add(RibbonStatusBar1)
        Controls.Add(DxRibbon1)
        Margin = New Padding(3, 4, 3, 4)
        Name = "frmAvanzamentoCSV"
        Ribbon = DxRibbon1
        StatusBar = RibbonStatusBar1
        Text = "Avanzamento Produzione CSV"
        CType(RepositoryItemRibbonSearchEdit1, ComponentModel.ISupportInitialize).EndInit()
        CType(DxRibbon1, ComponentModel.ISupportInitialize).EndInit()
        CType(DxGrid1, ComponentModel.ISupportInitialize).EndInit()
        CType(GridView1, ComponentModel.ISupportInitialize).EndInit()
        CType(RepositoryItemCheckEdit1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents DxRibbon1 As DXControls.DXRibbon
    Friend WithEvents RibbonPage1 As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents RibbonPageGroupImpostazioni As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents RibbonStatusBar1 As DevExpress.XtraBars.Ribbon.RibbonStatusBar
    Friend WithEvents DxGrid1 As DXControls.DXGrid
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colNumProgrRiga As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCodArt As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colVarianteArt As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDes As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colUnitaMisura As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colQuantita As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colQuantitaProdotta As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDataInizioSched As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDataFineSched As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCodMagPrincipale As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCodAreaMagPrinc As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colStatoDescrizione As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNumeroDocumento As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RibbonPageGroupPLC As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents Sel As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents BarStaticItemPLC As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents Timer1 As Timer
    Friend WithEvents BarButtonItemScriviESO As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPageGroupAltro As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarButtonItemUpdateGrid As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItemParamGen As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents colUmSecondaria As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colQtaUmSecondaria As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colQuantitaProdotta2 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BarButtonAvviaProd As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents colRifLottoAlfanum As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents barInfoPallet As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents barInfoPallet2 As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents bbAzzera As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents colNotaInterna As DevExpress.XtraGrid.Columns.GridColumn
End Class
