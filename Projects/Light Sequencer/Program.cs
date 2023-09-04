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

namespace Light_Sequencer
{
    partial class Program : MyGridProgram
    {
        //Defaults
        string defaultEasingDirection = "InOut";
        string defaultEasingType = "cubic";
        //ACTUAL SCRIPT STARTS HERE
        public Program()
        {
            // Initialize self-updating
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
        }

        public void Main(string argument, UpdateType updateSource)
        {
            LoadData();
        }

        public void LoadData()
        {
            // Initialize a dictionary to hold BlockName and corresponding IMyLightingBlock
            Dictionary<string, IMyLightingBlock> blockDictionary = new Dictionary<string, IMyLightingBlock>();

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
                        string easingDirection = parts.Length > 3 ? parts[3].Trim() : defaultEasingDirection; // Default value
                        string easingType = parts.Length > 4 ? parts[4].Trim() : defaultEasingType; // Default value

                        AnimationSection animation_s = new AnimationSection(blockName, targetIntensity, targetTime, easingDirection, easingType);
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
        }

        public class AnimationSection
        {
            public string BlockName { get; set; }
            public float TargetIntensity { get; set; }
            public float TargetTime { get; set; }
            public string EasingDirection { get; set; }
            public string EasingType { get; set; }

            public AnimationSection(string blockName, float targetIntensity, float targetTime, string easingDirection, string easingType)
            {
                BlockName = blockName;
                TargetIntensity = targetIntensity;
                TargetTime = targetTime;
                EasingDirection = easingDirection ?? "InOut";
                EasingType = easingType ?? "cubic";
            }
        }


    }
}
