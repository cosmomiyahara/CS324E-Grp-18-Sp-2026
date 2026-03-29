using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A6;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private MouseState _lastMouseState;
    private KeyboardState _lastKeyboardState;
    private Texture2D _vineTexture;
    private Texture2D _background;
    private Texture2D _background1;
    private List<Vine> _vines;
    private Vector2 _gravity;
    private Color _backGroundColor;
    private bool _addingVines = false;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _gravity = new Vector2(0f, 0.5f);
        _vines = new List<Vine>();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _vineTexture =  Content.Load<Texture2D>("Vine");
        _background = Content.Load<Texture2D>("Background");
        _background1 = Content.Load<Texture2D>("Background1");
        _backGroundColor = Color.White;

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        MouseState mouseState = Mouse.GetState();
        KeyboardState keyboardState = Keyboard.GetState();



        if (keyboardState.IsKeyDown(Keys.V) && _lastKeyboardState.IsKeyUp(Keys.V))
        {
            _addingVines = ! _addingVines;
            if (_addingVines)
            {
                _backGroundColor = Color.Gray;
            }
            else
            {
                _backGroundColor = Color.White;
            }
        }
        if ((mouseState.LeftButton == ButtonState.Pressed) && (_lastMouseState.LeftButton == ButtonState.Released) && (mouseState.Y < 235) && (_addingVines))
        {
            _vines.Add(new Vine(_vineTexture, new  Vector2(mouseState.X, mouseState.Y), _gravity, 1.0f));
            Console.WriteLine("active");
        }
        else if ((mouseState.LeftButton == ButtonState.Pressed) && (_lastMouseState.LeftButton == ButtonState.Released))
        {
            for (int i = 0; _vines.Count > i; i++)
            {
                _vines[i].LinkCheck();
            }
        }
        if ((mouseState.LeftButton == ButtonState.Released) && (_lastMouseState.LeftButton == ButtonState.Pressed))
        {
            for (int i = 0; _vines.Count > i; i++)
            {
                _vines[i].LinkCheck();
            }
        }
        
        
        for (int i = 0; i < _vines.Count; i++)
        {
            _vines[i].Update(gameTime);
        }

        _lastKeyboardState = keyboardState;
        _lastMouseState = mouseState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        _spriteBatch.Draw(_background, Vector2.Zero, _backGroundColor);
        _spriteBatch.End();
        for (int i = 0; i < _vines.Count; i++)
        {
            _vines[i].Draw(_spriteBatch);
        }
        if (!_addingVines)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background1, Vector2.Zero, _backGroundColor);
            _spriteBatch.End();
        }
        base.Draw(gameTime);
    }
}