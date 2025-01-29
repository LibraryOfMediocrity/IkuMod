using LBoL.Base;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.ConfigData;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;

namespace IkuMod.StatusEffects
{
    public sealed class TestGraceDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            return config;
        }
    }

    [EntityLogic(typeof(TestGraceDef))]
    public sealed class TestGrace : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            this._active = false;
            this._using = false;
            base.HandleOwnerEvent<DamageDealingEventArgs>(unit.DamageDealing, new GameEventHandler<DamageDealingEventArgs>(this.OnDamageDealing));
            base.HandleOwnerEvent<BlockShieldEventArgs>(unit.BlockShieldGaining, new GameEventHandler<BlockShieldEventArgs>(this.OnBlockShieldGaining));
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.ReactOwnerEvent<UsUsingEventArgs>(base.Battle.UsUsed, new EventSequencedReactor<UsUsingEventArgs>(this.OnUsUsed));
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsing));
            base.ReactOwnerEvent<UsUsingEventArgs>(base.Battle.UsUsing, new EventSequencedReactor<UsUsingEventArgs>(this.OnUsUsing));
        }

        private void OnDamageDealing(DamageDealingEventArgs args)
        {
            if (args.DamageInfo.DamageType == DamageType.Attack)
            {
                Card card = args.ActionSource as Card;
                if (card != null && card.CardType == CardType.Friend)
                {
                    return;
                }
                args.DamageInfo = args.DamageInfo.IncreaseBy(base.Level);
                args.AddModifier(this);
                if (args.Cause != ActionCause.OnlyCalculate)
                {
                    this._active = _using;
                }
            }
        }

        private void OnBlockShieldGaining(BlockShieldEventArgs args)
        {
            if (args.Type == BlockShieldType.Direct)
            {
                return;
            }
            ActionCause cause = args.Cause;
            if (cause == ActionCause.Card || cause == ActionCause.OnlyCalculate || cause == ActionCause.Us)
            {
                if (args.HasBlock)
                {
                    args.Block += (float)base.Level;
                }
                if (args.HasShield)
                {
                    args.Shield += (float)base.Level;
                }
                args.AddModifier(this);
                if (args.Cause != ActionCause.OnlyCalculate)
                {
                    this._active = _using;
                }
            }
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            _using = false;
            if (this._active)
            {
                yield return new RemoveStatusEffectAction(this, true, 0.1f);
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnUsUsed(UsUsingEventArgs args)
        {
            _using = false;
            if (this._active)
            {
                yield return new RemoveStatusEffectAction(this, true, 0.1f);
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
        {
            _using = true;
            yield break;
        }

        private IEnumerable<BattleAction> OnUsUsing(UsUsingEventArgs args)
        {
            _using = true;
            yield break;
        }

        public override string UnitEffectName
        {
            get
            {
                return "CallShenling";
            }
        }

        private bool _active;

        private bool _using;
    }
}

