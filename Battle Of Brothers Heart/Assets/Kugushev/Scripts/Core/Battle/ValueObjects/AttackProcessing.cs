using System;
using Kugushev.Scripts.Core.Battle.Enums;
using UnityEngine;

namespace Kugushev.Scripts.Core.Battle.ValueObjects
{
    public readonly struct AttackProcessing
    {
        public AttackProcessing(AttackStatus status)
        {
            Status = status;
            Time = DateTime.Now;
        }

        public AttackStatus Status { get; }
        public DateTime Time { get; }

        public bool IsReadyForNextStep()
        {
            var now = DateTime.Now;
            switch (Status)
            {
                case AttackStatus.Prepared:
                    return now >= Time + BattleConstants.SwordAttackBeforeHurtTime;
                case AttackStatus.Executed:
                    return now >= Time + BattleConstants.SwordAttackAfterHurtTime;
                case AttackStatus.OnCooldown:
                    return now >= Time + BattleConstants.SwordAttackCooldown;
                default:
                    Debug.LogError($"Unexpected status {Status}");
                    break;
            }

            return false;
        }
    }
}