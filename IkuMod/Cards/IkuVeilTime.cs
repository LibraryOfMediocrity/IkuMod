using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.EntityLib.Cards;
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.StatusEffects;
using IkuMod.StatusEffects;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using IkuMod.BattleActions;

namespace IkuMod.Cards
{
    public sealed class IkuVeilTimeDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Rarity = Rarity.Rare;
            config.Cost = new ManaGroup() { Any = 3, Blue = 2 };
            config.UpgradedCost = new ManaGroup() { Any = 3, Blue = 1 };
            config.Value1 = 3;
            config.UpgradedValue1 = 5;
            config.Mana = new ManaGroup() { Any = 1 };
            config.TargetType = TargetType.Nobody;
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            config.RelativeEffects = new List<string>() { "TimeIsLimited", "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "TimeIsLimited", "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuVeilTimeDef))]
    public sealed class IkuVeilTime : LimitedStopTimeCard
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
                yield return new VeilCardAction(card);
            }
            yield return base.BuffAction<ExtraTurn>(1, 0, 0, 0, 0.2f);
            yield return base.BuffAction<IkuNextTurnDraw>(base.Value1, 0, 0, 0, 0.2f);
            yield return new RequestEndPlayerTurnAction();
            if (base.Limited)
            {
                yield return base.DebuffAction<TimeIsLimited>(base.Battle.Player, 1, 0, 0, 0, true, 0.2f);
            }
            yield break;
        }

    }
}
