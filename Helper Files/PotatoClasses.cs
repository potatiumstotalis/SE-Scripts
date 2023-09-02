using System;

namespace SE_Potato_Libraries
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

        public static float Ease(EasingType type, EasingDirection direction, float t, float b, float c, float d)
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

        private static float LinearEase(EasingDirection direction, float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        private static float QuadraticEase(EasingDirection direction, float t, float b, float c, float d)
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

        private static float CubicEase(EasingDirection direction, float t, float b, float c, float d)
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

        private static float QuarticEase(EasingDirection direction, float t, float b, float c, float d)
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

        private static float QuinticEase(EasingDirection direction, float t, float b, float c, float d)
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

        private static float SineEase(EasingDirection direction, float t, float b, float c, float d)
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
