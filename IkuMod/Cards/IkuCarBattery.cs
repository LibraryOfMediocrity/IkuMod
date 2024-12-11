using IkuMod.Cards.Template;
using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoL.Base;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using IkuMod.StatusEffects;

namespace IkuMod.Cards
{
    public sealed class IkuCarBatteryDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1 };
            config.Value1 = 2;
            config.Value2 = 1;
            config.TargetType = TargetType.Nobody;
            config.Keywords = Keyword.Debut;
            config.UpgradedKeywords = Keyword.Debut;
            config.RelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuCarBatteryDef))]
    public sealed class IkuCarBattery : Card
    {
        public int DebutValue
        {
            get
            {
                return (this.IsUpgraded ? base.Value1 : base.Value2);
            }
        }

        private new string ExtraDescription1
        {
            get
            {
                return this.LocalizeProperty("ExtraDescription1", true, true);
            }
        }

        protected override string GetBaseDescription()
        {
            if (!base.DebutActive)
            {
                return this.ExtraDescription1;
            }
            return base.GetBaseDescription();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new DrawManyCardAction(base.Value1);
            if (base.PlayInTriggered)
            {
                yield return BuffAction<IkuSurgeSe>(base.Value2 + DebutValue, 0, 0, 0, 0.2f);
            }
            else
            {
                yield return BuffAction<IkuSurgeSe>(base.Value2, 0, 0, 0, 0.2f);
            }
            yield break;
        }
    }
}
