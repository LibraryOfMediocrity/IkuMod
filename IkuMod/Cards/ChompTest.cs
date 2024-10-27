using HarmonyLib;
using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace IkuMod.Cards
{
    public sealed class ChompTestDef : TrapTemplateTest
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Type = CardType.Skill;
            config.Cost = new ManaGroup() { Any = 1, White = 1 };
            config.Rarity = Rarity.Uncommon;
            config.Value1 = 10;
            config.UpgradedValue1 = 15;
            config.Value2 = 5;
            config.Keywords = Keyword.Retain;
            config.UpgradedKeywords = Keyword.Retain;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(ChompTestDef))]
    public sealed class ChompTest : TrapCardTest
    {
        public override Unit[] DefaultTarget()
        {
            
            return SelectUnit(TrapSelector.LeastLife);
        }

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnEnding, new EventSequencedReactor<UnitEventArgs>(this.OnTurnEnding));
        }

        private IEnumerable<BattleAction> OnTurnEnding(UnitEventArgs args)
        {

            if (this.Zone == CardZone.Hand)
            {
                Debug.Log("Turn Ending reacted");

                this.NotifyActivating();
                if (DefaultTarget().Length == 1)
                {
                    EnemyUnit enemyUnit = (EnemyUnit)DefaultTarget()[0];
                    //these checks are unnecessary but I cant be bothered to remove them
                    if (enemyUnit != null && enemyUnit.IsAlive)
                    {
                        //iterate through TrapTriggered results and return them immediately so effects happen immediately
                        //instead of waiting for the actions to be iterated through later
                        var BattleActions = this.TrapTriggered(DefaultTarget());
                        foreach (var action in BattleActions)
                        {
                            yield return action;
                        }
                        yield break;
                        //return this.TrapTriggered(DefaultTarget());
                    }
                }
                else
                {
                    Debug.Log("Aoe Turn Ending");
                    EnemyUnit[] enemyUnits = new EnemyUnit[DefaultTarget().Length];
                    int i = 0;
                    foreach (EnemyUnit enemyUnit in (EnemyUnit[])DefaultTarget())
                    {
                        //these checks are unnecessary but I cant be bothered to remove them
                        if (enemyUnit != null || enemyUnit.IsAlive)
                        {
                            enemyUnits[i] = enemyUnit;
                            i++;
                        }
                    }
                    var BattleActions = this.TrapTriggered(enemyUnits);
                    foreach (var action in BattleActions)
                    {
                        yield return action;
                    }
                    yield break;
                }
            }
            yield break;
            //return null;
        }

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] selector)
        {
            //do something so two chompers don't trigger at the same time
            if (selector.Length == 1)
            {
                EnemyUnit enemy = (EnemyUnit)selector[0];

                Debug.Log("Chomper Triggered: " + enemy.Name);
                Debug.Log("Pre kill values: Hp - " + enemy.Hp + ", Chomp Range - " + base.Value1);

                if (enemy != null && enemy.IsAlive && enemy.Hp <= this.Value1)
                {
                    yield return new ForceKillAction(base.Battle.Player, enemy);
                }
                this.DeltaValue1 += this.Value2;

                Debug.Log("Post kill values: Hp - " + enemy.Hp + ", Chomp Range - " + base.Value1);
            }
            else
            {
                EnemyUnit[] enemies = (EnemyUnit[])selector;
                foreach (EnemyUnit enemy in enemies)
                {
                    Debug.Log("Chomper Triggered: " + enemy.Name);
                    Debug.Log("Pre kill values: Hp - " + enemy.Hp + ", Chomp Range - " + base.Value1);

                    if (enemy != null && enemy.IsAlive && enemy.Hp <= this.Value1)
                    {
                        yield return new ForceKillAction(base.Battle.Player, enemy);
                    }
                    
                    Debug.Log("Post kill values: Hp - " + enemy.Hp + ", Chomp Range - " + base.Value1);
                }
                this.DeltaValue1 += this.Value2;
            }
            yield break;
        }
    }
}
