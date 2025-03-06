using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuDevilDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.Rarity = Rarity.Uncommon;
            config.TargetType = TargetType.SingleEnemy;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 1, Blue = 1 };
            config.Damage = 10;
            config.UpgradedDamage = 13;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuDevilDef))]
    public sealed class IkuDevil : Card
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<VeilCardEventArgs>(IkuGameEvents.PostVeilEvent, delegate { VeilCardTurnCount++; });
            base.HandleBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, delegate { VeilCardTurnCount = 0; });
        }

        public int VeilCardTurnCount = 0;

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            for (int i = 0; i <= VeilCardTurnCount; i++)
            {
                yield return AttackAction(selector);
            }
            yield break;
        }
    }
}
