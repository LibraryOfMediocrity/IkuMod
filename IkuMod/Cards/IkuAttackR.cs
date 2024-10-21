using System;
using System.Collections.Generic;
using IkuMod.Cards.Template;
using System.Text;
using LBoL.ConfigData;
using LBoL.Base;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;

namespace IkuMod.Cards
{
    public sealed class IkuAttackRDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = "Simple1";
            config.GunNameBurst = "Simple2";
            config.Rarity = Rarity.Common;
            config.Type = CardType.Attack;
            config.IsPooled = false;
            config.FindInBattle = false;
            config.HideMesuem = false;
            config.Damage = 10;
            config.UpgradedDamage = 14;
            config.TargetType = TargetType.SingleEnemy;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1, Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 2 };
            config.Keywords = Keyword.Basic;
            config.UpgradedKeywords = Keyword.Basic;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    [EntityLogic(typeof(IkuAttackRDef))]
    public sealed class IkuAttackR : Card
    {

    }
}
