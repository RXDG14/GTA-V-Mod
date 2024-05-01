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
    private ObjectPool pool = new ObjectPool();
    private readonly NativeMenu menu = new NativeMenu("LemonUI", "Welcome to LemonUI!");

    public mod_v1() // Change "YouTubeTutorial" to the name of your program.
    {
        Tick += OnTick;
        KeyUp += OnKeyUp;
        KeyDown += OnKeyDown;

        CreateMenu();
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

    protected void CreateMenu()
    {
        NativeItem regularItem = new NativeItem("Regular Item", "This is a regular NativeItem, you can only activate it.");
        NativeCheckboxItem checkboxItem = new NativeCheckboxItem("Checkbox Item", false);
        NativeDynamicItem<int> dynamicItem = new NativeDynamicItem<int>("Dynamic Item", 10);

        menu.Add(submenu);
        menu.Add(regularItem);
        menu.Add(checkboxItem);
        menu.Add(dynamicItem);

        pool.Add(menu);
        pool.Add(submenu);
        menu.Visible = true;
    }
}