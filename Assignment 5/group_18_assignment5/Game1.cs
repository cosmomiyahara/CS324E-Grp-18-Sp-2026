//A5 group 18, Cosmo, Zaviyan, Brady

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_18_assignment5;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    VertexPositionColor[] trackSmall;
    VertexPositionColor[] trackLarge;
    BasicEffect effect;
    Model carModel;
    Vehicle playerVehicle;
    private Texture2D carTexture;
    private Vector3 thrusterOffset;
    private Spaceship spaceship;
    private Model spaceshipModel;
    private Model thrusterModel;
    private Model center;
    Model truckModel;
    Truck truck;
    


    // Transformation Matrices
    Matrix world = Matrix.Identity;
    Matrix view = Matrix.CreateLookAt(new Vector3(0, 50, 50), Vector3.Zero, Vector3.Up);
    Matrix projection;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45),
            GraphicsDevice.Viewport.AspectRatio, 
            0.1f, 1000f);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        effect = new BasicEffect(GraphicsDevice);
        int segments = 100;
        thrusterOffset = new Vector3(0, 17, -20);
        // Generate the small circuit (inner)
        trackSmall = CreateTrack(segments, 12f, 4f, Color.DarkSlateGray);
        
        // Generate the large circuit (outer)
        trackLarge = CreateTrack(segments, 20f, 4f, Color.Gray);
        carModel = Content.Load<Model>("FREE_CAR_01");
        carTexture =  Content.Load<Texture2D>("001_COLOR_BASIC");
        spaceshipModel = Content.Load<Model>("craft_speederC");
        thrusterModel = Content.Load<Model>("chimney");
        center = Content.Load<Model>("hangar_roundA");
        playerVehicle = new Vehicle(carModel, 12f, 0f, 1.5f, carTexture);
        spaceship = new Spaceship(spaceshipModel, thrusterModel, thrusterOffset);
        truckModel = Content.Load<Model>("truck_vehicle");
        truck = new Truck(truckModel);
    }
    private VertexPositionColor[] CreateTrack(int segments, float radius, float thickness, Color color)
    {
        VertexPositionColor[] vertices = new VertexPositionColor[(segments + 1) * 2];

        for (int i = 0; i <= segments; i++)
        {
            float angle = MathHelper.TwoPi * i / segments;
            float x = (float)Math.Cos(angle);
            float z = (float)Math.Sin(angle);

            // Inner edge of the ribbon
            vertices[i * 2] = new VertexPositionColor(
                new Vector3(x * (radius - thickness / 2), 0, z * (radius - thickness / 2)), color);

            // Outer edge of the ribbon
            vertices[i * 2 + 1] = new VertexPositionColor(
                new Vector3(x * (radius + thickness / 2), 0, z * (radius + thickness / 2)), color);
        }
        return vertices;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        playerVehicle.Update(gameTime);
        spaceship.Update(gameTime);
        truck.Update(gameTime);

        double time = gameTime.TotalGameTime.TotalSeconds * 0.5f;
        float camX = (float)Math.Cos(time) * 50f;
        float camZ = (float)Math.Sin(time) * 50f;
        Vector3 cameraPosition = new Vector3(camX, 50f, camZ);
        view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        effect.World = world;
        effect.View = view;
        effect.Projection = projection;
        effect.VertexColorEnabled = true;

        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();

            // Draw Small Circuit
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, 
                trackSmall, 0, trackSmall.Length - 2);
        
            // Draw Large Circuit
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, 
                trackLarge, 0, trackLarge.Length - 2);
        }
        GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        playerVehicle.Draw(view, projection);
        spaceship.Draw(view, projection);
        truck.Draw(view, projection);
        
        
        
        Matrix scale = Matrix.CreateScale(0.5f);
        Matrix offset = Matrix.CreateTranslation(0, 0, 40);
// If you want it at the center, the scale IS the world matrix.
        Matrix world1 = scale;

        foreach (ModelMesh mesh in center.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                // Apply the scale here
                effect.World = world1; 
        
                effect.View = view;
                effect.Projection = projection;
                effect.EnableDefaultLighting(); 
            }
            mesh.Draw();
        }
        world1 = scale * offset;
        foreach (ModelMesh mesh in center.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                // Apply the scale here
                effect.World = world1; 
        
                effect.View = view;
                effect.Projection = projection;
                effect.EnableDefaultLighting(); 
            }
            mesh.Draw();
        }
        
        base.Draw(gameTime);
    }
}