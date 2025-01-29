using LBoL.Core;
using LBoL.Core.Cards;

namespace IkuMod.BattleActions
{
    public class VeilCardEventArgs : GameEventArgs
    {
        public Card Card { get; internal set; }

        public override string GetBaseDebugString()
        {
            return "Card: " + this.Card;
        }
    }
}
