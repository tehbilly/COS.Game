using System;
using COS.Game.Services;
using Serilog;

namespace COS.Game
{
    public class Engine : IDisposable
    {
        private Timer _timer;
        public ILogger Logger { get; private set; }

        private bool _running;
        // TODO: Determine if/how this should be separated from the engine
        public readonly GameWindow Window;
        public readonly IServiceRegistry Services = new ServiceRegistry();
        public EntityManager EntityManager { get; private set; }
        public readonly SystemsManager Systems = new SystemsManager();
        private readonly Input _input = new Input();
        
        public int UpdatesPerSecond { get; set; } = 60;
        private float _sinceLastUpdate = 0;

        public Engine(WindowOptions options)
        {
            Window = new GameWindow(options);
            Window.KeyPressed += _input.OnKeyPressed;
            Window.KeyReleased += _input.OnKeyReleased;
            Initialize();
        }

        private void Initialize()
        {
            EntityManager = new EntityManager(Services);
            
            Logger = new SerilogLogger(new LoggerConfiguration()
                // TODO: Base this on configuration
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger());
            
            Services.Add(Logger);
            Services.Add(_input);
            
            // Register core systems
            Systems.Add(_input); // Runs post-update to move pressed keys to down
        }

        public void Run()
        {
            _running = true;
            _timer = new Timer();

            // Needed to catch a request from the window itself trying to be closed
            Window.Closed += (o, a) => { Stop(); };
            
            // Run any init systems
            Systems.Init();
            
            while (Window.IsOpen && _running)
            {
                Window.DispatchEvents();
                Window.Clear();
                
                // Run IUpdateSystem instances if delta time is large enough
                _sinceLastUpdate += _timer.Restart().AsSeconds();
                if (_sinceLastUpdate >= 1f / UpdatesPerSecond)
                {
                    Systems.Update(_sinceLastUpdate);
                    _sinceLastUpdate = 0;
                    
                    // Now that IUpdateSystems have been run we can run IPostUpdateSystems
                    Systems.PostUpdate();
                }
                
                // Now we gotta draw, pardner
                Systems.Draw();
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