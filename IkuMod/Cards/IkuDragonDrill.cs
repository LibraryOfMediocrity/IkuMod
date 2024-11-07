using IkuMod.Cards.Template;
using IkuMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace IkuMod.Cards
{
    public sealed class IkuDragonDrillDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Attack;
            config.GunName = "YoumuKan";
            config.GunNameBurst = "YoumuKan";
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 2, Red = 2 };
            config.Damage = 5;
            config.Value1 = 3;
            config.UpgradedValue1 = 4;
            config.Value2 = 2;
            config.TargetType = TargetType.SingleEnemy;
            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy; 
            config.RelativeEffects = new List<string>() { "IkuSurgeSe", "IkuConduitDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuSurgeSe", "IkuConduitDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuDragonDrillDef))]
    public sealed class IkuDragonDrill : Card
    {
        private int hits;

        public override bool Triggered
        {
            get
            {
                if (base.Battle.Player.HasStatusEffect<IkuSurgeSe>())
                {
                    IkuSurgeSe status = base.Battle.Player.GetStatusEffect<IkuSurgeSe>();
                    return base.Battle != null && status.Level > 1;
                }
                return false;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (PlayInTriggered)
            {
                hits = base.Value1 + base.Value2;
            }
            else
            {
                hits = base.Value1;
            }
            for (int i = 0; i < hits; i++)
            {
                yield return AttackAction(selector);
            }
            if (PlayInTriggered)
            {
                IkuSurgeSe status = base.Battle.Player.GetStatusEffect<IkuSurgeSe>();
                status.SurgeUsed();
            }
            yield break;
        }
    }
}
