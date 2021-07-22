using Kugushev.Scripts.Game.Factories;
using Kugushev.Scripts.Game.Views;
using Leopotam.Ecs;

namespace Kugushev.Scripts.Game.Systems
{
    internal class UnitsSpawner : IEcsInitSystem
    {
        private EcsWorld _world;

        private WorldView _worldView;
        private PlayerUnitFactory _playerUnitFactory;
        private BanditUnitFactory _banditUnitFactory;

        public void Init()
        {
            CreatePlayer();
            CreateBandits();
        }

        private void CreatePlayer()
        {
            _playerUnitFactory.Create(_world, _worldView);
        }

        private void CreateBandits()
        {
            for (int i = 0; i < 5; i++)
                _banditUnitFactory.Create(_world, _worldView);
        }
        
    }
}