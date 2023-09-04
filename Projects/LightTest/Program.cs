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
using static IngameScript.Program.PotatoAnimation;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        // Initialize self-updating
        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.None; // No automatic updates
        }

        public void Save()
        {
            // Save state here
        }

        // Default Values
        string lightName;
        float maxTime;
        float targetIntensity;
        PotatoAnimation.Time.Direction direction;
        PotatoAnimation.Time.Type type;
        float currentTime;
        float initialIntensity;
        bool isAnimating;

        public void Main(string argument, UpdateType updateSource)
        {
            if ((updateSource & UpdateType.Terminal) != 0 || (updateSource & UpdateType.Trigger) != 0 || (updateSource & UpdateType.Script) != 0)
            {
                // Parse the argument
                string[] args = argument.Split(',');
                lightName = args.Length > 0 ? args[0].Trim() : "Light1";
                targetIntensity = args.Length > 1 ? float.Parse(args[1].Trim()) : 2f;
                maxTime = args.Length > 2 ? float.Parse(args[2].Trim()) : 5f;
                direction = args.Length > 3 ? (PotatoAnimation.Time.Direction)Enum.Parse(typeof(PotatoAnimation.Time.Direction), args[3].Trim()) : PotatoAnimation.Time.Direction.InOut;
                type = args.Length > 4 ? (PotatoAnimation.Time.Type)Enum.Parse(typeof(PotatoAnimation.Time.Type), args[4].Trim()) : PotatoAnimation.Time.Type.sine;

                // Get the light block
                IMyInteriorLight light = GridTerminalSystem.GetBlockWithName(lightName) as IMyInteriorLight;

                if (light != null)
                {
                    // Capture the initial intensity
                    initialIntensity = light.Intensity;

                    // Start the animation
                    isAnimating = true;

                    // Enable self-updating
                    Runtime.UpdateFrequency = UpdateFrequency.Update1;
                }
            }

            if (isAnimating)
            {
                // Get the light block again
                IMyInteriorLight light = GridTerminalSystem.GetBlockWithName(lightName) as IMyInteriorLight;

                if (light != null)
                {

                    // Update the light intensity
                    light.Intensity = PotatoAnimation.Time.Animate(currentTime, maxTime, initialIntensity, targetIntensity, direction, type);
                    Echo("Intensity: " + light.Intensity);

                    // Update the current time
                    currentTime += 0.1f;

                    // Reset the timer and stop animating if it reaches maxTime
                    if (currentTime >= maxTime)
                    {
                        currentTime = 0f;
                        isAnimating = false;

                        // Disable self-updating
                        Runtime.UpdateFrequency = UpdateFrequency.None;
                    }
                }
            }
        }
    }
}
