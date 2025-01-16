using IkuMod.Cards.Template;
using IkuMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuDischargeDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Defense;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1, Red = 1 };
            config.Block = 15;
            config.UpgradedBlock = 20;
            config.Value1 = 3;
            config.UpgradedValue1 = 4;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.RelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuDischargeDef))]
    public sealed class IkuDischarge : Card
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<CardUsingEventArgs>(base.Battle.CardUsed, delegate (CardUsingEventArgs _)
            {
                this.NotifyChanged();
            });
        }

        public int SurgeCount
        {
            get
            {
                if (base.Battle != null)
                {
                    int num = base.Value1 - base.Battle.TurnCardUsageHistory.Count;
                    return num > 0?num:0;
                }
                else return base.Value1;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return BuffAction<IkuSurgeSe>(SurgeCount);
            yield break;
        }
    }
}
