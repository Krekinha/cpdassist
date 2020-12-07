using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
namespace CPDAssist.MDFe
{
    public class geraDbMdfe
    {
        public static void gVeiculos()
        {
            using (IObjectContainer db = Db4oEmbedded.OpenFile("..\\..\\DBase\\dbMdfe.db"))
            {
                var cars = new List<Veiculo>();

                var mot1 = new Motorista() { CPF = "05355520685", Matricula = "568", Nome = "EDUARDO DOS REIS BATISTA" };
                var mot2 = new Motorista() { CPF = "49337190630", Matricula = "569", Nome = "FABIANO OLIVEIRA DE PAULA" };
                var mot3 = new Motorista() { CPF = "03959336683", Matricula = "571", Nome = "FERNANDO LOPES SANTOS" };
                var mot4 = new Motorista() { CPF = "38711273615", Matricula = "567", Nome = "MARCELO CAPOVILLA" };
                var mot5 = new Motorista() { CPF = "05476723811", Matricula = "572", Nome = "PAULO HENRIQUE SARAIVA" };
                var mot6 = new Motorista() { CPF = "08196321635", Matricula = "574", Nome = "THIAGO CAETANO SOARES" };
                var mot7 = new Motorista() { CPF = "05739590647", Matricula = "573", Nome = "VENI PEREIRA DA COSTA" };
                var mot8 = new Motorista() { CPF = "04102545689", Matricula = "575", Nome = "JOSE FRANCISCO VIEIRA JUNIOR" };
                var mot9 = new Motorista() { CPF = "73690716691", Matricula = "576", Nome = "GENARIO CERQUEIRA DE CARVALHO" };

                cars.Add(new Veiculo { Placa = "HXO2461", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot1 } });
                cars.Add(new Veiculo { Placa = "JSO9224", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot2 } });
                cars.Add(new Veiculo { Placa = "DPB7119", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot3 } });
                cars.Add(new Veiculo { Placa = "MET3489", Tara = 19000, CapKG = 39800, TPRod = "06", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot4 } });
                cars.Add(new Veiculo { Placa = "BXI4406", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot5 } });
                cars.Add(new Veiculo { Placa = "AMZ8419", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot6 } });
                cars.Add(new Veiculo { Placa = "MEB4051", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot7 } });
                cars.Add(new Veiculo { Placa = "JQU7507", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot8 } });
                cars.Add(new Veiculo { Placa = "MQP8818", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot9 } });

                foreach (var car in cars)
                {
                    db.Store(car);
                }
                db.Close();
            }
        }

        public static void gEmitente()
        {
            using (IObjectContainer db = Db4oEmbedded.OpenFile("..\\..\\DBase\\dbMdfe.db"))
            {
                var emit = new emit();
                emit.CNPJ = "21795927000488";
                emit.IE = "4432571780338";
                emit.xNome = "DISTRIBUIDORA ALIANCA LTDA";
                emit.xFant = "DISTRIBUIDORA ALIANCA";
                emit.enderEmit.xLgr = "RUA COMERCINHO";
                emit.enderEmit.nro = "834";
                emit.enderEmit.xBairro = "SANTO ANTONIO";
                emit.enderEmit.cMun = 3133303;
                emit.enderEmit.xMun = "ITAOBIM";
                emit.enderEmit.CEP = "39625000";
                emit.enderEmit.UF = "MG";
                emit.enderEmit.fone = "3337341734";
                emit.enderEmit.email = "contato@aliancaitaobim.com.br";

                db.Store(emit);
                db.Close();
            }
        }

        public static void gMUNCode()
        {
            using (IObjectContainer db = Db4oEmbedded.OpenFile("..\\..\\DBase\\dbMdfe.db"))
            {
                var muns = new List<MUNCode>();
                var reader = new StreamReader(File.OpenRead("..\\..\\DBase\\xmun.csv"), Encoding.GetEncoding("iso-8859-1"));
                while (!reader.EndOfStream)

                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    muns.Add(new MUNCode {Cod = values[0], Nome = RemoveAccents(values[1]).ToUpper() });
                }

                

                foreach (var mun in muns)
                {
                    db.Store(mun);
                }
                db.Close();
            }
        }

        public static void gMDFeConfig()
        {
            using (IObjectContainer db = Db4oEmbedded.OpenFile("..\\..\\DBase\\dbMdfe.db"))
            {
                var config = MDFConfig.StandardConfig();

                db.Store(config);
                db.Close();
            }
        }

        public static string RemoveAccents(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }
    }
}
