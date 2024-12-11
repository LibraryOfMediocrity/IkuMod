using HarmonyLib;
using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Presentation.UI.Widgets;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class AuraCardTestDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Skill;
            config.Rarity = Rarity.Uncommon;
            config.HideMesuem = true;
            config.FindInBattle = false;
            config.IsPooled = false;
            config.Mana = null;
            config.Value1 = 1;
            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.RelativeEffects = new List<string>() { "TempFirepower", "AuraDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "TempFirepower", "AuraDisc" };
            config.TargetType = TargetType.Nobody;
            config.Cost = new ManaGroup() { Any = 2 };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }



    [EntityLogic(typeof(AuraCardTestDef))]
    public sealed class AuraCardTest : Card
    {
        public int AuraCount = 0;

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarting));
            base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        private IEnumerable<BattleAction> OnTurnStarting(UnitEventArgs args)
        {
            AuraCount = 0;
            this.IsForbidden = false;
            yield break;
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if(AuraCount == 0)
            {
                if (args.Card.CardType == CardType.Attack && this.Zone == CardZone.Hand)
                {
                    this.NotifyActivating();
                    yield return BuffAction<TempFirepower>(AuraCount);
                }

            }
            yield break;
        }

        public override bool Triggered => AuraCount > 0;

        private new string ExtraDescription1
        {
            get
            {
                return this.LocalizeProperty("ExtraDescription1", true, true);
            }
        }

        protected override string GetBaseDescription()
        {
            if (AuraCount > 0)
            {
                return this.ExtraDescription1;
            }
            return base.GetBaseDescription();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if(AuraCount == 0) yield return PerformAction.SummonFriend(this);
            AuraCount += base.Value1;
            this.NotifyChanged();
            yield return new MoveCardAction(this, CardZone.Hand);
        }
    }
}
