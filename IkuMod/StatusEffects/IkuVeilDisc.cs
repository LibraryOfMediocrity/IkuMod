using LBoLEntitySideloader.Attributes;
using LBoL.Core.StatusEffects;
using LBoL.ConfigData;
using LBoL.Base;


namespace IkuMod.StatusEffects
{
    public sealed class IkuVeilDiscDef : IkuStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            return config;
        }

    }

    [EntityLogic(typeof(IkuVeilDiscDef))]
    public sealed class IkuVeilDisc : StatusEffect
    {
        public ManaGroup Mana
        {
            get
            {
                return new ManaGroup() { Any = 2 };
            }
        }
    }
}
