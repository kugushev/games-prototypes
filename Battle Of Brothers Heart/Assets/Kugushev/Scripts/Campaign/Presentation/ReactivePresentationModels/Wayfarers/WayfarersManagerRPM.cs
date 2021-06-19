using Kugushev.Scripts.Campaign.Core.Models.Wayfarers;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Campaign.Presentation.ReactivePresentationModels.Wayfarers
{
    public class WayfarersManagerRPM : MonoBehaviour
    {
        [Inject] private WayfarersManager _model = default!;


    }
}