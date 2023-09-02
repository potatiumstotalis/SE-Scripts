// ||||||||||||||||||Easing Functions:||||||||||||||||||

using SE_Potato_Libraries;  // Include the namespace where your utility class is defined
using static SE_Potato_Libraries.Easing.EasingType;  // Import EasingType enum
using static SE_Potato_Libraries.Easing.EasingDirection;  // Import EasingDirection enum

// ...

public class Program : MyGridProgram
{
    public void Main(string argument)
    {
        float t = 0.5;  // Current time
        float b = 0;    // Start value
        float c = 1;    // Change in value
        float d = 1;    // Duration

        float easedValue = EasingUtility.Ease(Quadratic, In, t, b, c, d);
        // Now you can use Quadratic and In directly
    }
}


// ||||||||||||||||||Animations:||||||||||||||||||