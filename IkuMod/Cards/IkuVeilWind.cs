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

namespace IkuMod.Cards
{
    public sealed class IkuVeilWindDef : IkuCardTemplate
    {
        public override bool UseDefault => true;
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.TargetType = TargetType.Nobody;
            config.FindInBattle = false;
            config.IsPooled = false;
            config.Cost = new ManaGroup() { Any = 0 };
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.Keywords = Keyword.Exile | Keyword.Retain;
            config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuVeilWindDef))]
    public sealed class IkuVeilWind : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            
            if (base.Battle.HandZone.Count > base.Value1)
            {
                SelectHandInteraction interaction = new SelectHandInteraction(base.Value1, base.Value1, base.Battle.HandZone)
                {
                    Source = this
                };
                yield return new InteractionAction(interaction, false);
                foreach (Card card in interaction.SelectedCards)
                {
                    yield return new VeilCardAction(card);
                }
                interaction = null;
            }
            else
            {
                Card[] cards = base.Battle.HandZone.ToArray<Card>();
                foreach (Card card in cards)
                {
                    yield return new VeilCardAction(card);
                }
            }
            yield return new DrawManyCardAction(base.Value1);
            yield break;
        }
    }
}
