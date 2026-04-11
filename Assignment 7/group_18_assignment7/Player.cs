using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_18_assignment7;

public class Player : Entity
{
        public float Speed = 200f;

        public override void Update(float dt)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Right))
                Position.X += Speed * dt;

            if (kb.IsKeyDown(Keys.Left))
                Position.X -= Speed * dt;

            if (kb.IsKeyDown(Keys.Up))
                Position.Y -= Speed * dt;

            if (kb.IsKeyDown(Keys.Down))
                Position.Y += Speed * dt;
        }

        public Bullet Shoot(Vector2 direction, Texture2D bulletTexture)
        {
            Bullet b = new Bullet();
            b.Position = Position;
            b.Velocity = direction * 500f;
            b.Sprite = bulletTexture;
            return b;
        }
}
