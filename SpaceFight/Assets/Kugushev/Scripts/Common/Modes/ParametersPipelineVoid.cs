using Kugushev.Scripts.Common.Utils;

namespace Kugushev.Scripts.Common.Modes
{
    public class ParametersPipelineVoid : ParametersPipeline<Void>
    {
        public override void Push(Void parameters)
        {
            // don't require because Void is ... void :)
        }

        public override bool TryPop(out Void value)
        {
            value = Void.Instance;
            return true;
        }
    }
}