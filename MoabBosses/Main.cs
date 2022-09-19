using Assets.Scripts.Models.Rounds;
using Assets.Scripts.Unity;
using BTD_Mod_Helper;
using MelonLoader;
using System;
using System.Threading.Tasks;

namespace MoabBosses
{
    public class Main : BloonsTD6Mod
    {
        public static string boss = "Bloonarius";

        public override void OnMainMenu()
        {
            base.OnMainMenu();

            MelonLogger.Msg(ConsoleColor.Blue, "Type the boss you would like to spawn (ex. \"Bloonarius\")");

            GetInputAsync();
        }

        public static string promoteBloon(string bloon, string boss)
        {
            //I swear i tried to use dictionaries and lists but it crashes for no reason
            if (bloon == "Moab") return boss + "1";
            if (bloon == "MoabFortified") return boss + "Elite1";
            if (bloon == "Bfb") return boss + "2";
            if (bloon == "BfbFortified") return boss + "Elite2";
            if (bloon == "Zomg") return boss + "3";
            if (bloon == "ZomgFortified") return boss + "Elite3";
            if (bloon == "DdtCamo") return boss + "4";
            if (bloon == "DdtFortifiedCamo") return boss + "Elite4";
            if (bloon == "Bad") return boss + "5";
            if (bloon == "BadFortified") return boss + "Elite5";
            return bloon;
        }
        private async void GetInputAsync()
        {
            boss = await Task.Run(() => Console.ReadLine());

            if (boss != "Bloonarius" && boss != "Lych" && boss != "Vortex")
            {
                MelonLogger.Msg(ConsoleColor.Blue, "ERROR: Not a valid boss (yell at emeryllium if this is wrong lmao).");
                boss = "Bloonarius";
            }
            else
            {
                MelonLogger.Msg(ConsoleColor.Blue, boss + " Loaded.");
            }

            for (int i = 0; i < Game.instance.model.roundSets.Length; i++)
            {
                RoundSetModel roundSet = Game.instance.model.roundSets[i];
                for (int j = 0; j < roundSet.rounds.Length; j++)
                {
                    RoundModel round = roundSet.rounds[j];

                    for (int k = 0; k < round.groups.Length; k++)
                    {
                        BloonGroupModel bloonGroup = round.groups[k];
                        bloonGroup.bloon = promoteBloon(bloonGroup.bloon, boss);
                    }
                }
            }
        }
    }
}