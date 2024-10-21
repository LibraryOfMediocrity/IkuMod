using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

//using IkuMod.BattleActions;

namespace IkuMod.UltimateSkills
{
    public sealed class IkuUltADef : IkuUltTemplate
    {
        public override UltimateSkillConfig MakeConfig()
        {
            UltimateSkillConfig config = GetDefaulUltConfig();
            config.Damage = 50;
            return config;
        }
    }

    [EntityLogic(typeof(IkuUltADef))]
    public sealed class IkuUltA : UltimateSkill
    {
        public IkuUltA()
        {
            TargetType = TargetType.SingleEnemy;
            GunName = "Simple2";
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield return PerformAction.Spell(base.Battle.Player, "IkuUltA");
            EnemyUnit enemy = selector.GetEnemy(Battle);
            yield return new DamageAction(Owner, enemy, Damage, GunName, GunType.Single);
            yield break;
        }
    }
}
