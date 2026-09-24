using MongoDB.Bson;
using MongoDB.Driver;

public class GameRepository
{
    private readonly IMongoDatabase _db;
    private readonly IMongoCollection<CharacterClassDocument> _classes;
    private readonly IMongoCollection<SaveGameDocument> _saves;

    public GameRepository()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        _db = client.GetDatabase("KalleLindberg");
        _classes = _db.GetCollection<CharacterClassDocument>("PlayerClasses");
        _saves = _db.GetCollection<SaveGameDocument>("SaveGames");
    }

    // Character classes - seed & read

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

    public async Task<CharacterClassDocument?> GetClassByNameAsync(string name)
    {
        return await _classes.Find(c => c.Name == name).FirstOrDefaultAsync();
    }

    // Saved classes - CRUD

    // Create + Update: new game has empty id, existing game has id set.
    public async Task SaveGameAsync(SaveGameDocument save)
    {
        if (save.Id == ObjectId.Empty)
            await _saves.InsertOneAsync(save);   // Setting save.Id after insert
        else
            await _saves.ReplaceOneAsync(s => s.Id == save.Id, save);
    }

    // Read: only characters that are still alive
    public async Task<List<SaveGameDocument>> GetLivingSavesAsync()
    {
        return await _saves.Find(s => !s.IsDead).ToListAsync();
    }

    // Delete
    public async Task DeleteSaveAsync(ObjectId id)
    {
        await _saves.DeleteOneAsync(s => s.Id == id);
    }
}