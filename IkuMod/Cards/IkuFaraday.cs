using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using IkuMod.StatusEffects;
using LBoL.Core.Battle.BattleActions;
using System.Diagnostics;
using UnityEngine;

namespace IkuMod.Cards
{
    public sealed class IkuFaradayDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1, Red = 1 };
            config.UpgradedCost = new ManaGroup() { Red = 1 };
            config.Damage = 10;
            config.UpgradedDamage = 12;
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.Value2 = 1;
            config.RelativeEffects = new List<string>() { "Graze" };
            config.UpgradedRelativeEffects = new List<string>() { "Graze" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }

        [EntityLogic(typeof(IkuFaradayDef))]
        public sealed class IkuFaraday : Card
        {
            protected override void OnEnterBattle(BattleController battle)
            {
                base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsing));
            }

            private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
            {
                //UnityEngine.Debug.Log("Activated cardusing: " + args.Card.Name);
                if (args.Card == this && Triggered)
                {
                    //UnityEngine.Debug.Log("Successfully activated playtwice");
                    IkuSurgeSe status = base.Battle.Player.GetStatusEffect<IkuSurgeSe>();
                    args.PlayTwice = true;
                    status.SurgeUsed();
                    args.AddModifier(this);
                }
                yield break;
            }

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
                yield return BuffAction<Graze>(base.Value1);
                yield return AttackAction(selector);
                yield return new DrawManyCardAction(base.Value2);
                yield break;
            }
        }
    }
}
