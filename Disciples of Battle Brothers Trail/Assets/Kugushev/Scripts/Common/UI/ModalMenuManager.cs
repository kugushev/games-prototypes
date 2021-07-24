using System;
using Kugushev.Scripts.Common.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Common.UI
{
    public class ModalMenuManager : MonoBehaviour
    {
        [SerializeField] private BaseRoot baseRoot;
        [SerializeField] private ModalMenu[] menusPrefabs;

        public T OpenModalMenu<T>() where T : ModalMenu
        {
            foreach (var prefab in menusPrefabs)
            {
                if (prefab.GetType() == typeof(T))
                {
                    var instance = Instantiate(prefab, transform);

                    baseRoot.Paused = true;

                    return (T) instance;
                }
            }

            throw new Exception($"Model menu {typeof(T)} not found");
        }

        public void CloseModalMenu(GameObject menu)
        {
            Destroy(menu);
            baseRoot.Paused = false;
        }
    }
}