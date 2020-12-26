using System.Collections;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Game.Features;
using Kugushev.Scripts.Presentation.Components.Abstractions;

namespace Kugushev.Scripts.Presentation.Components
{
    public class ActivityComponent : BaseComponent<IActive>
    {
        private IEnumerator Start() =>
            Model.BehaviorTreeManager
                .RunLoop()
                .ToCoroutine();
    }
}