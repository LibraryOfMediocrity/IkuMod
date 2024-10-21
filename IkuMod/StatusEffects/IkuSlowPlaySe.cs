using IkuMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace IkuMod.StatusEffects
{
    public sealed class IkuSlowPlaySeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.HasCount = true;
            config.LimitStackType = StackType.Max;
            return config;
        }
    }

    [EntityLogic(typeof(IkuSlowPlaySeDef))]
    public sealed class IkuSlowPlaySe : StatusEffect
    {
        private bool IsActive = true;
        public int Cards
        {
            get
            {
                return base.Battle.TurnCardUsageHistory.Count;
            }
        }

        protected override void OnAdded(Unit unit)
        {
            this.SetCount();
            base.HandleOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, delegate (CardUsingEventArgs _)
            {
                this.SetCount();
            });
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnStarting));
        }

        private void SetCount()
        {
            if (base.Battle.TurnCardUsageHistory.Count <= base.Limit)
            {
                base.Count = base.Limit - base.Battle.TurnCardUsageHistory.Count;
                IsActive = true;
            }
            else
            {
                base.Count = 0;
                IsActive = false;
            }
        }

        private IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
        {
            if (IsActive)
            {
                base.NotifyActivating();
                yield return BuffAction<TempFirepower>(base.Level, 0, 0, 0, 0.2f);
                yield return BuffAction<TempSpirit>(base.Level, 0, 0, 0, 0.2f);
            }
            this.SetCount();
            yield break;
        }
    }
}
