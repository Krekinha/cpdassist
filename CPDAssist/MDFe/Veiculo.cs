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
