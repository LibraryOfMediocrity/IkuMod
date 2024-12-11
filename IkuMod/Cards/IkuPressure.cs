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
using System.Linq;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuPressureDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.Rarity = Rarity.Common;
            config.TargetType = TargetType.AllEnemies;
            config.GunName = GunNameID.GetGunFromId(4521);
            config.GunNameBurst = GunNameID.GetGunFromId(4532);
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 2, Blue = 1 };
            config.Damage = 9;
            config.UpgradedDamage = 11;
            config.Value1 = 2;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuPressureDef))]
    public sealed class IkuPressure : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            string gunname = base.GunName;
            for (int i = 0; i < base.Value1; i++)
            {
                yield return AttackAction(selector, gunname);
            }
            Card[] cards = (from card in base.Battle.HandZone where card != this select card).ToArray();
            foreach (Card card in cards)
            {
                if (card.Cost.Amount > 1)
                {
                    yield return new VeilCardAction(card);
                }
            }
            yield break;
        }
    }
}
