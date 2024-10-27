using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Stations;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuReturnDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Defense;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 1, Blue = 1 };
            config.TargetType = TargetType.Self;
            config.Block = 10;
            config.UpgradedBlock = 12;
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuReturnDef))]
    public sealed class IkuReturn : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new CastBlockShieldAction(base.Battle.Player, base.Block);
            if (base.Battle.DiscardZone.Count > 0)
            {
                SelectCardInteraction interaction = new SelectCardInteraction(0, base.Value1, base.Battle.DiscardZone)
                {
                    Source = this
                };
                yield return new InteractionAction(interaction, false);
                if (interaction.SelectedCards.Count > 0)
                {
                    foreach (Card card in interaction.SelectedCards)
                    {
                        yield return new VeilCardAction(card);
                    }
                }
                interaction = null;
            }
            
            yield break;
        }
    }

}
