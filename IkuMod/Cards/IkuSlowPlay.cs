using IkuMod.Cards.Template;
using IkuMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace IkuMod.Cards
{
    public sealed class IkuSlowPlayDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 1, Hybrid = 2, HybridColor = 5 };
            config.TargetType = TargetType.Self;
            config.Value1 = 4;
            config.Value2 = 4;
            config.UpgradedValue2 = 5;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuSlowPlayDef))]
    public sealed class IkuSlowPlay : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<IkuSlowPlaySe>(base.Value1, 0, base.Value2, 0, 0.2f);
            yield break;
        }
    }

}
