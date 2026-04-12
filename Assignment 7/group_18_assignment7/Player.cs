using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_18_assignment7;

public class Player : Entity
{
    private int _spriteWidth;
    private int _spriteHeight;
    private Rectangle _currentFrame;
    private int _frameWCounter;
    private int _frameHCounter;
    private int _startingW;
    private int _startingH;
    private int _endingW;
    private int _endingH;
    public int InkCounter = 200;
    private int _numberOfFrames = 0;
    public Timer AnimationTimer;
    private enum Animations{Idle, Walking, Drawing, Dying}
    private Animations _currentAnimation;
    private Animations _lastAnimation;
    private SpriteEffects _spriteEffects;
    
    public Player(Vector2 pos, Texture2D sprite, float speed, int health) : base(pos, sprite, speed, health)
    {
        _spriteWidth = sprite.Width/6;
        _spriteHeight = sprite.Height/6;
        AnimationTimer = new Timer(0.05f);
        _frameWCounter = 0;
        _frameWCounter = 0;
        _currentAnimation = Animations.Idle;
        _currentFrame = new Rectangle(_spriteWidth*_frameWCounter, _spriteHeight*_frameHCounter, _spriteWidth, _spriteHeight);
        
    }

        public override void Update(float dt)
        {
            if (!AnimationTimer.IsPaused)
            {
                KeyboardState kb = Keyboard.GetState();
                Vector2 direction = Vector2.Zero;

                if (kb.IsKeyDown(Keys.Right) || kb.IsKeyDown(Keys.D))
                {
                    direction.X += 1;
                    _currentAnimation = Animations.Walking;
                    _spriteEffects = SpriteEffects.None;
                }

                if (kb.IsKeyDown(Keys.Left) || kb.IsKeyDown(Keys.A))
                {
                    direction.X -= 1;
                    _currentAnimation = Animations.Walking;
                    _spriteEffects = SpriteEffects.FlipHorizontally;
                }

                if (kb.IsKeyDown(Keys.Up) || kb.IsKeyDown(Keys.W))
                {
                    direction.Y -= 1;
                    _currentAnimation = Animations.Walking;
                }

                if (kb.IsKeyDown(Keys.Down) || kb.IsKeyDown(Keys.S))
                {
                    direction.Y += 1;
                    _currentAnimation = Animations.Walking;
                }

                if (kb.IsKeyUp(Keys.Right) && kb.IsKeyUp(Keys.Left) && kb.IsKeyUp(Keys.Up) && kb.IsKeyUp(Keys.Down) &&
                    kb.IsKeyUp(Keys.D) && kb.IsKeyUp(Keys.S) && kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.W))
                {
                    _currentAnimation = Animations.Idle;
                }

                if (direction != Vector2.Zero)
                {
                    direction.Normalize();
                    Position += direction * Speed * dt;
                }


                if (AnimationTimer.Update(dt))
                {
                    switch (_currentAnimation)
                    {
                        case Animations.Idle:
                            _startingW = 0;
                            _startingH = 0;
                            _endingW = 5;
                            _endingH = 0;
                            _numberOfFrames = 6;
                            break;
                        case Animations.Walking:
                            _startingW = 0;
                            _startingH = 1;
                            _endingW = 2;
                            _endingH = 2;
                            _numberOfFrames = 9;
                            break;
                        case Animations.Drawing:
                            _startingW = 2;
                            _startingH = 3;
                            _endingW = 4;
                            _endingH = 3;
                            _numberOfFrames = 3;
                            break;
                    }

                    if (_lastAnimation != _currentAnimation)
                    {
                        _frameWCounter = _startingW;
                        _frameHCounter = _startingH;
                    }

                    _frameWCounter = (_frameWCounter + 1) % 6;
                    if (_frameWCounter == 0)
                    {
                        _frameHCounter = (_frameHCounter + 1) % 6;
                    }

                    if (_frameWCounter == _endingW && _frameHCounter == _endingH)
                    {
                        _frameWCounter = _startingW;
                        _frameHCounter = _startingH;
                    }

                    _currentFrame = new Rectangle(_spriteWidth * _frameWCounter, _spriteHeight * _frameHCounter,
                        _spriteWidth, _spriteHeight);

                    _lastAnimation = _currentAnimation;
                }
            }
        }

        public Bullet Shoot(Vector2 direction, Texture2D bulletTexture)
        {
            Bullet b = new Bullet(Position, bulletTexture, 500f, -1, direction);
            return b;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Sprite, Position, _currentFrame, Color.White, 0f,  new Vector2(_currentFrame.Width/2, _currentFrame.Height/2), 1f, _spriteEffects, 0f);
            spriteBatch.End();
        }
        
        
}
