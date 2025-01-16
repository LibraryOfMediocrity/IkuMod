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
    public sealed class IkuOceanDef : IkuCardTemplate
    {

        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Ability;
            config.Rarity = Rarity.Rare;
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Blue };
            config.Cost = new ManaGroup() { Blue = 2, Red = 2 };
            config.UpgradedCost = new ManaGroup() { Blue = 1, Red = 1 };
            config.Value1 = 1;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuOceanDef))]
    public sealed class IkuOcean : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<IkuOceanSe>(base.Value1);
            yield break;
        }
    }
}
