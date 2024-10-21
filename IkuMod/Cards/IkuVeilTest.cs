using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using IkuMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace IkuMod.Cards
{
    public sealed class IkuVeilTestDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.HideMesuem = true;
            config.Cost = new ManaGroup { Any = 1 };
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.TargetType = TargetType.Nobody;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuVeilTestDef))]
    public sealed class IkuVeilTest : Card
    {
        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card card) => card != this).ToList<Card>();
            if (list.Count <= 0)
            {
                return null;
            }
            return new SelectHandInteraction(0, base.Value1, list);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                IReadOnlyList<Card> cards = ((SelectHandInteraction)precondition).SelectedCards;
                if (cards.Count > 0)
                {
                    foreach (Card card in cards)
                    {
                        yield return new VeilCardAction(card);
                    }
                }
            }
            yield return BuffAction<IkuVeilNextSe>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }

    }
}
