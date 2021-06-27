﻿using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Core.Exceptions;
using Kugushev.Scripts.Common.Core.ValueObjects;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Core.Utils;
using Kugushev.Scripts.Game.Core.ValueObjects.Tiles;
using UnityEngine;
using Zenject;
using static Kugushev.Scripts.Game.Core.GameConstants.World;

using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Game.Core.Managers
{
    public class WorldManager : IInitializable
    {
        private GroundTile[,]? _ground;
        private IReadOnlyList<City>? _cities;

        public bool Initialized { get; private set; }
        public event Action? WorldInitialized;
        public GroundTile[,] Ground => _ground ?? throw new PropertyIsNotInitializedException();
        public IReadOnlyList<City> Cities => _cities ?? throw new PropertyIsNotInitializedException();

        void IInitializable.Initialize()
        {
            var seed = DateTime.Now.Millisecond;
            Debug.Log($"Seed {seed}");
            Random.InitState(seed);

            _ground = CreateWorld();
            _cities = CreateCities();

            Initialized = true;
            WorldInitialized?.Invoke();
        }

        private GroundTile[,] CreateWorld()
        {
            var ground = new GroundTile[Width, Height];
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                ground[x, y] = new GroundTile(TileType.Grass);

            return ground;
        }

        private IReadOnlyList<City> CreateCities()
        {
            var cities = new List<City>();

            int areaWidth = Width / CitiesInHorizontal;
            int areaHeight = Height / CitiesInVertical;

            for (int areaX = 0; areaX < CitiesInHorizontal; areaX++)
            for (int areaY = 0; areaY < CitiesInVertical; areaY++)
            {
                var areaBorderX = areaWidth / CityAreaRatioDivider;
                int fromX = areaX * areaWidth + areaBorderX;
                int toX = (areaX + 1) * areaWidth - areaBorderX;
                int x = Random.Range(fromX, toX);

                var areaBorderY = areaHeight / CityAreaRatioDivider;
                int fromY = areaY * areaHeight + areaBorderY;
                int toY = (areaY + 1) * areaHeight - areaBorderY;
                int y = Random.Range(fromY, toY);

                cities.Add(new City(new Position(new Vector2
                    {
                        x = WorldHelper.NormalizeX(x),
                        y = WorldHelper.NormalizeY(y)
                    }
                )));
            }

            return cities;
        }
    }
}