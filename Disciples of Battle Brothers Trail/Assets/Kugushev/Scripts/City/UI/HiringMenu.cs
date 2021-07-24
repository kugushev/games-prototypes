using Kugushev.Scripts.Common.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.City.UI
{
    public class HiringMenu : ModalMenu
    {
        [SerializeField] private Button exit;

        protected override void Awake()
        {
            base.Awake();
            exit.onClick.AddListener(CloseMenu);
        }
    }
}