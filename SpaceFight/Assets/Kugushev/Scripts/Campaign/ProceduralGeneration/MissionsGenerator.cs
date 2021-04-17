using System;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Interfaces;
using Kugushev.Scripts.Campaign.ValueObjects;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kugushev.Scripts.Campaign.ProceduralGeneration
{
    [CreateAssetMenu(menuName = CampaignConstants.MenuPrefix + nameof(MissionsGenerator))]
    public class MissionsGenerator : ScriptableObject
    {
        [SerializeField] private PoliticalActionsRepository? politicalActionsRepository;

        public void GenerateMissions(IMissionsSet setToFill)
        {
            Asserting.NotNull(politicalActionsRepository);

            int normalMissionsCount = CampaignConstants.NormalMissionsCount;
            int hardMissionsCount = CampaignConstants.HardMissionsCount;
            int insaneMissionsCount = CampaignConstants.InsaneMissionsCount;


            while (normalMissionsCount > 0 || hardMissionsCount > 0 || insaneMissionsCount > 0)
            {
                int random = Random.Range(0, normalMissionsCount + hardMissionsCount + insaneMissionsCount);

                if (random < normalMissionsCount) // normal difficulty
                {
                    normalMissionsCount--;
                    setToFill.AddMission(CreateNormalMission(politicalActionsRepository));
                }
                else if (random < normalMissionsCount + hardMissionsCount) // hard difficulty
                {
                    hardMissionsCount--;
                    setToFill.AddMission(CreateHardMission(politicalActionsRepository));
                }
                else // insane difficulty
                {
                    insaneMissionsCount--;
                    setToFill.AddMission(CreateInsaneMission(politicalActionsRepository));
                }
            }
        }

        private MissionInfo CreateNormalMission(PoliticalActionsRepository repository)
        {
            var difficulty = Difficulty.Normal;
            var politicalAction = repository.GetRandom(difficulty);
            return new MissionInfo(NextSeed, difficulty, politicalAction, playerHomeProductionMultiplier: 2);
        }

        private MissionInfo CreateHardMission(PoliticalActionsRepository repository)
        {
            var difficulty = Difficulty.Hard;
            var politicalAction = repository.GetRandom(difficulty);
            var seed = NextSeed;
            var range = Random.Range(0, 3);
            switch (range)
            {
                case 0:
                    return new MissionInfo(seed, difficulty, politicalAction, enemyStartPowerMultiplier: 4);
                case 1:
                    return new MissionInfo(seed, difficulty, politicalAction, enemyExtraPlanets: 1);
                case 2:
                    return new MissionInfo(seed, difficulty, politicalAction, enemyHomeProductionMultiplier: 2);
                default:
                    throw new Exception($"Invalid random range {range}");
            }
        }

        private MissionInfo CreateInsaneMission(PoliticalActionsRepository repository)
        {
            var difficulty = Difficulty.Insane;
            var politicalAction = repository.GetRandom(difficulty);
            return new MissionInfo(NextSeed, difficulty, politicalAction,
                enemyStartPowerMultiplier: 4,
                enemyExtraPlanets: 1,
                enemyHomeProductionMultiplier: 2);
        }

        private static int NextSeed => Random.Range(CampaignConstants.MissionSeedMin, CampaignConstants.MissionSeedMax);
    }
}