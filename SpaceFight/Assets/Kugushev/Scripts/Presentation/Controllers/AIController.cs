using Kugushev.Scripts.Game.Missions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class AIController : MonoBehaviour
    {
        [FormerlySerializedAs("missionsManager")] [SerializeField] private MissionManager missionManager;

        void Update()
        {
            if (missionManager.AIAgents != null)
                foreach (var agent in missionManager.AIAgents)
                {
                    agent.Act();
                }
        }
    }
}