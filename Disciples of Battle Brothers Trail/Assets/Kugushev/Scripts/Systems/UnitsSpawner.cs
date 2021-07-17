using Kugushev.Scripts.Common;
using Kugushev.Scripts.Factories;
using Leopotam.Ecs;

namespace Kugushev.Scripts.Systems
{
    [CampaignSystem]
    public class UnitsSpawner : IEcsInitSystem
    {
        private EcsWorld _world;

        private PlayerUnitFactory _playerUnitFactory;
        private BanditUnitFactory _banditUnitFactory;

        public void Init()
        {
            CreatePlayer();
            CreateBandits();
        }

        private void CreatePlayer()
        {
            _playerUnitFactory.Create(_world);
        }


        private void CreateBandits()
        {
            for (int i = 0; i < 5; i++)
            {
                _banditUnitFactory.Create(_world);
            }
        }
    }
}