using IkuMod.BattleActions;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.StatusEffects
{
    public sealed class IkuExileStatusSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Keywords = Keyword.Exile;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            return config;
        }
    }

    [EntityLogic(typeof(IkuExileStatusSeDef))]
    public sealed class IkuExileStatusSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<VeilCardEventArgs>(CustomGameEventManager.PreVeilEvent, new EventSequencedReactor<VeilCardEventArgs>(this.OnCardVeiled));
        }

        private IEnumerable<BattleAction> OnCardVeiled(VeilCardEventArgs args)
        {
            if (args.Card.CardType == CardType.Status || args.Card.CardType == CardType.Misfortune)
            {
                args.CancelBy(this);
                if (args.Card.Zone != CardZone.Exile) yield return new ExileCardAction(args.Card);
                yield return new DrawManyCardAction(base.Level);
            }
            yield break;
        }
    }
}
