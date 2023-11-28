using System;

namespace Bunzhia;

public static class Program
{
    public static Game? game;
    [STAThread]
    static void Main(string[] args)
    {
        using(Game game = new Game(1280, 720))
            game.Run();
    }
}