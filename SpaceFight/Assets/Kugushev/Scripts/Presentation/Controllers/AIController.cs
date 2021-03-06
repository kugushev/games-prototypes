using Kugushev.Scripts.Common.Interfaces;
using Kugushev.Scripts.Mission.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class AIController : MonoBehaviour
    {
        [FormerlySerializedAs("missionsManager")] [SerializeField] private MissionManager missionManager;

        void Update()
        {
            if (missionManager.State != null)
            {
                if (missionManager.State.Value.Green.Commander is IAIAgent greenAgent) 
                    greenAgent.Act();
                
                if (missionManager.State.Value.Red.Commander is IAIAgent redAgent) 
                    redAgent.Act();
            }
        }
    }
}