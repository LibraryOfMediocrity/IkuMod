using IkuMod.Cards.Template;
using IkuMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuTailwindDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.GunName = GunNameID.GetGunFromId(7310);
            config.GunNameBurst = GunNameID.GetGunFromId(7311);
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Blue = 1 };
            config.IsXCost = true;
            config.Damage = 6;
            config.UpgradedDamage = 8;
            config.Value1 = 1;
            config.Value2 = 1;
            config.UpgradedValue2 = 2;
            config.TargetType = TargetType.SingleEnemy;
            config.RelativeEffects = new List<string>() { "TempFirepower" };
            config.UpgradedRelativeEffects = new List<string>() { "TempFirepower" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuTailwindDef))]
    public sealed class IkuTailwind : Card
    {
        public override ManaGroup GetXCostFromPooled(ManaGroup pooledMana)
        {
            return pooledMana;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            for (int i = 0; i < base.SynergyAmount(consumingMana, ManaColor.Any, 1); i++)
            {
                yield return AttackAction(selector);
                if (base.Battle.BattleShouldEnd) yield break;
            }

            int num = base.SynergyAmount(consumingMana, ManaColor.Blue, 1);
            yield return BuffAction<IkuNextTurnDraw>(base.Value1 * num);
            yield return BuffAction<IkuNextTurnTempFire>(base.Value2 * num);

        }
    }
}
