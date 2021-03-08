﻿using System.Collections.Generic;
using Kugushev.Scripts.Common;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Achievements.Abstractions
{
    public abstract class AbstractAchievement: ScriptableObject
    {
        protected const string MenuName = CommonConstants.MenuPrefix + "Achievements/";
        
        // todo: use for localization https://docs.unity3d.com/Packages/com.unity.localization@0.10/manual/QuickStartGuide.html
        
        public abstract AchievementId Id { get; }
        public abstract int Level { get; }
        public abstract string Caption { get; }
        public abstract string Description { get; }

        public abstract bool Check(IReadOnlyList<MissionEvent> missionEvents, Faction faction);
    }
}