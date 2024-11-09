using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using IkuMod.Cards;

namespace IkuMod.Exhibits
{
    public sealed class IkuExhibitUDef : IkuExhibitTemplate
    {
        public override ExhibitConfig MakeConfig()
        {
            ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();

            exhibitConfig.Value1 = 1;
            exhibitConfig.Mana = new ManaGroup() { Blue = 1 };
            exhibitConfig.BaseManaColor = ManaColor.Blue;
            exhibitConfig.RelativeCards = new List<string>() { "IkuVeilWind" };

            return exhibitConfig;
        }
    }

    [EntityLogic(typeof(IkuExhibitUDef))]
    public sealed class IkuExhibitU : ShiningExhibit
    {

        protected override void OnEnterBattle()
        {
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnStarted));
        }

        private IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.Player.TurnCounter == 1)
            {
                base.NotifyActivating();
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<IkuVeilWind>() });
            }
            yield break;
        }
    }
}

