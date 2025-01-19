using IkuMod.Cards.Template;
using LBoL.ConfigData;
using LBoL.Base;
using System;
using System.Collections.Generic;
using System.Text;
using LBoLEntitySideloader.Attributes;
using LBoL.Core.Cards;
using LBoL.Core.Battle;
using LBoL.Core;
using System.Linq;
using IkuMod.StatusEffects;

namespace IkuMod.Cards
{
    public sealed class IkuThunderDef : IkuCardTemplate
    {
        public override bool UseDefault
        {
            get { return true; }
        }

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 2 };
            config.Block = 14;
            config.UpgradedBlock = 18;
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.RelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuThunderDef))]
    public sealed class IkuThunder : Card
    {
        public override bool Triggered
        {
            get
            {
                return base.Battle != null && base.Battle.BattleCardUsageHistory.Count != 0 && base.Battle.BattleCardUsageHistory.Last<Card>().Config.Type != CardType.Attack;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.DefenseAction();
            if (PlayInTriggered)
            {
                yield return BuffAction<IkuSurgeSe>(base.Value1, 0, 0, 0, 0.2f);
            }
            yield break;
        }
    }
}
