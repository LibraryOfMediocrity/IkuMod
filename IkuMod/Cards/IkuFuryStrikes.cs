using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core;
using IkuMod.StatusEffects;

namespace IkuMod.Cards
{
    public sealed class IkuFuryStrikesDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 5 };
            config.TargetType = TargetType.SingleEnemy;
            config.Damage = 7;
            config.Value1 = 1;
            config.Value2 = 1;
            config.Mana = new ManaGroup() { Any = 0 };
            config.RelativeKeyword = Keyword.Exile;
            config.UpgradedRelativeKeyword = Keyword.Exile;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuSurgeSe", "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuFuryStrikesDef))]
    public sealed class IkuFuryStrikes : Card
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<VeilCardEventArgs>(CustomGameEventManager.PostVeilEvent, new EventSequencedReactor<VeilCardEventArgs>(this.OnCardVeiled));
        }

        private IEnumerable<BattleAction> OnCardVeiled(VeilCardEventArgs args)
        {
            if (args.Card == this)
            {
                for (int i = 0; i < base.Value2; i++)
                {
                    IkuFuryStrikes card = Library.CreateCard<IkuFuryStrikes>();
                    card.FreeCost = true;
                    card.IsExile = true;
                    if (this.IsUpgraded) card.IsUpgraded = true;
                    yield return new AddCardsToHandAction(card);
                }
            }
            yield break;
        }

        public int Hits { get; private set; } = 2;

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            string gunname = base.GunName;
            for (int i = 0; i < Hits; i++)
            {
                yield return AttackAction(selector, gunname);
                if (gunname != "Instant") gunname = "Instant";
            }
            if (this.IsUpgraded)
            {
                yield return BuffAction<IkuSurgeSe>(base.Value1);
            }
            yield break;
        }
    }
}
