using IkuMod.Cards.Template;
using LBoL.ConfigData;
using LBoL.Base;
using System;
using System.Collections.Generic;
using System.Text;
using LBoLEntitySideloader.Attributes;
using LBoL.Core.Cards;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoL.Core.Battle.BattleActions;
using System.Linq;
using LBoL.Core.Battle.Interactions;
using LBoL.Base.Extensions;
using IkuMod.BattleActions;
using UnityEngine;

namespace IkuMod.Cards
{
    public sealed class IkuAirDrawDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Skill;
            config.Rarity = Rarity.Common;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.TargetType = TargetType.Nobody;
            config.Cost = new ManaGroup() { Blue = 1 };
            config.Value1 = 3;
            config.Value2 = 3;
            config.UpgradedValue2 = 2;
            /*
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Value1 = 2;
            config.UpgradedValue1 = 3;
            */
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuAirDrawDef))]
    public sealed class IkuAirDraw : Card
    {
        private string Header
        {
            get
            {
                return this.LocalizeProperty("Header");
            }
        }
        //what if it just upgraded to always move 3 cards
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new DrawManyCardAction(base.Value1);
            if (base.Battle.HandZone.Count > base.Value2)
            {
                SelectCardInteraction interaction = new SelectCardInteraction(base.Value2, base.Value1, base.Battle.HandZone)
                {
                    Source = this,
                    Description = Header
                };
                yield return new InteractionAction(interaction, false);
                foreach (Card card in interaction.SelectedCards)
                {
                    yield return new VeilCardAction(card);
                }
            }
            else if (base.Battle.HandZone.Count > 0)
            {
                Card[] cards = base.Battle.HandZone.ToArray();
                foreach (Card card in cards)
                {
                    yield return new VeilCardAction(card);
                }
            }
            yield break;
        }
        /*
        public override bool DiscardCard
        {
            get
            {
                return true;
            }
        }

        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card hand) => hand != this).ToList<Card>();
            if (list.Count <= 0)
            {
                return null;
            }
            return new SelectHandInteraction(0, base.Value1, list);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            
            SelectHandInteraction selectHandInteraction = (SelectHandInteraction)precondition;
            IReadOnlyList<Card> readOnlyList = ((selectHandInteraction != null) ? selectHandInteraction.SelectedCards : null);
            if (readOnlyList != null && readOnlyList.Count > 0)
            {
                yield return new DiscardManyAction(readOnlyList);
                if (base.Battle.DrawZone.Count > 0)
                {
                    int num = readOnlyList.Count;
                    while (num > 0 && base.Battle.DrawZone.Count != 0 && base.Battle.HandZone.Count != base.Battle.MaxHand)
                    {
                        List<Card> list = base.Battle.DrawZone.Where((Card card) => !card.IsForbidden && card.Cost.Amount <= 1).ToList<Card>();
                        if (list.Count > 0)
                        {
                            Card card2 = list.Sample(base.BattleRng);
                            yield return new MoveCardAction(card2, CardZone.Hand);
                        }
                        num--;
                    }
                }
            }
            
            yield break;
        }
        */
    }
}
