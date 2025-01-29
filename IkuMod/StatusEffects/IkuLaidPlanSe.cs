using IkuMod.BattleActions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.StatusEffects
{
    public sealed class IkuLaidPlanSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            return config;
        }
    }

    [EntityLogic(typeof(IkuLaidPlanSeDef))]
    public sealed class IkuLaidPlanSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnEnding, new EventSequencedReactor<UnitEventArgs>(this.OnTurnEnding));
        }

        private string Header
        {
            get
            {
                return this.LocalizeProperty("Header");
            }
        }

        private IEnumerable<BattleAction> OnTurnEnding(UnitEventArgs args)
        {
            if (base.Battle.HandZone.Count > 0 && !base.Battle.BattleShouldEnd)
            {
                this.NotifyActivating();
                SelectCardInteraction interaction = new SelectCardInteraction(0, base.Level, base.Battle.HandZone)
                {
                    Source = this,
                    Description = Header
                };
                yield return new InteractionAction(interaction, false);
                if (interaction.SelectedCards.Count > 0)
                {
                    foreach (Card card in interaction.SelectedCards)
                    {
                        yield return new VeilCardAction(card);
                    }
                }
            }
            yield break;
        }
    }
}
