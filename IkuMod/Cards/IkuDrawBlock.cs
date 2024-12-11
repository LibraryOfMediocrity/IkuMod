using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Neutral.Black;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuDrawBlockDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Defense;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 1, Blue = 2 };
            config.TargetType = TargetType.Self;
            config.Block = 18;
            config.UpgradedBlock = 24;
            config.Value1 = 3;
            config.UpgradedValue1 = 4;
            config.Mana = new ManaGroup() { Blue = 1 };
            config.UpgradedMana = new ManaGroup() { Philosophy = 1 };
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block | Keyword.Philosophy;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuDrawBlockDef))]
    public sealed class IkuDrawBlock : Card
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<VeilCardEventArgs>(CustomGameEventManager.PreVeilEvent, new EventSequencedReactor<VeilCardEventArgs>(this.OnCardVeiled));
        }

        private IEnumerable<BattleAction> OnCardVeiled(VeilCardEventArgs args)
        {
            if (args.Card == this)
            {
                base.NotifyActivating();
                for (int i = 1; i <= base.Value1; i++)
                {
                    if (base.Battle.DrawZone.Count == 0)
                    {
                        break;
                    }

                    if (base.Battle.HandZone.Count == base.Battle.MaxHand)
                    {
                        break;
                    }

                    List<Card> list = base.Battle.DrawZone.Where((Card card) => !card.IsForbidden && card.ConfigCost.Amount <= 3).ToList();
                    if (list.Count > 0)
                    {
                        Card card2 = list.Sample(base.BattleRng);
                        yield return new MoveCardAction(card2, CardZone.Hand);
                    }
                }
                yield return new GainManaAction(base.Mana);
            }
            yield break;
        }
    }
}
