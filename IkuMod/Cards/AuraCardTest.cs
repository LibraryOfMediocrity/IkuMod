using HarmonyLib;
using IkuMod.BattleActions;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Presentation.UI.Widgets;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class AuraCardTestDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Skill;
            config.Rarity = Rarity.Uncommon;
            config.HideMesuem = true;
            config.FindInBattle = false;
            config.IsPooled = false;
            config.Mana = null;
            config.Value1 = 1;
            config.Colors = new List<ManaColor>() { ManaColor.Colorless };
            config.TargetType = TargetType.Nobody;
            config.Cost = new ManaGroup() { Any = 2 };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }



    [EntityLogic(typeof(AuraCardTestDef))]
    public sealed class AuraCardTest : Card
    {

        private readonly string date = DateTime.Now.ToString("MM.dd");

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            UnityEngine.Debug.Log("The date is "+date);
            yield break;
        }
    }
}
