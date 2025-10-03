using System;

namespace ExecutiveDisorder.Core.Entity
{
    public class GameObject
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public bool IsActive { get; set; } = true;

        public GameObject(string name, float x = 0, float y = 0)
        {
            Name = name;
            X = x;
            Y = y;
        }
    }
}
