using HackathonAPI.Entities;
using HackathonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HackathonAPI.Controllers
{
    [ApiController]
    [Route("/api/categorias")]
    public class CargosController : ControllerBase
    {
        private readonly ILivroService _livroService;

        public CargosController(ILivroService livroService) =>
            _livroService = livroService;

        [HttpGet]
        public ActionResult<List<Categoria>> ObterCategorias()
        {
            var categorias = _livroService.ObterCategoriasDeLivros();
            if (categorias == null || categorias.Count == 0)
                return NoContent();

            return Ok(categorias);
        }
    }
}