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

        group.MapGet("/", async (IGamesRepository repository) => 
            (await repository.GetAllAsync()).Select(game => game.AsDto()));

        group.MapGet("/{id}", async (IGamesRepository repository, int id) =>
        {
            Game? game = (await repository.GetAsync(id));

			return game is not null ? Results.Ok(game) : Results.NotFound();

        }
         ).WithName(GetGameEndpointName);

        group.MapPost("/", async (IGamesRepository repository, CreateGameDto gameDto) =>
        {
            Game game = new()
            {
                Name = gameDto.Name,
                Genre = gameDto.Genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                ImageUrl = gameDto.ImageUrl
            };

				await repository.CreateAsync(game);

            return Results.CreatedAtRoute(GetGameEndpointName, 
							new { id = game.Id }, game);
        });

        group.MapPut("/{id}", async (IGamesRepository repository, int id, UpdateGameDto updateGameDto) =>
        {

            Game? gameOld = await repository.GetAsync(id);
            if (gameOld is null)
            {
                return Results.NotFound();
            }

            gameOld.Name = updateGameDto.Name;
            gameOld.Genre = updateGameDto.Genre;
            gameOld.Price = updateGameDto.Price;
            gameOld.ReleaseDate = updateGameDto.ReleaseDate;
            gameOld.ImageUrl = updateGameDto.ImageUrl;

			await repository.UpdateAsync(gameOld);

            return Results.NoContent();

        });

        group.MapDelete("/{id}", async (IGamesRepository repository, int id) =>
        {
            Game? game = await repository.GetAsync(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            await repository.DeleteAsync(game.Id);

            return Results.NoContent();
        });

        return group;
    }
}
