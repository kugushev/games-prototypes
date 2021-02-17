using Kugushev.Scripts.Game.Missions;
using Kugushev.Scripts.Game.Missions.Managers;
using Kugushev.Scripts.Presentation.PresentationModels;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class PlanetarySystemController : MonoBehaviour
    {
        [FormerlySerializedAs("missionsManager")] [SerializeField]
        private MissionManager missionManager;

        [SerializeField] private Transform sun;
        [SerializeField] private GameObject planetPrefab;

        private void Start()
        {
            var system = missionManager.State?.CurrentPlanetarySystem;
            if (system == null)
            {
                Debug.LogError("Current planetary system is not set");
                return;
            }

            sun.localScale = new Vector3(system.SunScale, system.SunScale, system.SunScale);

            var worldTransform = transform;
            foreach (var planet in system.Planets)
            {
                var instance = Instantiate(planetPrefab, planet.Position.Point, Quaternion.identity, worldTransform);
                var presentationModel = instance.GetComponent<PlanetPresentationModel>();
                presentationModel.Planet = planet;
            }
        }
    }
}