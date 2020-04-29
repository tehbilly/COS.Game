using System;
using SFML.System;

namespace COS.Game
{
    public class Timer : IDisposable
    {
        private readonly Clock _clock;

        public Timer()
        {
            _clock = new Clock();
        }

        public void Dispose() => _clock.Dispose();

        public Time Restart() => _clock.Restart();

        public Time ElapsedTime => _clock.ElapsedTime;
    }
}