using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeCompteSG.Domain
{
    public class TransactionsDeDebitParCategorie
    {
        public required string Categorie { get; set; }
        public required int montant { get; set; } = 0;
    }
}
