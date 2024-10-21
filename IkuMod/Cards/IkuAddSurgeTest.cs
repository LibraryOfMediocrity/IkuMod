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
    public sealed class IkuAddSurgeTestDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup { Any = 1 };
            config.HideMesuem = true;
            config.FindInBattle = false;
            config.IsPooled = false;
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.TargetType = TargetType.Self;
            config.RelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuAddSurgeTestDef))]
    public sealed class IkuAddSurgeTest : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<IkuSurgeSe>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }
        
    }
}
