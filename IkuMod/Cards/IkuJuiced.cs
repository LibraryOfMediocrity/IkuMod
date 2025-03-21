﻿using IkuMod.Cards.Template;
using IkuMod.StatusEffects;
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
    public sealed class IkuJuicedDef : IkuCardTemplate 
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Ability;
            config.Rarity = Rarity.Rare;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 2, Red = 3 };
            config.UpgradedCost = new ManaGroup() { Red = 3 };
            config.Mana = new ManaGroup() { Philosophy = 1 };
            config.Value1 = 1;
            config.Value2 = 2;
            config.RelativeKeyword = Keyword.Philosophy;
            config.UpgradedRelativeKeyword = Keyword.Philosophy;
            config.UpgradedRelativeCards = new List<string>() { "IkuRedSprite" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuJuicedDef))]
    public sealed class IkuJuiced : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<IkuJuicedSe>(base.Value1);
            yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<IkuRedSprite>(), Library.CreateCard<IkuRedSprite>() });
            yield break;
        }
    }
}
