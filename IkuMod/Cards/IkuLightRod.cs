using IkuMod.Cards.Template;
using IkuMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace IkuMod.Cards
{
    public sealed class IkuLightRodDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 2, Hybrid = 1, HybridColor = 5 };
            config.UpgradedCost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 5 }; 
            config.TargetType = TargetType.SingleEnemy;
            config.Damage = 17;
            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.Mana = new ManaGroup() { Philosophy = 3 };
            config.RelativeKeyword = Keyword.Philosophy;
            config.UpgradedRelativeKeyword = Keyword.Philosophy;
            config.RelativeEffects = new List<string>() { "IkuSurgeSe", "IkuConduitDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuSurgeSe", "IkuConduitDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuLightRodDef))]
    public sealed class IkuLightRod : Card
    {
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
            yield return base.AttackAction(selector, base.GunName);
            if (base.PlayInTriggered)
            {
                IkuSurgeSe status = base.Battle.Player.GetStatusEffect<IkuSurgeSe>();
                yield return new GainManaAction(base.Mana);
                status.SurgeUsed();
            }
            yield break;
        }

    }
}
