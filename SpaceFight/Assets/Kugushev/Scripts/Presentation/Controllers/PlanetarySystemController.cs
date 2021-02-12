using Kugushev.Scripts.Game.Managers;
using Kugushev.Scripts.Presentation.PresentationModels;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Controllers
{
    public class PlanetarySystemController : MonoBehaviour
    {
        [SerializeField] private MissionsManager missionsManager;
        [SerializeField] private Transform sun;
        [SerializeField] private GameObject planetPrefab;

        private void Start()
        {
            var system = missionsManager.CurrentPlanetarySystem;
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