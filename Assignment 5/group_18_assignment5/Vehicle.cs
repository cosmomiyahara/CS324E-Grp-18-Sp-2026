using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace group_18_assignment5;

public class Vehicle
{
    private float angle;
    private float radius;
    private float speed; 
    private Model carModel;
    private float scaleSize = 0.02f;
    private float modelRotationOffset = 0f;
    private Texture2D  carTexture;
    public Matrix World { get; private set; }
    private Matrix[] transforms;

    public Vehicle(Model model, float trackRadius, float startAngle, float travelSpeed, Texture2D texture)
    {
        carModel = model;
        radius = trackRadius;
        angle = startAngle;
        speed = travelSpeed;
        carTexture = texture;
        transforms = new Matrix[carModel.Bones.Count];
    }
    
    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        double totalSeconds = gameTime.TotalGameTime.TotalSeconds;
        float lerpAmount = (float)(Math.Sin(totalSeconds * 1.5) + 1.0) / 2.0f;
        float currentSpeed = MathHelper.Lerp(0.3f, speed, lerpAmount);
    
        angle -= currentSpeed * dt;
        angle = MathHelper.WrapAngle(angle);
      
        float bounceHeight = (float)Math.Sin(totalSeconds * 10.0) * 0.5f;
        Matrix scale = Matrix.CreateScale(scaleSize);
        Matrix localRotation = Matrix.CreateRotationY(modelRotationOffset);
        Matrix localBounce = Matrix.CreateTranslation(0, bounceHeight, 0);
        Matrix offsetTranslation = Matrix.CreateTranslation(new Vector3(radius, 0.1f, 0));
        Matrix orbitRotation = Matrix.CreateRotationY(angle);
        World = scale * localRotation * localBounce * offsetTranslation * orbitRotation;
    }
    
    public void Draw(Matrix view, Matrix projection)
    {
        Matrix[] transforms = new Matrix[carModel.Bones.Count];
        carModel.CopyAbsoluteBoneTransformsTo(transforms);
        foreach (ModelMesh mesh in carModel.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = transforms[mesh.ParentBone.Index] * World;
                effect.View = view;
                effect.Projection = projection;
                effect.LightingEnabled = false;
                effect.TextureEnabled = true;
                effect.Texture = carTexture; 
            }
            mesh.Draw();
        }
    }


}

