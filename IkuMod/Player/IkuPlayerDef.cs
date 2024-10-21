using Cysharp.Threading.Tasks;
//using DG.Tweening;
using LBoL.ConfigData;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
using IkuMod.Config;
using IkuMod.Loadouts;
using IkuMod.ImageLoader;
using IkuMod.Localization;
//using IkuMod.BattleActions;

namespace IkuMod.Player
{
    public sealed class IkuPlayerDef : PlayerUnitTemplate
    {
        public UniTask<Sprite>? LoadSpellPortraitAsync { get; private set; }

        public override IdContainer GetId()
        {
            return BepinexPlugin.modId;
        }

        public override LocalizationOption LoadLocalization()
        {
            return IkuLocalization.PlayerUnitBatchLoc.AddEntity(this);
        }

        public override PlayerImages LoadPlayerImages()
        {
            return IkuImageLoader.LoadPlayerImages(BepinexPlugin.playerName);
        }

        public override PlayerUnitConfig MakeConfig()
        {
            return IkuLoadouts.playerUnitConfig;
        }

        [EntityLogic(typeof(IkuPlayerDef))]
        public sealed class IkuMod : PlayerUnit
        {
        }
    }
}
