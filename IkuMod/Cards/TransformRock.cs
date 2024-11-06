using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.EnemyUnits.Normal;
using LBoL.EntityLib.PlayerUnits;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class TransformRockDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Ability;
            config.Rarity = Rarity.Uncommon;
            config.HideMesuem = true;
            config.FindInBattle = false;
            config.IsPooled = false;
            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.TargetType = TargetType.SingleEnemy;
            config.Cost = new ManaGroup() { Any = 1 };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(TransformRockDef))]
    public sealed class TransformRock : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return PerformAction.TransformModel(base.Battle.Player, nameof(Yaoshi));
            yield break;
        }
    }
}
