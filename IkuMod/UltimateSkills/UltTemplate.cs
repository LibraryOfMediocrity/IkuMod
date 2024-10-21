using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
using IkuMod.Config;
using IkuMod.ImageLoader;
using IkuMod.Localization;

namespace IkuMod.UltimateSkills
{
    public class IkuUltTemplate : UltimateSkillTemplate
    {
        public override IdContainer GetId()
        {
            return IkuDefaultConfig.GetDefaultID(this);
        }

        public override LocalizationOption LoadLocalization()
        {
            return IkuLocalization.UltimateSkillsBatchLoc.AddEntity(this);
        }

        public override Sprite LoadSprite()
        {
            return IkuImageLoader.LoadUltLoader(ult: this);
        }

        public override UltimateSkillConfig MakeConfig()
        {
            throw new System.NotImplementedException();
        }

        public UltimateSkillConfig GetDefaulUltConfig()
        {
            return IkuDefaultConfig.GetDefaultUltConfig();
        }
    }
}
