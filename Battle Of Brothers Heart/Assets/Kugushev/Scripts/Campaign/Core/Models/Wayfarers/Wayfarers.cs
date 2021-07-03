using System;
using System.Collections.Generic;
using System.Linq;
using Kugushev.Scripts.Common.Core.AI;
using Kugushev.Scripts.Game.Core.Managers;
using Kugushev.Scripts.Game.Core.Models.WorldUnits;
using UniRx;

namespace Kugushev.Scripts.Campaign.Core.Models.Wayfarers
{
    public class Wayfarers : IAgentsOwner, IDisposable
    {
        private readonly AgentsManager _agentsManager;
        private readonly BattleManager _battleManager;
        private readonly ReactiveCollection<BanditWayfarer> _bandits;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        public Wayfarers(AgentsManager agentsManager, WorldUnitsManager worldUnitsManager,
            GameModeManager gameModeManager, BattleManager battleManager)
        {
            _agentsManager = agentsManager;
            _battleManager = battleManager;
            _agentsManager.Register(this);

            Player = new PlayerWayfarer(worldUnitsManager.Player, gameModeManager, battleManager);

            _bandits = new ReactiveCollection<BanditWayfarer>(
                worldUnitsManager.Bandits.Select(CreateWayfarer));
            worldUnitsManager.Bandits.ObserveAdd()
                .Subscribe(addEvent => _bandits.Add(CreateWayfarer(addEvent.Value)))
                .AddTo(_disposables);
            worldUnitsManager.Bandits.ObserveRemove()
                .Subscribe(removeEvent => _bandits.RemoveAt(removeEvent.Index))
                .AddTo(_disposables);
        }


        public PlayerWayfarer Player { get; }

        public IReadOnlyReactiveCollection<BanditWayfarer> Bandits => _bandits;

        IEnumerable<IAgent> IAgentsOwner.Agents
        {
            get
            {
                yield return Player;
                foreach (var bandit in Bandits)
                {
                    yield return bandit;
                }
            }
        }

        private BanditWayfarer CreateWayfarer(BanditWorldUnit unit)
            => new BanditWayfarer(unit, _battleManager);

        void IDisposable.Dispose()
        {
            _agentsManager.Unregister(this);

            foreach (var disposable in _disposables)
                disposable.Dispose();
            _disposables.Clear();
        }
    }
}