using System;
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
                World.AddExplosion(playerPos, ExplosionType.StickyBomb, 20f, 2f, null, true, false);
            }
        };
    }

    protected void CreateMenu()
    {
        //NativeItem regularItem = new NativeItem("Regular Item", "This is a regular NativeItem, you can only activate it.");
        NativeCheckboxItem checkboxItem = new NativeCheckboxItem("Checkbox Item", false);
        NativeDynamicItem<int> dynamicItem = new NativeDynamicItem<int>("Dynamic Item", 10);

        //menu.Add(regularItem);
        menu.Add(checkboxItem);
        menu.Add(dynamicItem);

        pool.Add(menu);
        menu.Visible = true;

        TeleportPlayer();
        Explode();
    }
}