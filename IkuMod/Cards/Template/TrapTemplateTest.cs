using LBoL.Base;
using LBoL.ConfigData;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards.Template
{
    public abstract class TrapTemplateTest : IkuCardTemplate
    {
        public CardConfig GetTrapDefaultConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.TargetType = TargetType.SingleEnemy;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Type = CardType.Attack;
            config.HideMesuem = true;
            config.FindInBattle = false;
            config.IsPooled = false;
            config.Keywords = Keyword.Retain | Keyword.Exile;
            config.UpgradedKeywords = Keyword.Retain | Keyword.Exile;
            return config;
        }
    }
}
