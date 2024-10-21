using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace IkuMod.StatusEffects
{
    public sealed class IkuSurgeSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            return config;
        }
    }
    [EntityLogic(typeof(IkuSurgeSeDef))]
    public sealed class IkuSurgeSe : StatusEffect
    {
        //mess with damage rate for balance -> 1.25f
        //find scaling through other means like firepower
        public float DamageRate { get; set; } = 1.5f;
        public bool cardUsing = false;
        public Card used;
        public bool added;

        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsing));
            base.HandleOwnerEvent<DamageDealingEventArgs>(unit.DamageDealing, new GameEventHandler<DamageDealingEventArgs>(this.OnDamageDealing));
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        private void OnDamageDealing(DamageDealingEventArgs args)
        {
            if (args.DamageInfo.DamageType == DamageType.Attack)
            {
                Card card = args.ActionSource as Card;
                if (card != null && card.CardType == CardType.Attack && cardUsing && card == used)
                {
                    args.DamageInfo = args.DamageInfo.MultiplyBy(this.DamageRate);
                    args.AddModifier(this);
                }
                else if(args.Cause == ActionCause.OnlyCalculate && card != null && card.CardType == CardType.Attack)
                {
                    args.DamageInfo = args.DamageInfo.MultiplyBy(this.DamageRate);
                    args.AddModifier(this);
                }
            }
        }
        private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
        {
            if (args.Card.CardType == CardType.Attack)
            {
                cardUsing = true;
                used = args.Card;
            }
            yield break;
        }
        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (cardUsing)
            {
                int num = base.Level - 1;
                base.Level = num;
                if (base.Level <= 0)
                {
                    yield return new RemoveStatusEffectAction(this, true, 0.1f);
                }
                cardUsing = false;
            }
            yield break;
        }

        public BattleAction SurgeUsed()
        {
            int num = base.Level - 1;
            base.Level = num;
            if (base.Level <= 0)
            {
                return new RemoveStatusEffectAction(this, true, 0.1f);
            }
            return null;
        }

    }
}
