using Kugushev.Scripts.Game.AI.Tactical;
using Kugushev.Scripts.Game.Managers;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private MissionsManager missionsManager;

        void Update()
        {
            if (missionsManager.AIAgents != null)
                foreach (var agent in missionsManager.AIAgents)
                {
                    agent.Act();
                }
        }
    }
}