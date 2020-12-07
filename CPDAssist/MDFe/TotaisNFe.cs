using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace CPDAssist.MDFe
{
    public class TotaisNFe
    {
        public string Placa { get; set; }
        public string Motorista { get; set; }
        public string CPFMotorista { get; set; }
        public string Rntrc { get; set; }
        public string CPFProprietario { get; set; }
        public string NomeProprietario { get; set; }
        public string IEProprietario { get; set; }
        public string UFProprietario { get; set; }
        public string TipoProprietario { get; set; }
        public int Tara { get; set; }
        public int CapKG { get; set; }
        public string TPRod { get; set; }
        public string TPCar { get; set; }
        public string UFCar { get; set; }
        public string UFIni { get; set; }
        public string MUNIni { get; set; }
        public string UFFim { get; set; }
        public string MUNFim { get; set; }
        public List<string> NFS = new List<string>();
        public List<ChavesNfe> Chaves = new List<ChavesNfe>();
        public DateTime nfsEmissao { get; set; } 
        public decimal TotalValor { get; set; }
        public string TotalPeso { get; set; }
        public string infAdc { get; set; }
        public string[] PathXmlFabrica { get; set; }

        public List<TotaisNFe> GetTotaisNFe(List<NFMDFe> nfs)
        {
            
            var lTotNFe = new List<TotaisNFe>();
            var grpNFS = nfs.GroupBy(x => x.Placa);

            foreach (var item in grpNFS)
            {
                var car = item.Select(x => x.Veiculo).First();
                var dataNFsEmissao = item.Select(x => x.Emissao);
                if (item.Select(x => x.Veiculo).First() == null)
                {
                    System.Windows.MessageBox.Show($"Veículo {item.Select(x => x.Placa).First()} não esta cadastrado");
                    continue;
                }
                var _ufIni = item.Select(x => x.UFO).Distinct();
                var _munIni = item.Select(x => x.Origem).Distinct();
                var _ufFim = item.Select(x => x.UFD).Distinct();
                var _munFim = item.Select(x => x.Destino).Distinct();

                var dupNF = item.GroupBy(s => s.NumNF).SelectMany(grp => grp.Skip(1)).Select(x => x.NumNF);

                var totNFe = new TotaisNFe();

                totNFe.Placa = item.Key;

                if (_ufIni.Count() == 1)
                {
                    totNFe.UFIni = _ufIni.First();
                }
                else
                {
                    MessageBox.Show(
                        $"O MDFe referente a placa {item.Key} não será gerado. " +
                        $"Motivo: Placa com diferentes pontos (UF) de origem");
                    continue;
                }

                if (_munIni.Count() == 1)
                {
                    totNFe.MUNIni = _munIni.First();
                }
                else
                {
                    MessageBox.Show(
                        $"O MDFe referente a placa {item.Key} não será gerado. " +
                        $"Motivo: Placa com diferentes pontos (MUNICIPIO) de origem");
                    continue;
                }

                totNFe.Motorista = item.Select(x => x.Veiculo.Motoristas.First().Nome).First();
                totNFe.CPFMotorista = item.Select(x => x.Veiculo.Motoristas.First().CPF).First();

                if (_ufFim.Count() == 1)
                {
                    totNFe.UFFim = _ufFim.First();
                }
                else
                {
                    MessageBox.Show(
                        $"O MDFe referente a placa {item.Key} não será gerado. " + 
                        $"Motivo: Placa com diferentes pontos (UF) de destino");
                    continue;
                }

                if (_munFim.Count() == 1)
                {
                    totNFe.MUNFim = _munFim.First();
                }
                else
                {
                    MessageBox.Show(
                        $"O MDFe referente a placa {item.Key} não será gerado. " +
                        $"Motivo: Placa com diferentes pontos (MUNICIPIO) de destino");
                    continue;
                }

                if (!dupNF.Any())
                {
                    totNFe.NFS.AddRange(item.Select(x => x.NumNF));
                }
                else
                {
                    MessageBox.Show(
                        $"O MDFe referente a placa {item.Key} não será gerado. " +
                        $"Motivo: Placa com duplicidade na NF ({dupNF.First()})");
                    continue;
                }

                totNFe.Tara = item.Select(x => x.Veiculo.Tara).First();
                totNFe.CapKG = item.Select(x => x.Veiculo.CapKG).First();
                totNFe.Chaves.AddRange(item.Select(x => x.Chaves));
                totNFe.TPRod = item.Select(x => x.Veiculo.TPRod).First();
                totNFe.TPCar = item.Select(x => x.Veiculo.TPCar).First();
                totNFe.UFCar = item.Select(x => x.Veiculo.UF).First();
                totNFe.Rntrc = car.PropRNTRC;
                totNFe.NomeProprietario = car.PropxNome;
                totNFe.CPFProprietario = car.PropCPF;
                totNFe.IEProprietario = car.PropIE;
                totNFe.UFProprietario = car.PropUF;
                totNFe.TipoProprietario = car.PropTipo;

                totNFe.TotalValor = item.Sum(x => x.ValorNF);
                totNFe.TotalPeso = item.Sum(n => n.Peso).ToString("N4", CultureInfo.CreateSpecificCulture("pt-BR")).Replace(".", "").Replace(",", ".");

                var Tnfs = totNFe.NFS.Aggregate("", (current, nf) => current + (nf + ", "));
                totNFe.infAdc = "REF A NF(s) " + Tnfs.Remove(Tnfs.LastIndexOf(','));
                totNFe.PathXmlFabrica = item.Select(x => x.pathNF).ToArray();
                totNFe.nfsEmissao = item.Select(x => x.Emissao).First();

                lTotNFe.Add(totNFe);

            }
            return lTotNFe;
        }
    }
}
