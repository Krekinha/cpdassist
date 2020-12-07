using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPDAssist.MDFe
{
    public class ArquivoExcelAverbacao
    {
        private List<NFMDFe> _nfs;
        private string _nomeArquivo;
        private HSSFWorkbook _workbookCatalogo;

        public ArquivoExcelAverbacao(List<NFMDFe> nfs)
        {
            this._nfs = nfs;
        }

        public void GerarArquivo(string nomeArquivo)
        {
            this._nomeArquivo = nomeArquivo;

            FileStream file = new FileStream(@".\DBase\avb.xls",
                    FileMode.Open, FileAccess.Read);
            _workbookCatalogo = new HSSFWorkbook(file);

            PreencherInformacoesProdutos();
            FinalizarGravacaoArquivo();
        }

        private void PreencherInformacoesProdutos()
        {
            ISheet sheetCatalogo =
                _workbookCatalogo.GetSheet("avb");

            int numeroProximaLinha = 5;
            foreach (NFMDFe nf in _nfs)
            {
                sheetCatalogo.GetCell(numeroProximaLinha, 1)
                    .SetCellValue(nf.Razao);
                sheetCatalogo.GetCell(numeroProximaLinha, 3)
                    .SetCellValue(nf.Placa);
                sheetCatalogo.GetCell(numeroProximaLinha, 4)
                    .SetCellValue("TERCEIRO");
                sheetCatalogo.GetCell(numeroProximaLinha, 5)
                    .SetCellValue(nf.Emissao);
                sheetCatalogo.GetCell(numeroProximaLinha, 6)
                    .SetCellValue(nf.UFO);
                sheetCatalogo.GetCell(numeroProximaLinha, 7)
                    .SetCellValue(nf.UFD);
                sheetCatalogo.GetCell(numeroProximaLinha, 8)
                    .SetCellValue(GetValBoleto(nf));
                sheetCatalogo.GetCell(numeroProximaLinha, 9)
                    .SetCellValue(nf.Natureza);
                sheetCatalogo.GetCell(numeroProximaLinha, 10)
                    .SetCellValue(nf.NumNF);
                numeroProximaLinha++;
            }
        }

        public void FinalizarGravacaoArquivo()
        {
            using (FileStream file = new FileStream(
                _nomeArquivo, FileMode.Create))
            {
                _workbookCatalogo.Write(file);
                file.Close();
            }
        }

        private double GetValBoleto(NFMDFe _nf)
        {
            if (_nf.ValBoleto == 0) return double.Parse(_nf.ValorNF.ToString());
            return double.Parse(_nf.ValBoleto.ToString());
        }
    }
}
