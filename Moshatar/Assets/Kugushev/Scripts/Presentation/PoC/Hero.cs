using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC
{
    public class Hero: MonoBehaviour
    {
        [SerializeField] private Transform head;
        
        public Vector3 HeadPosition => head.position;
    }
}