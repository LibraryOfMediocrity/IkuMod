using IkuMod.Cards.Template;
using IkuMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuDynamoDef : IkuCardTemplate
    {
        public override bool UseDefault => true;
    
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Ability;
            config.Rarity = Rarity.Rare;
            config.TargetType = TargetType.Nobody;
            config.Colors = new List<ManaColor>() { ManaColor.Blue, ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1, Blue = 1, Red = 1 };   
            config.UpgradedCost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 5 };
            config.Value1 = 1;
            config.RelativeEffects = new List<string>() { "IkuSurgeSe", "IkuVeilDisc" };
            config.UpgradedRelativeEffects  = new List<string>() { "IkuSurgeSe", "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuDynamoDef))]
    public sealed class IkuDynamo : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<IkuDynamoSe>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
}
