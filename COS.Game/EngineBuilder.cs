using Autofac;
using Serilog;

namespace COS.Game
{
    public class EngineBuilder
    {
        public WindowOptionsBuilder WindowOptions { get; }
        public SystemsBuilder Systems { get; }
        
        private readonly ContainerBuilder _containerBuilder = new ContainerBuilder();

        public EngineBuilder()
        {
            WindowOptions = new WindowOptionsBuilder(this);
            Systems = new SystemsBuilder(this, _containerBuilder);
        }

        private void RegisterLogger()
        {
            // Set the Serilog root logger
            Log.Logger = new LoggerConfiguration()
                // TODO: Base this on configuration
                .WriteTo.Console()
#if DEBUG      
                .MinimumLevel.Debug()
#endif
                .CreateLogger();

            _containerBuilder.Register(c => new SerilogLogger(Log.Logger))
                .As<ILogger>();
        }

        public Engine Build()
        {
            RegisterLogger();
            
            _containerBuilder
                .Register(c => new GameWindow(WindowOptions.CreateWindowOptions()))
                .SingleInstance()
                .AsSelf();

            // Input manager
            _containerBuilder.Register(c => new Input())
                .SingleInstance()
                .AsSelf()
                .As<IPostUpdateSystem>();
            
            // TODO: Remove this?
            _containerBuilder.Register(c => new EntityManager())
                .SingleInstance()
                .AsSelf();

            var engine = new Engine();
            _containerBuilder.RegisterInstance(engine)
                .As<Engine>();
            engine.Container = _containerBuilder.Build();
            engine.Initialize();

            return engine;
        }
    }

    public class SystemsBuilder
    {
        private readonly EngineBuilder _engineBuilder;
        internal readonly ContainerBuilder ContainerBuilder;

        internal SystemsBuilder(EngineBuilder engineBuilder, ContainerBuilder containerBuilder)
        {
            _engineBuilder = engineBuilder;
            ContainerBuilder = containerBuilder;
        }

        public SystemsBuilder Register<T>() where T : class
        {
            ContainerBuilder.RegisterType<T>()
                .SingleInstance()
                .AsImplementedInterfaces()
                .AsSelf();
            return this;
        }

        public SystemsBuilder Register(object obj)
        {
            ContainerBuilder.RegisterInstance(obj)
                .SingleInstance()
                .AsImplementedInterfaces()
                .AsSelf()
                .ExternallyOwned();
            return this;
        }

        public EngineBuilder EndSystems()
        {
            return _engineBuilder;
        }
    }

    public class WindowOptionsBuilder
    {
        private readonly EngineBuilder _engineBuilder;
        private string _name = "COS.Game";
        private uint _width = 800;
        private uint _height = 600;
        private WindowFlags _flags = WindowFlags.Default;

        internal WindowOptionsBuilder(EngineBuilder engineBuilder)
        {
            _engineBuilder = engineBuilder;
        }

        public WindowOptionsBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public WindowOptionsBuilder Width(uint width)
        {
            _width = width;
            return this;
        }

        public WindowOptionsBuilder Height(uint height)
        {
            _height = height;
            return this;
        }

        public WindowOptionsBuilder Flags(WindowFlags flags)
        {
            _flags = flags;
            return this;
        }

        internal WindowOptions CreateWindowOptions()
        {
            return new WindowOptions(_name, _width, _height, _flags);
        }
        
        public EngineBuilder EndWindowOptions()
        {
            return _engineBuilder;
        }
    }
}