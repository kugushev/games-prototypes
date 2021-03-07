using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private MissionModelProvider missionModelProvider;

        void Update()
        {
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