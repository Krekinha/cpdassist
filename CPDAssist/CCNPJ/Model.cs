using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CPDAssist.MDFe;

namespace CPDAssist.CCNPJ
{
    public class Model
    {
        public ObservableCollection<ClienteCNPJ> Clientes;
        public ObservableCollection<NFMDFe> NFS;
        private static object _syncLock = new object();

        public Model()
        {
            Clientes = new ObservableCollection<ClienteCNPJ>();
            NFS = new ObservableCollection<NFMDFe>();

            BindingOperations.EnableCollectionSynchronization(Clientes, _syncLock);
            BindingOperations.EnableCollectionSynchronization(NFS, _syncLock);
        }
    }
}
