using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using IkuMod.Cards;
//using IkuMod.Exhibits;
//using IkuMod.IkuUlt;
namespace IkuMod.Loadouts
{
    public class IkuLoadouts
    {
        /*
        public static string UltimateSkillA = nameof(IkuUltA);
        public static string UltimateSkillB = nameof(IkuUltB);

        public static string ExhibitA = nameof(IkuExhibitA);
        public static string ExhibitB = nameof(IkuExhibitB);
        */
        public static List<string> DeckA = new List<string>{
            "Shoot", "Shoot", "Boundary", "Boundary", 
            "IkuAttackU", "IkuAttackU", "IkuBlockR", 
            "IkuBlockR", "IkuBlockR", "IkuWaterWind"
            //put iku cards here when done
        };

        public static List<string> DeckB = new List<string>{
            "Shoot", "Shoot", "Boundary", "Boundary",
            "IkuAttackR", "IkuAttackR", "IkuBlockU", 
            "IkuBlockU", "IkuBlockU", "IkuThunder"
            // put iku cards here when done
            
        };

        public static PlayerUnitConfig playerUnitConfig = new PlayerUnitConfig(
            Id: BepinexPlugin.modId,
            ShowOrder: 8,
            Order: 0,
            UnlockLevel: new int?(0),
            ModleName: "",
            NarrativeColor: "#534DEB",
            IsSelectable: true,
            MaxHp: 77,
            InitialMana: new ManaGroup() { Blue = 2, Red = 2 },
            InitialMoney: 55,
            InitialPower: 0,
            UltimateSkillA: "IkuUltA",
            UltimateSkillB: "IkuUltB",
            ExhibitA: "IkuExhibitU",
            ExhibitB: "IkuExhibitR",


            DeckA: IkuLoadouts.DeckA,
            DeckB: IkuLoadouts.DeckB,
            DifficultyA: 2,
            DifficultyB: 1
        );
    }
}

