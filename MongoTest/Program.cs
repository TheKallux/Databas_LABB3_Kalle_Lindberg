var repo = new GameRepository();
await repo.SeedClassesAsync();

foreach (var c in await repo.GetClassesAsync())
    Console.WriteLine($"{c.Name} ({c.StartHp} HP)");