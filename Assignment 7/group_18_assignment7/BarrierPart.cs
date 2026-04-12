using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_18_assignment7;

public class BarrierPart : Entity
{


    public float Rotation;
    public Rectangle Bounds;

    public BarrierPart(Vector2 pos, Texture2D sprite, float speed, int health, float rotation) : base(pos, sprite, speed, health)
    {
        Position = pos;
        Bounds = new Rectangle((int)Position.X - (Sprite.Width/2), (int)Position.Y - (Sprite.Height/2), Sprite.Width, Sprite.Height);
        Rotation = rotation;
        
    }
}