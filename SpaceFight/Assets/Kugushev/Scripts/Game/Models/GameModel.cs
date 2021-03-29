using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.ValueObjects;

namespace Kugushev.Scripts.Game.Models
{
    public class GameModel : Poolable<GameModel.State>
    {
        public readonly struct State
        {
            public readonly Parliament Parliament;

            public State(Parliament parliament)
            {
                Parliament = parliament;
            }
        }

        private readonly List<PoliticalAction> _politicalActions = new List<PoliticalAction>(64);

        public GameModel(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        public Parliament Parliament => ObjectState.Parliament;
        public IReadOnlyList<PoliticalAction> PoliticalActions => _politicalActions;

        public void AddPoliticalActions(IReadOnlyList<PoliticalAction> politicalActions) =>
            _politicalActions.AddRange(politicalActions);

        protected override void OnClear(State state)
        {
            state.Parliament.Dispose();
            _politicalActions.Clear();
        }

        protected override void OnRestore(State state) => _politicalActions.Clear();
    }
}