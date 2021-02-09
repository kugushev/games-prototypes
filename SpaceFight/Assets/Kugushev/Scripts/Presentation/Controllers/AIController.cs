using Kugushev.Scripts.Game.AI.Tactical;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private SimpleAI ai;
        
        void Update()
        {
            ai.Act();
        }
    }
}
