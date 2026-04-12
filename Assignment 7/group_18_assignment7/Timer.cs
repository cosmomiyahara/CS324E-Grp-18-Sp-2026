using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace group_18_assignment7;

public class Timer
{
    public float TimeElapsed;
    public bool IsPaused;
    public float TimeScale = 1.0f;
    private float _threshHold;

    public Timer(float threshHold)
    {
        _threshHold = threshHold;
    }
    
    public bool Update(float dt)
    {
        if (!IsPaused)
        {
            TimeElapsed += dt;
            if (TimeElapsed > _threshHold && _threshHold > -1)
            {
                TimeElapsed -= _threshHold;
                return true;
                
            }
        }
        return false;
    }
}