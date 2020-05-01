﻿using System;
using System.IO;
using SfmlTexture = SFML.Graphics.Texture;

namespace COS.Game.Graphics
{
    public sealed class Texture : IDisposable
    {
        internal readonly SfmlTexture SfmlTexture;

        private Texture(SfmlTexture sfmlTexture)
        {
            SfmlTexture = sfmlTexture;
        }

        public static Texture FromFile(string fileName)
        {
            return new Texture(new SfmlTexture(fileName));
        }

        public static Texture FromFile(Stream stream)
        {
            return new Texture(new SfmlTexture(stream));
        }

        public bool Smooth
        {
            get => SfmlTexture.Smooth;
            set => SfmlTexture.Smooth = value;
        }

        public bool Repeated
        {
            get => SfmlTexture.Repeated;
            set => SfmlTexture.Repeated = value;
        }

        public Vector2u Size => SfmlTexture.Size.ToHidden();

        public void Dispose()
        {
            SfmlTexture?.Dispose();
        }
    }
}