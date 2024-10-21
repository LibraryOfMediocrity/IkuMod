using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuSmiteDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 2, Blue = 2 };
            config.UpgradedCost = new ManaGroup() { Any = 3, Blue = 1 };
            config.Damage = 16;
            config.UpgradedDamage = 20;
            config.Value1 = 8;
            config.UpgradedValue1 = 10;
            config.TargetType = TargetType.SingleEnemy;
            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }

        [EntityLogic(typeof(IkuSmiteDef))]
        public sealed class IkuSmite : Card
        {
            protected override void OnEnterBattle(BattleController battle)
            {
                base.ReactBattleEvent<VeilCardEventArgs>(CustomGameEventManager.PostVeilEvent, new EventSequencedReactor<VeilCardEventArgs>(this.OnCardVeiled));
            }

            private int Veilcount;

            private IEnumerable<BattleAction> OnCardVeiled(VeilCardEventArgs args)
            {
                if (args.Card == this) Veilcount++;
                yield break;
            }

            public override int AdditionalDamage => base.Value1 * Veilcount;

            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                yield return AttackAction(selector);
                yield break;
            }
        }
    }
}
