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
        public class Potato_Utilities
        {
            public class Animation
            {
                // Fields and methods for managing a sequence of Animation_Parts
            }

            public class Animation_Part
            {
                public enum PartType
                {
                    EaseIn,
                    EaseOut,
                    Hold
                }

                public enum AnimationType
                {
                    time,
                    distance
                }

                public enum EasingType
                {
                    linear,
                    sine,
                    half,
                    cubic
                }

                public enum EasingDirection
                {
                    ezin,
                    ezout
                }

                public double Animate(AnimationType animationType, EasingType easingType, EasingDirection easingDirection, double startValue, double endValue, double variable1, double variable2)
                {
                    switch (animationType)
                    {
                        case AnimationType.time:
                            return AnimateTime(startValue, endValue, variable1, variable2, easingType, easingDirection);
                        case AnimationType.distance:
                            return AnimateDistance(startValue, endValue, variable1, easingType, easingDirection);
                        default:
                            throw new ArgumentException("Invalid animation type");
                    }
                }

                public static double AnimateTime(double startValue, double endValue, double currentTime, double duration, EasingType easingType, EasingDirection easingDirection)
                {
                    double difference = endValue - startValue; // Automatically handles direction
                    return Ease(easingType, currentTime, startValue, difference, duration);
                }

                public static double AnimateDistance(double startValue, double endValue, double currentPosition, EasingType easingType, EasingDirection easingDirection)
                {
                    double difference = endValue - startValue; // Automatically handles direction
                    double target = Math.Abs(endValue - currentPosition);
                    return Ease(easingType, currentPosition, startValue, difference, target);
                }



                // Easing Variable Explanations:
                /* 
                 * c: Current Time/Position — The current time or position of the animation.
                 * s: Starting Value — The initial value of the property being animated.
                 * d: Difference — The change in the property value, calculated as the ending value minus the starting value.
                 * t: Target Duration/Position — The total time or target position the animation will take or reach.
                 */

                public static double Ease(EasingType type, double c, double s, double d, double t)
                {
                    return type == EasingType.linear ? LinearEase(c, s, d, t) :
                           type == EasingType.sine ? SineEase(c, s, d, t) :
                           type == EasingType.half ? HalfEase(c, s, d, t) :
                           type == EasingType.cubic ? CubicEase(c, s, d, t) :
                           0; // Default case, can also throw an exception if you prefer
                }

                private static double LinearEase(double c, double s, double d, double t) => d * c / t + s;
                private static double SineEase(double c, double s, double d, double t) => d * Math.Sin(c / t * (Math.PI / 2)) + s;
                private static double HalfEase(double c, double s, double d, double t) => d * Math.Sqrt(c / t) + s;
                private static double CubicEase(double c, double s, double d, double t) => d * (c /= t) * c * c + s;

            }

            public class Sequence
            {
                private Queue<Action> animationQueue = new Queue<Action>();

                public void AddToQueue(Action animation)
                {
                    animationQueue.Enqueue(animation);
                }

                public void Execute()
                {
                    while (animationQueue.Count > 0)
                    {
                        Action animation = animationQueue.Dequeue();
                        animation();
                    }
                }
            }
        }
    }
}
