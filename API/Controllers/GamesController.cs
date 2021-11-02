using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class GamesController : BaseApiController
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGamesService _gamesService;
        private readonly IMapper _mapper;

        public GamesController(IGameRepository gameRepository, IGamesService gamesService, IMapper mapper)
        {
            _gameRepository = gameRepository;
            _gamesService = gamesService;
            _mapper = mapper;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GameInfoDto>>> SearchGames(string query)
        {
            var results = await _gamesService.SearchGamesAsync(query);
            return Ok(_mapper.Map<IEnumerable<GameInfoDto>>(results));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameDetailsDto>> GetGameDetails(long id)
        {
            var game = await _gamesService.GetGameDetailsAsync(id);
            return _mapper.Map<GameDetailsDto>(game);
        }

        [HttpGet("library/{username}")]
        public async Task<ActionResult<IEnumerable<GameInfoDto>>> GetLibrary(string username)
        {
            var userGames = await _gameRepository.GetLibraryAsync(username);
            var library = new List<GameInfoDto>();

            foreach (var userGame in userGames)
            {
                var game = await _gamesService.GetGameAsync(userGame.Id);
                library.Add(_mapper.Map<GameInfoDto>(game));
            }

            return Ok(library);
        }
    }
}
