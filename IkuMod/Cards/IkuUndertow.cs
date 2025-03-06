using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuUndertowDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Defense;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 1, Blue = 1 };
            config.Shield = 11;
            config.UpgradedShield = 15;
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.RelativeKeyword = Keyword.Shield;
            config.UpgradedRelativeKeyword = Keyword.Shield;
            config.RelativeEffects = new List<string>() { "Weak", "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "Weak", "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuUndertowDef))]
    public sealed class IkuUndertow : Card
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<VeilCardEventArgs>(IkuGameEvents.PostVeilEvent, delegate { CardVeiledThisTurn = true; });
            base.HandleBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, delegate {  CardVeiledThisTurn = false; });
        }

        private bool CardVeiledThisTurn = false;

        public override bool Triggered
        {
            get
            {
                return CardVeiledThisTurn;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new CastBlockShieldAction(base.Battle.Player, base.Shield);
            if (PlayInTriggered)
            {
                foreach (BattleAction action in DebuffAction<Weak>(base.Battle.AllAliveEnemies, base.Value1)) yield return action;
            }
            yield break;
        }
    }
}
