using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class DebuffCopierTestDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Skill;
            config.Rarity = Rarity.Uncommon;
            config.HideMesuem = true;
            config.FindInBattle = false;
            config.IsPooled = false;
            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.TargetType = TargetType.SingleEnemy;
            config.Cost = new ManaGroup() { Any = 1 };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(DebuffCopierTestDef))]
    public sealed class DebuffCopierTest  : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            var statusEffect = selector.SelectedEnemy.StatusEffects;
            foreach (StatusEffect status in statusEffect)
            {
                if (status != null && status.Type == StatusEffectType.Negative)
                {
                    Type type = status.GetType();
                    int level = 0, duration = 0, limit = 0;
                    if (status.HasLevel) level = status.Level;
                    if (status.HasDuration) duration = status.Duration;
                    foreach (BattleAction action in DebuffAction(type, base.Battle.AllAliveEnemies, level, duration, status.Limit))
                    {
                        yield return action;
                    }
                }
            }
            yield break;
        }
    }

}
