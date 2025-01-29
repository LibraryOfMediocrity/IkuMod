using IkuMod.BattleActions;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.StatusEffects
{
    public sealed class IkuDynamoSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { "IkuSurgeSe", "IkuVeilDisc" };
            return config;
        }
    }

    [EntityLogic(typeof(IkuDynamoSeDef))]
    public sealed class IkuDynamoSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<VeilCardEventArgs>(IkuGameEvents.PostVeilEvent, new EventSequencedReactor<VeilCardEventArgs>(this.OnCardVeiled));
        }

        private IEnumerable<BattleAction> OnCardVeiled(VeilCardEventArgs args)
        {
            if (args.Card.CardType == CardType.Attack)
            {
                this.NotifyActivating();
                yield return BuffAction<IkuSurgeSe>(base.Level, 0, 0, 0, 0.2f);
            }
            yield break;
        }
    }
}
