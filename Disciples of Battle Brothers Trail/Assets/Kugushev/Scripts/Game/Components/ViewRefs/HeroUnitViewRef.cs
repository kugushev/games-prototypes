using Kugushev.Scripts.Game.Views;

namespace Kugushev.Scripts.Game.Components.ViewRefs
{
    public readonly struct HeroUnitViewRef
    {
        public readonly HeroUnitView View;

        public HeroUnitViewRef(HeroUnitView view) => View = view;
    }
}