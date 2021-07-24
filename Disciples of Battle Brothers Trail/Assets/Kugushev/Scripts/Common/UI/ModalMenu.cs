using System;
using UnityEngine;

namespace Kugushev.Scripts.Common.UI
{
    public abstract class ModalMenu : MonoBehaviour
    {
        private ModalMenuManager _manager;

        protected virtual void Awake()
        {
            _manager = transform.parent.GetComponent<ModalMenuManager>();
        }

        protected void CloseMenu() => _manager.CloseModalMenu(gameObject);
    }
}