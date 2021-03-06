using Kugushev.Scripts.Mission.Managers;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.Presentation.PresentationModels;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class PlanetarySystemController : MonoBehaviour
    {
        [FormerlySerializedAs("missionsManager")] [SerializeField]
        private MissionManagerOld missionManager;

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

            SetupSun(system);
            SetupPlanets(system);
        }

        private void SetupSun(PlanetarySystem system)
        {
            ref readonly var sunModel = ref system.GetSun();
            sun.localScale = new Vector3(sunModel.Size, sunModel.Size, sunModel.Size);
            sun.position = sunModel.Position.Point;
        }

        private void SetupPlanets(PlanetarySystem system)
        {
            var worldTransform = transform;
            foreach (var planet in system.Planets)
            {
                var instance = Instantiate(planetPrefab, planet.Position.Point, Quaternion.identity, worldTransform);
                var presentationModel = instance.GetComponent<PlanetPresentationModel>();
                presentationModel.Planet = planet;
            }
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.magenta;
        //     var sunModel = new Sun(new Position(new Vector3(0f, 1.508f, 0.5f)), 0.3f);
        //
        //     Gizmos.DrawSphere(sunModel.Position.Point, sunModel.Radius);
        // }
    }
}