using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.StatusEffects
{
    public sealed class IkuDefensiveSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Keywords = Keyword.Block;
            return config;
        }
    }

    [EntityLogic(typeof(IkuDefensiveSeDef))]
    public sealed class IkuDefensiveSe : StatusEffect
    {

        public bool cardUsing = false;
        public Card used;

        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<DamageEventArgs>(base.Battle.Player.DamageDealt, new EventSequencedReactor<DamageEventArgs>(this.OnPlayerDamageDealt));
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsing));
            base.HandleOwnerEvent<UnitEventArgs>(unit.TurnEnded, delegate (UnitEventArgs _)
            {
                this.React(new RemoveStatusEffectAction(this, true, 0.1f));
            });
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

        private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
        {
            if (args.Card.CardType == CardType.Attack)
            {
                cardUsing = true;
                this.NotifyActivating();
                used = args.Card;
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnPlayerDamageDealt(DamageEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            if (args.Cause == ActionCause.Card && args.ActionSource == used)
            {
                int damageInfo = (int)args.DamageInfo.Damage;
                if (damageInfo > 0f)
                {
                    yield return new CastBlockShieldAction(Owner, damageInfo, 0, BlockShieldType.Direct, false);
                }
            }
            yield break;
        }
    }
}
