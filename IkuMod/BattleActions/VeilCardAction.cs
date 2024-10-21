using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using System;
using System.Collections.Generic;

namespace IkuMod.BattleActions
{
    public sealed class VeilCardAction : EventBattleAction<VeilCardEventArgs>
    {
        internal VeilCardAction(Card card = null) 
        {
            base.Args = new VeilCardEventArgs
            {
                Card = card
            };
        }

        public override IEnumerable<Phase> GetPhases()
        {
            yield return base.CreateEventPhase<VeilCardEventArgs>("PreVeil", Args, CustomGameEventManager.PreVeilEvent);

            yield return base.CreatePhase("Resolve", new Action(this.ResolvePhase), true);

            yield return base.CreatePhase("Main", delegate
            {
                Args.Card?.DecreaseTurnCost(new ManaGroup { Any = 2 });
            }, hasViewer: true);

            yield return base.CreateEventPhase<VeilCardEventArgs>("PostVeil", Args, CustomGameEventManager.PostVeilEvent);
        }

        private void ResolvePhase()
        {
            base.React(new Reactor(new MoveCardToDrawZoneAction(Args.Card, DrawZoneTarget.Random)));
        }
    }
}
