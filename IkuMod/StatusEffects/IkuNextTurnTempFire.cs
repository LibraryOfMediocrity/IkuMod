using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Units;

namespace IkuMod.StatusEffects
{
    public sealed class IkuNextTurnTempFireDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { "TempFirepower" };
            return config;
        }
    }

    [EntityLogic(typeof(IkuNextTurnTempFireDef))]
    public sealed class IkuNextTurnTempFire : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            base.NotifyActivating();
            yield return BuffAction<TempFirepower>(base.Level);
            yield return new RemoveStatusEffectAction(this, true, 0.1f);
            yield break;
        }
    }
}
