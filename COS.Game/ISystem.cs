using System;
using System.Collections.Generic;

// TODO: Determine if abstract clases with references to things like the engine, window, service registry, etc. would
// be better
namespace COS.Game
{
    public interface ISystem {}

    public interface IInitSystem : ISystem
    {
        void Init();
    }

    public interface IUpdateSystem : ISystem
    {
        void Update(float dt);
    }

    public interface IPostUpdateSystem : ISystem
    {
        void PostUpdate();
    }

    public interface IDrawSystem : ISystem
    {
        void Draw();
    }

    public class SystemsManager
    {
        private bool _initialized = false;
        private readonly List<IInitSystem> _initSystems = new List<IInitSystem>();
        private readonly List<IUpdateSystem> _updateSystems = new List<IUpdateSystem>();
        private readonly List<IPostUpdateSystem> _postUpdateSystems = new List<IPostUpdateSystem>();
        private readonly List<IDrawSystem> _drawSystems = new List<IDrawSystem>();

        public void Add<T>(T system) where T : class, ISystem
        {
            if (!_initialized && system is IInitSystem initSystem) _initSystems.Add(initSystem);
            if (system is IUpdateSystem updateSystem) _updateSystems.Add(updateSystem);
            if (system is IPostUpdateSystem postUpdateSystem) _postUpdateSystems.Add(postUpdateSystem);
            if (system is IDrawSystem drawSystem) _drawSystems.Add(drawSystem);
        }

        internal void Init()
        {
            if (_initialized)
                throw new InvalidOperationException("Unable to initialize SystemsManager more than once");
            
            foreach (var system in _initSystems) system.Init();
            
            // No need for these anymore
            _initSystems.Clear();
            _initialized = true;
        }

        internal void Update(float dt)
        {
            foreach (var system in _updateSystems) system.Update(dt);
        }

        internal void PostUpdate()
        {
            foreach (var system in _postUpdateSystems) system.PostUpdate();
        }

        internal void Draw()
        {
            foreach (var system in _drawSystems) system.Draw();
        }
    }
}