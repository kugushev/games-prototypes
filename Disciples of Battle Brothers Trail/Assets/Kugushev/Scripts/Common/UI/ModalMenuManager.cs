using Kugushev.Scripts.Common.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Common.UI
{
    public class ModalMenuManager : MonoBehaviour
    {
        [SerializeField] private BaseRoot baseRoot;
        [SerializeField] private ModalMenu[] menusPrefabs;

        public void OpenModalMenu<T>()
        {
            foreach (var prefab in menusPrefabs)
            {
                if (prefab.GetType() == typeof(T))
                {
                    Instantiate(prefab, transform);

                    baseRoot.Paused = true;

                    return;
                }
            }
        }

        public void CloseModalMenu(GameObject menu)
        {
            Destroy(menu);
            baseRoot.Paused = false;
        }
    }
}