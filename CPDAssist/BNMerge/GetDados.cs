using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System.util;
using System.ComponentModel;
using CPDAssit.BNMerge;

namespace CPDAssit.BNMerge
{
    public class GetDados
    {
        #region REF NFe
        public IList<dadosNF> getNF(string pathNF, object sender, out int page)
        {
            #region Layout antigo ver 3.10
            /*iTextSharp.text.pdf.parser.Vector numLOC = new iTextSharp.text.pdf.parser.Vector(415.53F, 528.12F, 1);
            iTextSharp.text.pdf.parser.Vector flsLOC = new iTextSharp.text.pdf.parser.Vector(527.658F, 528.927F, 1);
            iTextSharp.text.pdf.parser.Vector romLOC = new iTextSharp.text.pdf.parser.Vector(94.477F, 72.962F, 1);
            iTextSharp.text.pdf.parser.Vector pgtLOC = new iTextSharp.text.pdf.parser.Vector(130.229F, 423.848F, 1);*/
            #endregion

            // Layout 4.0
            iTextSharp.text.pdf.parser.Vector numLOC = new iTextSharp.text.pdf.parser.Vector(414.99F, 527.72F, 1);
            iTextSharp.text.pdf.parser.Vector flsLOC = new iTextSharp.text.pdf.parser.Vector(527.431F, 528.452F, 1);
            iTextSharp.text.pdf.parser.Vector romLOC = new iTextSharp.text.pdf.parser.Vector(94.2505F, 72.66F, 1);
            iTextSharp.text.pdf.parser.Vector pgtLOC = new iTextSharp.text.pdf.parser.Vector(130.002F, 423.374F, 1);

            IList<dadosNF> dnf = new List<dadosNF>();
            PdfReader reader = new PdfReader(pathNF);
            int bols = 0;

            RectangleJ rectNUM = new RectangleJ(numLOC[0], numLOC[1], 1, 1);
            RectangleJ rectFLS = new RectangleJ(flsLOC[0], flsLOC[1], 1, 1);
            RectangleJ rectROM = new RectangleJ(romLOC[0], romLOC[1], 1, 1);
            RectangleJ rectPGT = new RectangleJ(pgtLOC[0], pgtLOC[1], 1, 1);

            ITextExtractionStrategy strategy;
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                string[] s2 = new string[2];
                int posRom;
                dadosNF _dnf = new dadosNF();

                _dnf.Page = i;
                string h;

                if (_dnf.Page == 216)
                    h = "216";

                RenderFilter[] filter = { new RegionTextRenderFilter(rectNUM), new RegionTextRenderFilter(rectFLS), new RegionTextRenderFilter(rectROM), new RegionTextRenderFilter(rectPGT)};
                 strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter[0]);
                string s = PdfTextExtractor.GetTextFromPage(reader, i, strategy);
                int num;
                if (int.TryParse(s.Replace(".", ""), out num))
                {
                    _dnf.Num = num;
                }
                else
                {
                    page = i;
                    return null;
                }

                try
                {
                    strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter[1]);
                    s2 = PdfTextExtractor.GetTextFromPage(reader, i, strategy).Split('/');
                    _dnf.Folha = int.Parse(s2[0]);
                    _dnf.TFolha = int.Parse(s2[1]);

                    strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter[2]);
                    s = PdfTextExtractor.GetTextFromPage(reader, i, strategy);
                    s = s.Replace(" ", "");
                    posRom = s.IndexOf("ROM:");
                    _dnf.Romaneio = int.Parse(s.Substring(posRom + 4, 3));
                    _dnf.Page = i;

                    strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter[3]);
                    string s3 = PdfTextExtractor.GetTextFromPage(reader, i, strategy);
                    s3 = s3.Replace(" ", "");

                    if (s3.Contains("COBRANCASIMPLES"))
                    {
                        _dnf.Boleto = "S";
                        bols += 1;
                    }
                    else
                    {
                        _dnf.Boleto = "N";
                    }

                    dnf.Add(_dnf);
                    (sender as BackgroundWorker).ReportProgress(i, reader.NumberOfPages);

                }
                catch (Exception ex)
                {
                    page = i;
                    return null;
                }

            }
            page = 0;
            return dnf;
        }
        #endregion
    }
}
