using Kugushev.Scripts.Common.ContextManagement;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Common
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + nameof(CommonInstaller))]
    public class CommonInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind(typeof(ParametersPipeline<>)).AsSingle();
        }
    }
}