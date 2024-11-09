﻿using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuEchoDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Skill;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Blue, ManaColor.Red };
            config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 5 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Value2 = 1;
            config.Value1 = 1;
            config.TargetType = TargetType.Self;
            config.RelativeKeyword = Keyword.Echo;
            config.UpgradedRelativeKeyword = Keyword.Echo;
            config.UpgradedKeywords = Keyword.Replenish;
            config.RelativeEffects = new List<string>() { "Graze", "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "Graze", "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuEchoDef))]
    public sealed class IkuEcho : Card
    {
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card card) => card != this).ToList<Card>();
            if (list.Count <= 0)
            {
                return null;
            }
            return new SelectHandInteraction(base.Value1, base.Value1, list);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<Graze>(base.Value2);
            if (precondition != null)
            {
                IReadOnlyList<Card> cards = ((SelectHandInteraction)precondition).SelectedCards;
                if (cards.Count > 0)
                {
                    foreach (Card card in cards)
                    {
                        yield return new VeilCardAction(card);
                        card.IsEcho = true;
                    }
                }
            }
            yield break;
        }
    }
}