using Microsoft.Xna.Framework;

namespace group_18_assignment7;

public class Timer
{
    public float TimeElapsed;
    public bool IsPaused;
    public float TimeScale = 1.0f;
    
    public void Update(float dt)
    {
        if (!IsPaused)
        {
            TimeElapsed += dt;
        }
    }
}