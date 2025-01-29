using IkuMod.Cards.Template;
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
    public sealed class Iku12VoltDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.Rarity = Rarity.Uncommon;
            config.TargetType = TargetType.SingleEnemy;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1, Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Damage = 10;
            config.UpgradedDamage = 13;
            config.Value1 = 1;
            config.RelativeEffects = new List<string>() { "IkuConduitDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuConduitDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(Iku12VoltDef))]
    public sealed class Iku12Volt : Card
    {
        public override bool Triggered
        {
            get
            {
                if (base.Battle.Player.HasStatusEffect<IkuSurgeSe>())
                {
                    IkuSurgeSe status = base.Battle.Player.GetStatusEffect<IkuSurgeSe>();
                    return base.Battle != null && status.Level > 1;
                }
                return false;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (PlayInTriggered)
            {
                IkuSurgeSe status = base.Battle.Player.GetStatusEffect<IkuSurgeSe>();
                status.SurgeUsed();
                yield return BuffAction<IkuSurgeSe>(base.Value1);
                yield return new MoveCardAction(this, CardZone.Hand);
            }
            yield break;
        }
    }
}
