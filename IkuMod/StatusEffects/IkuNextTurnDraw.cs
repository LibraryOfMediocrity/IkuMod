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
    public sealed class IkuNextTurnDrawDef : IkuStatusEffectTemplate 
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            return config;
        }
    }

    [EntityLogic(typeof(IkuNextTurnDrawDef))]
    public sealed class IkuNextTurnDraw : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            base.NotifyActivating();
            yield return new DrawManyCardAction(base.Level);
            yield return new RemoveStatusEffectAction(this, true, 0.1f);
            yield break;
        }
    }
}
