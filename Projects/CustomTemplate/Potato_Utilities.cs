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
            public static class Easing
            {
                public enum EasingType
                {
                    Linear,
                    Quadratic,
                    Cubic,
                    Quartic,
                    Quintic,
                    Sine
                }

                public enum EasingDirection
                {
                    In,
                    Out
                }

                public static double Ease(EasingType type, EasingDirection direction, double t, double b, double c, double d)
                {
                    switch (type)
                    {
                        case EasingType.Linear:
                            return LinearEase(direction, t, b, c, d);
                        case EasingType.Quadratic:
                            return QuadraticEase(direction, t, b, c, d);
                        case EasingType.Cubic:
                            return CubicEase(direction, t, b, c, d);
                        case EasingType.Quartic:
                            return QuarticEase(direction, t, b, c, d);
                        case EasingType.Quintic:
                            return QuinticEase(direction, t, b, c, d);
                        case EasingType.Sine:
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
                    if (direction == EasingDirection.In)
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
                    if (direction == EasingDirection.In)
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
                    if (direction == EasingDirection.In)
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
                    if (direction == EasingDirection.In)
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
                    if (direction == EasingDirection.In)
                    {
                        return -c * Math.Cos(t / d * (Math.PI / 2)) + c + b;
                    }
                    else
                    {
                        return c * Math.Sin(t / d * (Math.PI / 2)) + b;
                    }
                }
            }
        }

        public enum SimpleEasingType
        {
            Linear,
            Quadratic,
            Cubic,
            Quartic,
            Quintic,
            Sine
        }

        public enum SimpleEasingDirection
        {
            In,
            Out
        }

        public double SimpleEase(SimpleEasingType type, SimpleEasingDirection direction, double t, double b, double c, double d)
        {
            return Potato_Utilities.Easing.Ease((Potato_Utilities.Easing.EasingType)type, (Potato_Utilities.Easing.EasingDirection)direction, t, b, c, d);
        }

        public void Main(string argument)
        {
            double t = 0.5;  // Current time
            double b = 0;    // Start value
            double c = 1;    // Change in value
            double d = 1;    // Duration

            double easedValue = SimpleEase(SimpleEasingType.Quadratic, SimpleEasingDirection.In, t, b, c, d);
        }
    }
}
