using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace group_18_assignment7;

public class Bullet : Entity
{
    public Vector2 Velocity;
    public Texture2D Sprite;
    public float LifeTime = 2.0f; // seconds

    public override void Update(float dt)
    {
        Position += Velocity * dt;
        LifeTime -= dt;

    }
    

}