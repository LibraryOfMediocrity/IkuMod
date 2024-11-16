using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Units;

namespace IkuMod.StatusEffects
{
    public sealed class IkuArcSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { "IkuSurgeSe" };
            return config;
        }
    }

    [EntityLogic(typeof(IkuArcSeDef))]
    public sealed class IkuArcSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<DamageDealingEventArgs>(unit.DamageDealing, new GameEventHandler<DamageDealingEventArgs>(this.OnDamageDealing));
        }

        private void OnDamageDealing(DamageDealingEventArgs args)
        {
            if (args.DamageInfo.DamageType == DamageType.Attack && base.Battle.Player.HasStatusEffect<IkuSurgeSe>())
            {
                Card card = args.ActionSource as Card;
                if (card != null && card.CardType == CardType.Attack)
                {
                    args.DamageInfo = args.DamageInfo.IncreaseBy(base.Level);
                    args.AddModifier(this);
                }
                else if (args.Cause == ActionCause.OnlyCalculate && card != null && card.CardType == CardType.Attack)
                {
                    args.DamageInfo = args.DamageInfo.IncreaseBy(base.Level);
                    args.AddModifier(this);
                }
            }
        }
    }
}
