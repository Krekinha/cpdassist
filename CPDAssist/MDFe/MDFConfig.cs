using CPDAssist.Properties;

namespace CPDAssist.MDFe
{
    public class MDFConfig
    {
        public string pathXMLFabrica { get; set; }
        public string pathAverbacao { get; set; }
        public string pathMDFe { get; set; }
        public string verProc { get; set; }
        public string verLayout { get; set; }
        public int modeloDoc { get; set; }
        public int serieDoc { get; set; }
        public int tipoEmissao { get; set; }
        public int tipoEmitente { get; set; }
        public int tipoAmbiente { get; set; }

        public static MDFConfig StandardConfig()
        {
            MDFConfig conf = new MDFConfig
            {
                //PathXMLFabrica = @"Z:\CPD\ARQUIVOS IMPORTANTES - CPD\XML - FABRICAS BRASIL KIRIN\",
                pathXMLFabrica = Settings.Default.pathXMLFabrica,
                pathAverbacao = Settings.Default.pathAverbacao,
                pathMDFe = Settings.Default.PathMDFe, //@"C:\Users\CPD\Desktop\MDFE\",
                verProc = Settings.Default.verProc,
                verLayout = Settings.Default.verLayout,
                tipoEmissao = Settings.Default.tipoEmissao,
                tipoEmitente = Settings.Default.tipoEmitente,
                tipoAmbiente = Settings.Default.tipoAmbiente,
                modeloDoc = Settings.Default.modeloDoc,
                serieDoc = Settings.Default.serieDoc
            };
            return conf;
        }
    }
}