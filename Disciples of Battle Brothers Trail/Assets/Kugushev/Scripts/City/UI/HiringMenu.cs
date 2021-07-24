using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.UI;
using Kugushev.Scripts.Game.Models.CityInfo;
using Kugushev.Scripts.Game.Models.HeroInfo;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Kugushev.Scripts.City.UI
{
    public class HiringMenu : ModalMenu
    {
        [SerializeField] private Button exit;
        [SerializeField] private Transform mercenariesList;
        [SerializeField] private MercenaryCard mercenaryCardPrefab;

        private readonly List<MercenaryCard> _items = new List<MercenaryCard>();
        private HiringDeskInfo _hiringDeskInfo;
        private Hero _hero;

        protected override void Awake()
        {
            base.Awake();
            exit.onClick.AddListener(CloseMenu);
        }

        public void InitMercenaries(HiringDeskInfo hiringDeskInfo, Hero hero)
        {
            _hiringDeskInfo = hiringDeskInfo;
            _hero = hero;

            foreach (var mercenary in hiringDeskInfo.Mercenaries)
            {
                var card = Instantiate(mercenaryCardPrefab, mercenariesList);
                card.Init(mercenary, _hero, _hiringDeskInfo);
                _items.Add(card);
            }

            hiringDeskInfo.Mercenaries.ObserveRemove().Subscribe(RemoveUnit).AddTo(this);
        }

        private void RemoveUnit(CollectionRemoveEvent<BattleUnit> evt)
        {
            var card = _items[evt.Index];
            _items.RemoveAt(evt.Index);
            Destroy(card.gameObject);
        }
    }
}