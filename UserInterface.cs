using Gum.Forms.Controls;
using MonoGameGum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace ToeJam___Earl_2._0_
{
    public class UserInterface
    {
        public void setup() {
            Console.WriteLine("Testing Setup.");
            // Creating a panel and adding it to the root.
            Panel mainMenuPanel = new Panel();
            mainMenuPanel.AddToRoot();
            Panel optionsPanel = new Panel();
            // Creating a button and adding it as a child element of the panel
            // which indirectly connects it to Gum's root container
            // anchored at the bottom of the panel, in ToeJam & Earl's case.
            Button startButton = new Button();
            
            startButton.Anchor(Gum.Wireframe.Anchor.Bottom);
            mainMenuPanel.AddChild(startButton);
            mainMenuPanel.Dock(Gum.Wireframe.Dock.Bottom);
            // Change the state of the UI by hiding one panel and showing another.
            mainMenuPanel.IsVisible = false;
            optionsPanel.IsVisible = true;
            // Set the X and Y position so it is 20px from the left edge
            // and 20px from the bottom edge.
           // startButton.Visual.X = 20;
            startButton.Visual.Y = -20;

            






        }
       

    }




}

