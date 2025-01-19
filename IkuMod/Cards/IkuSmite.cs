using IkuMod.BattleActions;
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
    public sealed class IkuSmiteDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.GunName = GunNameID.GetGunFromId(6100);
            config.GunNameBurst = GunNameID.GetGunFromId(6100);
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 2, Blue = 2 };
            config.UpgradedCost = new ManaGroup() { Any = 3, Blue = 1 };
            config.Damage = 16;
            config.UpgradedDamage = 20;
            config.Value1 = 8;
            config.UpgradedValue1 = 10;
            config.TargetType = TargetType.SingleEnemy;
            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }

        [EntityLogic(typeof(IkuSmiteDef))]
        public sealed class IkuSmite : Card
        {

            public override IEnumerable<BattleAction> OnDraw()
            {
                return this.OnEnterHand();
            }

            public override IEnumerable<BattleAction> OnMove(CardZone srcZone, CardZone dstZone)
            {
                if (dstZone != CardZone.Hand)
                {
                    return null;
                }
                return this.OnEnterHand();
            }

            protected override void OnEnterBattle(BattleController battle)
            {
                if (base.Zone == CardZone.Hand)
                {
                    base.React(this.OnEnterHand());
                }
            }

            private IEnumerable<BattleAction> OnEnterHand()
            {
                base.DeltaDamage += base.Value1;
                yield break;
            }

            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                yield return AttackAction(selector);
                yield break;
            }
        }
    }
}
