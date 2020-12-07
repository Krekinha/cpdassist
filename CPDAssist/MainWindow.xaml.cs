using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Globalization;
using CPDAssit.BNMerge;
using CPDAssist.CCNPJ;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using System.IO.Compression;
using System.Threading;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using CPDAssist.MDFe;
using CPDAssist.Properties;


using System.Data;
using System.Drawing;

using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CPDAssit
{

    public partial class MainWindow : Window
    {
        private Model model { get; set; }
        private ICollectionView cvNFs { get; set; }
        private string pcName = Environment.MachineName;

        public MainWindow()
        {
            InitializeComponent();

            //Atualiza BD
            /*geraDbMdfe.gMDFeConfig();
            geraDbMdfe.gEmitente();
            geraDbMdfe.gMUNCode();
            geraDbMdfe.gVeiculos()*/;

            model = new Model();
            
            dgvConsultaCNPJ.ItemsSource = model.Clientes;
            

            cvNFs = CollectionViewSource.GetDefaultView(model.NFS);
            cvNFs.GroupDescriptions.Add(new PropertyGroupDescription("Placa"));

            dgvXMLNF.ItemsSource = cvNFs;

            ApplyConfig();
        }

        private void ApplyConfig()
        {
            txtPathXMLFabrica.Text = Settings.Default.pathXMLFabrica;
            txtPathAverbacao.Text = Settings.Default.pathAverbacao;
        }

        #region REF NFe

        IList<dadosNF> dnf = new List<dadosNF>(); // Dados do PDF das Danfes
        string pathDanfe = "";                   // Caminho do arquivo NF
        int pagErrNF = 0;                       // Num da pag do arquivo NF que deu erro.

        private void cmdOpenNF_Click(object sender, RoutedEventArgs e)
        {
            //Diálogo que abre o arquivo NF
             Microsoft.Win32.OpenFileDialog opd = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".pdf",                   // Padrão .pdf
                Filter = "PDF Arquivo (*.pdf)|*.pdf", // Somente PDF
                Multiselect = false                  // Soemnte um arquivo
            };

            bool? result = opd.ShowDialog();

            if (result == true)
            {
                // Atribui caminho da NF à variàvel e ao txt
                txtPathNF.Text = opd.FileName;
                pathDanfe = opd.FileName;
                // Converte String em DateTime
                lblDataDoArquivoNF.Content = DataDoArquivoNF(pathDanfe);
                lblNfMsg.Content = "Lendo arquivo PDF...";
                lblNfMsg.Foreground = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#BFFFFFFF");
                if (dnf == null)                // Caso o último work tenha retornado null para dnf
                    dnf = new List<dadosNF>(); // Instancia novamente.

                // Instancia um novo work
                BackgroundWorker workerNF = new BackgroundWorker();
                workerNF.WorkerReportsProgress = true;
                workerNF.DoWork += workerNF_DoWork;
                workerNF.ProgressChanged += workerNF_ProgressChanged;
                workerNF.RunWorkerCompleted += workerNF_RunWorkerCompleted;
                // Inicia o work conforme código em .DoWork
                workerNF.RunWorkerAsync();
            }

        }

        private DateTime DataDoArquivoNF(string path)
        {
            // Converte a string da data de criação do arquivo em DateTime.

            DateTime data = File.GetLastWriteTime(path);
            return data;
        }

        private void workerNF_DoWork(object sender, DoWorkEventArgs e)
        {
            // Todo worker inici aqui suas atividade

            int page;                                       // Out para página do PDF com erro
            GetDados gd = new GetDados();                  // Classe que obtêm os dados da NF
            dnf.Clear();                                  // Clear para não mesclar com dados antigos
            dnf = gd.getNF(pathDanfe, sender, out page); // Passar sender como argumento para que o work funcione em outra classe
            pagErrNF = page;                            // Passa o out para o global
        }

        private void workerNF_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Sempre que houver alteração no progresso do 
            //worker, essa função será chamada

            double max = Convert.ToDouble(e.UserState);        // Progresso atual
            double perc = (e.ProgressPercentage / max) * 100; // Convertido em %
            pbNF.Value = perc;                               // Aplicado ao value do progress
        }

        private void workerNF_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Após finalizar seu trabalho o worker irá chamar essa função
            AnalyzeNF();
        }

        private void AnalyzeNF()
        {
            if (dnf != null)
            {                
                List<int> temp = new List<int>();

                foreach (var item in dnf)
                {
                    temp.Add(item.Num);
                }

                lblIntervaloDeNotas.Content = temp.Min().ToString() + " - " + temp.Max().ToString();
                lblNumPaginasNF.Content = dnf.Count;
                lblNumNotas.Content = temp.Distinct().Count();
                List<int> fullLst = Enumerable.Range(temp.Min(), (temp.Max() - temp.Min()) + 1).ToList();
                int[] diference = fullLst.Except(temp).ToArray();
                string[] str = GroupFaltasNB(diference);
                FaltasNFDetails(str, diference.Count());
                NFMultiPagDetails();
                lblNfMsg.Content = "Arquivo carregado";
                lblNfMsg.Foreground = new SolidColorBrush(Colors.SpringGreen);
                lblNumBoletos.Content = dnf.Where(d => d.Boleto == "S")
                                           .GroupBy(d => d.Num)
                                           .Select(d => d.First()).Count();
            }
            else
            {

                lblNfMsg.Content = "Este arquivo não contém um conjunto válido de DANFE's (pag. " + pagErrNF.ToString() + ")";
                lblNfMsg.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(249, 73, 73));
                lblNumPaginasNF.Content = "-";
                lblNumNotas.Content = "-";
                lblIntervaloDeNotas.Content = "-";
                lstFaltasNF.Items.Clear();
                lstFaltasNF.Items.Add("-");

            }


        }

        private string[] GroupFaltasNB(int[] fullSeq)
        {
            // Linq para agrupar o intervalo de sequências de NFs que faltam no arquivo Ex: 100-110
            var groupedSeq = fullSeq.GroupBy(num => fullSeq.Where(card => card >= num)
                         .OrderBy(card => card).TakeWhile((card, index) => card == num + index)
                         .Last()).Where(seq => seq.Count() >= 2).Select(seq => seq.OrderBy(num => num)).ToList();

            List<int> GroupSeq = new List<int>();
            List<string> result = new List<string>();
            for (int i = 0; i < groupedSeq.Count; i++)
            {
                int[] n = groupedSeq[i].ToArray();
                result.Add(n.Min().ToString() + " a " + n.Max().ToString());
                for (int i2 = 0; i2 < n.Length; i2++)
                {
                    GroupSeq.Add(n[i2]);
                }
            }
            List<int> UngroupSeq = fullSeq.Except(GroupSeq).ToList();
            if (UngroupSeq.Count > 0)
            {
                for (int i = 0; i < UngroupSeq.Count; i++)
                {
                    result.Add(UngroupSeq[i].ToString());
                }
            }


            return result.ToArray();
        }

        private void FaltasNFDetails(string[] faltas, int count)
        {
            lstFaltasNF.Items.Clear();
            if (count > 0)
            {
                Array.Sort(faltas);           
                foreach (string item in faltas)
                {
                    lstFaltasNF.Items.Add("• " + item);
                }
                lstFaltasNF.Items.Add(string.Format("( {0} Notas )", count));
            }
            else
            {
                lstFaltasNF.Items.Add(count);
            }

            
        }

        private void NFMultiPagDetails()
        {
            string wbol = "";
            lstNFMultiPaginas.Items.Clear();
            List<dadosNF> nfMP = dnf
                .Where(d => d.TFolha > 1)
                .GroupBy(d => d.Num)
                .Select(d => d.First()).ToList();

            if (nfMP.Count() > 0)
            {
                foreach (var item in nfMP)
                {
                    if (item.Boleto == "S")
                        wbol = " (BOL)";

                    lstNFMultiPaginas.Items.Add("Rom " + item.Romaneio + " - NF: " + item.Num + wbol);
                    wbol = "";
                }
            }
            else
            {
                lstNFMultiPaginas.Items.Add("0");
            }
        }

        private void cmdReorderNFs_Click(object sender, RoutedEventArgs e)
        {
            if (dnf.Count <= 0)
            {
                System.Windows.MessageBox.Show("Não há dados de Notas Fiscais");
            }
            else
            {
                reorderPagesNF();
            }
        }

        private void reorderPagesNF()
        {
            var result = dnf.OrderBy(x => x.Romaneio).ThenBy(x => x.Boleto == "N").ThenBy(x => x.Num).Select(x => x.Page);
            int[] code = new int[result.Count()];
            for (int i = 0; i < result.Count(); i++)
            {
                code[i] = result.ElementAt(i);
            }
            MergePdf(code);
        }

        private void MergePdf(int[] order)
        {
            var DanfeFile = pathDanfe;
            string output = "";
            var readerNF = new PdfReader(DanfeFile);
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                output = saveFileDialog1.FileName;
                try
                {
                    using (FileStream stream = new FileStream(output, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (Document pdfDoc = new Document(readerNF.GetPageSizeWithRotation(1)))
                        {
                            using (PdfCopy copy = new PdfCopy(pdfDoc, stream))
                            {
                                pdfDoc.Open();

                                copy.SetLinearPageMode();
                                copy.AddDocument(readerNF);

                                copy.ReorderPages(order);

                                if (pdfDoc != null)
                                    pdfDoc.Close();

                            }
                        }
                    }
                }
                catch (IOException io)
                {

                    System.Windows.MessageBox.Show(io.Message);
                }
            }

        }
        private void getPageCode(IList<Romaneio> roms)
        {
            List<string> pagCodeJoin = new List<string>();
            List<PageCode> pageCode = new List<PageCode>();

            int seq = 1;
            int limit = dnf.Count;
            foreach (var item in roms)
            {
                foreach (var bol in item.Grampeados)
                {
                    for (int inf = 0; inf < bol.NFcBol.Count; inf++)
                    {
                        pagCodeJoin.Add("NF-" + bol.NFcBol[inf].Page.ToString());
                        pageCode.Add(new PageCode { Nome = "NF", PageA = bol.NFcBol[inf].Page, PageF = seq });
                        seq++;
                    }
                    pagCodeJoin.Add("BT-" + bol.Boleto.Page.ToString());
                    pageCode.Add(new PageCode { Nome = "BT", PageA = bol.Boleto.Page, PageF = seq, PagMerged = (limit + bol.Boleto.Page) });
                    seq++;
                }
                foreach (var nf in item.NFsBol)
                {
                    pagCodeJoin.Add("NF-" + nf.Page);
                    pageCode.Add(new PageCode { Nome = "NF", PageA = nf.Page, PageF = seq });
                    seq++;
                }
            }
            //decodeOrderPages(pageCode);
        }

        #endregion

        #region REF CCNPJ

        string folderPedidos = Directory.GetCurrentDirectory() + @"\Pedidos\";
        string folderPTPED = @"F:\SOF\VDWIN\PTPED";
        int tConsultados, pConsultados = 0;

        private void mniCopiarSitCNPJ_Click(object sender, RoutedEventArgs e)
        {
            string cnpj = DesmascararCNPJ(((ClienteCNPJ)dgvConsultaCNPJ.CurrentItem).CNPJ.ToString());
        }
        private void mniConsultarNovamente_Click(object sender, RoutedEventArgs e)
        {
            if (pConsultados == tConsultados)
            {
                string cnpj = DesmascararCNPJ(((ClienteCNPJ)dgvConsultaCNPJ.CurrentItem).CNPJ.ToString());
                foreach (ClienteCNPJ item in model.Clientes)
                {
                    if (item.CNPJ == ((ClienteCNPJ)dgvConsultaCNPJ.CurrentItem).CNPJ.ToString())
                    {
                        ClienteCNPJ cli = ConsultaNovamenteCNPJ(item);
                        item.Detalhes = cli.Detalhes;
                        item.Situacao = cli.Situacao;
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Há consultas em andamento. Tente novamente em alguns segundos.");
            }


        }
        private void mniConsultarCNPJ_Click(object sender, RoutedEventArgs e)
        {
            string cnpj = DesmascararCNPJ(((ClienteCNPJ)dgvConsultaCNPJ.CurrentItem).CNPJ.ToString());
            conSintegra cs = new conSintegra(cnpj);
            cs.Show();
        }
        private void cmdMonitorarPedidos_Click(object sender, RoutedEventArgs e)
        {
            MonitorGZ(folderPTPED);
            MonitorTXT();

            FileSystemWatcher fsw = new FileSystemWatcher(folderPedidos, "*.gz");
            fsw.Created += new FileSystemEventHandler(fswGZ_Created);
            fsw.NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime;
            fsw.EnableRaisingEvents = true;
            cmdMonitorarPedidos.Content = "Monitorando ...";
            cmdMonitorarPedidos.IsEnabled = false;
        }
        private void fswGZ_Created(object sender, FileSystemEventArgs e)
        {
            FileInfo gzfile = new FileInfo(e.FullPath);
            int NumberOfRetries = 3000;
            int DelayOnRetry = 10;

            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    Decompress(gzfile);
                    File.Delete(e.FullPath);
                    break; // When done we can break loop
                }
                catch (IOException)
                {
                    // You may check error code to filter some exceptions, not every error
                    // can be recovered.
                    if (i == NumberOfRetries) // Last one, (re)throw exception and exit
                        throw;

                    Thread.Sleep(DelayOnRetry);
                }
            }
        }
        public static void Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
        }
        public void MonitorTXT()
        {
            if (!Directory.Exists(folderPedidos))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), folderPedidos));
            }

            DirectoryInfo di = new DirectoryInfo(folderPedidos);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            FileSystemWatcher fsw = new FileSystemWatcher(folderPedidos, "*.txt");
            fsw.Created += new FileSystemEventHandler(fswTXT_Created);
            fsw.NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime;
            fsw.EnableRaisingEvents = true;

        }
        private void fswTXT_Created(object sender, FileSystemEventArgs e)
        {
            int NumberOfRetries = 3000;
            int DelayOnRetry = 10;

            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    getColeta(e.FullPath, e.Name);
                    break; // When done we can break loop
                }
                catch (IOException)
                {
                    // You may check error code to filter some exceptions, not every error
                    // can be recovered.
                    if (i == NumberOfRetries) // Last one, (re)throw exception and exit
                        throw;

                    Thread.Sleep(DelayOnRetry);
                }
            }
        }
        public void getColeta(string path, string nome)
        {
            string vendedor = nome.Substring(11, 3);
            decimal val = 0;

            List<ClienteCNPJ> clientes = new List<ClienteCNPJ>();
            List<ClienteCNPJ> Fclientes = new List<ClienteCNPJ>();
            List<Pedido> peds = new List<Pedido>();
            var Lines = File.ReadLines(path).Select(a => a.Split('|'));

            foreach (var line in Lines)
            {
                if (line[0] == "CCLI.TXT" && line[15].Substring(9, 4) != "0000")
                {
                    clientes.Add(new ClienteCNPJ
                    {
                        Regiao = int.Parse(line[1]),
                        Numero = int.Parse(line[2]),
                        Razao = line[3],
                        CNPJ = MascararCNPJ(line[15]),
                        IE = line[17],
                        Vendedor = vendedor
                    });
                }
                if (line[0] == "CAPAPEDIDO.TXT")
                {
                    if (decimal.TryParse(line[18], out val))
                    {
                        val = decimal.Parse(line[18].Replace(".", ","));
                        peds.Add(new Pedido { Regiao = int.Parse(line[3]), Numero = int.Parse(line[4]), ValorTotal = val });
                    }

                }
            }

            foreach (var item in peds)
            {
                ClienteCNPJ cli = clientes.Find(x => x.Regiao == item.Regiao && x.Numero == item.Numero);
                if (cli != null)
                {
                    tConsultados += 1;
                }
            }

            Consultados();

            foreach (var item in peds)
            {
                ClienteCNPJ cli = clientes.Find(x => x.Regiao == item.Regiao && x.Numero == item.Numero);
                if (cli != null)
                {
                    cli.Pedido = item;
                    Add:
                    try
                    {
                        ConsultaCNPJ(cli);
                    }
                    catch (Exception)
                    {
                        goto Add;
                    }
                }

            }

        }
        private void ConsultaCNPJ(ClienteCNPJ cli)
        {
            string fileEnt = @"C:\ACBrMonitorPLUS\ent.txt";
            string fileRes = @"C:\ACBrMonitorPLUS\sai.txt";

            if (File.Exists(fileEnt))
                File.Delete(fileEnt);

            if (File.Exists(fileRes))
                File.Delete(fileRes);

            if (cli.IE == "ISENTO" || cli.IE == "")
            {
                cli.Situacao = "ERRO";
                cli.Detalhes = new cadCliente { xMotivo = "CLIENTE SEM IE PARA CONSULTA" };
                model.Clientes.Add(cli);
                pConsultados += 1;
                Consultados();
                goto fim;
            }

            string text = "NFe.ConsultaCadastro(MG," + DesmascararCNPJ(cli.CNPJ) +")";
            File.WriteAllText(fileEnt, text);

            verif:
            if (File.Exists(fileRes))
            {
                cli.Detalhes = lerConsulta(fileRes);
                cli.Situacao = cliSituacao(cli.Detalhes.cSit);
                model.Clientes.Add(cli);
                pConsultados += 1;
                Consultados();
                goto fim;
            }
            else
            {
                goto verif;
            }

            fim:;
        }
        private ClienteCNPJ ConsultaNovamenteCNPJ(ClienteCNPJ cli)
        {
            string fileEnt = @"C:\ACBrMonitorPLUS\ent.txt";
            string fileRes = @"C:\ACBrMonitorPLUS\sai.txt";

            if (File.Exists(fileEnt))
                File.Delete(fileEnt);

            if (File.Exists(fileRes))
                File.Delete(fileRes);

            if (cli.IE == "ISENTO" || cli.IE == "")
            {
                cli.Situacao = "ERRO";
                cli.Detalhes = new cadCliente { xMotivo = "CLIENTE SEM IE PARA CONSULTA" };

                return cli;
            }

            string text = "NFe.ConsultaCadastro(MG," + DesmascararCNPJ(cli.IE) + ",1)";
            File.WriteAllText(fileEnt, text);

            verif:
            if (File.Exists(fileRes))
            {
                cli.Detalhes = lerConsulta(fileRes);
                cli.Situacao = cliSituacao(cli.Detalhes.cSit);
                return cli;
            }
            else
            {
                goto verif;
            }
        }
        private cadCliente lerConsulta(string path)
        {
            string linha;
            string[] campo;
            cadCliente cli = new cadCliente();
            int NumberOfRetries = 3000;
            int DelayOnRetry = 10;

            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    StreamReader file = new StreamReader(path);
                    string cabecalho = file.ReadLine(); //Ler cabeçalho antes para ignorá-lo no loop
                    if (cabecalho.Contains("ERRO"))
                    {
                        cli.cSit = "2";
                        string all = file.ReadToEnd();
                        string[] str = all.Split();
                        foreach (var item in str)
                        {
                            cli.xMotivo += item + " ";
                        }

                        return cli;
                    }
                    while ((linha = file.ReadLine()) != null)
                    {
                        campo = linha.Split('=');

                        if (campo[0] == "cSit")
                            cli.cSit = campo[1];

                        if (campo[0] == "xMotivo")
                            cli.xMotivo = campo[1];

                    }
                    file.Close();
                    return cli;
                }
                catch (IOException)
                {
                    // You may check error code to filter some exceptions, not every error
                    // can be recovered.
                    if (i == NumberOfRetries) // Last one, (re)throw exception and exit
                        throw;

                    Thread.Sleep(DelayOnRetry);
                }
            }


            return cli;

        }
        private string cliSituacao(string situacao)
        {
            if (situacao == "1")
            {
                return "HABILITADO";
            }
            if (situacao == "0")
            {
                return "REJEIÇÃO";
            }
            if (situacao == "2")
            {
                return "ERRO";
            }

            return null;
        }
        private void MonitorGZ(string Opath)
        {
            FileSystemWatcher fsw = new FileSystemWatcher(Opath, "*.gz");
            fsw.Created += new FileSystemEventHandler(fswGZPTPED_Created);
            fsw.NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime;
            fsw.EnableRaisingEvents = true;
        }
        private void fswGZPTPED_Created(object sender, FileSystemEventArgs e)
        {
            int NumberOfRetries = 3000;
            int DelayOnRetry = 10;

            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    // Do stuff with file
                    File.Copy(e.FullPath, folderPedidos + e.Name, true);
                    break; // When done we can break loop
                }
                catch (IOException)
                {
                    // You may check error code to filter some exceptions, not every error
                    // can be recovered.
                    if (i == NumberOfRetries) // Last one, (re)throw exception and exit
                        throw;

                    Thread.Sleep(DelayOnRetry);
                }
            }
        }
        private string MascararCNPJ(string cnpj)
        {
            string _cnpj = String.Format(@"{0:00\.000\.000\/0000\-00}", long.Parse(cnpj));
            return _cnpj;
        }
        private string DesmascararCNPJ(string cnpj)
        {
            return cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
        }
        private void Consultados()
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                lblConsultados.Content = string.Format("Consultados {0}/{1}", pConsultados, tConsultados);
            }));
        }
        #endregion

        #region REF MDFe XML

        private void cmdOpenXMLMDFe_Click(object sender, RoutedEventArgs e)
        {
            /*geraDbMdfe.gMUNCode();
            geraDbMdfe.gEmitente();
            geraDbMdfe.gVeiculos();
            geraDbMdfe.gMDFeConfig();*/


            Microsoft.Win32.OpenFileDialog opd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".xml",                   // Padrão .xml
                Filter = "XML Arquivo (*.xml)|*.xml", // Somente XML
                Multiselect = true                  // vários arquivos
            };

            bool? result = opd.ShowDialog();

            if (result == true)
            {
                foreach (var file in opd.FileNames)
                {
                    string pathXML = file;

                    try
                    {
                        NFMDFe nf = LerXMLNF(pathXML);
                        nf.pathNF = file;

                        for (int i = 0; i < dgvXMLNF.Items.Count; i++)
                        {
                            if (((NFMDFe) dgvXMLNF.Items[i]).NumNF == nf.NumNF)
                            {
                                System.Windows.MessageBox.Show($"Já existe uma NF {nf.NumNF} na lista atual", "Erro ao adicionar NF");
                                goto EndForeach;
                            }

                        }

                        model.NFS.Add(nf);

                        cmdGerarMDFe.IsEnabled = true;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Erro : " + ex.Message);
                    }
                    EndForeach:
                    ;
                }
            }
            
        }

        private NFMDFe LerXMLNF(string _xml)
        {
            NFMDFe nf = new NFMDFe();

            XmlDocument xml = new XmlDocument();
            xml.Load(_xml);

            List<Veiculo> Cars = new Veiculo().getVeiculos();

            try
            {
                XmlNodeList xnInfo = xml.GetElementsByTagName("infNFe");
                foreach (XmlNode xn in xnInfo)
                {
                    nf.CodUF = xn["ide"]["cUF"].InnerText;
                    nf.NumNF = xn["ide"]["nNF"].InnerText;
                    nf.Natureza = xn["ide"]["natOp"].InnerText;
                    nf.Emissao = DateTime.Parse(xn["ide"]["dhEmi"].InnerText);
                    nf.TipoEmissao = xn["ide"]["tpEmis"].InnerText;

                    nf.CNPJEmitente = xn["emit"]["CNPJ"].InnerText;
                    nf.Razao = xn["emit"]["xNome"].InnerText;
                    nf.Origem = xn["emit"]["enderEmit"]["xMun"].InnerText;
                    nf.UFO = xn["emit"]["enderEmit"]["UF"].InnerText;

                    nf.Destino= xn["dest"]["enderDest"]["xMun"].InnerText;
                    nf.UFD = xn["dest"]["enderDest"]["UF"].InnerText;

                    try
                    {
                        nf.Placa = xn["transp"]["veicTransp"]["placa"].InnerText;
                    }
                    catch (Exception)
                    {
                        string inputDefault = "";
                        string err = "";
                        Diag:
                        InputDialog inputDialog = new InputDialog($"Dados do veículo não encontrados no XML de NF {nf.NumNF}.\nInforme o número da placa do veículo.", inputDefault, err);
                        if (inputDialog.ShowDialog() == true)
                        {
                            if (validaPlacaVeiculo(inputDialog.Answer))
                            {
                                nf.Placa = inputDialog.Answer;
                            }
                            else
                            {
                                err = "Informe uma placa no formato:\nABC1234";
                                inputDefault = inputDialog.Answer;
                                goto Diag;
                            }
                        }
                        else
                        {
                            nf.Placa = null;
                        }
                    }

                    
                    nf.Peso = decimal.Parse(xn["transp"]["vol"]["pesoB"].InnerText.Replace(".", ","));

                    nf.ValorNF = decimal.Parse(xn["total"]["ICMSTot"]["vNF"].InnerText.Replace(".", ","));
                    nf.ValICMSP = decimal.Parse(xn["total"]["ICMSTot"]["vICMS"].InnerText.Replace(".", ","));
                    nf.ValICMSST = decimal.Parse(xn["total"]["ICMSTot"]["vST"].InnerText.Replace(".", ","));
                    
                }

                XmlNodeList xnKey = xml.GetElementsByTagName("infProt");
                foreach (XmlNode xn in xnKey)
                {
                    nf.Chaves = new ChavesNfe
                    {
                        Chave = xn["chNFe"].InnerText,
                        SegCodBarras = calcularSegundoCodigoBarras(nf)
                    };
                }

                Veiculo car = Cars.Find(x => x.Placa == nf.Placa);
                nf.Veiculo = car;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Erro : " + ex.Message);
            }
            /*catch (XPathException xpex)
            {

                System.Windows.MessageBox.Show("Erro : " + xpex.Message);
            }*/

            return nf;
        }

        private void TextoToClipboard(string texto)
        {
            System.Windows.Clipboard.SetText(texto);
            toastNotification();
        }

        private void toastNotification()
        {
           
        }

        private void mniRemoverItem_Click(object sender, RoutedEventArgs e)
        {
            model.NFS.Remove((NFMDFe)dgvXMLNF.CurrentItem);
            cvNFs.Refresh();

            if (dgvXMLNF.Items.Count <= 0)
                cmdGerarMDFe.IsEnabled = false;
        }

        private void cmdGerarMDFe_Click(object sender, RoutedEventArgs e)
        {
            int seqMDFe = Settings.Default.seqMDFe;
            string err = "";
            string inputDefault = seqMDFe.ToString();

            Diag:
            InputDialog inputDialog = new InputDialog("Informe o número fiscal do MDFe", inputDefault, err);
            if (inputDialog.ShowDialog() == true)
            {
                if (validaSeqMDFe(inputDialog.Answer))
                {
                    try
                    {
                        seqMDFe = int.Parse(inputDialog.Answer);
                        var cars = new Veiculo().getVeiculos();

                        using (IObjectContainer db = Db4oEmbedded.OpenFile(@".\DBase\dbMdfe.db"))
                        {
                            var nfs = new List<NFMDFe>();
                            nfs.AddRange(dgvXMLNF.Items.OfType<NFMDFe>());

                            //var cars = db.Query<Veiculo>(typeof(Veiculo)).ToList();
                            var emit = db.Query<emit>(typeof(emit));
                            var config = db.Query<MDFConfig>(typeof(MDFConfig));

                            gerarMDFe(new TotaisNFe().GetTotaisNFe(nfs), emit[0], cars, seqMDFe, MDFConfig.StandardConfig());
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message);
                    }

                }
                else
                {
                    err = "O valor deve ser um inteiro";
                    inputDefault = inputDialog.Answer;
                    goto Diag;
                }
            }
        }

        private void gerarMDFe(List<TotaisNFe> nfsTotais, emit emit, List<Veiculo> cars, int numMDFe, MDFConfig config)
        {
            if (nfsTotais == null || nfsTotais.Count <= 0) return;

            var _cUF = new UFCode().getUFCode(emit.enderEmit.UF);
            var conf = MDFConfig.StandardConfig();
            
            
            foreach (var nfsT in nfsTotais)
            {
                string chaveOri = $"{_cUF}{DateTime.Now:yyMM}{emit.CNPJ}" +
                      $"{conf.modeloDoc}{conf.serieDoc:000}{numMDFe:000000000}{conf.tipoEmissao}" +
                      $"{numMDFe:00000000}";
                int DV = gerarCVMDFe(chaveOri);

                var mdfe = new MDFe();

                mdfe.infMDFe.id = $"MDFe{chaveOri}{DV}";
                mdfe.infMDFe.versao = conf.verLayout;
                mdfe.infMDFe.ide.cUF = _cUF;
                mdfe.infMDFe.ide.tpAmb = conf.tipoAmbiente;
                mdfe.infMDFe.ide.tpEmit = conf.tipoEmitente;
                mdfe.infMDFe.ide.mod = conf.modeloDoc;
                mdfe.infMDFe.ide.serie = conf.serieDoc.ToString();
                mdfe.infMDFe.ide.nMDF = numMDFe;
                mdfe.infMDFe.ide.cMDF = $"{numMDFe:00000000}";
                mdfe.infMDFe.ide.cDV = DV;
                mdfe.infMDFe.ide.modal = 1;
                mdfe.infMDFe.ide.dhEmi = $"{DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local):yyyy-MM-ddTHH:mm:ssK}";
                mdfe.infMDFe.ide.tpEmis = conf.tipoEmissao;
                mdfe.infMDFe.ide.procEmi = 3;
                mdfe.infMDFe.ide.verProc = conf.verProc;
                mdfe.infMDFe.ide.UFIni = nfsT.UFIni;
                mdfe.infMDFe.ide.UFFim = nfsT.UFFim;

                string cMUNC = new MUNCode().getMyMUNCode(nfsT.MUNIni);
                if (cMUNC != "")
                    mdfe.infMDFe.ide.infMunCarrega.cMunCarrega = cMUNC;
                if (cMUNC == "")
                    mdfe.infMDFe.ide.infMunCarrega.cMunCarrega = new MUNCode().getMUNCode(nfsT.MUNIni);
                mdfe.infMDFe.ide.infMunCarrega.xMunCarrega = nfsT.MUNIni;

                mdfe.infMDFe.emit = emit;

                mdfe.infMDFe.infModal.versaoModal = conf.verLayout;
                //mdfe.infMDFe.infModal.rodo.veicTracao.cInt = nfsT.Placa;
                mdfe.infMDFe.infModal.rodo.infANTT.RNTRC = nfsT.Rntrc;
                mdfe.infMDFe.infModal.rodo.veicTracao.placa = nfsT.Placa;
                mdfe.infMDFe.infModal.rodo.veicTracao.tara = nfsT.Tara;
                mdfe.infMDFe.infModal.rodo.veicTracao.capKG = nfsT.CapKG;
                mdfe.infMDFe.infModal.rodo.veicTracao.prop.CPF = nfsT.CPFProprietario;
                mdfe.infMDFe.infModal.rodo.veicTracao.prop.RNTRC = nfsT.Rntrc;
                mdfe.infMDFe.infModal.rodo.veicTracao.prop.xNome = nfsT.NomeProprietario;
                mdfe.infMDFe.infModal.rodo.veicTracao.prop.IE = nfsT.IEProprietario;
                mdfe.infMDFe.infModal.rodo.veicTracao.prop.UF = nfsT.UFProprietario;
                mdfe.infMDFe.infModal.rodo.veicTracao.prop.tpProp = nfsT.TipoProprietario;
                mdfe.infMDFe.infModal.rodo.veicTracao.condutor.xNome = nfsT.Motorista;
                mdfe.infMDFe.infModal.rodo.veicTracao.condutor.CPF = nfsT.CPFMotorista;
                mdfe.infMDFe.infModal.rodo.veicTracao.tpRod = nfsT.TPRod;
                mdfe.infMDFe.infModal.rodo.veicTracao.tpCar = nfsT.TPCar;
                mdfe.infMDFe.infModal.rodo.veicTracao.UF = nfsT.UFCar;

                string cMUND = new MUNCode().getMyMUNCode(nfsT.MUNFim);
                if (cMUND != "")
                    mdfe.infMDFe.infDoc.infMunDescarga.cMunDescarga = cMUND;
                if (cMUND == "")
                    mdfe.infMDFe.infDoc.infMunDescarga.cMunDescarga = new MUNCode().getMUNCode(nfsT.MUNFim);

                mdfe.infMDFe.infDoc.infMunDescarga.xMunDescarga = nfsT.MUNFim;

                foreach (var ch in nfsT.Chaves)
                {
                    mdfe.infMDFe.infDoc.infMunDescarga.Chave.Add(new infNFe {chNFe = ch.Chave});
                }

                //SEGURO
                mdfe.infMDFe.seg.infResp.respSeg = 2;
                mdfe.infMDFe.seg.infResp.CNPJ = "06326025000166";
                mdfe.infMDFe.seg.infSeg.xSeg = "BUONNY PROJETOS";
                mdfe.infMDFe.seg.infSeg.CNPJ = "06326025000166";
                mdfe.infMDFe.seg.nApol = "87372016010621000195";
                mdfe.infMDFe.seg.nAver = "581597026";

                //TOTAIS
                mdfe.infMDFe.tot.qNFe = nfsT.NFS.Count;
                mdfe.infMDFe.tot.vCarga = nfsT.TotalValor;
                mdfe.infMDFe.tot.cUnid = "01";
                mdfe.infMDFe.tot.qCarga = nfsT.TotalPeso;
                mdfe.infMDFe.infAdic.infCpl = nfsT.infAdc;

                mdfe.Signature.SignedInfo.Reference.URI = $"#MDFe{chaveOri}{DV}";
                mdfe.Signature.SignedInfo.Reference.Transforms.Add(new CPDAssist.MDFe.Transform() { Algorithm = "http://www.w3.org/2000/09/xmldsig#enveloped-signature" });
                mdfe.Signature.SignedInfo.Reference.Transforms.Add(new CPDAssist.MDFe.Transform() { Algorithm = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315" });

                XmlSerializer writer = new XmlSerializer(typeof(MDFe));
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;
                settings.DoNotEscapeUriAttributes = false;

                var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"//MDFE//MDFe-{nfsT.Motorista.Substring(0, nfsT.Motorista.IndexOf(" "))}-{nfsT.Placa}-{nfsT.UFIni}-{nfsT.UFFim}-{DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc):dd.MM.yy}.xml";

                using (XmlWriter sw = XmlWriter.Create(path, settings))
                {
                    writer.Serialize(sw, mdfe);
                }
                numMDFe += 1;
                gravaSeqMDFe(numMDFe);

                if (nfsT.MUNIni != "ITAOBIM")
                    salvarXMLFabrica(nfsT.PathXmlFabrica, Settings.Default.pathXMLFabrica, $"XML FABRICA { DateTime.SpecifyKind(nfsT.nfsEmissao, DateTimeKind.Utc):MM-yyyy}");

                salvarXMLPastaAverbacao(nfsT.PathXmlFabrica, Settings.Default.pathAverbacao);
            }
        }

        private int gerarCVMDFe(string chave)
        {
            string chaveInvertida = ReverseString(chave);
            int[] t = { 2, 3, 4, 5, 6, 7, 8, 9 };
            int somatorio = 0;
            int posicaoParaCalculo = 0;
            foreach (var v in chaveInvertida)
            {

                somatorio = somatorio + (int.Parse(v.ToString()) * t[posicaoParaCalculo]);
                if (posicaoParaCalculo == 7)
                {
                    posicaoParaCalculo = 0;
                }
                else
                {
                    posicaoParaCalculo += 1;
                }
            }

            int resto = somatorio % 11;
            int dv;
            if (resto == 0 || resto == 1)
            {
                dv = 0;
            }
            else
            {
                dv = (11 - resto);
            }

            return dv;
        }

        public string ReverseString(string strChave)
        {
            char[] arrChar = strChave.ToCharArray();
            Array.Reverse(arrChar);
            string invertida = new String(arrChar);

            return invertida;
        }

        private void gravaSeqMDFe(int seq)
        {
            Settings.Default.seqMDFe = seq;
            Settings.Default.Save();
        }

        private bool validaSeqMDFe(string seq)
        {
            int outSeq;
            return int.TryParse(seq, out outSeq);
        }

        private bool validaPlacaVeiculo(string input)
        {
            var regex = @"^[a-zA-Z]{3}\d{4}$";
            var match = Regex.Match(input, regex);

            if (!match.Success)
            {
                return false;
            }
            return true;
        }

        private void cmdRemoverSelecao_Click(object sender, RoutedEventArgs e)
        {
            var newItens = new List<NFMDFe>();
            foreach (var item in model.NFS)
            {
                if (!item.IsDelete)
                {
                    newItens.Add(item);
                    
                }
            }
            model.NFS.Clear();
            foreach (var item in newItens)
            {
                model.NFS.Add(item);
            }
            cvNFs.Refresh();
        }

        private void chkHeader_Click(object sender, RoutedEventArgs e)
        {
            if(chkHeader.IsChecked == true)
            {
                foreach (var item in model.NFS)
                {
                    item.IsDelete = true;
                }
            }
            else
            {
                foreach (var item in model.NFS)
                {
                    item.IsDelete = false;
                }
            }
        }

        private void chkItem_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkHeader.IsChecked == true)
            {
                chkHeader.IsChecked = false;
            }
        }

        private void cmdPathXMLFabrica_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Settings.Default.pathXMLFabrica = fbd.SelectedPath;
                    Settings.Default.Save();
                    txtPathXMLFabrica.Text = Settings.Default.pathXMLFabrica;
                }
            }
        }

        private void cmdGerarPlanilha_Click(object sender, RoutedEventArgs e)
        {
            var nfs = new List<NFMDFe>();
            nfs.AddRange(dgvXMLNF.Items.OfType<NFMDFe>());

            var saveFileDialogCatalogo = new Microsoft.Win32.SaveFileDialog();

            if (saveFileDialogCatalogo.ShowDialog() == true)
            {
                ArquivoExcelAverbacao arq = new ArquivoExcelAverbacao(nfs);
                arq.GerarArquivo(saveFileDialogCatalogo.FileName);
                System.Windows.MessageBox.Show("O arquivo " +
                    saveFileDialogCatalogo.FileName +
                    " foi gerado com sucesso!");
            }

        }

        private void cmdAddAlterarVeiculo_Click(object sender, RoutedEventArgs e)
        {
            string path = "DBase\\cars.csv";
            System.Diagnostics.Process.Start("notepad.exe", path);
        }

        private void salvarXMLFabrica(string[] sourceFiles, string targetDirectory, string nomePasta)
        {
            try
            {
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                foreach (var item in sourceFiles)
                {
                    var filename = item.Split('\\').Last();
                    var targetXML = targetDirectory + $"\\{nomePasta}\\"; //$"\\XML FABRICA {DateTime.SpecifyKind(nf.Emissao, DateTimeKind.Utc):MM-yyyy}\\";

                    if (!Directory.Exists(targetXML))
                    {
                        Directory.CreateDirectory(targetXML);
                    }

                    File.Copy(item, targetXML + filename, true);
                }
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show($"Erro ao copiar XML para a pasta {targetDirectory} \n\n Erro: {ex.Message}");
            }
        }

        private void cmdPathAverbacao_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Settings.Default.pathAverbacao = fbd.SelectedPath;
                    Settings.Default.Save();
                    txtPathAverbacao.Text = Settings.Default.pathAverbacao;
                }
            }
        }

        private void salvarXMLPastaAverbacao(string[] sourceFiles, string targetDirectory)
        {
            try
            {
                if (!Directory.Exists(targetDirectory))
                {
                    System.Windows.MessageBox.Show($"Pasta {targetDirectory} não encontrada");
                }

                foreach (var item in sourceFiles)
                {
                    var filename = item.Split('\\').Last();
                    var targetXML = targetDirectory + $"\\";

                    File.Copy(item, targetXML + filename, true);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erro ao copiar XML para a pasta {targetDirectory} \n\n Erro: {ex.Message}");
            }
        }

        private string calcularSegundoCodigoBarras(NFMDFe _nf)
        {
            if (_nf.TipoEmissao == "2" || _nf.TipoEmissao == "5")
            {
                return "codigo";
            }
            else
            {
                return null;
            }
        }

        #endregion


    }
}
