using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using IkuMod.Cards.Template;
using IkuMod.BattleActions;
using System.Linq;
using LBoL.Core.Battle.Interactions;

//using IkuMod.BattleActions;

namespace IkuMod.UltimateSkills
{
    public sealed class IkuUltADef : IkuUltTemplate
    {
        public override UltimateSkillConfig MakeConfig()
        {
            UltimateSkillConfig config = GetDefaulUltConfig();
            config.Damage = 20;
            config.Value1 = 5;
            config.RelativeEffects = new List<string>() { "IkuVeilDisc" };
            return config;
        }
    }

    [EntityLogic(typeof(IkuUltADef))]
    public sealed class IkuUltA : UltimateSkill
    {
        public IkuUltA()
        {
            TargetType = TargetType.AllEnemies;
            GunName = GunNameID.GetGunFromId(4711);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield return PerformAction.Spell(base.Battle.Player, "IkuUltA");
            Unit[] targets = selector.GetUnits(base.Battle);
            yield return new DamageAction(Owner, targets, Damage, GunName, GunType.Single);
            if (!base.Battle.BattleShouldEnd && base.Battle.HandZone.Count > 0)
            {
                SelectHandInteraction interaction = new SelectHandInteraction(0, 100, base.Battle.HandZone)
                {
                    Source = this
                };
                yield return new InteractionAction(interaction, false);
                foreach (Card card in interaction.SelectedCards)
                {
                    yield return new VeilCardAction(card);
                }
                yield return new DrawManyCardAction(base.Value1);
            }
            yield break;
        }
    }
}
