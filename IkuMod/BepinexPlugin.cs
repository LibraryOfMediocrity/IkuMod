using BepInEx;
using HarmonyLib;
using LBoL.Base;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;


namespace IkuMod
{
    [BepInPlugin(IkuMod.PInfo.GUID, IkuMod.PInfo.Name, IkuMod.PInfo.version)]
    [BepInDependency(LBoLEntitySideloader.PluginInfo.GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(AddWatermark.API.GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("LBoL.exe")]
    public class BepinexPlugin : BaseUnityPlugin
    {
        public static string modId = "IkuMod";

        public static string playerName = "Iku";

        public static List<ManaColor> offColors = new List<ManaColor>() { ManaColor.White, ManaColor.Black, ManaColor.Colorless, ManaColor.Green };

        private static readonly Harmony harmony = IkuMod.PInfo.harmony;

        internal static BepInEx.Logging.ManualLogSource log;

        internal static TemplateSequenceTable sequenceTable = new TemplateSequenceTable();

        internal static IResourceSource embeddedSource = new EmbeddedSource(Assembly.GetExecutingAssembly());
        internal static DirectorySource directorySource = new DirectorySource(IkuMod.PInfo.GUID, "");


        private void Awake()
        {
            log = Logger;

            // very important. Without this the entry point MonoBehaviour gets destroyed
            DontDestroyOnLoad(gameObject);
            gameObject.hideFlags = HideFlags.HideAndDontSave;

            EntityManager.RegisterSelf();

            harmony.PatchAll();

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(AddWatermark.API.GUID))
                WatermarkWrapper.ActivateWatermark();
        }

        private void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchSelf();
        }


    }
}
