using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.MissionPresentation.Controllers
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private MissionModelProvider? missionModelProvider;

        void Update()
        {
            Asserting.NotNull(missionModelProvider);

            if (missionModelProvider.TryGetModel(out var missionModel))
            {
                if (missionModel.Green.Commander is IAIAgent greenAgent)
                    greenAgent.Act();

                if (missionModel.Red.Commander is IAIAgent redAgent)
                    redAgent.Act();
            }
        }
    }
}