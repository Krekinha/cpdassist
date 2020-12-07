using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Pdf;
using System.Drawing.Printing;
using CPDAssit.BNMerge;

namespace CPDAssit.Tools
{
    public class Tools
    {
        //Tools
        private void gerarDB(IList<dadosNF> dnf, IList<dadosBT> dbt)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

            foreach (var item in dnf)
            {
                sb.AppendLine("dnf.Add(new dadosNF(" + item.Num + ", " + item.Folha + ", " + item.TFolha + ", " + item.Romaneio + ", " + (item.Page) + "));");
            }
            System.IO.File.WriteAllText(@"C:\Users\krekm\Desktop\PDFM\dbNF.txt", sb.ToString());


            foreach (var item in dbt)
            {
                string refnf = "";
                string sep = ", ";
                for (int i = 0; i < item.RefNF.Length; i++)
                {
                    if ((i + 1) == item.RefNF.Length)
                        sep = "";
                    refnf += item.RefNF[i].ToString() + sep;
                }
                sb2.AppendLine("dbt.Add(new dadosBT(" + "new int[] {" + refnf + "}, " + item.Romaneio + ", " + item.Page + "));");
            }
            System.IO.File.WriteAllText(@"C:\Users\krekm\Desktop\PDFM\dbBT.txt", sb2.ToString());
        }

        public void Imprimir()
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(@"sample.pdf");
            PrinterSettings.PaperSourceCollection collections = doc.PrintDocument.PrinterSettings.PaperSources;
            List<PdfPaperSourceTray> loPaperSources = new List<PdfPaperSourceTray>();
            if (loPaperSources.Count >= 4)
            {
                PdfPaperSourceTray tray = new PdfPaperSourceTray();
                tray.StartPage = 0;
                tray.EndPage = 1;
                tray.PrintPaperSource = collections[2];
                loPaperSources.Add(tray);
                PdfPaperSourceTray tray1 = new PdfPaperSourceTray();
                tray1.StartPage = 2;
                tray1.EndPage = 3;
                tray1.PrintPaperSource = collections[3];
                loPaperSources.Add(tray);
            }
            else
            {
                for (int i = 0; i < collections.Count; i++)
                {
                    PdfPaperSourceTray tray = new PdfPaperSourceTray();
                    tray.StartPage = i * 2;
                    tray.EndPage = (i + 1) * 2;
                    tray.PrintPaperSource = collections[i];
                    loPaperSources.Add(tray);
                    Console.WriteLine("Tray disponivel: " + collections[i].SourceName);

                    /*PdfPaperSourceTray tray = new PdfPaperSourceTray();
                    tray.StartPage = 1;
                    tray.EndPage = 2;
                    tray.PrintPaperSource = collections[3];
                    loPaperSources.Add(tray);*/
                }
            }
            //doc.PageSettings.ListPaperSourceTray = loPaperSources;
            //doc.PrintDocument.Print();
            // test auto-aprove


            Console.ReadLine();
        }
    }
}
