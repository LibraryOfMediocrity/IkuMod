using System;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using IkuMod.Config;
using IkuMod.ImageLoader;
using IkuMod.Localization;
using LBoL.Core;

namespace IkuMod.Exhibits
{
    public class IkuExhibitTemplate : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return IkuDefaultConfig.GetDefaultID(this);
        }

        public override LocalizationOption LoadLocalization()
        {
            return IkuLocalization.ExhibitsBatchLoc.AddEntity(this);
        }

        public override ExhibitSprites LoadSprite()
        {
            return IkuImageLoader.LoadExhibitSprite(exhibit: this);
        }

        public override ExhibitConfig MakeConfig()
        {
            return GetDefaultExhibitConfig();
        }

        public ExhibitConfig GetDefaultExhibitConfig()
        {
            return IkuDefaultConfig.GetDefaultExhibitConfig();
        }


    }
}
