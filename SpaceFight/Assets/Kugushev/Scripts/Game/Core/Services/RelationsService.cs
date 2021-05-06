﻿using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.Enums;

namespace Kugushev.Scripts.Game.Core.Services
{
    public class RelationsService
    {
        public static Relation FromLevel(int level)
        {
            if (level < GameConstants.MinRelationLevel)
                level = GameConstants.MinRelationLevel;
            if (level > GameConstants.MaxRelationLevel)
                level = GameConstants.MaxRelationLevel;

            if (level < -7)
                return Relation.Enemy;
            if (level < -2)
                return Relation.Hater;
            if (level <= 2)
                return Relation.Indifferent;
            if (level <= 7)
                return Relation.Partner;
            return Relation.Loyalist;
        }
    }
}