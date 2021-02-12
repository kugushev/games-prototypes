using Cysharp.Threading.Tasks;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Common;
using Kugushev.Scripts.Game.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kugushev.Scripts.Game.Managers
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "Missions Manager")]
    public class MissionsManager : ScriptableObject
    {
        public async UniTask NextMission(PlanetarySystem planetarySystem)
        {
            if (CurrentPlanetarySystem != null)
            {
                Debug.LogError("Can't start next mission. Current is not finished yet");
                return;
            }
            
            CurrentPlanetarySystem = planetarySystem;
            await SceneManager.LoadSceneAsync("MissionScene");
        }
        
        public PlanetarySystem CurrentPlanetarySystem { get; private set; }
    }
}