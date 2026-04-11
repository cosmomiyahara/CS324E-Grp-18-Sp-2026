using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_18_assignment7;

public class Entity
{
    public Vector2 Position;
    public Texture2D Sprite;
    public float Speed;

    public virtual void Update(float dt) {}    
    public virtual void Draw(SpriteBatch spriteBatch) {}
}