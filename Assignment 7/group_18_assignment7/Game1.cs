using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_18_assignment7;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    //FONT
    private String s;
    private SpriteFont _font;
    //TIMER
    private Timer _timer;
    private KeyboardState _prevKb;
    public float TimeScale = 1.0f;
    private bool _slowMo = false;
    //BULLET
    private List<Bullet> _bullets = new List<Bullet>();
    private MouseState _prevMouse;
    private Texture2D _bulletTexture;
    //PLAYER
    private Vector2 _playerPosition;
    private float _playerSpeed = 200f;
    private Player _player;
    private Texture2D _playerTexture;
    //BARRIERS
    private List<Barrier> _barriers;
    private Texture2D _barrierTexture;
    

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _timer = new Timer(-1f);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("fontt");
        _bulletTexture = Content.Load<Texture2D>("bullet");
        _playerTexture = Content.Load<Texture2D>("playeranims");
        _barrierTexture = Content.Load<Texture2D>("InkBlot");
        _player = new Player(_playerPosition, _playerTexture,  _playerSpeed, 20);
        _player.Position = new Vector2(200, 200);
        _barriers = new List<Barrier>();

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;

        KeyboardState kb = Keyboard.GetState();
        _player.Update(dt);

        if (kb.IsKeyDown(Keys.P) && _prevKb.IsKeyUp(Keys.P))
        {
            _timer.IsPaused = !_timer.IsPaused;
            _player.AnimationTimer.IsPaused = !_player.AnimationTimer.IsPaused;
        }

        if ((kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) && _player.InkCounter > 0 && !_timer.IsPaused)
        {
            TimeScale = 0.2f; // slow motion
            _slowMo = true;
        }
        else
        {
            TimeScale = 1.0f;
            _slowMo = false;
        }

        MouseState mouse = Mouse.GetState();

        if (mouse.LeftButton == ButtonState.Pressed &&
            _prevMouse.LeftButton == ButtonState.Released)
        {
            Vector2 mousePos = new Vector2(mouse.X, mouse.Y);

            if (!_slowMo && !_timer.IsPaused)
            {
                Vector2 direction = mousePos - _player.Position;

                if (direction != Vector2.Zero)
                    direction.Normalize();

                Bullet b = _player.Shoot(direction, _bulletTexture);
                _bullets.Add(b);
            }
            else if (!_timer.IsPaused)
            {
                Barrier b = new Barrier(_barrierTexture, mousePos, 1f, _player);
                _barriers.Add(b);
                
            }
        }

        _prevMouse = mouse;
        _prevKb = kb;
        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            _bullets[i].Update(dt);

            if (_bullets[i].LifeTime <= 0)
            {
                _bullets.RemoveAt(i);
            }
        }

        foreach (var b in _barriers)
        {
            b.Update(gameTime);
        }
        
        
        _timer.Update(dt);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_font, _timer.TimeElapsed.ToString("0.00"), new Vector2(10, 10), Color.White);
        _spriteBatch.DrawString(_font, (_player.InkCounter/10).ToString(), new Vector2(140, 10), Color.White);
        foreach (var b in _bullets)
        {
            _spriteBatch.Draw(
                b.Sprite,
                b.Position,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                0.01f,
                SpriteEffects.None,
                0f
            );
        }
        _spriteBatch.End();
        _player.Draw(_spriteBatch);
        foreach (var b in _barriers)
        {
            b.Draw(_spriteBatch);

        }

        base.Draw(gameTime);
    }
}