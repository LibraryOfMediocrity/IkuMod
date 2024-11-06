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
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Value1 = 2;
            config.UpgradedValue1 = 3;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuAirDrawDef))]
    public sealed class IkuAirDraw : Card
    {
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
    }
}
