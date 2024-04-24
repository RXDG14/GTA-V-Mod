using System;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.UI;

public class mod_v1 : Script // Change "YouTubeTutorial" to the name of your program.
{
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
        if(e.KeyCode == Keys.F10)
        {
            Vehicle spawnedCar = World.CreateVehicle(VehicleHash.Alpha, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 2f, Game.Player.Character.Heading + 90);
            Notification.Show("Your car has spawned");
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