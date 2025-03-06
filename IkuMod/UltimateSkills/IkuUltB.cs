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
using System.Linq;

//using IkuMod.BattleActions;

namespace IkuMod.UltimateSkills
{
    public sealed class IkuUltBDef : IkuUltTemplate
    {
        public override UltimateSkillConfig MakeConfig()
        {
            UltimateSkillConfig config = GetDefaulUltConfig();
            config.Damage = 30;
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

        public DamageInfo HalfDamage
        {
            get
            {
                return this.Damage.MultiplyBy(0.5f);
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield return PerformAction.Spell(base.Battle.Player, "IkuUltB");
            EnemyUnit target = selector.GetEnemy(Battle);
            yield return PerformAction.Gun(base.Battle.Player, target, GunNameID.GetGunFromId(4500));
            yield return new DamageAction(Owner, target, Damage, GunName, GunType.Single);
            List<Unit> list = base.Battle.EnemyGroup.Alives.Where((EnemyUnit enemy) => enemy != target).Cast<Unit>().ToList<Unit>();
            yield return new DamageAction(Battle.Player, list, HalfDamage, "Instant");
            yield break;
        }
    }
}
