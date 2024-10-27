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
    public sealed class IkuTenshiSwordDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.Rarity = Rarity.Uncommon;
            config.TargetType = TargetType.SingleEnemy;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Blue, ManaColor.Black, ManaColor.Red, ManaColor.Green, ManaColor.Colorless };
            config.Cost = new ManaGroup() { Any = 2 };
            config.Damage = 10;
            config.Value1 = 0;
            config.Keywords = Keyword.Exile;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuTenshiSwordDef))]
    public sealed class IkuTenshiSword : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            for (int i = 0; i < base.Value1; i++)
            {
                yield return AttackAction(selector);
            }
        }
    }
}
