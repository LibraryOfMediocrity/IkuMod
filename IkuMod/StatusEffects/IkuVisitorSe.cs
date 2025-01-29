using IkuMod.Cards;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IkuMod.StatusEffects
{
    public sealed class IkuVisitorSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            return config;
        }
    }

    [EntityLogic(typeof(IkuVisitorSeDef))]
    public sealed class IkuVisitorSe : StatusEffect
    {
        //choose card to add
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(OnTurnStarted));
        }

        private string Header
        {
            get
            {
                return this.LocalizeProperty("Header");
            }
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            this.NotifyActivating();
            Card[] array = new Card[2];
            array[0] = Library.CreateCard<IkuRedSprite>();
            array[1] = Library.CreateCard<IkuVeilWind>();
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(array, canSkip: true)
            {
                Source = this,
                Description = Header
            };
            yield return new InteractionAction(interaction);
            if (interaction.SelectedCard != null)
            {
                Card[] cards = new Card[base.Level];
                cards[0] = interaction.SelectedCard;
                for (int i = 1; i < base.Level; i++)
                {
                    cards[i] = Library.CreateCard(interaction.SelectedCard.GetType());
                }
                yield return new AddCardsToHandAction(cards);
            }

            /*
            List<IkuRedSprite> list1 = base.Battle.HandZone.OfType<IkuRedSprite>().ToList<IkuRedSprite>();
            if (list1.Count < base.Level)
            {
                base.NotifyActivating();
                yield return new AddCardsToHandAction(Library.CreateCards<IkuRedSprite>(base.Level - list1.Count, false), AddCardsType.Normal);
            }
            List<IkuVeilWind> list2 = base.Battle.HandZone.OfType<IkuVeilWind>().ToList<IkuVeilWind>();
            if (list2.Count < base.Level)
            {
                base.NotifyActivating();
                yield return new AddCardsToHandAction(Library.CreateCards<IkuVeilWind>(base.Level - list2.Count, false), AddCardsType.Normal);
            }
            */
            yield break;
        }
    }
}
