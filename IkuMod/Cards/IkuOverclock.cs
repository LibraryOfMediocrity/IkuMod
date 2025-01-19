using IkuMod.Cards.Template;
using IkuMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuOverclockDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Skill;
            config.Rarity = Rarity.Uncommon;
            config.TargetType = TargetType.AllEnemies;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1, Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Value1 = 3;
            config.UpgradedValue1 = 4;
            config.Value2 = 2;
            config.RelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuOverclockDef))]
    public sealed class IkuOverclock : Card
    {
        public override bool Triggered
        {
            get
            {
                List<Card> cards = base.Battle.HandZone.Where((Card card) => card.CardType == CardType.Attack).ToList();
                return cards.Count == 0;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            StatusEffect surge = base.Battle.Player.GetStatusEffect<IkuSurgeSe>();
            if (surge != null)
            {
                yield return new DamageAction(base.Battle.Player, base.Battle.AllAliveEnemies, DamageInfo.Reaction(surge.Level * base.Value2));
            }
            if (PlayInTriggered)
            {
                yield return new DrawManyCardAction(base.Value1);
            }
            yield break;
        }
    }
}
