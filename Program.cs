using System;
using System.IO;
using System.Text.Json;
using CodersVsZombies.Game;
using CodersVsZombies.MonteCarlo;

var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
var baseOutputPath = "./result";

var Timestamp = () => DateTime.Now.ToString("yyyyMMddHHmmssffff");

foreach (string file in Directory.EnumerateFiles("./data", "*.json"))
{
    
    Console.WriteLine($"\n ---------------------- \nStarting New Game");
    Console.WriteLine($"Board: {file}");
    var gameOutputBasePath = $"{baseOutputPath}/{file.Replace(".json", "")}";
    var actualRunningPath = $"{gameOutputBasePath}/{Timestamp()}";
    if (!Directory.Exists(gameOutputBasePath))
        Directory.CreateDirectory(gameOutputBasePath);
    
    Directory.CreateDirectory(actualRunningPath);
    using var outputFile = new StreamWriter(actualRunningPath + "/game-log.txt", true);
    
    string contents = File.ReadAllText(file);
    var board = JsonSerializer.Deserialize<Board>(contents);
    
    var game = new Game(board.CurrentPlayer, board.Humans, board.Zombies, board.Score);
    var round = 1;

    outputFile.WriteLine($"Game board: {file}");
    Console.WriteLine("Playing the game...");
    while(game.Actions.Any() && !game.GameOver) {
        outputFile.WriteLine($"Round: {round}");
        outputFile.WriteLine($"Zombie left: {game.Zombies.Count}");
        outputFile.WriteLine($"Humans left: {game.Humans.Count}");

        Directory.CreateDirectory($"{actualRunningPath}/{round}");
        File.WriteAllText($"{actualRunningPath}/{round}/game.json", JsonSerializer.Serialize(game, serializerOptions));

        var output = MonteCarloTreeSearch.GetTopActions(game, timeBudget: Math.Max(5000 - (round * 250), 100)).ToList();
        
        File.WriteAllText($"{actualRunningPath}/{round}/monte-carlo-output.json", JsonSerializer.Serialize(output, serializerOptions));
        outputFile.WriteLine($"Action choosen: MOVE TO ({output.First().Action.Position.X}, {output.First().Action.Position.Y})");
        
        round++;
        game.ApplyAction(output.First().Action);
    }

    Console.WriteLine($"Result: {(game.Loose ? "LOOSEER... ZOMBIE WINS" : "YOU WIN!" )}");
    Console.WriteLine($"Score: {game.Score}");

    outputFile.WriteLine($"Result: {(game.Loose ? "LOOSEER... ZOMBIE WINS" : "YOU WIN!" )}");
    outputFile.WriteLine($"Score: {game.Score}");
}
