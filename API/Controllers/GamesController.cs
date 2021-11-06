using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models.Pagination;
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
        private readonly IGamesService _gamesService;
        private readonly IMapper _mapper;

        public GamesController(IGamesService gamesService, IMapper mapper)
        {
            _gamesService = gamesService;
            _mapper = mapper;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GameInfoDto>>> SearchGames(string query, int pageNumber, int pageSize)
        {
            var gamesParams = new GamesParams
            {
                Query = query,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var results = await _gamesService.SearchGamesAsync(gamesParams);

            Response.AddPaginationHeader(results.CurrentPage, results.PageSize,
                results.TotalCount, results.TotalPages);

            return Ok(_mapper.Map<IEnumerable<GameInfoDto>>(results));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameDetailsDto>> GetGameDetails(long id)
        {
            var game = await _gamesService.GetGameDetailsAsync(id);
            return _mapper.Map<GameDetailsDto>(game);
        }
    }
}
