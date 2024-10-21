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
using LBoL.Core.Battle.BattleActions;
using UnityEngine.Rendering;
using IkuMod.StatusEffects;

namespace IkuMod.Cards
{
    public sealed class IkuVeilBlockDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Defense;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 1, Blue = 1 };
            config.TargetType = TargetType.Self;
            config.Block = 12;
            config.UpgradedBlock = 17;
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuVeilBlockDef))]
    public sealed class IkuVeilBlock : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new CastBlockShieldAction(base.Battle.Player, base.Block, true);
            yield return BuffAction<IkuVeilNextSe>(Value1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
}
