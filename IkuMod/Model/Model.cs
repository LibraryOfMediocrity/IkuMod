using Cysharp.Threading.Tasks;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader.Utils;
using UnityEngine;
using IkuMod.Localization;
using LBoL.Presentation;
using IkuMod.Player;
using System.Collections.Generic;

namespace IkuMod.model

{
    public sealed class IkuModel : UnitModelTemplate
    {
        //If a custom model is used, use a custom sprite for the Ultimate animation.
        public static string spellsprite_name = "IkuSpell.png";

        public override IdContainer GetId()
        {
            return new IkuPlayerDef().UniqueId;
        }

        public override LocalizationOption LoadLocalization()
        {
            return IkuLocalization.UnitModelBatchLoc.AddEntity(this);
        }

        public override ModelOption LoadModelOptions()
        {
            
            //Load the custom character's sprite.
            return new ModelOption(ResourceLoader.LoadSpriteAsync("IkuModel.png", BepinexPlugin.directorySource, ppu: 490));
            
        }

        public override UniTask<Sprite> LoadSpellSprite()
        {
            
            //Load the custom character's portrait.
            return ResourceLoader.LoadSpriteAsync("IkuSpell.png", BepinexPlugin.directorySource);
            
        }

        public override UnitModelConfig MakeConfig()
        {
            
            UnitModelConfig config = DefaultConfig().Copy();
            config.Flip = true;
            config.Type = 0;
            config.Offset = new Vector2(0, -0.10f);
            config.HasSpellPortrait = true;
            config.SpellColor = new List<Color32> {
                new Color32(83, 77, 235, byte.MaxValue),
                new Color32(117, 113, 233, byte.MaxValue),
                new Color32(114, 110, 240, 150),
                new Color32(118, 114, 220, byte.MaxValue) };
            return config;
            
        }
    }
}
