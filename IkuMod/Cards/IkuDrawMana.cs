using IkuMod.Cards.Template;
using LBoL.ConfigData;
using LBoL.Base;
using System;
using System.Collections.Generic;
using System.Text;
using LBoLEntitySideloader.Attributes;
using LBoL.Core.Cards;
using LBoL.Core.Battle;
using IkuMod.BattleActions;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core;

namespace IkuMod.Cards
{
    public sealed class IkuDrawManaDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 1, Blue = 1 };
            config.TargetType = TargetType.Nobody;
            config.Mana = new ManaGroup() { Blue = 1, Philosophy = 1 };
            config.UpgradedMana = new ManaGroup() { Blue = 1, Philosophy = 2 }; 
            config.Value1 = 3;
            config.UpgradedValue1 = 4;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuDrawManaDef))]
    public sealed class IkuDrawMana : Card
    {

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<VeilCardEventArgs>(CustomGameEventManager.PostVeilEvent, new EventSequencedReactor<VeilCardEventArgs>(this.OnCardVeiled));
            //UnityEngine.Debug.Log("Card Entered Battle");
        }

        private IEnumerable<BattleAction> OnCardVeiled(VeilCardEventArgs args)
        {
            //UnityEngine.Debug.Log("OnCardVeiled was triggered");
            if (args.Card == this)
            {
                yield return new GainManaAction(base.Mana);
            }
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new DrawManyCardAction(base.Value1); 
            yield break;
        }


    }
}
