using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace IkuMod.Cards
{
    public sealed class TrapTestDef : TrapTemplateTest
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Cost = new ManaGroup() { Any = 0 };
            config.Damage = 8;
            config.UpgradedDamage = 10;
            config.Rarity = Rarity.Common;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(TrapTestDef))]
    public sealed class TrapTest : TrapCardTest
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<DamageEventArgs>(base.Battle.Player.DamageReceived, new EventSequencedReactor<DamageEventArgs>(this.OnDamageRecieving));
        }

        private IEnumerable<BattleAction> OnDamageRecieving(DamageEventArgs args)
        {
            Debug.Log("Damage Recieved");
            if (args.Source != base.Battle.Player && args.Source.IsAlive && args.DamageInfo.DamageType == DamageType.Attack && args.DamageInfo.Amount > 0f)
            {
                base.NotifyActivating();
                var BattleActions = this.TrapTriggered(GetUnits(args.Source));
                foreach (var action in BattleActions)
                {
                    yield return action;
                }
                yield break;
            }
            yield break;
        }

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] selector)
        {
            Debug.Log("Trap Triggered");
            yield return AttackAction(selector, this.GunName, base.Damage);
            yield break;
        }

    }
}
