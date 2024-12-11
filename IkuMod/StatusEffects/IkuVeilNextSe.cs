using IkuMod.BattleActions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace IkuMod.StatusEffects
{
    public sealed class IkuVeilNextSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Order = 11;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            return config;
        }
    }

    [EntityLogic(typeof(IkuVeilNextSeDef))]
    public sealed class IkuVeilNextSe : StatusEffect
    {
        bool veilnext;
        protected override void OnAdded(Unit unit)
        {
            veilnext = false;
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.ReactOwnerEvent<CardUsingEventArgs>(base.Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsing));
            base.ReactOwnerEvent<VeilCardEventArgs>(CustomGameEventManager.PostVeilEvent, new EventSequencedReactor<VeilCardEventArgs>(this.OnCardVeiled));
        }
        
        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (veilnext && EdgeCases(args))
            {
                yield return new VeilCardAction(args.Card);
                int num = base.Level - 1;
                base.Level = num;
                if (base.Level <= 0)
                {
                    yield return new RemoveStatusEffectAction(this, true, 0.1f);
                }
            }
            else
            {
                veilnext = true;
            }
            yield break;
        }

        Card card = null;

        private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
        {
            card = null;
            yield break;
        }

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
