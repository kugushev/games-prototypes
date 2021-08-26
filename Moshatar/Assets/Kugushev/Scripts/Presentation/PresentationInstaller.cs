using Kugushev.Scripts.Common;
using Kugushev.Scripts.Presentation.PoC.Fight;
using Kugushev.Scripts.Presentation.PoC.Music;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation
{
    [CreateAssetMenu(menuName = Constants.AssetMenu + nameof(PresentationInstaller))]
    public class PresentationInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {

        }
    }
}