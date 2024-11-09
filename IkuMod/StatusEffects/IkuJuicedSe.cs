using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IkuMod.StatusEffects
{
    public sealed class IkuJuicedSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            return config;
        }
    }

    [EntityLogic(typeof(IkuJuicedSeDef))]
    public sealed class IkuJuicedSe : StatusEffect
    {
        public ManaGroup GainMana { get; private set; }

        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            GainMana = new ManaGroup() { Philosophy = base.Level };
        }

        public override bool Stack(StatusEffect other)
        {
            if (HasLevel)
            {
                _level = Config.LevelStackType switch
                {
                    StackType.Add => Math.Min(Level + other.Level, 999),
                    StackType.Max => Math.Max(Level, other.Level),
                    StackType.Min => Math.Min(Level, other.Level),
                    StackType.Keep => Level,
                    StackType.Overwrite => other.Level,
                    _ => throw new InvalidDataException($"Invalid stack type {Config.LevelStackType} for {DebugName}"),
                };
            }
            GainMana = new ManaGroup() { Philosophy = base.Level };
            this.NotifyChanged();
            return true;
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (args.Card != null && args.Card.CardType == CardType.Attack && args.ConsumingMana != ManaGroup.Empty)
            {
                yield return new GainManaAction(GainMana);
                this.NotifyActivating();
            }
            yield break;
        }
    }
}
