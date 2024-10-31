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
using UnityEngine.Rendering;
using IkuMod.StatusEffects;

namespace IkuMod.Cards
{
    public sealed class IkuGeneratorDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Ability;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 2 };
            config.TargetType = TargetType.Nobody;
            config.Value1 = 1;
            config.Value2 = 2;
            config.RelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuGeneratorDef))]
    public sealed class IkuGenerator : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (this.IsUpgraded)
            {
                yield return BuffAction<IkuSurgeSe>(base.Value2, 0, 0, 0, 0.2f);
            }
            yield return BuffAction<IkuGeneratorSe>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
}
