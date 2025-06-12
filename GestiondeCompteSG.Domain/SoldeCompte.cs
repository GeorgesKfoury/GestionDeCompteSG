using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeCompteSG.Domain
{
    public class SoldeCompte
    {
        public required DateOnly Date { get; set; }
        public required float Amount { get; set; } 
    }
}
