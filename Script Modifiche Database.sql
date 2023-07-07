CREATE TABLE [dbo].[YConfigPLC_Registri](
	[Registro] [int] NOT NULL,
	[Campo] [int] NOT NULL,
	[RW] [varchar](1) NULL,
 CONSTRAINT [PK_YConfigPLC_Registri] PRIMARY KEY CLUSTERED 
(
	[Registro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[YConfigPLC_Registri] ADD  CONSTRAINT [DF_YConfigPLC_Registri_RW]  DEFAULT (' ') FOR [RW]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dato in Lettura (R) o Scrittura (W) da/verso PLC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'YConfigPLC_Registri', @level2type=N'COLUMN',@level2name=N'RW'
GO

INSERT INTO YConfigPLC_Registri (Registro, Campo, RW) VALUES (8, 1, 'R')
INSERT INTO YConfigPLC_Registri (Registro, Campo, RW) VALUES (10, 2, 'R')
INSERT INTO YConfigPLC_Registri (Registro, Campo, RW) VALUES (12, 3, 'R')
INSERT INTO YConfigPLC_Registri (Registro, Campo, RW) VALUES (14, 4, 'R')
INSERT INTO YConfigPLC_Registri (Registro, Campo, RW) VALUES (204, 5, 'W')
INSERT INTO YConfigPLC_Registri (Registro, Campo, RW) VALUES (203, 6, 'W')

GO

CREATE VIEW [dbo].[YOrdiniProduzione]
AS
SELECT        dbo.DocElencoGen.DataRegistrazione, dbo.DocElencoGen.PeriodoRifNumeraz, dbo.DocElencoGen.CodSerie, dbo.DocElencoGen.NumRegistraz, dbo.OrdProduzRighe.DBGruppo, dbo.OrdProduzRighe.IdDocumento, 
                         dbo.OrdProduzRighe.IdRiga, dbo.OrdProduzRighe.Stato, dbo.OrdProduzRighe.NumProgrRiga, dbo.OrdProduzRighe.CodArt, dbo.OrdProduzRighe.VarianteArt, dbo.OrdProduzRighe.Des, dbo.OrdProduzRighe.UnitaMisura, 
                         dbo.OrdProduzRighe.Quantita, ISNULL(SUM(dbo.OrdProduzMovAv.Quantita), 0) AS QuantitaProdotta, SUM(ISNULL(OrdProduzMovAv_Ore.Quantita, 0)) AS QuantitaProdotta_HH, dbo.OrdProduzRighe.DataInizioSched, 
                         dbo.OrdProduzRighe.DataFineSched, dbo.OrdProduzRighe.CodMagPrincipale, dbo.OrdProduzRighe.CodAreaMagPrinc, ISNULL(dbo.DocRigheRifLotti.RifLottoAlfanum, ' ') AS RifLottoAlfanum, 
                         dbo.OrdProduzRighe.NotaInterna
FROM            dbo.OrdProduzRighe INNER JOIN
                         dbo.DocElencoGen ON dbo.OrdProduzRighe.DBGruppo = dbo.DocElencoGen.DBGruppo AND dbo.OrdProduzRighe.IdDocumento = dbo.DocElencoGen.IdDocumento LEFT OUTER JOIN
                             (SELECT        DBGruppo, IdDocOrd, IdRigaOrdProdFinito, UnitaMisura, Quantita
                               FROM            dbo.OrdProduzMovAv AS OrdProduzMovAv_1
                               WHERE        (UnitaMisura = 'HH')) AS OrdProduzMovAv_Ore ON dbo.OrdProduzRighe.DBGruppo = OrdProduzMovAv_Ore.DBGruppo AND dbo.OrdProduzRighe.IdDocumento = OrdProduzMovAv_Ore.IdDocOrd AND 
                         dbo.OrdProduzRighe.IdRiga = OrdProduzMovAv_Ore.IdRigaOrdProdFinito LEFT OUTER JOIN
                         dbo.OrdProduzMovAv ON dbo.OrdProduzRighe.UnitaMisura = dbo.OrdProduzMovAv.UnitaMisura AND dbo.OrdProduzRighe.DBGruppo = dbo.OrdProduzMovAv.DBGruppo AND 
                         dbo.OrdProduzRighe.IdDocumento = dbo.OrdProduzMovAv.IdDocOrd AND dbo.OrdProduzRighe.IdRiga = dbo.OrdProduzMovAv.IdRigaOrdProdFinito LEFT OUTER JOIN
                         dbo.DocRigheRifLotti ON dbo.OrdProduzRighe.DBGruppo = dbo.DocRigheRifLotti.DBGruppo AND dbo.OrdProduzRighe.IdDocumento = dbo.DocRigheRifLotti.IdDocumento AND 
                         dbo.OrdProduzRighe.IdRiga = dbo.DocRigheRifLotti.IdRigaDoc
GROUP BY dbo.OrdProduzRighe.DBGruppo, dbo.OrdProduzRighe.IdDocumento, dbo.OrdProduzRighe.IdRiga, dbo.OrdProduzRighe.Stato, dbo.OrdProduzRighe.NumProgrRiga, dbo.OrdProduzRighe.CodArt, dbo.OrdProduzRighe.VarianteArt, 
                         dbo.OrdProduzRighe.Des, dbo.OrdProduzRighe.UnitaMisura, dbo.OrdProduzRighe.Quantita, dbo.OrdProduzRighe.DataInizioSched, dbo.OrdProduzRighe.DataFineSched, dbo.OrdProduzRighe.CodMagPrincipale, 
                         dbo.OrdProduzRighe.CodAreaMagPrinc, dbo.DocElencoGen.DataRegistrazione, dbo.DocElencoGen.PeriodoRifNumeraz, dbo.DocElencoGen.CodSerie, dbo.DocElencoGen.NumRegistraz, dbo.DocRigheRifLotti.RifLottoAlfanum, 
                         dbo.OrdProduzRighe.NotaInterna
GO
