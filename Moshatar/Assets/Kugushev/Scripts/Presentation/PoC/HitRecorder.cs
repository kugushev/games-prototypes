using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Presentation.PoC
{
    [RequireComponent(typeof(LineRenderer))]
    public class HitRecorder : MonoBehaviour
    {
        [SerializeField] private string weaponTag;

        [Inject] private HitsManager _hitsManager;

        private LineRenderer _lineRenderer;
        private readonly Vector3[] _positions = new Vector3[2];
        private readonly WaitForSeconds _wait = new WaitForSeconds(1);
        private int _recordedDamage;

        public string WeaponTag => weaponTag;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public void RegisterHitEnter(Vector3 position, int damage)
        {
            Hide();
            _positions[0] = position;
            _recordedDamage = damage;
        }

        public void RegisterHitExit(Vector3 position)
        {
            _positions[1] = position;

            _lineRenderer.enabled = true;
            _lineRenderer.SetPositions(_positions);

            _hitsManager.Register(weaponTag, _recordedDamage, _positions);

            _recordedDamage = -1;
            StartCoroutine(CountDownToHide());
        }

        private IEnumerator CountDownToHide()
        {
            yield return _wait;
            Hide();
        }

        private void Hide() => _lineRenderer.enabled = false;
    }
}