using Aplicacao.AppService.Personagem;
using Aplicacao.AppService.Personagem.DTO;
using Infra.Repositorio.Personagens.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CRUDBasico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonagemController : ControllerBase
    {
        private readonly IPersonagemAppService _appService;

        public PersonagemController(IPersonagemAppService appService)
        {
            _appService = appService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PersonagemAppService), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InserirPersonagem([FromBody] PersonagemRequest request)
        {
            var response = await _appService.InserirPersonagemAsync(request);

            return Created(string.Empty, response);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(PersonagemAppService), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditarPersonagem([FromBody] PersonagemEditarRequest request)
        {
            var response = await _appService.EditarPersonagemAsync(request);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PersonagemAppService), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTodosOsPersonagens()
        {
            var personagens = _appService.GetTodosOsPersonagens();

            return Ok(personagens);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(PersonagemAppService), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApagarPersonagem(int personagemId)
        {
            var response = await _appService.ApagarPersonagemAsync(personagemId);

            return Ok(response);
        }
    }
}
