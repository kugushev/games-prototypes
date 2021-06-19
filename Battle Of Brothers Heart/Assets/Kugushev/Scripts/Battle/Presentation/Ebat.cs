using UnityEngine;
using UnityEngine.EventSystems;

namespace Kugushev.Scripts.Battle.Presentation
{
    public class Ebat: MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            print("Ebat");
        }
    }
}