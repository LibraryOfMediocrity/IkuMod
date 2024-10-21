using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using IkuMod.Config;
using IkuMod.ImageLoader;
using IkuMod.Localization;


namespace IkuMod.Cards.Template
{
    public abstract class IkuCardTemplate : CardTemplate
    {
        public override IdContainer GetId()
        {
            return IkuDefaultConfig.GetDefaultID(this);
        }

        public virtual bool UseDefault
        {
            get { return false; }
        }

        public override CardImages LoadCardImages()
        {
            //change to false when done
            return IkuImageLoader.LoadCardImages(this, UseDefault);
        }

        public override LocalizationOption LoadLocalization()
        {
            return IkuLocalization.CardsBatchLoc.AddEntity(this);
        }


        public CardConfig GetCardDefaultConfig()
        {
            return IkuDefaultConfig.GetCardDefaultConfig();
        }
    }


}

