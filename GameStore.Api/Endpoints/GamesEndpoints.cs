using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Repositories;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";


    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {

        var group = routes.MapGroup("/games").WithParameterValidation();

        group.MapGet("/", (IGamesRepository repository) => 
            repository.GetAll().Select(game => game.AsDto()));

        group.MapGet("/{id}", (IGamesRepository repository, int id) =>
        {
            Game? game = repository.Get(id);

			return game is not null ? Results.Ok(game) : Results.NotFound();

        }
         ).WithName(GetGameEndpointName);

        group.MapPost("/", (IGamesRepository repository, CreateGameDto gameDto) =>
        {
            Game game = new()
            {
                Name = gameDto.Name,
                Genre = gameDto.Genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                ImageUrl = gameDto.ImageUrl
            };

				repository.Create(game);

            return Results.CreatedAtRoute(GetGameEndpointName, 
							new { id = game.Id }, game);
        });

        group.MapPut("/{id}", (IGamesRepository repository, int id, UpdateGameDto updateGameDto) =>
        {

            Game? gameOld = repository.Get(id);
            if (gameOld is null)
            {
                return Results.NotFound();
            }

            gameOld.Name = updateGameDto.Name;
            gameOld.Genre = updateGameDto.Genre;
            gameOld.Price = updateGameDto.Price;
            gameOld.ReleaseDate = updateGameDto.ReleaseDate;
            gameOld.ImageUrl = updateGameDto.ImageUrl;

			repository.Update(gameOld);

            return Results.NoContent();

        });

        group.MapDelete("/{id}", (IGamesRepository repository, int id) =>
        {
            Game? game = repository.Get(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            repository.Delete(game.Id);

            return Results.NoContent();
        });

        return group;
    }
}
