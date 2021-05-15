using System.Collections;
using System.Collections.Generic;
using Kugushev.Scripts.Campaign.Constants;
using Kugushev.Scripts.Campaign.Interfaces;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Enums;
using Kugushev.Scripts.Game.Core.Repositories;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Campaign.Core.Services
{
    internal class MissionsGenerationService
    {
        private readonly IntriguesRepository _intriguesRepository;

        public MissionsGenerationService(IntriguesRepository intriguesRepository) =>
            _intriguesRepository = intriguesRepository;

        public IEnumerable<MissionInfo> GenerateMissions()
        {
            var result = new List<MissionInfo>();

            Asserting.NotNull(_intriguesRepository);

            int normalMissionsCount = CampaignConstants.NormalMissionsCount;
            int hardMissionsCount = CampaignConstants.HardMissionsCount;
            int insaneMissionsCount = CampaignConstants.InsaneMissionsCount;


            while (normalMissionsCount > 0 || hardMissionsCount > 0 || insaneMissionsCount > 0)
            {
                int random = Random.Range(0, normalMissionsCount + hardMissionsCount + insaneMissionsCount);

                if (random < normalMissionsCount) // normal difficulty
                {
                    normalMissionsCount--;
                    result.Add(CreateNormalMission(_intriguesRepository));
                }
                else if (random < normalMissionsCount + hardMissionsCount) // hard difficulty
                {
                    hardMissionsCount--;
                    result.Add(CreateHardMission(_intriguesRepository));
                }
                else // insane difficulty
                {
                    insaneMissionsCount--;
                    result.Add(CreateInsaneMission(_intriguesRepository));
                }
            }

            return result;
        }

        private MissionInfo CreateNormalMission(IntriguesRepository repository)
        {
            var difficulty = Difficulty.Normal;
            var politicalAction = repository.GetRandom(difficulty);
            return new MissionInfo(NextSeed, difficulty, politicalAction, playerHomeProductionMultiplier: 2);
        }

        private MissionInfo CreateHardMission(IntriguesRepository repository)
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
                    throw new SpaceFightException($"Invalid random range {range}");
            }
        }

        private MissionInfo CreateInsaneMission(IntriguesRepository repository)
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