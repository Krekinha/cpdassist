using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o;

namespace CPDAssist.MDFe
{
    public class UFCode
    {
        public int Cod { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }


        public List<UFCode> setUFCode()
        {
            var ufs = new List<UFCode>();
            ufs.Add(new UFCode { Cod = 11, Nome = "Rondônia", Sigla = "RO" });
            ufs.Add(new UFCode { Cod = 12, Nome = "Acre", Sigla = "AC" });
            ufs.Add(new UFCode { Cod = 13, Nome = "Amazonas", Sigla = "AM" });
            ufs.Add(new UFCode { Cod = 14, Nome = "Roraima", Sigla = "RR" });
            ufs.Add(new UFCode { Cod = 15, Nome = "Pará", Sigla = "PA" });
            ufs.Add(new UFCode { Cod = 16, Nome = "Amapá", Sigla = "AP" });
            ufs.Add(new UFCode { Cod = 17, Nome = "Tocantins", Sigla = "TO" });
            ufs.Add(new UFCode { Cod = 21, Nome = "Maranhão", Sigla = "MA" });
            ufs.Add(new UFCode { Cod = 22, Nome = "Piauí", Sigla = "PI" });
            ufs.Add(new UFCode { Cod = 23, Nome = "Ceará", Sigla = "CE" });
            ufs.Add(new UFCode { Cod = 24, Nome = "Rio Grande do Norte", Sigla = "RN" });
            ufs.Add(new UFCode { Cod = 25, Nome = "Paraíba", Sigla = "PB" });
            ufs.Add(new UFCode { Cod = 26, Nome = "Pernambuco", Sigla = "PE" });
            ufs.Add(new UFCode { Cod = 27, Nome = "Alagoas", Sigla = "AL" });
            ufs.Add(new UFCode { Cod = 28, Nome = "Sergipe", Sigla = "SE" });
            ufs.Add(new UFCode { Cod = 29, Nome = "Bahia", Sigla = "BA" });
            ufs.Add(new UFCode { Cod = 31, Nome = "Minas Gerais", Sigla = "MG" });
            ufs.Add(new UFCode { Cod = 32, Nome = "Espírito Santo", Sigla = "ES" });
            ufs.Add(new UFCode { Cod = 33, Nome = "Rio de Janeiro", Sigla = "RJ" });
            ufs.Add(new UFCode { Cod = 35, Nome = "São Paulo", Sigla = "PR" });
            ufs.Add(new UFCode { Cod = 41, Nome = "Paraná", Sigla = "RO" });
            ufs.Add(new UFCode { Cod = 42, Nome = "Santa Catarina", Sigla = "SC" });
            ufs.Add(new UFCode { Cod = 43, Nome = "Rio Grande do Sul", Sigla = "RS" });
            ufs.Add(new UFCode { Cod = 50, Nome = "Mato Grosso do Sul", Sigla = "MS" });
            ufs.Add(new UFCode { Cod = 51, Nome = "Mato Grosso", Sigla = "MT" });
            ufs.Add(new UFCode { Cod = 52, Nome = "Goiás", Sigla = "GO" });
            ufs.Add(new UFCode { Cod = 53, Nome = "Distrito Federal", Sigla = "DF" });

            return ufs;

        }

        public int getUFCode(string uf)
        {
            var ufs = setUFCode().Where(x => x.Sigla == uf).Select(x => x.Cod);
            return ufs.First();
        }
    }

    public class MUNCode
    {
        public string Cod { get; set; }
        public string Nome { get; set; }


        public List<MUNCode> setMyMUNCode()
        {
            var muns = new List<MUNCode>();
            muns.Add(new MUNCode { Cod = "3133303", Nome = "ITAOBIM"});
            muns.Add(new MUNCode { Cod = "2900702", Nome = "ALAGOINHAS" });
            muns.Add(new MUNCode { Cod = "3304557", Nome = "RIO DE JANEIRO" });
            muns.Add(new MUNCode { Cod = "3300803", Nome = "CACHOEIRAS DE MACACU" });
            muns.Add(new MUNCode { Cod = "3523909", Nome = "ITU" });
            muns.Add(new MUNCode { Cod = "5200308", Nome = "ALEXANIA" });

            return muns;

        }


        public string getMUNCode(string mun)
        {
            using (IObjectContainer db = Db4oEmbedded.OpenFile("dbMdfe.db"))
            {
                var _muns = db.Query<MUNCode>(typeof(MUNCode)).ToList();
                return _muns.Where(x => x.Nome == mun).Select(x => x.Cod).First();
            }
        }

        public string getMyMUNCode(string mun)
        {
            var muns = setMyMUNCode().Where(x => x.Nome == mun).Select(x => x.Cod);

            return muns != null ? muns.First() : "";
        }
    }
}
