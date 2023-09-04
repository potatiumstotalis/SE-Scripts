using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
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
        //ACTUAL SCRIPT STARTS HERE
        P_Animation.Time.Direction defaultEasingDirection = P_Animation.Time.Direction.InOut;
        P_Animation.Time.Type defaultEasingType = P_Animation.Time.Type.cubic;

        List<AnimationSection> animations = new List<AnimationSection>();
        static Dictionary<string, IMyLightingBlock> blockDictionary = new Dictionary<string, IMyLightingBlock>();
        int animationCount = 0;

        bool sequenceisRunning = false;

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.None; // No automatic updates
        }
        public void Main(string argument, UpdateType updateSource)
        {
            Echo("Animation Count: " + animationCount);
            if ((updateSource & UpdateType.Terminal) != 0)
            {

                if (argument.ToLower() == "start")
                {
                    animationCount = 0;
                    sequenceisRunning = true;
                    Runtime.UpdateFrequency = UpdateFrequency.Update1;
                }
                else if (argument.ToLower() == "set")
                {
                    LoadData();
                }
            }

            if (sequenceisRunning)
            {
                for (int i = 0; i < animations.Count; i++)
                {
                    Echo($"Animation {i}: IsFinished = {animations[i].IsFinished()}, Trigger = {animations[i].Trigger()}");
                    if (i == 0) { if (!animations[i].IsFinished()) { animations[i].Animate(true); } else { animations[i].Animate(false); } }
                    else 
                    {
                        if (animations[i - 1].Trigger())
                        {
                            animations[i].Animate(true);
                        }
                        else
                        {
                            animations[i].Animate(false);
                        }
                    }

                }
            }
        }

        public void LoadData()
        {
            animations.Clear();
            blockDictionary.Clear();

            string customData = Me.CustomData;
            string[] lines = customData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length >= 2)
                {
                    string blockName = parts[0].Trim();
                    float targetValue = parts.Length > 1 ? float.Parse(parts[1].Trim()) : 5f;
                    float targetTime = parts.Length > 2 ? float.Parse(parts[2].Trim()) : 10f;
                    float transitionTime = parts.Length > 3 ? float.Parse(parts[3].Trim()) : 8f;
                    P_Animation.Time.Direction easingDirection = parts.Length > 4 ? (P_Animation.Time.Direction)Enum.Parse(typeof(P_Animation.Time.Direction), parts[4].Trim()) : defaultEasingDirection; // Default value
                    P_Animation.Time.Type easingType = parts.Length > 5 ? (P_Animation.Time.Type)Enum.Parse(typeof(P_Animation.Time.Type), parts[5].Trim()) : defaultEasingType; // Default value
                    string blockParameter = parts.Length > 6 ? parts[6].Trim() : "intensity";  // Default Value

                    AnimationSection animation = new AnimationSection(blockName, targetValue, targetTime, transitionTime, easingDirection, easingType, blockParameter);
                    animations.Add(animation);

                    if (!blockDictionary.ContainsKey(blockName))
                    {
                        IMyLightingBlock block = GridTerminalSystem.GetBlockWithName(blockName) as IMyLightingBlock;
                        if (block != null)
                        {
                            blockDictionary[blockName] = block;
                        }
                        else
                        {
                            Echo(blockName + " not found.");
                        }
                    }
                }
            }
        }

        public void Save()
        {
            // Save state here
        }


        public class AnimationSection
        {
            public string BlockName { get; set; }
            public float TargetIntensity { get; set; }
            public float TargetTime { get; set; }
            public float TransitionTime { get; set; }
            public P_Animation.Time.Direction EasingDirection { get; set; }
            public P_Animation.Time.Type EasingType { get; set; }
            public string BlockParameter { get; set; }

            float initialIntensity = 0f;
            bool isAnimating = false;
            bool transitionTrigger = false;
            bool isFinished = false;
            float currentTime = 0f;

            Program p;

            public AnimationSection(string blockName, float targetIntensity, float targetTime, float transitionTime, P_Animation.Time.Direction easingDirection, P_Animation.Time.Type easingType, string blockParameter)
            {
                BlockName = blockName;
                TargetIntensity = targetIntensity;
                TargetTime = targetTime;
                TransitionTime = transitionTime;
                EasingDirection = easingDirection;
                EasingType = easingType;
                BlockParameter = blockParameter;
            }

            public void Animate(bool animationTrigger)
            {
                IMyLightingBlock light = blockDictionary[BlockName];

                if (light != null && animationTrigger)
                {
                    // Capture the initial intensity
                    initialIntensity = light.Intensity;

                    // Start the animation
                    isAnimating = true;
                    isFinished = false;
                }

                if (light != null && isAnimating)
                {
                    if (BlockParameter.ToLower() == "intensity")
                    {
                        light.Intensity = P_Animation.Time.Animate(currentTime, TargetTime, initialIntensity, TargetIntensity, EasingDirection, EasingType);
                    }
                    else if (BlockParameter.ToLower() == "radius")
                    {
                        light.Radius = P_Animation.Time.Animate(currentTime, TargetTime, initialIntensity, TargetIntensity, EasingDirection, EasingType);
                    }

                    // Update the current time
                    currentTime += 0.1f;
                }

                //Trigger the next animation on the exact time
                if (currentTime >= TransitionTime)
                {
                    transitionTrigger = true;
                }
                else
                {
                    transitionTrigger = false;
                }

                // Reset the timer and stop animating if it reaches maxTime
                if (currentTime >= TargetTime)
                {
                    currentTime = 0f;
                    isAnimating = false;
                    isFinished = true;
                }
            }

            public bool Trigger()
            {
                p.Echo($"Animation {BlockName}: currentTime = {currentTime}, TransitionTime = {TransitionTime}");
                return transitionTrigger;
            }

            public bool IsFinished()
            {
                return isFinished; 
            }
        }
    }
}
