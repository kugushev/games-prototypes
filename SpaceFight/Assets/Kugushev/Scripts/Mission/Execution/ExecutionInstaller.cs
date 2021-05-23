using Kugushev.Scripts.Common.Factories;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Execution.Factories;
using Kugushev.Scripts.Mission.Execution.Interfaces;
using Kugushev.Scripts.MissionPresentation.Components;
using Zenject;

namespace Kugushev.Scripts.Mission.Execution
{
    public class ExecutionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .InstallPrefabFactory<Army, IFleetPresentationModel, ArmyPresentationModel,
                    ArmyPresentationModel.Factory, ArmyFactory>();
        }
    }
}