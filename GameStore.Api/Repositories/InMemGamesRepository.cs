using GameStore.Api.Entities;

namespace GameStore.Api.Repositories;

public class InMemGamesRepository : IGamesRepository
{
    private readonly List<Game> games = new()
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


    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await Task.FromResult(games);
    }

    public async Task<Game?> GetAsync(int id)
    {
        return await Task.FromResult(games.Find(g => g.Id == id));
    }

    public async Task CreateAsync(Game game)
    {
        game.Id = games.Max(g => g.Id) + 1;
        games.Add(game);

        await Task.CompletedTask;

    }

    public async Task UpdateAsync(Game updatedGame)
    {
        var index = games.FindIndex(game => game.Id == updatedGame.Id);
        games[index] = updatedGame;

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var index = games.FindIndex(game => game.Id == id);
        games.RemoveAt(index);
        
        await Task.CompletedTask;
    }

}
