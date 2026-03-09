using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace group_18_assignment5;

public class Truck
{
    private Model truckModel;
    private Matrix[] transforms;
    private float angle;
    private float radius = 20f;
    private float speed = 1.0f;
    public Matrix World { get; private set; }
    public Truck(Model model)
    {
        truckModel = model;
        transforms = new Matrix[truckModel.Bones.Count];
        angle = 0f;
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        double totalSeconds = gameTime.TotalGameTime.TotalSeconds;
        float lerpAmount = (float)(Math.Sin(totalSeconds * 1.2) + 1.0) / 2.0f;
        float currentSpeed = MathHelper.Lerp(0.4f, speed, lerpAmount);
        angle += currentSpeed * dt;
        angle = MathHelper.WrapAngle(angle);
        float bounceHeight = (float)Math.Sin(totalSeconds * 6.0) * 0.2f;
        float tiltAmount = (float)Math.Sin(totalSeconds * 3.0) * 0.05f;
        Matrix scale = Matrix.CreateScale(0.5f);
        Matrix modelRotation = Matrix.CreateRotationY(MathHelper.PiOver2);
        Matrix tilt = Matrix.CreateRotationZ(tiltAmount);
        Matrix bounce = Matrix.CreateTranslation(0, bounceHeight, 0);
        Matrix offset = Matrix.CreateTranslation(new Vector3(radius, 0.5f, 0));
        Matrix orbit = Matrix.CreateRotationY(angle);
        World = scale * modelRotation * tilt * bounce * offset * orbit;
    }

    public void Draw(Matrix view, Matrix projection)
    {
        truckModel.CopyAbsoluteBoneTransformsTo(transforms);
        foreach (ModelMesh mesh in truckModel.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = transforms[mesh.ParentBone.Index] * World;
                effect.View = view;
                effect.Projection = projection;
                effect.EnableDefaultLighting();
                effect.TextureEnabled = false;
                effect.DiffuseColor = new Vector3(0.8f, 0.1f, 0.1f);
            }
            mesh.Draw();
        }
    }
}