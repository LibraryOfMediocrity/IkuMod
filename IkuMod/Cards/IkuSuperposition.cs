using IkuMod.BattleActions;
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
    public sealed class IkuSuperpositionDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Skill;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Blue, ManaColor.Red };
            config.IsPooled = false;
            config.Value1 = 0;
            config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 5 }; 
            config.Mana = new ManaGroup() { Philosophy = 3 };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuSuperpositionDef))]
    public sealed class IkuSuperposition : Card
    {
        //handlebattleevent, playtwice
        protected override void OnEnterBattle(BattleController battle)
        {
            if (!IsCopy && !IsPlayTwiceToken) SetValue1();
            base.HandleBattleEvent<CardsEventArgs>(base.Battle.CardsAddedToDiscard, new GameEventHandler<CardsEventArgs>(this.OnAddCard));
            base.HandleBattleEvent<CardsEventArgs>(base.Battle.CardsAddedToHand, new GameEventHandler<CardsEventArgs>(this.OnAddCard));
            base.HandleBattleEvent<CardsEventArgs>(base.Battle.CardsAddedToExile, new GameEventHandler<CardsEventArgs>(this.OnAddCard));
            base.HandleBattleEvent<CardsAddingToDrawZoneEventArgs>(base.Battle.CardsAddedToDrawZone, new GameEventHandler<CardsAddingToDrawZoneEventArgs>(this.OnCardsAddedToDrawZone));
            base.HandleBattleEvent<CardUsingEventArgs>(base.Battle.CardPlaying, delegate (CardUsingEventArgs args)
            {
                TrySetLinked(args.Card);
            });
            base.ReactBattleEvent<VeilCardEventArgs>(IkuGameEvents.PostVeilEvent, new EventSequencedReactor<VeilCardEventArgs>(this.OnCardVeiled));
        }

        private void SetValue1()
        {
            base.DeltaValue1 = base.Battle.EnumerateAllCards().Where((Card card)  => card.GetType() == this.GetType()).ToList().Count;
        }

        private void OnAddCard(CardsEventArgs args)
        {
            foreach (Card card in args.Cards)
            {
                if (card.GetType() == this.GetType() && card.IsCopy) TrySetLinked(card);
            }
        }

        private void OnCardsAddedToDrawZone(CardsAddingToDrawZoneEventArgs args)
        {
            foreach (Card card in args.Cards)
            {
                if (card.GetType() == this.GetType() && card.IsCopy) TrySetLinked(card);
            }
        }

        private void TrySetLinked(Card card)
        {
            if (card is IkuSuperposition super && super.Value1 == base.Value1) super.Linked = this.Linked;
        }

        public List<Card> Linked { get; set; }

        public string DiscCards
        {
            get
            {
                if (Linked == null || base.Battle == null || Linked.Count <= 0)
                {
                    return this.LocalizeProperty("IfEmpty", true, true);
                }
                string cards = "";
                if(Linked.Count > 3)
                {
                    for(int i = 0; i < 3; i++)
                    {
                        cards += Linked[i].Name + (i == 2 ? "...+" + (Linked.Count - 3) : ", ");
                    }
                }
                else
                {
                    for (int i = 0; i < Linked.Count; i++)
                    {
                        cards += Linked[i].Name + (i == Linked.Count-1 ? "." : ", ");
                    }
                }
                return cards;
            }
        }

        private IEnumerable<BattleAction> OnCardVeiled(VeilCardEventArgs args)
        {
            if (args.Card == this)
            {
                foreach(Card card in Linked)
                {
                    if (card.Zone == CardZone.Draw || card.Zone == CardZone.Discard) yield return new VeilCardAction(card);
                }
            }
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (Linked != null && Linked.Count > 0)
            {
                foreach (Card card in Linked)
                {
                    if (card.Zone == CardZone.Draw || card.Zone == CardZone.Discard) yield return new MoveCardAction(card, CardZone.Hand);
                    if (IsUpgraded) yield return new GainManaAction(Mana);
                }
            }
            yield break;
        }
    }
}
