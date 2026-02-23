//group 18 assignment 4
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_18_assignment4;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _scene;
    private Texture2D _fish;
    private Texture2D _fin;
    private Texture2D _croco;
    private Texture2D _leg;
    private SwimmingAnimal _myFish;
    private SwimmingAnimal _myCroco;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _scene = Content.Load<Texture2D>("images/scene_biome");
        _fish = Content.Load<Texture2D>("images/newfish2");
        _fin = Content.Load<Texture2D>("images/fish_fin2");
        _croco = Content.Load<Texture2D>("images/newcroco2");
        _leg = Content.Load<Texture2D>("images/croco_paw");

        _myFish = new SwimmingAnimal(_fish, _fin, new Vector2(100, 250), 2.0f);

        _myFish.appendageOffset = new Vector2[] {new Vector2(0,20)};
        _myFish.appendagePhases = new float[] { 0f };

        _myFish.appendageOrigin = new Vector2(0, -70);
        
        _myCroco = new SwimmingAnimal(_croco, _leg, new Vector2(50, 225), 1.0f);
        _myCroco.appendageOffset = new Vector2[] 
        {
            new Vector2(-60, 20),  // Front-Left
            new Vector2(-60, -10), // Front-Right (Drawn slightly "higher" to look like it's behind)
            new Vector2(60, 20),   // Back-Left
            new Vector2(60, -10)   // Back-Right
        };
        _myCroco.appendageOrigin = new Vector2(_leg.Width/2f, -90);
        _myCroco.appendagePhases = new float[] 
        {
            0f,     // Front-Left
            3.14f,  // Front-Right (swings opposite)
            3.14f,  // Back-Left (swings opposite of Front-Left)
            0f      // Back-Right (matches Front-Left)
        };

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        int screenWidth = _graphics.PreferredBackBufferWidth;

        if (_myFish != null)
        {
            _myFish.Update(gameTime, screenWidth);
        }

        if (_myCroco != null)
        {
            _myCroco.Update(gameTime, screenWidth);
        }

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        if (_scene != null)
        {
            _spriteBatch.Draw(_scene, new Rectangle(0, 0,
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight), Color.White);
        }
        _spriteBatch.End();
        if (_myFish != null)
        {
            _myFish.Draw(_spriteBatch);
        }
    
        if (_myCroco != null)
        {
            _myCroco.Draw(_spriteBatch);
        }

        

        base.Draw(gameTime);
    }
}