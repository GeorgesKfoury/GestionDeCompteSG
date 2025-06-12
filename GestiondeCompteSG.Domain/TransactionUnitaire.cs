using GestionDeCompteSG.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeCompteSG.Domain
{
    public class TransactionUnitaire
    {
        public required DateOnly Date { get; set; }
        public required float Montant { get; set; }
        public required Devise Devise { get; set; }
        public required string Categorie { get; set; }
    }
}
