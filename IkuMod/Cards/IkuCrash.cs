using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuCrashDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.TargetType = TargetType.AllEnemies;
            config.GunName = GunNameID.GetGunFromId(4111);
            config.GunNameBurst = GunNameID.GetGunFromId(4111);
            config.Rarity = Rarity.Common;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1, Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 2 };
            config.Damage = 13;
            config.Value1 = 1;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuCrashDef))]
    public sealed class IkuCrash : Card
    {
        private string Header
        {
            get
            {
                return this.LocalizeProperty("Header");
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (base.Battle.BattleShouldEnd) yield break;
            List<Card> list;
            if (this.IsUpgraded)
            {
                list = (from card in base.Battle.DrawZoneToShow.Concat(base.Battle.DiscardZone) where card != this select card).ToList<Card>();
            }
            else
            {
                list = (from card in base.Battle.DiscardZone where card != this select card).ToList<Card>();
            }
            SelectCardInteraction interaction = new SelectCardInteraction(0, base.Value1, list)
            {
                Source = this,
                Description = Header
            };
            yield return new InteractionAction(interaction, false);
            if (interaction.SelectedCards.Count > 0)
            {
                foreach (Card card in interaction.SelectedCards)
                {
                    yield return new MoveCardToDrawZoneAction(card, DrawZoneTarget.Top);
                }
            }
            yield break;
        }
    }
}
