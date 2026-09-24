using MongoDB.Driver;

public class GameRepository
{
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<CharacterClassDocument> _classes;

    public GameRepository()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        _db = client.GetDatabase("KalleLindberg");
        _classes = _db.GetCollection<CharacterClassDocument>("PlayerClasses");
    }

    public async Task SeedClassesAsync()
    {
        var count = await _classes.CountDocumentsAsync(_ => true);
        if (count > 0) return;

        await _classes.InsertManyAsync(new[]
        {
            new CharacterClassDocument { Name = "Warrior", StartHp = 120, Symbol = '@' },
            new CharacterClassDocument { Name = "Wizard",  StartHp = 80,  Symbol = '@' },
            new CharacterClassDocument { Name = "Thief",   StartHp = 100, Symbol = '@' }
        });
    }

    public async Task<List<CharacterClassDocument>> GetClassesAsync()
    {
        return await _classes.Find(_ => true).ToListAsync();
    }
}