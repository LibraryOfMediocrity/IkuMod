using IkuMod.BattleActions;
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
using System.Text;

namespace IkuMod.StatusEffects
{
    public sealed class IkuSkyBarrierSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Keywords = Keyword.Shield;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            return config;
        }
    }

    [EntityLogic(typeof(IkuSkyBarrierSeDef))]
    public sealed class IkuSkyBarrierSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<VeilCardEventArgs>(IkuGameEvents.PostVeilEvent, new EventSequencedReactor<VeilCardEventArgs>(this.OnCardVeiled));
        }

        private IEnumerable<BattleAction> OnCardVeiled(VeilCardEventArgs args)
        {
            this.NotifyActivating();
            yield return new CastBlockShieldAction(Owner, 0, base.Level, BlockShieldType.Direct);
            yield break;
        }
    }
}
