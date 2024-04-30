using System;
using System.Data.Common;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

public class mod_v1 : Script // Change "YouTubeTutorial" to the name of your program.
{
    Vector3 playerPos = new Vector3();
    float playerHeading = 0;
    bool canTeleport = false;
    readonly ObjectPool pool = new ObjectPool();
    NativeCheckboxItem checkbox2 = new NativeCheckboxItem("Checkbox", false);


    public mod_v1() // Change "YouTubeTutorial" to the name of your program.
    {
        Tick += OnTick;
        KeyUp += OnKeyUp;
        KeyDown += OnKeyDown;
        //CreateMenu(); kind of like begin play/construction script in ue5
    }

    private void OnTick(object sender, EventArgs e)
    {
        pool.Process();
        CheckIfMenuItemActivated();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F8)// save location
        {
            playerPos = Game.Player.Character.Position;
            canTeleport = true;
            //Game.Player.Character.CanRagdoll = true;
            //Vehicle playerVehicle = Game.Player.Character.CurrentVehicle; 
            // Pass in the Hash name and the parameters listed on Native DB.
            //Function.Call(Hash.SET_VEHICLE_RADIO_ENABLED, Game.Player.Character.CurrentVehicle, false);
            Function.Call(Hash.ADD_EXPLOSION, Game.Player.Character.Position.X + 10f, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, 2, 3f, true, false, 1f, false);
            //Function.Call(Hash.SET_CLOCK_TIME,12, 34, 56);
            Notification.Show("Cash?");
        }
        if (e.KeyCode == Keys.F9 && canTeleport)// teleport
        {
            Game.Player.Character.Position = playerPos;
            Game.Player.Character.Heading = playerHeading;// playerPos.ToHeading();
        }
        if (e.KeyCode == Keys.F10)
        {
            Vehicle spawnedCar = World.CreateVehicle(VehicleHash.Alpha, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 2f, Game.Player.Character.Heading + 90);
            Notification.Show("Car spawned");
        }
        if(e.KeyCode == Keys.F11)
        {
            Vehicle playerVehicle = Game.Player.Character.CurrentVehicle;
            if(playerVehicle != null)
            {
                playerVehicle.Repair();
            }
        }
        if (e.KeyCode == Keys.F12)
        {
            CreateMenu();
        }
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        
    }

    public void CreateMenu()
    {
        NativeMenu menu = new NativeMenu("Mod", "Name", "Description");
        NativeItem item1 = new NativeItem("Explosion");
        NativeItem item2 = new NativeItem("Teleport");
        menu.Add(item1);
        menu.Add(item2);
        menu.Visible = true;
        NativeCheckboxItem checkbox1 = new NativeCheckboxItem("Checkbox", false);
        menu.Add(checkbox2);
        pool.Add(menu);
        //if (item1.sele)
        //{
          //  Notification.Show("lol");
        //}
    }

    public void CheckIfMenuItemActivated()
    {
        if(checkbox2.Checked)
        {
            Function.Call(Hash.ADD_EXPLOSION, Game.Player.Character.Position.X + 10f, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, 2, 3f, true, false, 1f, false);
            checkbox2.Checked = false;
        }
    }
}