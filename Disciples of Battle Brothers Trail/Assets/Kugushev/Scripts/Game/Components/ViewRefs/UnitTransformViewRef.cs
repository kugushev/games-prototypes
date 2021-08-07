﻿using Kugushev.Scripts.Game.Views;

namespace Kugushev.Scripts.Game.Components.ViewRefs
{
    public readonly struct UnitTransformViewRef
    {
        public readonly UnitTransformView View;

        public UnitTransformViewRef(UnitTransformView view) => View = view;
    }
}