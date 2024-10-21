using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
using IkuMod.ImageLoader;
using IkuMod.Localization;
using IkuMod.Config;

namespace IkuMod.StatusEffects
{
    public class IkuStatusEffectTemplate : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return IkuDefaultConfig.GetDefaultID(this);
        }

        public override LocalizationOption LoadLocalization()
        {
            return IkuLocalization.StatusEffectsBatchLoc.AddEntity(this);
        }

        public override Sprite LoadSprite()
        {
            return IkuImageLoader.LoadStatusEffectLoader(status: this);
        }

        public override StatusEffectConfig MakeConfig()
        {
            return GetDefaultStatusEffectConfig();
        }

        public static StatusEffectConfig GetDefaultStatusEffectConfig()
        {
            return IkuDefaultConfig.GetDefaultStatusEffectConfig();
        }
    }
}
