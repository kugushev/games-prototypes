using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Managers;
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