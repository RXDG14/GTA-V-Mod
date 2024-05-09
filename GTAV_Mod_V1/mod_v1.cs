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
using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

public class mod_v1 : Script
{
    private ObjectPool pool = new ObjectPool();
    private readonly NativeMenu menu = new NativeMenu("Mod_v1");

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
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F12)
        {
            menu.Visible = !menu.Visible;
        }
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        
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
            }
        };
    }

    void Explode()
    {
        NativeItem regularItem = new NativeItem("Explosion");
        menu.Add(regularItem);
        menu.ItemActivated += (sender, e) => {
            if (e.Item == regularItem)
            {
                //Vector3 playerPos = Game.Player.Character.ForwardVector;
                //playerPos.X = playerPos.X + 2f;
                Vector3 playerPos = Game.Player.Character.Position;
                Vector3 playerFront = Game.Player.Character.FrontPosition;
                World.AddExplosion(playerFront, ExplosionType.StickyBomb, 20f, 2f, null, true, false);
            }
        };
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
                for (int i = 0; i < weaponsList.Count; i++)
                {
                    weaponsList[i].InfiniteAmmo = true;
                    weaponsList[i].InfiniteAmmoClip = true;
                }
            }
        };
    }

    void ResetWantedLevel()
    {
        NativeItem regularItem = new NativeItem("Reset wanted level");
        menu.Add(regularItem);
        menu.ItemActivated += (sender, e) => {
            if (e.Item == regularItem)
            {
                Game.Player.WantedLevel = 0;
            }
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

    protected void CreateMenu()
    {
        pool.Add(menu);
        menu.Visible = true;

        Explode();
        AddWeapons();
        TeleportPlayer();
        ResetWantedLevel();
        InvinciblePlayer();
    }
}