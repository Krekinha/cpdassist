using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using System.util;
using iTextSharp.text.pdf.parser;

namespace CPDAssit.BNMerge
{
    public class GetMergedFile
    {
        public IList<SequenceMerge> GetMerged(string pathMerged, out int page)
        {
            page = 1;
            IList<SeqNFBT> sNFBT = new List<SeqNFBT>();
            PdfReader reader = new PdfReader(pathMerged);

            RectangleJ rectDanfe = new RectangleJ(448.004F, 568.367F, 38, 14); //DANFE
            RectangleJ rectBanco = new RectangleJ(149.23F, 799.52F, 40.81F, 18.99F); //341-7
            
            ITextExtractionStrategy strategy;
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                page = i;

                SeqNFBT _seqNfBt = new SeqNFBT();

                _seqNfBt.Page = i;
                page = i;

                RenderFilter[] filter = { new RegionTextRenderFilter(rectDanfe), new RegionTextRenderFilter(rectBanco) };

                strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter[0]);
                string s = PdfTextExtractor.GetTextFromPage(reader, i, strategy);
                
                if ((s.Replace(".", "") == "DANFE"))
                {
                    _seqNfBt.Nome = "NF";
                    sNFBT.Add(_seqNfBt);
                }
                else
                {
                    strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter[1]);
                    string s2 = PdfTextExtractor.GetTextFromPage(reader, i, strategy);
                    if ((s2.Replace(" ", "") == "341-7"))
                    {
                        _seqNfBt.Nome = "BT";
                        sNFBT.Add(_seqNfBt);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return seqM(sNFBT);
        }

        private IList<SequenceMerge> seqM(IList<SeqNFBT> seq)
        {
            IList<SequenceMerge> sm = new List<SequenceMerge>();
            bool nfOpen = false;
            bool btOpen = false;

            for (int i = 0; i < seq.Count; i++)
            {
                if (seq[i].Nome == "NF")
                {
                    if (!nfOpen)
                    {
                        sm.Add(new SequenceMerge() { Nome = "NF", StartPage = seq[i].Page });
                        nfOpen = true;
                    }
                        
                }
                else
                {
                    if (!btOpen)
                    {
                        sm.Add(new SequenceMerge() { Nome = "BT", StartPage = seq[i].Page });
                        nfOpen = true;
                    }
                }
            }
            return null;
        }
    }


    public class SeqNFBT
    {
        public string Nome { get; set; }
        public int Page { get; set; }
    }

    public class SequenceMerge
    {
        public string Nome { get; set; }
        public int StartPage { get; set; }
        public int Finalpage { get; set; }
    }
}
