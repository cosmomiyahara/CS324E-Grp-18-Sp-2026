using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace group_18_assignment7;

public class Bullet : Entity
{
    public Vector2 Velocity;
    public Bullet(Vector2 pos, Texture2D sprite, float speed, int health, Vector2 direction) : base(pos, sprite, speed, health)
    {
        Velocity = speed*direction;
    }
    
    public float LifeTime = 2.0f; // seconds

    public override void Update(float dt)
    {
        Position += Velocity * dt;
        LifeTime -= dt;

    }
    

}