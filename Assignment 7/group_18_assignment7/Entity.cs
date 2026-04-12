using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_18_assignment7;

public class Entity
{
    public Vector2 Position;
    public Texture2D Sprite;
    public float Speed;
    public int Health;

    public Entity(Vector2 pos, Texture2D sprite, float speed, int health)
    {
        Position = pos;
        Sprite = sprite;
        Speed = speed;
        Health = health;
    }

    public virtual void Update(float dt) {}    
    public virtual void Draw(SpriteBatch spriteBatch) {}
    
}