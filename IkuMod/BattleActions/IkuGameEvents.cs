using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Units;

namespace IkuMod.BattleActions
{
    [HarmonyPatch]
    class IkuGameEvents
    {
        static public GameEvent<VeilCardEventArgs> PreVeilEvent { get; set; }
        static public GameEvent<VeilCardEventArgs> PostVeilEvent { get; set; }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterBattle))]
        private static bool Prefix(GameRunController __instance)
        {
            //UnityEngine.Debug.Log("New Custom Events Loaded");
            PreVeilEvent = new GameEvent<VeilCardEventArgs>();
            PostVeilEvent = new GameEvent<VeilCardEventArgs>();
            return true;
        }
    }
}
