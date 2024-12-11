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
using System.Linq;

namespace IkuMod.Cards
{
    public sealed class IkuTempoHitDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            //maybe common
            //if common, make surge gain conditional(attack follow up)
            config.Rarity = Rarity.Common;
            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;
            config.GunName = GunNameID.GetGunFromId(400);
            config.GunNameBurst = GunNameID.GetGunFromId(401);
            config.Damage = 10;
            config.UpgradedDamage = 12;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Value1 = 1;
            config.Value2 = 1;
            config.UpgradedValue2 = 2;
            config.RelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuSurgeSe" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }
    [EntityLogic(typeof(IkuTempoHitDef))]
    public sealed class IkuTempoHit : Card
    {
        //draw cards and apply status effect
        public override bool Triggered
        {
            get
            {
                return base.Battle != null && base.Battle.BattleCardUsageHistory.Count != 0 && base.Battle.BattleCardUsageHistory.Last<Card>().Config.Type.Equals(CardType.Attack);
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector, base.GunName);
            yield return new DrawCardAction();
            if (base.PlayInTriggered)
            {
                yield return BuffAction<IkuSurgeSe>(base.Value2, 0, 0, 0, 0.2f);
            }
            yield break;
        }
    }
}
