using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC.Duel
{
    public class HeroHeadController: MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
    }
}