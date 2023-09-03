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
    partial class Program
    {
        public class PotatoAnimation
        {
            public static class Sequence
            {
                //Write methods for sequencing/concatenating animations
            }

            //Easing: AnimateTime.Ease(value, startValue, endValue, direction, type)
            public static class Time
            {
                public enum Direction { In, Out, InOut }
                public enum Type { linear, quad, cubic, quart, quint, sine, expo, circ }


                //Normalize from startValue-endValue to 0-1
                public static float Normalize(float value, float startValue, float endValue)
                {
                    // Check if startValue and endValue are the same to avoid division by zero
                    if (startValue == endValue)
                    {
                        throw new ArgumentException("startValue and endValue cannot be the same.");
                    }

                    // Determine the direction
                    bool isPositiveDirection = endValue > startValue;

                    // Normalize the value based on the direction
                    float normalizedValue;
                    if (isPositiveDirection)
                    {
                        normalizedValue = (value - startValue) / (endValue - startValue);
                    }
                    else
                    {
                        normalizedValue = (startValue - value) / (startValue - endValue);
                    }

                    return normalizedValue;
                }

                //Denormalize from 0-1 to startValue-endValue
                public static float Denormalize(float normalizedValue, float startValue, float endValue)
                {
                    // Check if the normalizedValue is out of the 0-1 range
                    if (normalizedValue < 0 || normalizedValue > 1)
                    {
                        throw new ArgumentException("normalizedValue must be between 0 and 1.");
                    }

                    // Determine the direction
                    bool isPositiveDirection = endValue > startValue;

                    // Denormalize the value based on the direction
                    float originalValue;
                    if (isPositiveDirection)
                    {
                        originalValue = startValue + normalizedValue * (endValue - startValue);
                    }
                    else
                    {
                        originalValue = startValue - normalizedValue * (startValue - endValue);
                    }

                    return originalValue;
                }
                
                //Ease the Normalized value
                public static float Ease(float t, Direction direction, Type type)
                {
                    switch (type)
                    {
                        case Type.linear:
                            return Linear(t);
                        case Type.quad:
                            return direction == Direction.In ? InQuad(t) : direction == Direction.Out ? OutQuad(t) : InOutQuad(t);
                        case Type.cubic:
                            return direction == Direction.In ? InCubic(t) : direction == Direction.Out ? OutCubic(t) : InOutCubic(t);
                        case Type.quart:
                            return direction == Direction.In ? InQuart(t) : direction == Direction.Out ? OutQuart(t) : InOutQuart(t);
                        case Type.quint:
                            return direction == Direction.In ? InQuint(t) : direction == Direction.Out ? OutQuint(t) : InOutQuint(t);
                        case Type.sine:
                            return direction == Direction.In ? InSine(t) : direction == Direction.Out ? OutSine(t) : InOutSine(t);
                        case Type.expo:
                            return direction == Direction.In ? InExpo(t) : direction == Direction.Out ? OutExpo(t) : InOutExpo(t);
                        case Type.circ:
                            return direction == Direction.In ? InCirc(t) : direction == Direction.Out ? OutCirc(t) : InOutCirc(t);
                        default:
                            throw new ArgumentException("Invalid easing type.");
                    }
                }

                //Return the Final value
                public static float Animate(float value, float startValue, float endValue, Direction dir, Type type)
                {
                    float normalizedValue = Normalize(value, startValue, endValue);
                    float easedValue = Ease(normalizedValue, dir, type);
                    float denormalizedValue = Denormalize(easedValue, startValue, endValue);
                    return denormalizedValue;
                }

                //Easing Functions | Imported from: https://gist.github.com/Kryzarel/bba64622057f21a1d6d44879f9cd7bd4
                public static float Linear(float t) => t;

                public static float InQuad(float t) => t * t;
                public static float OutQuad(float t) => 1 - InQuad(1 - t);
                public static float InOutQuad(float t)
                {
                    if (t < 0.5) return InQuad(t * 2) / 2;
                    return 1 - InQuad((1 - t) * 2) / 2;
                }

                public static float InCubic(float t) => t * t * t;
                public static float OutCubic(float t) => 1 - InCubic(1 - t);
                public static float InOutCubic(float t)
                {
                    if (t < 0.5) return InCubic(t * 2) / 2;
                    return 1 - InCubic((1 - t) * 2) / 2;
                }

                public static float InQuart(float t) => t * t * t * t;
                public static float OutQuart(float t) => 1 - InQuart(1 - t);
                public static float InOutQuart(float t)
                {
                    if (t < 0.5) return InQuart(t * 2) / 2;
                    return 1 - InQuart((1 - t) * 2) / 2;
                }

                public static float InQuint(float t) => t * t * t * t * t;
                public static float OutQuint(float t) => 1 - InQuint(1 - t);
                public static float InOutQuint(float t)
                {
                    if (t < 0.5) return InQuint(t * 2) / 2;
                    return 1 - InQuint((1 - t) * 2) / 2;
                }

                public static float InSine(float t) => (float)-Math.Cos(t * Math.PI / 2);
                public static float OutSine(float t) => (float)Math.Sin(t * Math.PI / 2);
                public static float InOutSine(float t) => (float)(Math.Cos(t * Math.PI) - 1) / -2;

                public static float InExpo(float t) => (float)Math.Pow(2, 10 * (t - 1));
                public static float OutExpo(float t) => 1 - InExpo(1 - t);
                public static float InOutExpo(float t)
                {
                    if (t < 0.5) return InExpo(t * 2) / 2;
                    return 1 - InExpo((1 - t) * 2) / 2;
                }

                public static float InCirc(float t) => -((float)Math.Sqrt(1 - t * t) - 1);
                public static float OutCirc(float t) => 1 - InCirc(1 - t);
                public static float InOutCirc(float t)
                {
                    if (t < 0.5) return InCirc(t * 2) / 2;
                    return 1 - InCirc((1 - t) * 2) / 2;
                }
            }

        }
    }
}
