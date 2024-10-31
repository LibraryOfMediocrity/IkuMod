using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using IkuMod.BattleActions;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using System.Linq;

namespace IkuMod.StatusEffects
{
    public sealed class IkuVeilFirstSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.HasCount = true;
            return config;
        }
    }

    [EntityLogic(typeof(IkuVeilFirstSeDef))]
    public sealed class IkuVeilFirstSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            this.SetCount();
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.ReactOwnerEvent<VeilCardEventArgs>(CustomGameEventManager.PostVeilEvent, new EventSequencedReactor<VeilCardEventArgs>(this.OnCardVeiled));
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnStarting));
        }

        private void SetCount()
        {
            if (base.Battle.TurnCardUsageHistory.Count <= base.Level)
            {
                base.Count = base.Level - base.Battle.TurnCardUsageHistory.Count;
            }
            else
            {
                base.Count = 0;
            }
        }

        private IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
        {
            this.SetCount();
            yield break;
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (base.Battle.TurnCardUsageHistory.Count <= base.Level) 
            {
                if (EdgeCases(args)) { yield return new VeilCardAction(args.Card); this.NotifyActivating(); }
                this.SetCount();
            }
            yield break;
        }

        Card card = null;
        private IEnumerable<BattleAction> OnCardVeiled(VeilCardEventArgs args)
        {
            card = args.Card;
            yield break;
        }

        private bool EdgeCases(CardUsingEventArgs args)
        {
            bool valid;

            valid = args.Card != null && args.Card.Zone != CardZone.None && args.Card != card && args.Card.Zone != CardZone.Exile && !args.Card.Summoning
                && args.Card.CardType != LBoL.Base.CardType.Ability && args.Card.CardType != LBoL.Base.CardType.Tool;
            return valid;
        }
    }
}
