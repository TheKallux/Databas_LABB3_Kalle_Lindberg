namespace LABB2;

class Program
{
    static async Task Main(string[] args)
    {
        var repo = new GameRepository();
        await repo.SeedClassesAsync();

        GameLoop game = new GameLoop(repo);
        await game.StartAsync();
    }
}