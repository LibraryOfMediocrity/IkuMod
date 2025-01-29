using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class CancelVeilTestDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.Damage = 1;
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

    [EntityLogic(typeof(CancelVeilTestDef))]
    public sealed class CancelVeilTest : Card
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<VeilCardEventArgs>(IkuGameEvents.PreVeilEvent, new EventSequencedReactor<VeilCardEventArgs>(this.OnCardVeiled));
            //UnityEngine.Debug.Log("Card Entered Battle");
        }

        private IEnumerable<BattleAction> OnCardVeiled(VeilCardEventArgs args)
        {
            UnityEngine.Debug.Log("OnCardVeiled was triggered");
            args.CancelBy(this);
            yield break;
        }
    }
}
