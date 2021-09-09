using MelonLoader;
using Harmony;
using Assets.Scripts.Unity.UI_New.InGame.Races;
using Assets.Scripts.Simulation.Towers.Weapons;
using Assets.Scripts.Simulation;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Unity.UI_New.Main;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Models.Towers;

using Assets.Scripts.Unity;



using Assets.Scripts.Simulation.Towers;

using Assets.Scripts.Utils;

using Il2CppSystem.Collections;
using Assets.Scripts.Unity.UI_New.Popups;
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Simulation.Objects;
using Assets.Scripts.Models;
using TMPro;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using System;
using Assets.Scripts.Simulation.Towers.Behaviors;
using Assets.Scripts.Simulation.Towers.Projectiles.Behaviors;
using System.Collections.Generic;
using System.Linq;
using Assets.Main.Scenes;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Simulation.Towers.Pets;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Models.Bloons.Behaviors;
using BTD_Mod_Helper;
using Il2CppSystem.Threading;
using Il2CppSystem.Threading.Tasks;
using System.Text.RegularExpressions;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Map;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;

namespace btd6ai
{
    public class Main : BloonsTD6Mod
    {
        public override void OnTitleScreen()
        {
            base.OnTitleScreen();

            Console.WriteLine("tackifying...");

            string[] names = new string[Game.instance.model.towers.Length];
            int i = 0;
            foreach (var tower in Game.instance.model.towers)
            {
                names[i] = tower.name;
                i++;
            }

            foreach (var tower in Game.instance.model.towers)
            {
                if (tower.HasBehavior<AttackModel>())
                {
                    float range = 1;
                    foreach (var attackModel in tower.GetBehaviors<AttackModel>())
                    {
                        attackModel.RemoveBehaviors<RotateToTargetModel>();

                        var weapons = attackModel.weapons;
                        foreach (var weapon in weapons)
                        {
                            if (weapon.emission.IsType<PrinceOfDarknessEmissionModel>() || weapon.emission.IsType<NecromancerEmissionModel>()) continue;
                            if (weapon.projectile.HasBehavior<CreateTowerModel>())
                            {
                                string name = weapon.projectile.GetBehavior<CreateTowerModel>().tower.name;
                                int index = Array.IndexOf(names, name);
                                weapon.projectile.GetBehavior<CreateTowerModel>().tower = Game.instance.model.towers.ElementAt(index);
                            }
                            if (weapon.projectile.HasBehavior<CreateTypedTowerModel>())
                            {
                                string name = weapon.projectile.GetBehavior<CreateTypedTowerModel>().boomTower.name;
                                int index = Array.IndexOf(names, name);
                                weapon.projectile.GetBehavior<CreateTypedTowerModel>().boomTower = Game.instance.model.towers.ElementAt(index);

                                name = weapon.projectile.GetBehavior<CreateTypedTowerModel>().coldTower.name;
                                index = Array.IndexOf(names, name);
                                weapon.projectile.GetBehavior<CreateTypedTowerModel>().coldTower = Game.instance.model.towers.ElementAt(index);

                                name = weapon.projectile.GetBehavior<CreateTypedTowerModel>().crushingTower.name;
                                index = Array.IndexOf(names, name);
                                weapon.projectile.GetBehavior<CreateTypedTowerModel>().crushingTower = Game.instance.model.towers.ElementAt(index);

                                name = weapon.projectile.GetBehavior<CreateTypedTowerModel>().energyTower.name;
                                index = Array.IndexOf(names, name);
                                weapon.projectile.GetBehavior<CreateTypedTowerModel>().energyTower = Game.instance.model.towers.ElementAt(index);
                            }

                            if (weapon.emission.IsType<LineProjectileEmissionModel>())
                            {
                                continue;
                            }

                            float count = 1;
                            weapon.projectile.pierce /= 2;
                            if (weapon.emission.IsType<ArcEmissionModel>())
                            {
                                count = (float)Math.Ceiling((decimal)weapon.emission.Cast<ArcEmissionModel>().count / 2);

                                if (weapon.emission.Cast<ArcEmissionModel>().angle == 360)
                                {
                                    count /= 3;
                                    range *= 1.5f;
                                }
                            }
                            if (weapon.emission.IsType<RandomEmissionModel>() && weapon.emission.Cast<RandomEmissionModel>().count != 1)
                            {
                                Console.WriteLine(tower.name);
                                weapon.emission.Cast<RandomEmissionModel>().angle = 360;
                                weapon.emission.Cast<RandomEmissionModel>().count *= 3;
                            }
                            else
                            {
                                if (!tower.name.Contains("Sentry"))
                                    weapon.emission = new ArcEmissionModel("ArcEmmissionModel_", (int)(6 * count), 0, 360, null, false, false);
                            }
                            weapon.animateOnMainAttack = false;
                            weapon.ejectX = 0;
                            weapon.ejectY = 0;
                        }
                        if(!tower.name.Contains("Village"))
                        attackModel.range *= range / 1.5f;
                    }
                if(!tower.name.Contains("Village"))
                tower.range *= range / 1.5f;
                }
            }
        }
    }
}