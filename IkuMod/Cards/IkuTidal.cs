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
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuTidalDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.TargetType = TargetType.SingleEnemy;
            config.GunName = GunNameID.GetGunFromId(30030);
            config.GunNameBurst = GunNameID.GetGunFromId(30030);
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 1, Blue = 1 };
            config.Damage = 7;
            config.UpgradedDamage = 10;
            config.Value1 = 7;
            config.UpgradedValue1 = 10;
            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuTidalDef))]
    public sealed class IkuTidal : Card
    {
        public int CardCount = 0;

        public override int AdditionalDamage => base.Value1 * CardCount;

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Card[] cards = (from card in base.Battle.HandZone where card != this select card).ToArray();
            foreach (Card card in cards)
            {
                yield return new VeilCardAction(card);
                CardCount++;
                this.NotifyChanged();
            }
            yield return AttackAction(selector);
            CardCount = 0;
            yield break;
        }
    }
}
