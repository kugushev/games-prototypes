using System.Collections;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Presentation.Common;
using TMPro;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PoC.Fight
{
    public class HeroStats : MonoBehaviour
    {
        public const int MaxHP = 10;
        
        [SerializeField] private TextMeshProUGUI goldText;

        private readonly WaitForSeconds _waitRestore = new WaitForSeconds(1);

        public ReactiveProperty<int> Gold { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> HP { get; } = new ReactiveProperty<int>(MaxHP);

        private void Awake()
        {
            Gold.Select(StringBag.FromInt).SubscribeToTextMeshPro(goldText).AddTo(this);
            StartCoroutine(HPRestore());
        }

        private IEnumerator HPRestore()
        {
            while (true)
            {
                if (HP.Value < MaxHP) 
                    HP.Value += 1;
                yield return _waitRestore;
            }
        }
    }
}