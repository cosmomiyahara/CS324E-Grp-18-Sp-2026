using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_18_assignment7;

public class Enemy : Entity
{
    private Player _target;

    public Enemy(Vector2 pos, Texture2D sprite, float speed, int health, Player target)
        : base(pos, sprite, speed, health)
    {
        _target = target;
    }

    public override void Update(float dt)
    {
        Vector2 direction = _target.Position - Position;

        if (direction != Vector2.Zero)
        {
            direction.Normalize();
            Position += direction * Speed * dt;
        }
    }

    public Rectangle GetBounds()
    {
        int width = (int)(Sprite.Width * 0.02f);
        int height = (int)(Sprite.Height * 0.02f);

        return new Rectangle(
            (int)Position.X - width / 2,
            (int)Position.Y - height / 2,
            width,
            height
        );
    }
    public bool IsHitBy(Bullet bullet)
    {
        Rectangle enemyBounds = new Rectangle(
            (int)Position.X - (int)(Sprite.Width * 0.02f) / 2,
            (int)Position.Y - (int)(Sprite.Height * 0.02f) / 2,
            (int)(Sprite.Width * 0.02f),
            (int)(Sprite.Height * 0.02f)
        );

        Rectangle bulletBounds = new Rectangle(
            (int)bullet.Position.X,
            (int)bullet.Position.Y,
            10,
            10
        );

        return enemyBounds.Intersects(bulletBounds);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(
            Sprite,
            Position,
            null,
            Color.White,
            0f,
            new Vector2(Sprite.Width / 2, Sprite.Height / 2),
            1f,
            SpriteEffects.None,
            0f
        );
        spriteBatch.End();
    }
}