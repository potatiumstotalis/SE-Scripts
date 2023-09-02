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
using static IngameScript.Program.Potato_Utilities.Animation;
using static IngameScript.Program.Potato_Utilities.Sequence;

namespace IngameScript
{
    partial class Program
    {
        public class Potato_Utilities
        {

            public class Animation
            {
                public enum AnimationType
                {
                    time,
                    distance
                }
                public enum EasingType
                {
                    linear,
                    quadratic,
                    cubic,
                    quartic,
                    quintic,
                    sine
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
                    double change = endValue - startValue;
                    if (endValue < startValue)
                    {
                        change = startValue - endValue;
                    }
                    return Ease(easingType, easingDirection, currentTime, startValue, change, duration);
                }
                public static double AnimateDistance(double startValue, double endValue, double currentPosition, EasingType easingType, EasingDirection easingDirection)
                {
                    double change = endValue - startValue;
                    if (endValue < startValue)
                    {
                        change = startValue - endValue;
                    }
                    double distance = Math.Abs(endValue - currentPosition);
                    return Ease(easingType, easingDirection, currentPosition, startValue, change, distance);
                }




                public static double Ease(EasingType type, EasingDirection direction, double t, double b, double c, double d)
                {
                    switch (type)
                    {
                        case EasingType.linear:
                            return LinearEase(direction, t, b, c, d);
                        case EasingType.quadratic:
                            return QuadraticEase(direction, t, b, c, d);
                        case EasingType.cubic:
                            return CubicEase(direction, t, b, c, d);
                        case EasingType.quartic:
                            return QuarticEase(direction, t, b, c, d);
                        case EasingType.quintic:
                            return QuinticEase(direction, t, b, c, d);
                        case EasingType.sine:
                            return SineEase(direction, t, b, c, d);
                        default:
                            throw new ArgumentException("Invalid easing type");
                    }
                }
                private static double LinearEase(EasingDirection direction, double t, double b, double c, double d)
                {
                    return c * t / d + b;
                }
                private static double QuadraticEase(EasingDirection direction, double t, double b, double c, double d)
                {
                    t /= d;
                    if (direction == EasingDirection.ezin)
                    {
                        return c * t * t + b;
                    }
                    else
                    {
                        return -c * t * (t - 2) + b;
                    }
                }
                private static double CubicEase(EasingDirection direction, double t, double b, double c, double d)
                {
                    t /= d;
                    if (direction == EasingDirection.ezin)
                    {
                        return c * t * t * t + b;
                    }
                    else
                    {
                        t--;
                        return c * (t * t * t + 1) + b;
                    }
                }
                private static double QuarticEase(EasingDirection direction, double t, double b, double c, double d)
                {
                    t /= d;
                    if (direction == EasingDirection.ezin)
                    {
                        return c * t * t * t * t + b;
                    }
                    else
                    {
                        t--;
                        return -c * (t * t * t * t - 1) + b;
                    }
                }
                private static double QuinticEase(EasingDirection direction, double t, double b, double c, double d)
                {
                    t /= d;
                    if (direction == EasingDirection.ezin)
                    {
                        return c * t * t * t * t * t + b;
                    }
                    else
                    {
                        t--;
                        return c * (t * t * t * t * t + 1) + b;
                    }
                }
                private static double SineEase(EasingDirection direction, double t, double b, double c, double d)
                {
                    if (direction == EasingDirection.ezin)
                    {
                        return -c * Math.Cos(t / d * (Math.PI / 2)) + c + b;
                    }
                    else
                    {
                        return c * Math.Sin(t / d * (Math.PI / 2)) + b;
                    }
                }
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
