using System;
using System.Collections.Generic;
using System.Linq;

namespace COS.Game
{
    public class Entity
    {
        public Guid ID { get; } = Guid.NewGuid();
        public EntityManager? EntityManager { get; internal set; }
        public string Name { get; set; } = "";

        internal readonly HashSet<Component> Components = new HashSet<Component>();
        internal readonly HashSet<Type> ComponentTypes = new HashSet<Type>();

        public Entity(string? name = null)
        {
            if (name != null)
                Name = name;
        }

        /// <summary>
        /// Implement this method in a subclass to perform entity initialization logic, such as adding default components.
        /// </summary>
        public virtual void Initialize() {}
        
        public void AddComponent(Component component)
        {
            var type = component.GetType();
            if (!ComponentTypes.Contains(type))
            {
                ComponentTypes.Add(type);
                // Let the entity manager know we've had a component added
                EntityManager?.EntityComponentAdded(this, type);
            }
            Components.Add(component);
        }

        public bool HasComponent<T>() => HasComponent(typeof(T));
        
        public bool HasComponent(Type type) => ComponentTypes.Any(c => c == type);

        internal bool HasAllComponents(params Component[] components)
        {
            return components.All(c => ComponentTypes.Contains(c.GetType()));
        }

        public T GetComponent<T>() where T : Component =>
            Components.FirstOrDefault(c => c.GetType() == typeof(T)) as T;

        protected bool Equals(Entity other)
        {
            return ID.Equals(other.ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Entity) obj);
        }

        public override int GetHashCode() => ID.GetHashCode();

        public override string ToString() => $"{GetType().Name}: {Name}";
    }
}