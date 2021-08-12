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
        [SerializeField] private Transform teamList;
        [SerializeField] private TeammateCard teammateCardPrefab;

        private readonly List<MercenaryCard> _mercenaryCards = new List<MercenaryCard>();
        private readonly List<TeammateCard> _teammateCards = new List<TeammateCard>();
        private HiringDeskInfo _hiringDeskInfo;
        private Hero _hero;

        protected override void Awake()
        {
            base.Awake();
            exit.onClick.AddListener(CloseMenu);
        }

        public void Init(HiringDeskInfo hiringDeskInfo, Hero hero)
        {
            _hiringDeskInfo = hiringDeskInfo;
            _hero = hero;

            foreach (var mercenary in hiringDeskInfo.Mercenaries)
            {
                var card = Instantiate(mercenaryCardPrefab, mercenariesList);
                card.Init(mercenary, _hero, _hiringDeskInfo);
                _mercenaryCards.Add(card);
            }

            hiringDeskInfo.Mercenaries.ObserveRemove().Subscribe(RemoveMerc).AddTo(this);

            foreach (var teammate in hero.Team)
            {
                AddTeammate(teammate);
            }

            hero.Team.ObserveAdd().Subscribe(evt => AddTeammate(evt.Value)).AddTo(this);
            hero.Team.ObserveRemove().Subscribe(RemoveTeammate).AddTo(this);
        }

        private void AddTeammate(Teammate teammate)
        {
            var card = Instantiate(teammateCardPrefab, teamList);
            card.Init(teammate, _hero);
            _teammateCards.Add(card);
        }

        private void RemoveTeammate(CollectionRemoveEvent<Teammate> evt)
        {
            var card = _teammateCards[evt.Index];
            _teammateCards.RemoveAt(evt.Index);
            Destroy(card.gameObject);
        }

        private void RemoveMerc(CollectionRemoveEvent<BattleUnit> evt)
        {
            var card = _mercenaryCards[evt.Index];
            _mercenaryCards.RemoveAt(evt.Index);
            Destroy(card.gameObject);
        }
    }
}