using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
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
using static IngameScript.Program.Animation;

namespace IngameScript
{
    partial class Program
    {
        public static class Animation
        {

            //How to use -> Animation.Time.Animate(currentTime, maxTime, startValue, endValue, ease_direction, ease_type);
            public static class Time
            {
                public enum Direction { In, Out, InOut }
                public enum Type { linear, quad, cubic, quart, quint, sine, expo, circ }


                //Choose Easing Function
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

                //Return the Final value (ANIMATE)
                public static float nTime;
                public static float easedTime;
                public static float nValue;
                public static float finalValue;

                public static float Animate(float time, float maxTime, float startValue, float endValue, Direction easedir, Type easetype)
                {
                    nTime = time / maxTime;
                    easedTime = Ease(nTime, easedir, easetype);

                    if (startValue < endValue)
                    {
                        nValue = endValue - startValue;
                        finalValue = (easedTime * nValue) + startValue;
                    }
                    else
                    {
                        nValue = startValue - endValue;
                        finalValue = ((-(easedTime - 1) * nValue) + endValue);
                    }
                    return finalValue;
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

            //How to use: -> Animation.Movement.Animate(currPosition, maxVelocity, startValue, endValue, moveFactor, ease_direction, ease_type);
            public static class Movement
            {
                public enum Direction { In, Out, InOut }
                public enum Type { linear, quad, cubic, quart, quint, sine, expo, circ }


                //Choose Easing Function
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

                //Return the Final value (ANIMATE)
                public static float nVelocity;
                public static float easedRatio;
                public static float nValue;
                public static float finalValue;
                public static float distance;
                public static float movementAdder;
                public static float currentRatio;

                public static float Animate(float currPosition, float maxVelocity, float startValue, float endValue, float moveFactor, Direction easedir, Type easetype)
                {
                    currentRatio = 0;
                    nVelocity = easedRatio * maxVelocity;

                    if (startValue < endValue)
                    {
                        if (currPosition > (endValue - 0.01f))
                        {
                            finalValue = 0;
                            currentRatio = 0;
                            easedRatio = 0;
                        }
                        else
                        {
                            distance = endValue - startValue;
                            if (currPosition < ((distance / 2) + startValue) && currentRatio < 0.5f)
                            {
                                currentRatio = ((currPosition - startValue) / distance) + moveFactor;
                            }
                            else if (currPosition > ((distance / 2) + startValue) && currentRatio < 0.5f)
                            {
                                currentRatio = ((endValue - currPosition) / distance) - moveFactor;
                            }
                            else
                            {
                                currentRatio = 0.5f;
                            }

                            easedRatio = Ease(currentRatio, easedir, easetype);
                            finalValue = (easedRatio * 2) * maxVelocity;
                        }
                    }
                    else
                    {
                        if (currPosition < (endValue + 0.01f))
                        {
                            finalValue = 0;
                            currentRatio = 0;
                            easedRatio = 0;
                        }
                        else
                        {
                            distance = startValue - endValue;
                            if (currPosition > ((distance / 2) + endValue) && currentRatio < 0.5f)
                            {
                                currentRatio = -(((currPosition - startValue) / distance) - moveFactor);
                            }
                            else if (currPosition < ((distance / 2) + endValue) && currentRatio < 0.5f)
                            {
                                currentRatio = -(((endValue - currPosition) / distance) + moveFactor);
                            }
                            else
                            {
                                currentRatio = 0.5f;
                            }

                            easedRatio = Ease(currentRatio, easedir, easetype);
                            finalValue = -((easedRatio * 2) * maxVelocity);
                        }
                    }

                    return finalValue;
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
