using IkuMod.Cards.Template;
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
    public sealed class IkuJuggernautDef : IkuCardTemplate
    {
        public override bool UseDefault => true;
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Defense;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.TargetType = TargetType.Self;
            config.Cost = new ManaGroup() { Any = 2, Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1, Red = 1 };
            config.Block = 18;
            config.Value1 = 1;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.RelativeCards = new List<string>() { "IkuRedSprite" };
            config.UpgradedRelativeCards = new List<string>() { "IkuRedSprite" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }

    }

    [EntityLogic(typeof(IkuJuggernautDef))]
    public sealed class IkuJuggernaut : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new CastBlockShieldAction(base.Battle.Player, base.Block);
            yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<IkuRedSprite>() });
            yield break;
        }
    }
}
