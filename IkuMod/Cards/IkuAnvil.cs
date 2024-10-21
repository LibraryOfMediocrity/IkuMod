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
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuAnvilDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Blue, ManaColor.Red };
            config.Cost = new ManaGroup() { Blue = 1, Red = 1 };
            config.UpgradedCost = new ManaGroup() { Hybrid = 1, HybridColor = 5 };
            config.TargetType = TargetType.SingleEnemy;
            config.Damage = 12;
            config.UpgradedDamage = 14;
            config.Value1 = 2;
            config.UpgradedValue1 = 3;
            config.Value2 = 1;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc", "IkuSurgeSe" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc", "IkuSurgeSe" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }

        [EntityLogic(typeof(IkuAnvilDef))]
        public sealed class IkuAnvil : Card
        {
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                yield return AttackAction(selector);
                if (!base.Battle.BattleShouldEnd && base.Battle.HandZone.Count > 0)
                {
                    int surgeCount = 0;
                    SelectHandInteraction interaction = new SelectHandInteraction(0, base.Value1, base.Battle.HandZone)
                    {
                        Source = this
                    };
                    yield return new InteractionAction(interaction, false);
                    foreach (Card card in interaction.SelectedCards)
                    {
                        yield return new VeilCardAction(card);
                        surgeCount += base.Value2;
                    }
                    if (surgeCount > 0) yield return BuffAction<IkuSurgeSe>(surgeCount, 0, 0, 0, 0.2f);
                }
                yield break;
            }
        }
    }
}
