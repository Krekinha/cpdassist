using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace CPDAssist.MDFe
{
    public class Veiculo
    {
        public string Placa { get; set; }
        public int Tara { get; set; }
        public int CapKG { get; set; }
        public string TPRod { get; set; }
        public string TPCar { get; set; }
        public string UF { get; set; }
        public string PropRNTRC { get; set; }
        public string PropCPF { get; set; }
        public string PropxNome { get; set; }
        public string PropIE { get; set; }
        public string PropUF { get; set; }
        public string PropTipo { get; set; }

        [FileExtensions]
        public ObservableCollection<Motorista> Motoristas { get; set; }

        public List<Veiculo> getVeiculos()
        {
            var cars = new List<Veiculo>();

            var reader = new StreamReader(File.OpenRead("DBase\\cars.csv"), Encoding.GetEncoding("iso-8859-1"));

            var  cabecalho = reader.ReadLine(); //Ler cabeçalho antes para ignorá-lo no loop

            while (!reader.EndOfStream)

            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                cars.Add(new Veiculo {
                    Placa = values[0],
                    Tara = int.Parse(values[1]),
                    CapKG = int.Parse(values[2]),
                    TPRod = values[3],
                    TPCar = values[4],
                    UF = values[5],
                    Motoristas = new ObservableCollection<Motorista> {
                        new Motorista {
                            Nome = RemoveAccents(values[6]).ToUpper(),
                            CPF = values[7]
                        }
                    },
                    PropRNTRC = values[8],
                    PropxNome = values[9],
                    PropCPF = values[10],
                    PropIE = string.Empty,
                    PropUF = values[11],
                    PropTipo = values[12]
                });
            }

            #region Hard-Coded

            /*var mot1 = new Motorista() { CPF = "05355520685", Matricula = "568", Nome = "EDUARDO DOS REIS BATISTA" };
            var mot2 = new Motorista() { CPF = "49337190630", Matricula = "569", Nome = "FABIANO OLIVEIRA DE PAULA" };
            var mot3 = new Motorista() { CPF = "03959336683", Matricula = "571", Nome = "FERNANDO LOPES SANTOS" };
            var mot4 = new Motorista() { CPF = "38711273615", Matricula = "567", Nome = "MARCELO CAPOVILLA" };
            var mot5 = new Motorista() { CPF = "05476723811", Matricula = "572", Nome = "PAULO HENRIQUE SARAIVA" };
            var mot6 = new Motorista() { CPF = "08196321635", Matricula = "574", Nome = "THIAGO CAETANO SOARES" };
            var mot7 = new Motorista() { CPF = "05739590647", Matricula = "573", Nome = "VENI PEREIRA DA COSTA" };
            var mot8 = new Motorista() { CPF = "04102545689", Matricula = "575", Nome = "JOSE FRANCISCO VIEIRA JUNIOR" };
            var mot9 = new Motorista() { CPF = "73690716691", Matricula = "576", Nome = "GENARIO CERQUEIRA DE CARVALHO" };
            var mot10 = new Motorista() { CPF = "06529196692", Matricula = "577", Nome = "RODINEI DE ARAUJO LIMA" };
            var mot11 = new Motorista() { CPF = "10186156600", Matricula = "579", Nome = "SAMUEL SANTOS DE SOUZA" };
            var mot12 = new Motorista() { CPF = "53016793768", Matricula = "580", Nome = "FRANCISCO PEREIRA NETO" };
            var mot13 = new Motorista() { CPF = "09207226600", Matricula = "567", Nome = "MARCELO CAPOVILLA FILHO" };
            var mot14 = new Motorista() { CPF = "90261771515", Matricula = "583", Nome = "MILTON FABIANO ABREU SANTOS" };
            var mot15 = new Motorista() { CPF = "54107512649", Matricula = "584", Nome = "RENIVALDO ALVES FERNANDES" };
            var mot16 = new Motorista() { CPF = "00740320629", Matricula = "585", Nome = "FABIANO RODRIGUES FREIRE" };
            var mot17 = new Motorista() { CPF = "07754960656", Matricula = "990", Nome = "DOUGLAS VILAS BOAS CAPOVILLA" };
            var mot18 = new Motorista() { CPF = "02620097657", Matricula = "991", Nome = "IVANIR DE JESUS RIBEIRO" };
            var mot19 = new Motorista() { CPF = "13092876601", Matricula = "998", Nome = "BRENO LOPES DE OLIVEIRA" };

            cars.Add(new Veiculo { Placa = "HXO2461", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot1 } });
            cars.Add(new Veiculo { Placa = "JSO9224", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot2 } });
            cars.Add(new Veiculo { Placa = "DPB7119", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot3 } });
            cars.Add(new Veiculo { Placa = "MET3489", Tara = 19000, CapKG = 39800, TPRod = "06", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot17 } });
            cars.Add(new Veiculo { Placa = "BXI4406", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot5 } });
            cars.Add(new Veiculo { Placa = "AMZ8419", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot6 } });
            cars.Add(new Veiculo { Placa = "MEB4051", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot3 } });
            cars.Add(new Veiculo { Placa = "JQU7507", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot8 } });
            cars.Add(new Veiculo { Placa = "MQP8818", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot9 } });
            cars.Add(new Veiculo { Placa = "GMW1910", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot11 } });
            cars.Add(new Veiculo { Placa = "GML5508", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot12 } });
            cars.Add(new Veiculo { Placa = "LZL5792", Tara = 19000, CapKG = 30500, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot3 } });
            cars.Add(new Veiculo { Placa = "JLR3156", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot15 } });
            cars.Add(new Veiculo { Placa = "BWA9391", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot16 } });
            cars.Add(new Veiculo { Placa = "HBG1255", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot18 } });
            cars.Add(new Veiculo { Placa = "HBG1257", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot18 } });
            cars.Add(new Veiculo { Placa = "NJI9504", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot5 } });
            cars.Add(new Veiculo { Placa = "GOV0040", Tara = 19000, CapKG = 39000, TPRod = "02", TPCar = "00", UF = "MG", Motoristas = new ObservableCollection<Motorista> { mot19 } });
            */

            #endregion

            return cars;
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
