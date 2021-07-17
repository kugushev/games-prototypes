using Kugushev.Scripts.Views;

namespace Kugushev.Scripts.Components.ViewRefs
{
    public readonly struct UnitTransformViewRef
    {
        public readonly UnitTransformView View;

        public UnitTransformViewRef(UnitTransformView view) => View = view;
    }
}