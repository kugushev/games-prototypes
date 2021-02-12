using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Entities;
using Kugushev.Scripts.Game.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Game.Managers
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Missions Manager")]
    public class MissionsManager : ScriptableObject
    {
        public async UniTask NextMission(PlanetarySystem planetarySystem, IReadOnlyList<IAIAgent> aiAgents)
        {
            if (CurrentPlanetarySystem != null || AIAgents != null)
            {
                Debug.LogError("Can't start next mission. Current is not finished yet");
                return;
            }

            CurrentPlanetarySystem = planetarySystem;
            AIAgents = aiAgents;
            
            await SceneManager.LoadSceneAsync("MissionScene");
        }
        
        public PlanetarySystem CurrentPlanetarySystem { get; private set; }
        
        public IReadOnlyList<IAIAgent> AIAgents { get; private set; }
        
        
    }
}