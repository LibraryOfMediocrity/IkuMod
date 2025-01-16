using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace IkuMod.StatusEffects
{
    public sealed class IkuOceanSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.HasCount = true;
            return config;
        }
    }

    [EntityLogic(typeof(IkuOceanSeDef))]
    public sealed class IkuOceanSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.HandleOwnerEvent<UnitEventArgs>(unit.TurnEnded, delegate (UnitEventArgs _)
            {
                base.Count = 0;
            });
        }
        //can be 3 because you only get 5 cards in hand without draw anyways (max 1 use if no draw)
        //but wait and see
        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            base.Count++;
            if (base.Count == 4)
            {
                yield return new DrawManyCardAction(base.Level);
                base.Count = 0;
            }
            yield break;
        }
    }
}
