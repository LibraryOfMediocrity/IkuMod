using LBoL.ConfigData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IkuMod.Cards.Template
{
    public static class GunNameID
    {
        private static IReadOnlyList<GunConfig> gunConfig = GunConfig.AllConfig();

        //*****************************************
        //To get a list of all the in-game gun IDs:
        //***************************************** 
        // 1) Install debug mode
        // 2) Start an enemy encounter
        // 3) Click F2
        // 4) Select "gun test"
        // 5) This will open a menu with all the gun options in the game. The IDs are located on the left of the gun names.

        public static string GetGunFromId(int id)
        {
            string gun_name = "";
            try
            {
                gun_name = (from config in gunConfig
                            where config.Id == id
                            select config.Name).ToList<string>()[0];
            }
            catch
            {
                UnityEngine.Debug.Log("id: " + id + " doesn't exist. Check whether the ID is correct.");
                gun_name = "Instant";
            }
            return gun_name;
        }
    }

}
