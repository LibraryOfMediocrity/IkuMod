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
using System.Linq;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuSwiftDef : IkuCardTemplate
    {
        public override bool UseDefault => true;

        public override CardConfig MakeConfig()
        {
            CardConfig config  = GetCardDefaultConfig();
            config.Type = CardType.Skill;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 0 };
            config.Value1 = 2;
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile | Keyword.Echo;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuSwiftDef))]
    public sealed class IkuSwift : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            
            for (int i = 0; i < base.Value1; i++)
            {
                if (base.Battle.DiscardZone.Count > 0)
                {
                    yield return new MoveCardAction(base.Battle.DiscardZone.Last<Card>(), CardZone.Hand);
                }
            }
            yield break;
        }
    }
}
