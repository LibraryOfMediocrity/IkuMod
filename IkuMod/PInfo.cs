using HarmonyLib;

namespace IkuMod
{
    public static class PInfo
    {
        // each loaded plugin needs to have a unique GUID. usually author+generalCategory+Name is good enough
        public const string GUID = "goat.lbol.Character.Iku";
        public const string Name = "Iku Mod";
        public const string version = "0.0.1";
        public static readonly Harmony harmony = new Harmony(GUID);

    }
}
