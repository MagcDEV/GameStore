using GameStore.Api.Entities;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    static List<Game> games = new()
    {
        new Game()
        {
            Id = 1,
            Name = "God of War",
            Genre = "Action",
            Price = 19.99M,
            ReleaseDate = new DateTime(1991, 2, 1),
            ImageUrl = "https://placehold.co/301",
        },
        new Game()
        {
            Id = 2,
            Name = "Street Figther",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateTime(1991, 2, 1),
            ImageUrl = "https://placehold.co/300",
        },
        new Game()
        {
            Id = 3,
            Name = "Final Fantasy",
            Genre = "RPG",
            Price = 59.99M,
            ReleaseDate = new DateTime(2010, 2, 1),
            ImageUrl = "https://placehold.co/300",
        }
    };
    public static RouteGroupBuilder MapGamesEndpoints(this IEndpointRouteBuilder routes)
    {

        var group = routes.MapGroup("/games").WithParameterValidation();

        group.MapGet("/", () => games);
        group.MapGet("/{id}", (int id) =>
        {
            Game? game = games.Find(g => g.Id == id);

            if (game is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(game);
        }
         ).WithName(GetGameEndpointName);

        group.MapPost("/", (Game game) =>
        {
            game.Id = games.Max(g => g.Id) + 1;
            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        group.MapPut("/{id}", (int id, Game game) =>
        {

            Game? gameOld = games.Find(g => g.Id == id);
            if (gameOld is null)
            {
                return Results.NotFound();
            }

            gameOld.Name = game.Name;
            gameOld.Genre = game.Genre;
            gameOld.Price = game.Price;
            gameOld.ReleaseDate = game.ReleaseDate;
            gameOld.ImageUrl = game.ImageUrl;

            return Results.NoContent();

        });

        group.MapDelete("/{id}", (int id) =>
        {
            Game? game = games.Find(g => g.Id == id);

            if (game is null)
            {
                return Results.NotFound();
            }

            games.Remove(game);

            return Results.NoContent();
        });

        return group;
    }
}