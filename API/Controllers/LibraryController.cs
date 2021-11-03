﻿using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class LibraryController : BaseApiController
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IGamesService _gamesService;
        private readonly IMapper _mapper;

        public LibraryController(ILibraryRepository libraryRepository, IUserRepository userRepository,
            IGameRepository gameRepository, IGamesService gamesService, IMapper mapper)
        {
            _libraryRepository = libraryRepository;
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _gamesService = gamesService;
            _mapper = mapper;
        }

        [HttpPost("add-game")]
        public async Task<ActionResult> AddGame(AddGameDto addGameDto)
        {
            var gameId = addGameDto.Id;

            // Make sure the user is logged in
            var username = User.GetUsername();
            var userId = User.GetUserId();

            if (username is null)
            {
                return Unauthorized();
            }

            // Check if the game exists in the db, otherwise fetch it and add it
            var gameEntity = await _gameRepository.GetGameByIdAsync(gameId);

            if (gameEntity is null)
            {
                var videoGame = await _gamesService.GetGameAsync(gameId);
                gameEntity = _mapper.Map<Game>(videoGame);
                _gameRepository.Add(gameEntity);
                await _gameRepository.SaveAllAsync();
            }

            // Check if the user already has this game in the list
            var userGame = await _libraryRepository.GetUserGameAsync(userId.Value, gameId);
            if (userGame is not null)
            {
                return BadRequest("You already have this game in your library");
            }

            var userEntity = await _userRepository.GetUserByIdAsync(userId.Value);

            // Check if the user still exists
            if (userEntity is null)
            {
                return Unauthorized();
            }

            // Add the UserGame entity
            userGame = new UserGame
            {
                SourceUserId = userId.Value,
                GameId = gameId
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

            // TODO: Use unit of work pattern to save changes!
            if (await _userRepository.SaveAllAsync())
            {
                return Ok();
            }

            return BadRequest("Failed to add game to library");
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<LibraryGameInfoDto>>> GetUser(string username)
            => Ok(await _libraryRepository.GetAsync(username));

        [HttpDelete("remove-game/{gameId}")]
        public async Task<ActionResult> RemoveGame(int gameId)
        {
            // Make sure the user is logged in
            var username = User.GetUsername();
            var userId = User.GetUserId();

            if (username is null)
            {
                return Unauthorized();
            }

            var userEntity = await _userRepository.GetUserByIdAsync(userId.Value);

            // Check if the user still exists
            if (userEntity is null)
            {
                return Unauthorized();
            }

            var userGame = await _libraryRepository.GetUserGameAsync(userId.Value, gameId);

            if (userGame is null)
            {
                return BadRequest("The game you are trying to remove is not in your library");
            }

            userEntity.Games.Remove(userGame);
            await _userRepository.SaveAllAsync();

            return Ok();
        }
    }
}
