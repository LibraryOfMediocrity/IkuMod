using IkuMod.Cards.Template;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IkuMod.Cards
{
    public sealed class IkuShockDef : IkuCardTemplate
    {
        public override bool UseDefault => true; 

        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.Rarity = Rarity.Uncommon;
            config.GunName = GunNameID.GetGunFromId(1100);
            config.GunNameBurst = GunNameID.GetGunFromId(1100);
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Red };
            config.TargetType = TargetType.SingleEnemy;
            config.Cost = new ManaGroup() { White = 1, Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 2 };
            config.Damage = 2;
            config.UpgradedDamage = 4;
            config.Keywords = Keyword.Accuracy | Keyword.Exile;
            config.UpgradedKeywords = Keyword.Accuracy | Keyword.Exile;
            config.RelativeEffects = new List<string>() { "Electric" };
            config.UpgradedRelativeEffects = new List<string>() { "Electric" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(IkuShockDef))]
    public sealed class IkuShock : Card
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<DamageEventArgs>(base.Battle.Player.DamageDealt, new EventSequencedReactor<DamageEventArgs>(this.OnPlayerDamageDealt));
        }

        private IEnumerable<BattleAction> OnPlayerDamageDealt(DamageEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            if (args.Cause == ActionCause.Card && args.ActionSource == this)
            {
                int damageInfo = (int) args.DamageInfo.Damage;
                if (damageInfo > 0f)
                {
                    yield return BuffAction<Electric>(damageInfo, 0, 0, 0, 0.2f);
                }
            }
            yield break;
        }
    }
}
