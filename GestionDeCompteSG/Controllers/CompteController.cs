using GestionDeCompteSG.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeCompteSG.Controllers
{
    [ApiController]
    public class CompteController : ControllerBase
    {
        private readonly IService _service;

        public CompteController(IService service)
        {
            _service = service;
        }
        [HttpGet(ApiEndpoints.Comptes.Get)]
        public async Task<IActionResult> Get([FromRoute] DateOnly date)
        {
            return Ok(await _service.GetCompteAsync(date));
        }
        [HttpGet(ApiEndpoints.Comptes.GetCategoriesDedebits)]
        public async Task<IActionResult> GetCategoriesPlusDebitantes([FromRoute] int nombre)
        {
            return Ok(await _service.GetCategoriesPlusDebitantesAsync(nombre));
        }
    }
}
