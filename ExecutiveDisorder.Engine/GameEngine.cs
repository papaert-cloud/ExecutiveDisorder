using System;
using System.Collections.Generic;
using ExecutiveDisorder.Core.Entity;

namespace ExecutiveDisorder.Engine
{
    public class GameEngine
    {
        private List<GameObject> gameObjects = new List<GameObject>();
        
        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        public void RemoveGameObject(string id)
        {
            gameObjects.RemoveAll(go => go.Id == id);
        }

        public void Update()
        {
            // Update game logic here
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.IsActive)
                {
                    // Update object logic
                }
            }
        }

        public void Render()
        {
            // Render game objects here
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.IsActive)
                {
                    Console.WriteLine($"Rendering {gameObject.Name} at ({gameObject.X}, {gameObject.Y})");
                }
            }
        }
    }
}
