using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Missions.Managers;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class MissionPreparationController : MonoBehaviour
    {
        [SerializeField] private MissionManager missionManager;

        public void AdjustTime(float sliderValue)
        {
            if (missionManager.State != null)
            {
                var dayOfYear = Mathf.FloorToInt(GameConstants.DaysInYear * sliderValue);
                missionManager.State.Value.CurrentPlanetarySystem.SetDayOfYear(dayOfYear);
            }
        }

        public void StartMission()
        {
            StartCoroutine(missionManager.StartMission().ToCoroutine());
        }
    }
}