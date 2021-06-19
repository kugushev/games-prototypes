using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Campaign.Presentation.Helpers;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels
{
    public class CityRPM : MonoBehaviour
    {
        private City _model = default!;

        [Inject]
        private void Init(City city)
        {
            _model = city;
        }

        private void Awake()
        {
            transform.position = new Vector3(
                WorldHelper.NormalizeX(_model.Position.x),
                WorldHelper.NormalizeY(_model.Position.y)
            );
        }


        public class Factory : PlaceholderFactory<City, CityRPM>
        {
        }
    }
}