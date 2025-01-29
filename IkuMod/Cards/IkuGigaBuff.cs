using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using LBoL.Core.Battle.BattleActions;

namespace IkuMod.Cards
{
    public sealed class IkuGigaBuffDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Ability;
            config.TargetType = TargetType.Self;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Blue, ManaColor.Red };
            config.Cost = new ManaGroup() { White = 1, Blue = 1, Red = 1 };
            config.Value1 = 3;
            config.UpgradedKeywords = Keyword.Retain;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuGigaBuffDef))]
    public sealed class IkuGigaBuff : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return SacrificeAction(base.Value1);
            var statusEffect = base.Battle.Player.StatusEffects;
            foreach (StatusEffect status in statusEffect)
            {
                if (status != null && status.Type == StatusEffectType.Positive && status.IsStackable && (status.HasLevel || status.HasDuration))
                {
                    if (status.HasLevel) status.Level *= 2;
                    if (status.HasDuration) status.Duration *= 2;
                }
            }
            yield break;
        }
    }
}
