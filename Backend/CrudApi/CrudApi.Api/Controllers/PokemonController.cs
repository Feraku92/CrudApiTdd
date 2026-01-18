using CrudApi.Application.Dtos;
using CrudApi.Application.Interfaces;
using CrudApi.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrudApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PokemonController : Controller
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var pokemons = _pokemonService.GetAllPokemons();
            return Ok(pokemons);
        }

        [HttpGet("search")]
        public IActionResult GetByNameOrNumber([FromQuery] string? name, [FromQuery] int? number)
        {
            var pokemon = _pokemonService.GetByNameOrNumber(name, number);
            if (pokemon == null) return NotFound(new { message = "No Pokémon found" });
            return Ok(pokemon);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] PokemonRequest request)
        {
            try
            {
                var pokemon = _pokemonService.CreatePokemon(request);
                return Ok(pokemon);
            }
            catch (DuplicatePokedexIdException ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] PokemonRequest request)
        {
            var updatedPokemon = _pokemonService.UpdatePokemon(id, request);
            if (updatedPokemon == null)
                return NotFound(new { message = "Pokemon not found" });
            return Ok(updatedPokemon);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var deleted = _pokemonService.DeletePokemon(id);
            if (!deleted) return NotFound(new { message = "Pokemon not found" });
            return NoContent();
        }
    }
}
