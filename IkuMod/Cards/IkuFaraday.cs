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
using System.Text;
using IkuMod.StatusEffects;
using LBoL.Core.Battle.BattleActions;
using System.Diagnostics;
using UnityEngine;

namespace IkuMod.Cards
{
    public sealed class IkuFaradayDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Ability;
            config.Rarity = Rarity.Rare;
            config.Colors = new List<ManaColor>() { ManaColor.Blue, ManaColor.Red };
            config.Cost = new ManaGroup() { Blue = 1, Red = 1 };
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.Keywords = Keyword.Initial;
            config.UpgradedKeywords = Keyword.Initial;
            config.RelativeEffects = new List<string>() { "Graze" };
            config.UpgradedRelativeEffects = new List<string>() { "Graze" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }

        [EntityLogic(typeof(IkuFaradayDef))]
        public sealed class IkuFaraday : Card
        {

            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                yield return BuffAction<IkuFaradaySe>(base.Value1);
                yield break;
            }
        }
    }
}
