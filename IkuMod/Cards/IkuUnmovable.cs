using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuUnmovableDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public CardType Cardtype { get; private set; }

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Defense;
            config.Rarity = Rarity.Uncommon;
            config.TargetType = TargetType.Nobody;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 0 };
            config.Shield = 30;
            config.UpgradedShield = 40;
            config.Value1 = 1;
            config.RelativeKeyword = Keyword.Shield;
            config.UpgradedRelativeKeyword = Keyword.Shield;
            config.RelativeEffects = new List<string>() { "Graze", "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "Graze", "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuUnmovableDef))]
    public sealed class IkuUnmovable : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return BuffAction<Graze>(base.Value1, 0, 0, 0, 0.2f);
            SelectHandInteraction interaction = new SelectHandInteraction(0, 100, base.Battle.HandZone)
            {
                Source = this
            };
            yield return new InteractionAction(interaction, false);
            foreach (Card card in interaction.SelectedCards)
            {
                yield return new VeilCardAction(card);
            }
            yield return new RequestEndPlayerTurnAction();
            yield break;
        }
    }
}
