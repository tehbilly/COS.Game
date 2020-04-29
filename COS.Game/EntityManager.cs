using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using COS.Game.Services;

namespace COS.Game
{
    public class EntityManager
    {
        private readonly HashSet<Entity> _entities = new HashSet<Entity>();
        private readonly IServiceRegistry _services;

        public EntityManager(IServiceRegistry services)
        {
            _services = services;
        }

        public void Add(Entity entity)
        {
            if (_entities.Contains(entity)) return;

            if (entity.EntityManager != null)
                throw new InvalidOperationException(
                    "Cannot add an entity that has already been added to an EntityManager");

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

        public IEnumerable<Entity> EntitiesWithComponent<T>() where T : Component
        {
            return _entities.Where(entity => entity.HasComponent<T>());
        }

        public IEnumerable<Entity> EntitiesWithAll(params Component[] components)
        {
            // TODO: Test
            return _entities.Where(entity => entity.HasAllComponents(components));
        }
    }
}