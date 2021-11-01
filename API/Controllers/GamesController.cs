using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class GamesController : BaseApiController
    {
        private readonly IGameRepository _gameRepository;

        public GamesController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<GameInfoDto>>> GetUsers()
            => throw new NotImplementedException();

        [HttpGet("{id}")]
        public async Task<ActionResult<GameInfoDto>> GetGame(int id)
            => await _gameRepository.GetGameInfoAsync(id);
    }
}
