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
        /* How to Use:
         * 
         * Run PB with arguments: pistonName, targetPosition, maxVelocity, moveFactor, direction, type
         * 
         * Example: Piston1,2,5,0.05,InOut,sine
         * 
         */

        //ACTUAL SCRIPT STARTS HERE
        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.None;
        }

        public void Save()
        {
        }

        string pistonName;
        float maxVelocity;
        float targetPosition;
        float moveFactor;
        P_Animation.Movement.Direction direction;
        P_Animation.Movement.Type type;
        float currentPosition;
        float initialPosition;
        bool isAnimating;

        public void Main(string argument, UpdateType updateSource)
        {
            if ((updateSource & UpdateType.Terminal) != 0 || (updateSource & UpdateType.Trigger) != 0 || (updateSource & UpdateType.Script) != 0)
            {
                string[] args = argument.Split(',');
                pistonName = args.Length > 0 ? args[0].Trim() : "Piston1";
                targetPosition = args.Length > 1 ? float.Parse(args[1].Trim()) : 10f;
                maxVelocity = args.Length > 2 ? float.Parse(args[2].Trim()) : 5f;
                moveFactor = args.Length > 3 ? float.Parse(args[3].Trim()) : 0.05f;
                direction = args.Length > 4 ? (P_Animation.Movement.Direction)Enum.Parse(typeof(P_Animation.Movement.Direction), args[4].Trim()) : P_Animation.Movement.Direction.InOut;
                type = args.Length > 5 ? (P_Animation.Movement.Type)Enum.Parse(typeof(P_Animation.Movement.Type), args[5].Trim()) : P_Animation.Movement.Type.sine;

                IMyPistonBase piston = GridTerminalSystem.GetBlockWithName(pistonName) as IMyPistonBase;
                if (piston != null)
                {
                    initialPosition = piston.CurrentPosition;
                    isAnimating = true;
                    Runtime.UpdateFrequency = UpdateFrequency.Update1;
                }
            }

            if (isAnimating)
            {
                IMyPistonBase piston = GridTerminalSystem.GetBlockWithName(pistonName) as IMyPistonBase;
                if (piston != null)
                {
                    currentPosition = piston.CurrentPosition;
                    piston.Velocity = P_Animation.Movement.Animate(currentPosition, maxVelocity, initialPosition, targetPosition, moveFactor, direction, type);
                    Echo("Current Position: " + piston.CurrentPosition);
                    Echo("Velocity: " + piston.Velocity);
                }
            }
        }
    }
}