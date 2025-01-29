using LBoLEntitySideloader.Attributes;
using LBoL.Core.StatusEffects;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.ConfigData;

namespace IkuMod.StatusEffects
{
    public sealed class IkuConduitDiscDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { "IkuSurgeSe" };
            return config;
        }
    }

    [EntityLogic(typeof(IkuConduitDiscDef))]
    public sealed class IkuConduitDisc : StatusEffect
    {
       
    }
}
