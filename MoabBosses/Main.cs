using Assets.Main.Scenes;
using Assets.Scripts.Models.Bloons;
using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Models.Rounds;
using Assets.Scripts.Simulation;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Simulation.Bloons.Behaviors;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Simulation.Track;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
using Harmony;
using MelonLoader;
using System;
using System.Collections.Generic;
using UnhollowerBaseLib;

namespace MoabBosses
{
    public class Main : BloonsTD6Mod
    {
        public bool nullifyCash = true;
        public override void OnTitleScreen()
        {
            for (int i = 0; i < Game.instance.model.roundSets.Length; i++)
            {
                RoundSetModel roundSet = Game.instance.model.roundSets[i];
                for (int j = 0; j < roundSet.rounds.Length; j++)
                {
                    RoundModel round = roundSet.rounds[j];

                    for (int k = 0; k < round.groups.Length; k++)
                    {
                        BloonGroupModel bloonGroup = round.groups[k];
                        bloonGroup.bloon = promoteBloon(bloonGroup.bloon);
                    }
                }
            }
        }
        public override void OnCashAdded(double amount, Simulation.CashType from, int cashIndex, Simulation.CashSource source, Tower tower)
        {
            if (source == Simulation.CashSource.Normal) InGame.instance.AddCash(-0.75 * amount);
            base.OnCashAdded(amount, from, cashIndex, source, tower);
        }
        public override void OnBloonDestroy(Bloon bloon)
        {
            InGame.instance.AddCash(1);
            base.OnBloonDestroy(bloon);
        }
        public static string promoteBloon(string bloon)
        {
            //I swear i tried to use dictionaries and lists but it crashes for no reason
            if (bloon == "Moab") return "Bloonarius1";
            if (bloon == "MoabFortified") return "BloonariusElite1";
            if (bloon == "Bfb") return "Bloonarius2";
            if (bloon == "BfbFortified") return "BloonariusElite2";
            if (bloon == "Zomg") return "Bloonarius3";
            if (bloon == "ZomgFortified") return "BloonariusElite3";
            if (bloon == "DdtCamo") return "Bloonarius4";
            if (bloon == "DdtCamoFortified") return "BloonariusElite4";
            if (bloon == "Bad") return "Bloonarius5";
            if (bloon == "BadFortified") return "BloonariusElite5";
            return bloon;
        }
    }
}