using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Battle.BattleActions;
using LBoL.Base;
using LBoL.Core.Cards;
using System.Linq;
using LBoL.Base.Extensions;
using LBoL.EntityLib.EnemyUnits.Lore;

namespace IkuMod.StatusEffects
{
    public sealed class IkuArmageddonSeDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            return config;
        }
    }

    [EntityLogic(typeof(IkuArmageddonSeDef))]
    public sealed class IkuArmageddonSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
        }

        public ManaGroup Mana { get; set; } = new ManaGroup() { Philosophy = 3 };

        public int Num { get; set; } = 3;

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            this.NotifyActivating();
            yield return BuffAction<IkuSurgeSe>(3);
            yield return new GainManaAction(Mana);
            int i = 1;
            while (i <= 3 && base.Battle.DrawZone.Count != 0 && base.Battle.HandZone.Count != base.Battle.MaxHand)
            {
                List<Card> list = base.Battle.DrawZone.Where((Card card) => card.CardType == CardType.Attack).ToList<Card>();
                if (list.Count > 0)
                {
                    Card card2 = list.Sample(base.GameRun.BattleRng);
                    yield return new MoveCardAction(card2, CardZone.Hand);
                }
                i++;
            }
            base.Level--;
            if (base.Level <= 0) yield return new RemoveStatusEffectAction(this);
            yield break;
        }
    }
}

