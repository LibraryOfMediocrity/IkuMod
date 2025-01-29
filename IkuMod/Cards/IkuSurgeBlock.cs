using IkuMod.Cards.Template;
using IkuMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using LBoL.Core.Battle.BattleActions;

namespace IkuMod.Cards
{
    public sealed class IkuSurgeBlockDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Defense;
            config.Block = 10;
            config.Shield = 10;
            config.UpgradedBlock = 12;
            config.UpgradedShield = 12;
            config.TargetType = TargetType.Self;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1, Red = 1 };
            config.RelativeKeyword = Keyword.Block | Keyword.Shield;
            config.UpgradedRelativeKeyword = Keyword.Block | Keyword.Shield;
            config.RelativeEffects = new List<string>() { "IkuSurgeSe", "IkuConduitDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuSurgeSe", "IkuConduitDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuSurgeBlockDef))]
    public sealed class IkuSurgeBlock : Card
    {
        //rework to just give block(barrier) again instead of trying to multiply the block value
        public override bool Triggered
        {
            get
            {
                return base.Battle != null && base.Battle.Player.HasStatusEffect<IkuSurgeSe>();
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new CastBlockShieldAction(Battle.Player, base.Block, true);
            if (base.PlayInTriggered)
            {
                yield return new CastBlockShieldAction(base.Battle.Player, base.Shield, true);
                IkuSurgeSe status = base.Battle.Player.GetStatusEffect<IkuSurgeSe>();
                yield return status.SurgeUsed();
            }
            yield break;
        }
        
    }
}
