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
    public sealed class IkuRampDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Skill;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 2, Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 2 };
            config.Value1 = 2;
            config.Value2 = 1;
            config.RelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.Keywords = Keyword.Grow;
            config.UpgradedKeywords = Keyword.Grow;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }

    }

    [EntityLogic(typeof(IkuRampDef))]
    public sealed class IkuRamp : Card
    {
        public override int AdditionalValue1 => GrowCount * base.Value2;

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<IkuSurgeSe>(base.Value1);
            yield break;
        }
    }
}
//2R(2) gain 1(2) surge growth this card gives 1 additional surge
//charge acceleration