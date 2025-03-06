using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace IkuMod.Cards
{
    public sealed class IkuQuantumDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Skill;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Blue, ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.Keywords = Keyword.Exile | Keyword.Retain;
            config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
            config.RelativeCards = new List<string>() { "IkuSuperposition" };
            config.UpgradedRelativeCards = new List<string>() { "IkuSuperposition" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuQuantumDef))]
    public sealed class IkuQuantum : Card
    {
        private string Header
        {
            get
            {
                return this.LocalizeProperty("Header");
            }
        }

        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card card) => card != this).ToList<Card>();
            if (list.Count <= 0)
            {
                return null;
            }
            return new SelectHandInteraction(0, base.Value1, list)
            {
                Source = this,
                Description = Header
            };
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            IkuSuperposition card = Library.CreateCard<IkuSuperposition>();
            if (precondition != null)
            {
                IReadOnlyList<Card> cards = ((SelectHandInteraction)precondition).SelectedCards;
                if (cards.Count > 0)
                {
                    card.Linked = cards.ToList();
                }
            }
            yield return new AddCardsToHandAction(card);
            yield break;
        }
    }
}
