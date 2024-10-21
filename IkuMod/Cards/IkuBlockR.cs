using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace IkuMod.Cards
{
    public sealed class IkuBlockRDef : IkuCardTemplate
    { 
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Defense;
            config.IsPooled = false;
            config.FindInBattle = false;
            config.HideMesuem = false;
            config.Block = 10;
            config.UpgradedBlock = 13;
            config.TargetType = TargetType.Self;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1, Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 2 };
            config.Keywords = Keyword.Basic;
            config.UpgradedKeywords = Keyword.Basic;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    [EntityLogic(typeof(IkuBlockRDef))]
    public sealed class IkuBlockR : Card
    {

    }
}
