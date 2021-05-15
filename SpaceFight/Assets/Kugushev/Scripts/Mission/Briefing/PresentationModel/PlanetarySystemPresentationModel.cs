using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Briefing.PresentationModel;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Utils;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.MissionPresentation.Controllers
{
    public class PlanetarySystemPresentationModel : MonoBehaviour
    {
        [SerializeField] private Transform sun = default!;
        [SerializeField] private GameObject planetPrefab = default!;

        [Inject] private IPlanetarySystem _model = default!;

        private void Start()
        {
            SetupSun();
            SetupPlanets();
        }

        private void SetupSun()
        {
            var size = _model.Sun.Size;
            sun.localScale = new Vector3(size, size, size);
            sun.position = _model.Sun.Position.Point;
        }

        private void SetupPlanets()
        {
            var worldTransform = transform;
            foreach (var planet in _model.Planets)
            {
                // todo: use zenject factory
                var instance = Instantiate(planetPrefab, planet.Position.Value.Point, Quaternion.identity,
                    worldTransform);
                var presentationModel = instance.GetComponent<PlanetPresentationModel>();
                presentationModel.Init(planet);
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