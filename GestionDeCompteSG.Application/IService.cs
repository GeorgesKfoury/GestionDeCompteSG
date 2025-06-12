using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeCompteSG.Application
{
    public interface IService
    {
        Task<bool> TraiteCsvAsync(string path);
        Task<float> GetCompteAsync(DateOnly date);
        Task<IEnumerable<string>> GetCategoriesPlusDebitantesAsync(int nombre);
    }
}
