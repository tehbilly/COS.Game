﻿using System;
using System.Collections.Generic;
using Autofac;
using IContainer = Autofac.IContainer;

namespace COS.Game
{
    public class Engine : IDisposable
    {
        private Timer _timer;
        public ILogger Logger { get; private set; }

        private bool _running;
        // TODO: Determine if/how this should be separated from the engine
        public GameWindow Window { get; private set; }
        internal IContainer Container { get; set; }
        public EntityManager EntityManager { get; private set; }

        private uint _updatesPerSecond = 60;
        public uint UpdatesPerSecond
        {
            get => _updatesPerSecond;
            set
            {
                _updatesPerSecond = value;
                Window.SetFramerateLimit(_updatesPerSecond);
            }
        }

        private float _sinceLastUpdate = 0;

        internal Engine() {}

        /// <summary>
        /// Perform initial setup, getting 
        /// </summary>
        internal void Initialize()
        {
            var input = Container.Resolve<Input>();
            Window = Container.Resolve<GameWindow>();
            Window.KeyPressed += input.OnKeyPressed;
            Window.KeyReleased += input.OnKeyReleased;
            EntityManager = Container.Resolve<EntityManager>();
            Logger = Container.Resolve<ILogger>();
        }

        public void Run()
        {
            _running = true;
            _timer = new Timer();

            // Needed to catch a request from the window itself trying to be closed
            Window.Closed += (o, a) => { Stop(); };
            
            // Run any init systems
            foreach (var system in Container.Resolve<IEnumerable<IInitSystem>>())
                system.Init();

            while (Window.IsOpen && _running)
            {
                Window.DispatchEvents();
                Window.Clear();
                
                // Run IUpdateSystem instances if delta time is large enough
                _sinceLastUpdate += _timer.Restart().AsSeconds();
                if (_sinceLastUpdate >= 1f / UpdatesPerSecond)
                {
                    foreach (var system in Container.Resolve<IEnumerable<IUpdateSystem>>())
                        system.Update(_sinceLastUpdate);

                    _sinceLastUpdate = 0;

                    // Now that IUpdateSystems have been run we can run IPostUpdateSystems
                    foreach (var system in Container.Resolve<IEnumerable<IPostUpdateSystem>>())
                        system.PostUpdate();
                }

                // Now we gotta draw, pardner
                foreach (var system in Container.Resolve<IEnumerable<IDrawSystem>>())
                    system.Draw();
                Window.Display();
            }

            if (Window.IsOpen) Window.Close();
        }

        public void Stop()
        {
            _running = false;
        }

        public void Dispose()
        {
            if (Window.IsOpen) Window.Close();
            Window.Dispose();
            _timer.Dispose();
        }
    }
}