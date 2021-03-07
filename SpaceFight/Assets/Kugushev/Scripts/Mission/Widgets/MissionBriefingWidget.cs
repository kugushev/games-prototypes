using Kugushev.Scripts.Mission.Constants;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Widgets
{
    public class MissionBriefingWidget : MonoBehaviour
    {
        [SerializeField] private MissionModelProvider missionManager;

        public void AdjustTime(float sliderValue)
        {
            if (missionManager.TryGetModel(out var model))
            {
                var dayOfYear = Mathf.FloorToInt(GameplayConstants.DaysInYear * sliderValue);
                model.PlanetarySystem.SetDayOfYear(dayOfYear);
            }
        }
    }
}