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
    public sealed class IkuTenshiSwordDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.Rarity = Rarity.Uncommon;
            config.TargetType = TargetType.SingleEnemy;
            config.GunName = GunNameID.GetGunFromId(4121);
            config.GunNameBurst = GunNameID.GetGunFromId(511);
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Blue, ManaColor.Black, ManaColor.Red, ManaColor.Green, ManaColor.Colorless };
            config.Cost = new ManaGroup() { Any = 2 };
            config.Damage = 15;
            config.Value1 = 0;
            config.Keywords = Keyword.Exile;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuTenshiSwordDef))]
    public sealed class IkuTenshiSword : Card
    {
        public int HitTimes
        {
            get
            {
                return base.Value1 + 1;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector, base.GunName);
            for (int i = 0; i < base.Value1; i++)
            {
                yield return AttackAction(selector, "Instant");
            }
        }
    }
}
