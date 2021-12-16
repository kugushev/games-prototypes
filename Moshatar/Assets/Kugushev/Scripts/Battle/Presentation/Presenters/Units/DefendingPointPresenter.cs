using System.Collections;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Battle.Presentation.Presenters.Units
{
    public class DefendingPointPresenter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem blastEffect;
        [SerializeField] private AudioSource blastSound;
        [SerializeField] private GameObject mainEffect;

        private DefendingPoint _model;

        public void Init(DefendingPoint defendingPoint)
        {
            _model = defendingPoint;

            _model.Character.HP.Subscribe(HpChanged).AddTo(this);

            _model.Die += Death;
        }

        private void HpChanged(int hp)
        {
            float max = _model.Character.MaxHP;
            float scale = hp / max;

            mainEffect.transform.localScale = new Vector3(scale, scale, scale);
        }

        private void Death()
        {
            _model.Die -= Death;
            blastSound.Play();
            blastEffect.Play();
            StartCoroutine(Dying());
        }

        private IEnumerator Dying()
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    }
}