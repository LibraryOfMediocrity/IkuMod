using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoL.Base.Extensions;

namespace IkuMod.Cards
{
    public sealed class IkuChainLightDef : IkuCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.GunName = GunNameID.GetGunFromId(6173);
            config.GunNameBurst = GunNameID.GetGunFromId(6173);
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.HideMesuem = false;
            config.Damage = 18;
            config.UpgradedDamage = 24;
            config.TargetType = TargetType.SingleEnemy;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 2, Red = 1 };
            config.UpgradedKeywords = Keyword.Accuracy;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }

    }

    [EntityLogic(typeof(IkuChainLightDef))]
    public sealed class IkuChainLight : Card
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<DieEventArgs>(base.Battle.EnemyDied, new EventSequencedReactor<DieEventArgs>(this.OnEnemyDied));
        }

        bool killed = false;
        
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector);
            while (killed)
            {
                killed = false;
                if (base.Battle.BattleShouldEnd)
                {
                    yield break;
                }
                EnemyUnit enemyUnit = base.Battle.EnemyGroup.Alives.MinBy((EnemyUnit unit) => unit.Hp);
                yield return base.AttackAction(enemyUnit);
            }
            yield break;
        }
        
        private IEnumerable<BattleAction> OnEnemyDied(DieEventArgs args)
        {
            if (args.DieSource == this && base.Battle != null)
            {
                killed = true;
            }
            yield break;
        }
    }
}
