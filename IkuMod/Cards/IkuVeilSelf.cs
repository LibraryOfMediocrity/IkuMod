using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuVeilSelfDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 1, Blue = 1 };
            config.TargetType = TargetType.SingleEnemy;
            config.Damage = 10;
            config.UpgradedDamage = 12;
            config.Value1 = 2;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuVeilSelfDef))]
    public sealed class IkuVeilSelf : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            string gunname = base.GunName;
            for (int i = 0; i < base.Value1; i++)
            {
                yield return AttackAction(selector, gunname);
                if (gunname != "Instant") gunname = "Instant";
            }
            yield return new VeilCardAction(this);
            yield break;
        }
    }
}
