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
    public sealed class IkuJudgementDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.Rarity = Rarity.Rare;
            config.TargetType = TargetType.SingleEnemy;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 4, Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 4 };
            config.Damage = 0;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuJudgementDef))]
    public sealed class IkuJudgement : Card
    {
        public override int AdditionalDamage
        {
            get
            {
                if (base.Battle != null && PendingTarget != null)
                {
                    return PendingTarget.MaxHp / 2;
                }
                else
                {
                    return 0;
                }
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            yield break;
        }
    }
}
