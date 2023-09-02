using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public Program()
        {
            // The constructor, called only once every session and
            // always before any other method is called. Use it to
            // initialize your script.
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means.
        }

        public void Main(string argument, UpdateType updateSource)
        {
            // The main entry point of the script, invoked every time
            // one of the programmable block's Run actions are invoked,
            // or the script updates itself. The updateSource argument
            // describes where the update came from.
        }

        // WRAPPER FUNCTIONS
        public double Animate(string animationType, string easingType, string easingDirection, double startValue, double endValue, double variable1, double variable2)
        {
            Potato_Utilities.Animation animationObj = new Potato_Utilities.Animation();
            Potato_Utilities.Animation.AnimationType parsedAnimationType = (Potato_Utilities.Animation.AnimationType)Enum.Parse(typeof(Potato_Utilities.Animation.AnimationType), animationType, true);
            Potato_Utilities.Animation.EasingType parsedEasingType = (Potato_Utilities.Animation.EasingType)Enum.Parse(typeof(Potato_Utilities.Animation.EasingType), easingType, true);
            Potato_Utilities.Animation.EasingDirection parsedEasingDirection = (Potato_Utilities.Animation.EasingDirection)Enum.Parse(typeof(Potato_Utilities.Animation.EasingDirection), easingDirection, true);

            return animationObj.Animate(parsedAnimationType, parsedEasingType, parsedEasingDirection, startValue, endValue, variable1, variable2);
        }

    }
}
