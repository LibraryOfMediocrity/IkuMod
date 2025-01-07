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
    public sealed class IkuVisitorDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Ability;
            config.Rarity = Rarity.Uncommon;
            config.Cost = new ManaGroup() { Any = 2, Hybrid = 2, HybridColor = 5 };
            config.UpgradedCost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 5 };
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Blue };
            config.Value1 = 1;
            config.RelativeCards = new List<string>() { "IkuRedSprite", "IkuVeilWind" };
            config.UpgradedRelativeCards = new List<string>() { "IkuRedSprite", "IkuVeilWind" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuVisitorDef))]
    public sealed class IkuVisitor : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<IkuVisitorSe>(base.Value1);
            yield break;
        }
    }
}
