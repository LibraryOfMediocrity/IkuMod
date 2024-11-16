using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.StatusEffects;

namespace IkuMod.Cards
{
    public sealed class IkuTenshiFriendDef :IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Friend;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Blue, ManaColor.Green };
            config.Cost = new ManaGroup() { Blue = 1, Green = 1 };
            config.Shield = 0;
            config.Value2 = 3;
            config.Value1 = 2;
            config.TargetType = TargetType.Nobody;
            config.Loyalty = 1;
            config.PassiveCost = 1;
            config.UpgradedPassiveCost = 2;
            config.ActiveCost = 0;
            config.UltimateCost = -3;
            config.UpgradedUltimateCost = -2;
            config.RelativeEffects = new List<string>() { "Firepower", "Spirit" };
            config.UpgradedRelativeEffects = new List<string>() { "Firepower", "Spirit" };
            config.RelativeKeyword = Keyword.Shield;
            config.UpgradedRelativeKeyword = Keyword.Shield;
            config.RelativeCards = new List<string>() { "IkuTenshiSword" };
            config.UpgradedRelativeCards = new List<string>() { "IkuTenshiSword" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuTenshiFriendDef))]
    public sealed class IkuTenshiFriend : Card
    {
        public int TimesHit { get; set; } = 0;
        public int TimesHitLast { get; set; } = 0;

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<DamageEventArgs>(base.Battle.Player.DamageTaking, new EventSequencedReactor<DamageEventArgs>(this.OnDamageTaking));
        }

        public string Indent { get; } = "<indent=80>";
        public string PassiveCostIcon
        {
            get
            {
                return string.Format("<indent=0><sprite=\"Passive\" name=\"{0}\">{1}", base.PassiveCost, Indent);
            }
        }
        public string ActiveCostIcon
        {
            get
            {
                return string.Format("<indent=0><sprite=\"Active\" name=\"{0}\">{1}", base.UltimateCost, Indent);
            }
        }
        public string UltimateCostIcon
        {
            get
            {
                return string.Format("<indent=0><sprite=\"Ultimate\" name=\"{0}\">{1}", base.ActiveCost, Indent);
            }
        }

        //Effect to trigger at the start of the end.
        public override IEnumerable<BattleAction> OnTurnStartedInHand()
        {
            TimesHitLast = TimesHit;
            var battleActions =  this.GetPassiveActions();
            foreach (var action in battleActions)
            {
                yield return action;
            }
            TimesHit = 0;
            yield break;
        }


        private IEnumerable<BattleAction> OnDamageTaking(DamageEventArgs args)
        {
            if (args.DamageInfo.Damage > 0f && !base.Battle.BattleShouldEnd && this.Summoned)
            {
                base.NotifyActivating();
                TimesHit++;
            }
            yield break;
        }

        public ShieldInfo PassiveShield
        {
            get { return new ShieldInfo(base.Value2 * TimesHitLast, BlockShieldType.Direct); }
        }


        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            //Triigger the effect only if the card has been summoned. 
            if (!base.Summoned || base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            base.NotifyActivating();
            //Increase base loyalty.
            base.Loyalty += base.PassiveCost;
            int num;
            //Trigger the action multiple times if "Mental Energy Injection" is active.
            for (int i = 0; i < base.Battle.FriendPassiveTimes; i = num + 1)
            {
                if (base.Battle.BattleShouldEnd)
                {
                    yield break;
                }
                yield return new CastBlockShieldAction(base.Battle.Player, PassiveShield, false);
                num = i;
            }
            yield break;
        }


        //When the summoned card is played, choose and resolve either the active or ultimate effect.
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            //
            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Ultimate)
            {
                //Adjust the card's loyalty. 
                //Because the ActiveCost is negative, the Cost has to be added instead of substracted.
                base.Loyalty += base.ActiveCost;
                base.UltimateUsed = true;
                IkuTenshiSword sword = Library.CreateCard<IkuTenshiSword>();
                sword.DeltaValue1 = base.Loyalty;
                yield return new AddCardsToHandAction(new IkuTenshiSword[] { sword });
                yield return base.SkillAnime;
            }
            else
            {
                base.Loyalty += base.UltimateCost;
                yield return BuffAction<Firepower>(base.Value1);
                yield return BuffAction<Spirit>(base.Value1);
                yield return base.SkillAnime;
            }
            yield break;
        }

    }
}
