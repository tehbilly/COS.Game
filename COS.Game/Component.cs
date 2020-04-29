using System;

namespace COS.Game
{
    public class Component
    {
        public Guid ID { get; internal set; }
        public Entity Entity { get; internal set; }
    }
}