using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Presentation.UI.Panels;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class AutoPlayTestDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Ability;
            config.Rarity = Rarity.Uncommon;
            config.HideMesuem = true;
            config.FindInBattle = false;
            config.IsPooled = false;
            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.TargetType = TargetType.SingleEnemy;
            config.Cost = new ManaGroup() { Any = 1 };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(AutoPlayTestDef))]
    public sealed class AutoPlayTest : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            SelectHandInteraction interaction = new SelectHandInteraction(0, 100, base.Battle.HandZone)
            {
                Source = this
            };
            yield return new InteractionAction(interaction, false);
            foreach (Card card in interaction.SelectedCards)
            {
                UnitSelector fauxSelector;
                //setting the selector to a unitselector with the card's targettype would probably work as well
                if (card.Config.TargetType == TargetType.AllEnemies) fauxSelector = new UnitSelector(TargetType.AllEnemies);
                else fauxSelector = selector;

                var battleactions = card.GetActions(fauxSelector, consumingMana, precondition, new List<DamageAction>(), card.CardType == CardType.Friend && !card.Summoned);
                //choosing a summoned teammate activates its first active/ult regardless of unity but still reduces unity
                //this can result in a negative unity teammate, which still works but has negative unity (you have to get it back)
                //so this instead returns the passive
                if (card.CardType == CardType.Friend && card.Summoned) battleactions = card.GetPassiveActions();

                foreach (var action in battleactions)
                {   
                    yield return action;
                }
            }
            yield break;
        }
    }
}
