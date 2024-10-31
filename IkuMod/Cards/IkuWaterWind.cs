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
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using System.Linq;
using IkuMod.BattleActions;
using LBoL.Presentation.UI.Panels;

namespace IkuMod.Cards
{
    public sealed class IkuWaterWindDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Blue = 2 };
            config.TargetType = TargetType.SingleEnemy;
            config.Damage = 16;
            config.UpgradedDamage = 18;
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuWaterWindDef))]
    public sealed class IkuWaterWind : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector);
            if (!base.Battle.BattleShouldEnd)
            {
                yield return new DrawManyCardAction(base.Value1);
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
                    Card[] cards = (from card in base.Battle.HandZone where card != this select card).ToArray();
                    foreach (Card card in cards)
                    {
                        yield return new VeilCardAction(card);
                    }
                }
            }
            yield break;
        }
    }
}
