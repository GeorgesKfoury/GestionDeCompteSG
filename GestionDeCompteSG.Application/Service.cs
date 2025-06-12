using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeCompteSG.Application
{
    public class Service : IService
    {
        private readonly IRepository _repository;
        public Service(IRepository repository)
        {
            _repository = repository;
        }
        async Task<IEnumerable<string>> IService.GetCategoriesPlusDebitantesAsync(int nombre)
        {
            var result = await _repository.GetCategoriesPlusDebitantesAsync(nombre);
            return result;
        }

        async Task<float> IService.GetCompteAsync(DateOnly date)
        {
            var result =await _repository.GetCompteAsync(date);
            return result;
        }

        async Task<bool> IService.TraiteCsvAsync(string path)
        {
            await _repository.TraiteCsvAsync(path);
            return true;
        }
    }
}
