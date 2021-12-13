using UnityEngine;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class EnemyWeapon: MonoBehaviour
    {
        [SerializeField] private EnemyUnitPresenter owner;

        public EnemyUnitPresenter Owner => owner;
    }
}