using Kugushev.Scripts.Common.Modes;
using Kugushev.Scripts.Common.Utils;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Common
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + nameof(CommonInstaller))]
    public class CommonInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ParametersPipeline<Void>>().FromInstance(new ParametersPipelineVoid()).AsSingle();
        }
    }
}