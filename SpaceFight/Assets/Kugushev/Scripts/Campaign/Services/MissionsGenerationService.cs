using System;
using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Enums;
using Kugushev.Scripts.Campaign.ValueObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Campaign.Services
{
    [CreateAssetMenu(menuName = CampaignConstants.MenuPrefix + nameof(MissionsGenerationService))]
    public class MissionsGenerationService : ScriptableObject
    {
        public List<MissionInfo> GenerateMissions()
        {
            int normalMissionsCount = CampaignConstants.NormalMissionsCount;
            int hardMissionsCount = CampaignConstants.HardMissionsCount;
            int insaneMissionsCount = CampaignConstants.InsaneMissionsCount;

            var missions = new List<MissionInfo>(CampaignConstants.MissionsCount);

            while (normalMissionsCount > 0 || hardMissionsCount > 0 || insaneMissionsCount > 0)
            {
                int random = Random.Range(0, normalMissionsCount + hardMissionsCount + insaneMissionsCount);

                if (random < normalMissionsCount) // normal difficulty
                {
                    normalMissionsCount--;
                    missions.Add(CreateNormalMission());
                }
                else if (random < normalMissionsCount + hardMissionsCount) // hard difficulty
                {
                    hardMissionsCount--;
                    missions.Add(CreateHardMission());
                }
                else // insane difficulty
                {
                    insaneMissionsCount--;
                    missions.Add(CreateInsaneMission());
                }
            }


            return missions;
        }

        private MissionInfo CreateNormalMission()
        {
            return new MissionInfo(NextSeed, Difficulty.Normal);
        }

        private MissionInfo CreateHardMission()
        {
            var seed = NextSeed;
            var difficulty = Difficulty.Hard;
            var range = Random.Range(0, 3);
            switch (range)
            {
                case 0:
                    return new MissionInfo(seed, difficulty, enemyStartPowerMultiplier: 4);
                case 1:
                    return new MissionInfo(seed, difficulty, enemyExtraPlanets: 1);
                case 2:
                    return new MissionInfo(seed, difficulty, enemyHomeProductionMultiplier: 2);
                default:
                    throw new Exception($"Invalid random range {range}");
            }
        }

        private MissionInfo CreateInsaneMission()
        {
            return new MissionInfo(NextSeed, Difficulty.Insane,
                enemyStartPowerMultiplier: 4,
                enemyExtraPlanets: 1,
                enemyHomeProductionMultiplier: 2);
        }

        private int NextSeed => Random.Range(CampaignConstants.MissionSeedMin, CampaignConstants.MissionSeedMax);
    }
}