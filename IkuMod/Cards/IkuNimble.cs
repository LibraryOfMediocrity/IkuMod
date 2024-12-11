using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace IkuMod.Cards
{
    public sealed class IkuNimbleDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.TargetType = TargetType.SingleEnemy;
            config.Cost = new ManaGroup() { Any = 1, Red = 1 };
            config.Damage = 6;
            config.UpgradedDamage = 7;
            config.Value1 = 3;
            config.Value2 = 1;
            config.UpgradedValue2 = 2;
            config.Mana = new ManaGroup() { Any = 1 };
            config.RelativeKeyword = Keyword.TempMorph;
            config.UpgradedRelativeKeyword = Keyword.TempMorph;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuNimbleDef))]
    public sealed class IkuNimble : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            for (int i = 0; i < base.Value1; i++)
            {
                yield return AttackAction(selector);
            }
            List<Card> cards = base.Battle.HandZone.Where((Card card) => card != this && card.Cost.Any > 0).ToList();
            if (cards.Count > 0)
            {
                List<Card> attacks = cards.Where((Card attack) => attack.CardType == CardType.Attack).ToList();
                List<Card> notAttacks = cards.Where((Card nottack) => nottack.CardType != CardType.Attack).ToList();

                Card[] discount = attacks.SampleManyOrAll(base.Value2, base.GameRun.BattleRng);

                if (discount.Length < base.Value2)
                {
                    Card[] extras = notAttacks.SampleManyOrAll(base.Value2 - discount.Length, base.GameRun.BattleRng);
                    discount = discount.Concat(extras).ToArray();
                }
                foreach (Card card in discount)
                {
                    card.NotifyActivating();
                    card.DecreaseTurnCost(base.Mana);
                }
            }
            yield break;
        }
    }
}
