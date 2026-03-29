using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A6;

public class VineLink
{

    public Vector2 Position;
    public Vector2 Velocity;
    public float Rotation;
    public Rectangle Bounds;
    private float _weight;
    public bool Clicked = false;
    private Texture2D _sprite;
    
    public VineLink(Vector2 position, float rotation,  float weight, Texture2D sprite)
    {
        Position = position;
        Rotation = rotation;
        _weight = weight;
        _sprite = sprite;
        Bounds = new Rectangle((int)position.X - (_sprite.Width/2), (int)position.Y - (_sprite.Height/2), _sprite.Width, _sprite.Height);
    }
    
    public void ApplyForce(Vector2 force)
    {
        Vector2 acceleration = force / _weight;
        Velocity += acceleration;
    }
    
    
    public void ApplySpringForce(Vector2 restingPosition, float ks, float kd)
    {
        Vector2 displacement = Position - restingPosition;
        Vector2 springForce = -(ks * displacement) - (kd * Velocity);
        ApplyForce(springForce);
    }
    
    public void Update(GameTime gameTime)
    {
        Bounds = new Rectangle((int)Position.X - (_sprite.Width/2), (int)Position.Y - (_sprite.Height/2), _sprite.Width, _sprite.Height);
        if (!Clicked)
        {
            Position += (Velocity * gameTime.ElapsedGameTime.Milliseconds) / 4;
        }
        else
        {
            MouseState mousePosition = Mouse.GetState();
            Position = new Vector2(mousePosition.X, mousePosition.Y);
        }
    }
    
}