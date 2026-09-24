# Dungeon Crawler with MongoDB

A console-based dungeon crawler written in C# (.NET 8). Progress is stored in MongoDB, so you can save and quit at any time and continue later with the same character.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- MongoDB Community Server running on `localhost:27017`

No database needs to be created beforehand. On startup the app creates the database and seeds the character classes if they do not already exist.

## Run

```bash
git clone https://github.com/TheKallux/Databas_LABB3_Kalle_Lindberg.git
cd Databas_LABB3_Kalle_Lindberg
dotnet run --project Databas_LABB3_Kalle_Lindberg
```

## How to play

| Key | Action |
|---|---|
| Arrow keys | Move |
| Escape | Save and quit |

Walk into enemies (or let them reach you) to fight. If your character dies, it can no longer be loaded.

From the start menu you can:

- create a new character (name + class)
- continue with a saved, living character
- delete a saved character

## Database

Database name: `KalleLindberg`

| Collection | Content |
|---|---|
| `PlayerClasses` | Character classes (name, start HP, symbol). Seeded with Warrior, Wizard and Thief. Classes added or edited in the database are picked up by the game. |
| `SaveGames` | One document per character: name, class, turns, dead flag, player state, walls (discovered or not) and enemies (type, position, HP). |

All database calls are asynchronous and go through `GameRepository` using `MongoDB.Driver`.

## Project structure

- `GameRepository.cs`: all database access (create, read, update, delete)
- `SaveGameDocument.cs`, `CharacterClassDocument.cs`: database models
- `LevelData.cs`: level state, including converting to and from save documents
- `GameLoop.cs`: start menu, game loop, saving and death handling
- `Combat.cs`, `Player.cs`, `Enemy.cs`, `Rat.cs`, `Snake.cs`, `Wall.cs`: game logic
