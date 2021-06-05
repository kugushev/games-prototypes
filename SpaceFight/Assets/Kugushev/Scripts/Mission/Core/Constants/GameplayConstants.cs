﻿namespace Kugushev.Scripts.Mission.Constants
{
    public static class GameplayConstants
    {
        public const string LeftHandCommander = "LeftHandCommander";
        public const string RightHandCommander = "RightHandCommander";
        public const int OrderPathCapacity = 128;
        public const int ArmiesPerFleetCapacity = 32;

        public const int SoftCapArmyPower = 50;
       
        public const float UnifiedDamage = 1;
        public const float SiegeRoundDelay = 0.4f;
        public const float FightRoundDelay = 0.6f;
        public const int ArmyCannonsCount = 3;
        public const float ArmySpeed = 0.1f;
        public const float ArmyAngularSpeed = 1f;
        public const float SunPowerSpeedMultiplier = 2f;

        public const float GapBetweenWaypoints = 0.05f;
        public const float CollisionError = 0.05f;

        public const int NeutralStartPowerMultiplier = 3;

        public const int DaysInYear = 360;
    }
}