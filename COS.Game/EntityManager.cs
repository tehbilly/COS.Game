using System;
using System.Collections.Generic;
using System.Linq;

namespace COS.Game
{
    public class EntityManager
    {
        private readonly HashSet<Entity> _entities = new HashSet<Entity>();
        private readonly Dictionary<Type, List<Entity>> _entitiesByComponent = new Dictionary<Type, List<Entity>>();

        public Entity CreateEntity(string name = "")
        {
            var entity = new Entity(name);
            Add(entity);
            return entity;
        }
        
        public void Add(Entity entity)
        {
            if (_entities.Contains(entity)) return;

            if (entity.EntityManager != null)
                throw new InvalidOperationException("Cannot add an entity that has already been added to an EntityManager");

            entity.EntityManager = this;
            entity.Initialize();

            _entities.Add(entity);
        }

        public bool Contains(Entity entity) => _entities.Contains(entity);

        public void Remove(Entity entity)
        {
            if (!_entities.Contains(entity)) return;
            entity.EntityManager = null;
        }

        internal void EntityComponentAdded(Entity entity, Type type)
        {
            if (!_entitiesByComponent.ContainsKey(type))
                _entitiesByComponent.Add(type, new List<Entity>());

            _entitiesByComponent[type].Add(entity);
        }

        public IEnumerable<Entity> EntitiesWithComponent<T>() where T : Component
        {
            return _entitiesByComponent[typeof(T)];
        }

        public IEnumerable<Entity> EntitiesWithAll(params Component[] components)
        {
            // TODO: Test
            // TODO: See if we can do a union 
            return _entities.Where(entity => entity.HasAllComponents(components));
        }
    }
}