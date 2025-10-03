using System;
using ExecutiveDisorder.Core.Entity;
using ExecutiveDisorder.Engine;

namespace ExecutiveDisorder.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Executive Disorder...");
            
            var engine = new GameEngine();
            var player = new GameObject("Player", 0, 0);
            engine.AddGameObject(player);

            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                engine.Update();
                engine.Render();
                
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        isRunning = false;
                    }
                }

                System.Threading.Thread.Sleep(16); // ~60 FPS
            }

            Console.WriteLine("Game ended. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
