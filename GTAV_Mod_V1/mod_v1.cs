using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.NaturalMotion;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

public class mod_v1 : Script
{
    private ObjectPool pool = new ObjectPool();
    private readonly NativeMenu menu = new NativeMenu("Mod_v1");

    bool canUseExplosion = false;
    bool explosionMenuCreated = false;
    bool canSuperJump = false;
    bool canUseSuperSpeed = false;
    bool canUseVehicleSuperSpeed = false;
    bool VehicleSuperSpeedButtonPressed = false;

    public mod_v1()
    {
        CreateMenu();

        Tick += OnTick;
        KeyUp += OnKeyUp;
        KeyDown += OnKeyDown;
    }

    private void OnTick(object sender, EventArgs e)
    {
        pool.Process();

        if (canSuperJump)
        {
            Game.Player.SetSuperJumpThisFrame();
        }
        if (canUseSuperSpeed)
        {
            Game.Player.SetRunSpeedMultThisFrame(1.49f);
            Function.Call(Hash.RESET_PLAYER_STAMINA, Game.Player); // infinite stamina
        }
        else
        {
            Game.Player.SetRunSpeedMultThisFrame(1f);
        }
        if (canUseVehicleSuperSpeed && VehicleSuperSpeedButtonPressed)
        {
            Vehicle a = Game.Player.Character.CurrentVehicle;
            if (a != null && Game.Player.Character.IsInVehicle())
            {
                a.ForwardSpeed = 20f + a.Speed;
                a.IsInvincible = true;
                a.CanBeVisiblyDamaged = false;
                Game.Player.Character.SetConfigFlag(32, false); // seatbelt always on
                VehicleSuperSpeedButtonPressed = false;
            }
        }
        if (canUseExplosion)
        {
            ExplosivePunchAndBullets();
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F12)
        {
            menu.Visible = !menu.Visible;
        }
        if (e.KeyCode == Keys.E && Game.Player.IsAiming)
        {
            Explode();
        }
        if (e.Shift)
        {
            VehicleSuperSpeedButtonPressed = true;
        }
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Shift)
        {
            VehicleSuperSpeedButtonPressed = false;
        }
    }

    void TeleportPlayer()
    {
        NativeItem regularItem = new NativeItem("Teleport to map marker");
        menu.Add(regularItem);
        menu.ItemActivated += (sender, e) => {
            if (e.Item == regularItem)
            {
                var blip = World.WaypointBlip;
                if(blip != null)
                {
                    Game.Player.Character.Position = blip.Position;
                }
                else
                {
                    Notification.Show("Set map marker before using teleport");
                }
            }
        };
    }

    void Explode()
    {
        if(explosionMenuCreated)
        {
            if (canUseExplosion)
            {
                RaycastResult ab = World.GetCrosshairCoordinates();
                World.AddExplosion(ab.HitPosition,
                            ExplosionType.StickyBomb,
                            20f,
                            0.2f,
                            null,
                            true,
                            false
                            );
            }
        }
        else
        {
            NativeCheckboxItem checkboxItem = new NativeCheckboxItem("Explosion","press E while aiming");
            menu.Add(checkboxItem);
            explosionMenuCreated = true;
            checkboxItem.Activated += (sender, e) =>
            {
                if (checkboxItem.Checked)
                {
                    canUseExplosion = true;
                    Notification.Show("Explosion effect Activated");
                }
                else
                {
                    canUseExplosion = false;
                    Notification.Show("Explosion effect Deactivated");
                }
            };
        }
    }

    void AddWeapons()
    {
        List<Weapon> weaponsList = new List<Weapon>();
        NativeItem regularItem = new NativeItem("Add Weapons");
        menu.Add(regularItem);
        menu.ItemActivated += (sender, e) => {
            if (e.Item == regularItem)
            {
                Game.Player.Character.Weapons.RemoveAll();
                Weapon weapon1 = Game.Player.Character.Weapons.Give(weaponHash: WeaponHash.RPG, 2, false, true);
                weaponsList.Add(weapon1);
                Weapon weapon2 = Game.Player.Character.Weapons.Give(weaponHash: WeaponHash.Grenade, 10, false, true);
                weaponsList.Add (weapon2);
                Weapon weapon3 = Game.Player.Character.Weapons.Give(weaponHash: WeaponHash.CombatMGMk2, 10, false, true);
                weaponsList.Add(weapon3);
                Weapon weapon4 = Game.Player.Character.Weapons.Give(weaponHash: WeaponHash.HeavySniperMk2, 999999, false, true);
                weaponsList.Add(weapon4);
                Weapon weapon5 = Game.Player.Character.Weapons.Give(weaponHash: WeaponHash.AssaultrifleMk2, 999999, false, true);
                weaponsList.Add(weapon5);
                Weapon weapon6 = Game.Player.Character.Weapons.Give(weaponHash: WeaponHash.PumpShotgunMk2, 999999, false, true);
                weaponsList.Add(weapon6);
                Weapon weapon7 = Game.Player.Character.Weapons.Give(weaponHash: WeaponHash.GrenadeLauncher, 999999, false, true);
                weaponsList.Add(weapon7);
                Weapon weapon8 = Game.Player.Character.Weapons.Give(weaponHash: WeaponHash.SpecialCarbineMk2, 999999, false, true);
                weaponsList.Add(weapon8);
                Weapon weapon9 = Game.Player.Character.Weapons.Give(weaponHash: WeaponHash.Molotov, 999999, false, true);
                weaponsList.Add(weapon9);
                for (int i = 0; i < weaponsList.Count; i++)
                {
                    weaponsList[i].InfiniteAmmo = true;
                    weaponsList[i].InfiniteAmmoClip = true;
                }
                Notification.Show("Weapons added with unlimited ammo");
            }
        };
    }

    void WantedLevel()
    {
        NativeListItem<int> list = new NativeListItem<int>("Wanted Level: ", 0 ,1, 2, 3, 4, 5);
        menu.Add(list);
        list.ItemChanged += (sender, e) =>
        {
            Game.Player.WantedLevel = list.SelectedIndex;
        };
    }

    void InvinciblePlayer()
    {
        NativeCheckboxItem checkboxItem = new NativeCheckboxItem("God mode/unlimited health");
        menu.Add(checkboxItem);
        checkboxItem.Activated += (sender, e) =>
        {
            if(checkboxItem.Checked)
            {
                Notification.Hide(0);
                Game.Player.Character.IsInvincible = true;
                Notification.Show("God mode Activated");
            }
            else
            { 
                Notification.Hide(0);
                Game.Player.Character.IsInvincible = false;
                Notification.Show("God mode Deactivated");
            }
        };
    }

    void SpawnVehicle()
    {
        NativeItem item = new NativeItem("Spawn Car");
        menu.Add(item);
        item.Activated += (sender, e) =>
        {
            Vehicle vehicle = World.CreateVehicle(
                VehicleHash.Buffalo, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 3.0f,
                Game.Player.Character.Heading + 90);
            
            vehicle.IsRadioEnabled = false;
            Notification.Show("Vehicle spawned");
        };
    }

    void SpawnRandomNPC()
    {
        NativeItem regularItem = new NativeItem("Spawn Random NPC");
        menu.Add(regularItem);
        regularItem.Activated += (sender, e) =>
        {
            World.CreateRandomPed(Game.Player.Character.Position + Game.Player.Character.ForwardVector * 10f);
            Notification.Show("NPC Spawned");
        };
    }

    void SuperJump()
    {
        NativeCheckboxItem checkboxItem = new NativeCheckboxItem("Super Jump");
        menu.Add(checkboxItem);
        checkboxItem.Activated += (sender, e) =>
        {
            if (checkboxItem.Checked)
            {
                canSuperJump = true;
                Notification.Show("Super jump Activated");
            }
            else
            {
                canSuperJump = false;
                Notification.Show("Super jump Deactivated");
            }
        };
    }

    void SuperSpeed()
    {
        NativeCheckboxItem checkboxItem = new NativeCheckboxItem("Super Speed");
        menu.Add(checkboxItem);
        checkboxItem.Activated += (sender, e) =>
        {
            if (checkboxItem.Checked)
            {
                canUseSuperSpeed = true;
                Notification.Show("Super speed Activated");
            }
            else
            {
                canUseSuperSpeed = false;
                Notification.Show("Super speed Deactivated");
            }
        };
    }

    void vehicleSpeedBoost()
    {
        NativeCheckboxItem checkboxItem = new NativeCheckboxItem("Vehicle Super Speed");
        menu.Add(checkboxItem);
        checkboxItem.Activated += (sender, e) =>
        {
            if (checkboxItem.Checked)
            {
                canUseVehicleSuperSpeed = true;
                Notification.Show("Vehicle super speed Activated");
            }
            else
            {
                canUseVehicleSuperSpeed = false;
                Notification.Show("Vehicle super speed Deactivated");
            }
        };
    }

    private void ExplosivePunchAndBullets()
    {
        Game.Player.SetExplosiveMeleeThisFrame();
        Game.Player.SetExplosiveAmmoThisFrame();
    }

    void AddMoney()
    {
        NativeItem item = new NativeItem("Add Money");
        menu.Add(item);
        item.Activated += (sender, e) =>
        {
            Game.Player.Money = Game.Player.Money + 1000000;
            Notification.Show("Money Added");
        };
    }

    protected void CreateMenu()
    {
        pool.Add(menu);
        menu.Visible = true;

        Explode();
        AddWeapons();
        WantedLevel();
        SpawnVehicle();
        SpawnRandomNPC();
        TeleportPlayer();
        InvinciblePlayer();
        SuperJump();
        SuperSpeed();
        vehicleSpeedBoost();
        AddMoney();
    }
}