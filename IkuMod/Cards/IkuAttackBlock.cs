using IkuMod.Cards.Template;
using LBoL.ConfigData;
using LBoL.Base;
using System;
using System.Collections.Generic;
using System.Text;
using LBoLEntitySideloader.Attributes;
using LBoL.Core.Cards;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Units;
using System.Linq;
using LBoL.Core.Intentions;
using LBoL.Core.Battle.BattleActions;

namespace IkuMod.Cards
{
    public sealed class IkuAttackBlockDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1, Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 2 };
            config.TargetType = TargetType.SingleEnemy;
            config.Damage = 13;
            config.UpgradedDamage = 17;
            config.Block = 13;
            config.UpgradedBlock = 17;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuAttackBlockDef))]
    public sealed class IkuAttackBlock : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            bool ShouldBlock = false;
            EnemyUnit enemy = selector.SelectedEnemy;
            if (enemy.Intentions.Any(delegate (Intention i)
            {
                if (!(i is AttackIntention))
                {
                    SpellCardIntention spellCardIntention = i as SpellCardIntention;
                    if (spellCardIntention == null || spellCardIntention.Damage == null)
                    {
                        return false;
                    }
                }
                return true;
            }))
            {
                ShouldBlock = true; 
            }
            yield return AttackAction(selector);
            if(ShouldBlock && !base.Battle.BattleShouldEnd) yield return new CastBlockShieldAction(base.Battle.Player, base.Block, true);
            yield break;
        }
    }
}
