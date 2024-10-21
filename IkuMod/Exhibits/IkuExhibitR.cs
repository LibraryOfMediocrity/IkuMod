using IkuMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;


namespace IkuMod.Exhibits
{
    public sealed class IkuExhibitRDef : IkuExhibitTemplate
    {
        public override ExhibitConfig MakeConfig()
        {
            ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();

            exhibitConfig.Value1 = 1;
            exhibitConfig.Mana = new ManaGroup() { Red = 1 };
            exhibitConfig.BaseManaColor = ManaColor.Red;
            exhibitConfig.HasCounter = true;
            exhibitConfig.InitialCounter = 0;
            exhibitConfig.Value2 = 4;
            exhibitConfig.RelativeEffects = new List<string>() { "IkuSurgeSe" };

            return exhibitConfig;
        }
    }

    [EntityLogic(typeof(IkuExhibitRDef))]
    public sealed class IkuExhibitR : ShiningExhibit
    {
        protected override void OnEnterBattle()
        {
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnStarted));
            base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }

        protected override void OnLeaveBattle()
        {
            base.Counter = 0;
        }

        private IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
        {
            //if (base.Battle.Player.TurnCounter == 1)
            //{
            //    base.NotifyActivating();
            //    yield return new ApplyStatusEffectAction<IkuSurgeSe>(base.Battle.Player, base.Value1, 0, 0, 0, 0.2f);
            //}
            yield break;
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (args.Card.CardType == CardType.Attack) base.Counter++;
            if (base.Counter == base.Value2)
            {
                base.NotifyActivating();
                yield return new ApplyStatusEffectAction<IkuSurgeSe>(base.Battle.Player, base.Value1, 0, 0, 0, 0.2f);
                base.Counter = 0;
            }
            yield break;
        }
    }
}


