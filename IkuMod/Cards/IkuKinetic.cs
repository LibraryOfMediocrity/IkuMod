using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuKineticDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.Rarity = Rarity.Rare;
            config.TargetType = TargetType.SingleEnemy;
            config.GunName = GunNameID.GetGunFromId(12090);
            config.GunNameBurst = GunNameID.GetGunFromId(12091);
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 2, White = 1 };
            config.Block = 10;
            config.Damage = 10;
            config.UpgradedDamage = 20;
            config.Value1 = 10;
            config.UpgradedValue1 = 20;
            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.RelativeKeyword = Keyword.Block | Keyword.Exile;
            config.UpgradedRelativeKeyword = Keyword.Block | Keyword.Exile;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuKineticDef))]
    public sealed class IkuKinetic : Card
    {
        private int cardCount = 0;

        public override int AdditionalDamage => base.Value1 * cardCount;

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Card[] cards = base.Battle.HandZone.Where((Card card) => card.CardType == CardType.Attack && card != this).ToArray();
            cardCount += cards.Length;
            int blockTimes = cards.Length;
            yield return new ExileManyCardAction(cards);
            for (int i = 0; i < blockTimes; i++)
            {
                yield return new CastBlockShieldAction(base.Battle.Player, base.Block, true);
            }
            yield return AttackAction(selector);
        }
    }
}
