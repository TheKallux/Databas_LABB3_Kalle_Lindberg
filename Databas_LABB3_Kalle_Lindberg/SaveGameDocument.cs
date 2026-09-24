using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class SaveGameDocument
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string PlayerName { get; set; } = "";
    public string ClassName { get; set; } = "";
    public bool IsDead { get; set; }
    public int Turns { get; set; }
    public int LevelWidth { get; set; }
    public int LevelHeight { get; set; }
    public PlayerDocument Player { get; set; } = new();
    public List<WallDocument> Walls { get; set; } = new();
    public List<EnemyDocument> Enemies { get; set; } = new();
}

public class PlayerDocument
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Health { get; set; }
}

public class WallDocument
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsDiscovered { get; set; }
}

public class EnemyDocument
{
    public string Type { get; set; } = "";
    public int X { get; set; }
    public int Y { get; set; }
    public int Health { get; set; }
}