using IkuMod.Cards.Template;
using IkuMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace IkuMod.Cards
{
    public sealed class IkuVeilFirstDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Blue = 2 };
            config.Value1 = 1;
            config.UpgradedRelativeCards = new List<string>() { nameof(UManaCard) };
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "IkuVeilDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuVeilFirstDef))]
    public sealed class IkuVeilFirst : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<IkuVeilFirstSe>(base.Value1, 0, 0, 0, 0.2f);
            if (this.IsUpgraded)
            {
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<UManaCard>() });
            }
            yield break;
        }
    }
}
