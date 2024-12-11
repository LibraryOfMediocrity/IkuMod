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

//using IkuMod.BattleActions;

namespace IkuMod.UltimateSkills
{
    public sealed class IkuUltBDef : IkuUltTemplate
    {
        public override UltimateSkillConfig MakeConfig()
        {
            UltimateSkillConfig config = GetDefaulUltConfig();
            config.Damage = 50;
            return config;
        }
    }

    [EntityLogic(typeof(IkuUltBDef))]
    public sealed class IkuUltB : UltimateSkill
    {
        public IkuUltB()
        {
            TargetType = TargetType.SingleEnemy;
            GunName = GunNameID.GetGunFromId(6400);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield return PerformAction.Spell(base.Battle.Player, "IkuUltB");
            EnemyUnit enemy = selector.GetEnemy(Battle);
            yield return PerformAction.Gun(base.Battle.Player, enemy, GunNameID.GetGunFromId(4500));
            yield return new DamageAction(Owner, enemy, Damage, GunName, GunType.Single);
            yield break;
        }
    }
}
