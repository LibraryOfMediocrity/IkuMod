﻿using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuAddVeilDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.Rarity = Rarity.Common;
            config.TargetType = TargetType.SingleEnemy;
            config.GunName = GunNameID.GetGunFromId(512);
            config.GunNameBurst = GunNameID.GetGunFromId(513);
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 1, Blue = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 2 };
            config.Damage = 10;
            config.UpgradedDamage = 14;
            config.Block = 10;
            config.UpgradedBlock = 14;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.RelativeCards = new List<string>() { "IkuVeilWind" };
            config.UpgradedRelativeCards = new List<string>() { "IkuVeilWind" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuAddVeilDef))]
    public sealed class IkuAddVeil : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return AttackAction(selector);
            yield return new AddCardsToHandAction(Library.CreateCard("IkuVeilWind"));
            yield break;
        }
    }
}
