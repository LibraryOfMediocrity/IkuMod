using IkuMod.Cards.Template;
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
    public sealed class IkuRailgunDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.GunName = GunNameID.GetGunFromId(7340);
            config.GunNameBurst = GunNameID.GetGunFromId(7341);
            config.Colors = new List<ManaColor>() { ManaColor.Green };
            config.Cost = new ManaGroup() { Any = 1, Green = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Damage = 16;
            config.UpgradedDamage = 20;
            config.Value1 = 8;
            config.UpgradedValue1 = 10;
            config.TargetType = TargetType.SingleEnemy;
            config.Keywords = Keyword.Grow | Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Grow | Keyword.Accuracy;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }

        [EntityLogic(typeof(IkuRailgunDef))]
        public sealed class IkuRailgun : Card
        {
            public override int AdditionalDamage => base.Value1 * base.GrowCount;

            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                yield return AttackAction(selector);
                
                yield break;
            }
        }
    }
}
