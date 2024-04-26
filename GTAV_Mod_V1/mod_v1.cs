using System;
using System.Data.Common;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

public class mod_v1 : Script // Change "YouTubeTutorial" to the name of your program.
{
    Vector3 playerPos = new Vector3();
    float playerHeading = 0;
    bool canTeleport = false;


    public mod_v1() // Change "YouTubeTutorial" to the name of your program.
    {
        Tick += OnTick;
        KeyUp += OnKeyUp;
        KeyDown += OnKeyDown;
    }

    private void OnTick(object sender, EventArgs e)
    {

    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F8)// save location
        {
            playerPos = Game.Player.Character.Position;
            canTeleport = true;
            Game.Player.Character.CanRagdoll = true;
            //playerHeading = Game.Player.Character.Heading;

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
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {

    }
}