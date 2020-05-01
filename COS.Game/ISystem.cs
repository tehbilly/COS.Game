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
}