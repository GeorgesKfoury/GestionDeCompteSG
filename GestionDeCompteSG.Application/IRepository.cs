using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeCompteSG.Application
{
    public interface IRepository
    {
        Task<float> GetCompteAsync(DateOnly date);
        Task<IEnumerable<string>> GetCategoriesPlusDebitantesAsync(int nombre);
        Task<bool> TraiteCsvAsync(string path);
    }
}
