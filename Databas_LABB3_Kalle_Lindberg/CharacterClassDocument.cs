using MongoDB.Bson;

public class CharacterClassDocument
{
    public ObjectId Id { get; set; }
    public string Name { get; set; } = "";
    public int StartHp { get; set; }
    public char Symbol { get; set; }
}