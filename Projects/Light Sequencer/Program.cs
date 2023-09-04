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
using VRage.Game.Utils;
using VRageMath;

namespace Light_Sequencer
{
    partial class Program : MyGridProgram
    {
        //Defaults
        Animation.Time.Direction defaultEasingDirection = Animation.Time.Direction.InOut;
        Animation.Time.Type defaultEasingType = Animation.Time.Type.cubic;


        //ACTUAL SCRIPT STARTS HERE
        List<Sequence> sequences = new List<Sequence>();
        static Dictionary<string, IMyLightingBlock> blockDictionary = new Dictionary<string, IMyLightingBlock>();

        public Program()
        {
            // Initialize self-updating
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if ((updateSource & UpdateType.Terminal) != 0 || (updateSource & UpdateType.Script) != 0 || (updateSource & UpdateType.Trigger) != 0 || (updateSource & UpdateType.IGC) != 0)
            {
                if (argument.ToLower() == "set") { LoadData(); }
            }
        }

        public void LoadData()
        {
            UnloadData();
            // Fetch Custom Data
            string customData = Me.CustomData;

            // Split the Custom Data into lines
            string[] lines = customData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // Initialize a list to hold Sequence instances
            List<Sequence> sequences = new List<Sequence>();

            Sequence currentSequence = null;

            foreach (string line in lines)
            {
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    // This is a SequenceName
                    currentSequence = new Sequence(line.Trim('[', ']'));
                    sequences.Add(currentSequence);
                }
                else if (currentSequence != null)
                {
                    // This is an Animation line
                    string[] parts = line.Split(',');
                    if (parts.Length >= 2)
                    {
                        string blockName = parts[0].Trim();
                        float targetIntensity = parts.Length > 1 ? float.Parse(parts[1].Trim()) : 5f; // Default value
                        float targetTime = parts.Length > 2 ? float.Parse(parts[2].Trim()) : 10f; // Default value
                        Animation.Time.Direction easingDirection = parts.Length > 3 ? (Animation.Time.Direction)Enum.Parse(typeof(Animation.Time.Direction), parts[3].Trim()) : defaultEasingDirection; // Default value
                        Animation.Time.Type easingType = parts.Length > 4 ? (Animation.Time.Type)Enum.Parse(typeof(Animation.Time.Type), parts[4].Trim()) : defaultEasingType; // Default value
                        string blockParameter = parts.Length > 5 ? parts[5].Trim() : "intensity";  // Default Value

                        AnimationSection animation_s = new AnimationSection(blockName, targetIntensity, targetTime, easingDirection, easingType, blockParameter);
                        currentSequence.Animations.Add(animation_s);

                        // Populate the blockDictionary
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
        }
        public void UnloadData()
        {
            // Clear the blockDictionary
            blockDictionary.Clear();

            // Clear the sequences list
            sequences.Clear();

            // If you have any other objects, set them to null or new instances as appropriate
            // For example:
            // someObject = null;
            // someList = new List<SomeType>();
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means.
        }

        public class Sequence
        {
            public string SequenceName { get; set; }
            public List<AnimationSection> Animations { get; set; }

            public Sequence(string sequenceName)
            {
                SequenceName = sequenceName;
                Animations = new List<AnimationSection>();
            }

            public void Animate()
            {
                for (int a = 0; a < Animations.Count; a++)
                {
                    if (a == 0)
                    {
                        Animations[a].ResetTarget();
                        Animations[a].Animate(true);
                    }
                    else
                    {
                        Animations[a].ResetTarget();
                        Animations[a].Animate(Animations[a - 1].Ready());
                    }
                }
            }
        }

        public class AnimationSection
        {
            public string BlockName { get; set; }
            public float TargetIntensity { get; set; }
            public float TargetTime { get; set; }
            public Animation.Time.Direction EasingDirection { get; set; }
            public Animation.Time.Type EasingType { get; set; }
            public string BlockParameter { get; set; }

            float initialIntensity = 0f;
            bool isAnimating = false;
            bool reachedTarget = false;
            float currentTime = 0f;

            Program p;

            public AnimationSection(string blockName, float targetIntensity, float targetTime, Animation.Time.Direction easingDirection, Animation.Time.Type easingType, string blockParameter)
            {
                BlockName = blockName;
                TargetIntensity = targetIntensity;
                TargetTime = targetTime;
                EasingDirection = easingDirection;
                EasingType = easingType;
                BlockParameter = blockParameter;
            }

            public void Animate(bool animationTrigger)
            {
                IMyLightingBlock light = blockDictionary[BlockName];

                if (light != null && animationTrigger && currentTime == 0f && !reachedTarget)
                {
                    // Capture the initial intensity
                    initialIntensity = light.Intensity;

                    // Start the animation
                    reachedTarget = false;
                    isAnimating = true;
                }

                if (light != null && isAnimating)
                {
                    if (BlockParameter.ToLower() == "intensity")
                    {
                        light.Intensity = Animation.Time.Animate(currentTime, TargetTime, initialIntensity, TargetIntensity, EasingDirection, EasingType);
                    }
                    else if (BlockParameter.ToLower() == "radius")
                    {
                        light.Radius = Animation.Time.Animate(currentTime, TargetTime, initialIntensity, TargetIntensity, EasingDirection, EasingType);
                    }

                    // Update the current time
                    currentTime += 0.1f;

                    // Reset the timer and stop animating if it reaches maxTime
                    if (currentTime >= TargetTime)
                    {
                        currentTime = 0f;
                        reachedTarget = true;
                        isAnimating = false;
                    }
                }
            }

            public bool Ready()
            {
                return reachedTarget;
            }

            public void ResetTarget()
            {
                reachedTarget = false;
            }
        }


    }
}
