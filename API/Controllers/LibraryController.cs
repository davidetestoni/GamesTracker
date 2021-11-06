using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class LibraryController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGamesService _gamesService;
        private readonly IMapper _mapper;

        public LibraryController(IUnitOfWork unitOfWork, IGamesService gamesService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _gamesService = gamesService;
            _mapper = mapper;
        }

        [HttpPost("add-game")]
        public async Task<ActionResult<LibraryGameInfoDto>> AddGame(AddGameDto addGameDto)
        {
            var gameId = addGameDto.Id;

            // Make sure the user is logged in
            var username = User.GetUsername();
            var userId = User.GetUserId();

            if (username is null || userId is null)
            {
                return Unauthorized();
            }

            // Check if the game exists in the db, otherwise fetch it and add it
            var gameEntity = await _unitOfWork.GameRepository.GetGameByIdAsync(gameId);

            if (gameEntity is null)
            {
                var videoGame = await _gamesService.GetGameAsync(gameId);
                gameEntity = _mapper.Map<Game>(videoGame);
                _unitOfWork.GameRepository.Add(gameEntity);
                await _unitOfWork.Complete();
            }

            // Check if the user already has this game in the list
            var userGame = await _unitOfWork.LibraryRepository.GetUserGameAsync(username, gameId);
            if (userGame is not null)
            {
                return BadRequest("You already have this game in your library");
            }

            var userEntity = await _unitOfWork.UserRepository.GetUserByIdAsync(userId.Value);

            // Check if the user still exists
            if (userEntity is null)
            {
                return Unauthorized();
            }

            // Add the UserGame entity
            userGame = new UserGame
            {
                SourceUserId = userId.Value,
                GameId = gameId,
                Status = addGameDto.Status,
                FinishedOn = addGameDto.FinishedOn,
                UserRating = addGameDto.UserRating
            };

            if (userEntity.Games is null)
            {
                userEntity.Games = new List<UserGame>();
            }

            if (gameEntity.PlayedBy is null)
            {
                gameEntity.PlayedBy = new List<UserGame>();
            }

            userEntity.Games.Add(userGame);
            gameEntity.PlayedBy.Add(userGame);

            if (await _unitOfWork.Complete())
            {
                return _mapper.Map<LibraryGameInfoDto>(userGame);
            }

            return BadRequest("Failed to add game to library");
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<LibraryGameInfoDto>>> GetLibrary(string username)
        {
            var userGames = await _unitOfWork.LibraryRepository.GetAllAsync(username);
            return Ok(userGames.Select(ug => _mapper.Map<LibraryGameInfoDto>(ug)));
        }

        [HttpGet("games/{gameId}")]
        public async Task<ActionResult<LibraryGameInfoDto>> GetGame(int gameId)
        {
            // Make sure the user is logged in
            var username = User.GetUsername();

            if (username is null)
            {
                return Unauthorized();
            }

            var userGame = await _unitOfWork.LibraryRepository.GetUserGameAsync(username, gameId);

            if (userGame is null)
            {
                return null;
            }

            return _mapper.Map<LibraryGameInfoDto>(userGame);
        }

        [HttpDelete("remove-game/{gameId}")]
        public async Task<ActionResult> RemoveGame(int gameId)
        {
            // Make sure the user is logged in
            var username = User.GetUsername();
            var userId = User.GetUserId();

            if (username is null || userId is null)
            {
                return Unauthorized();
            }

            var userEntity = await _unitOfWork.UserRepository.GetUserByIdAsync(userId.Value);

            // Check if the user still exists
            if (userEntity is null)
            {
                return Unauthorized();
            }

            var userGame = await _unitOfWork.LibraryRepository.GetUserGameAsync(username, gameId);

            if (userGame is null)
            {
                return BadRequest("The game you are trying to remove is not in your library");
            }

            userEntity.Games.Remove(userGame);

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }

            return BadRequest("Failed to remove game from library");
        }

        [HttpPut("update-game")]
        public async Task<ActionResult> UpdateGame(UpdateGameDto updateGameDto)
        {
            // Make sure the user is logged in
            var username = User.GetUsername();

            if (username is null)
            {
                return Unauthorized();
            }

            var userGame = await _unitOfWork.LibraryRepository.GetUserGameAsync(username, updateGameDto.Id);

            if (userGame is null)
            {
                return BadRequest("The game you are trying to update is not in your library");
            }

            userGame.Status = updateGameDto.Status;
            userGame.FinishedOn = updateGameDto.FinishedOn;
            userGame.UserRating = updateGameDto.UserRating;

            _unitOfWork.LibraryRepository.Update(userGame);
            
            if (await _unitOfWork.Complete())
            {
                return Ok();
            }

            return BadRequest("Failed to update game");
        }
    }
}
