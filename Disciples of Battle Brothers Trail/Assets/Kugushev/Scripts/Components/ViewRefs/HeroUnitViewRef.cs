using Kugushev.Scripts.Views;

namespace Kugushev.Scripts.Components.ViewRefs
{
    public readonly struct HeroUnitViewRef
    {
        public readonly HeroUnitView View;

        public HeroUnitViewRef(HeroUnitView view) => View = view;
    }
}