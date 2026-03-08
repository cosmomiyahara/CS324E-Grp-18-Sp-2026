namespace group_18_assignment5;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Spaceship
{
    private Model shipModel;
    private Model fireModel; 
    private Matrix[] shipTransforms;
    private Matrix[] fireTransforms;
    private float pathWidth = 20f;
    private float pathLength = 5f;
    private float flightSpeed = 1.0f;
    private float minHeight = 2f;
    private float maxHeight = 15f;
    private Vector3 exhaustOffset; 
    public Matrix ShipWorld { get; private set; }
    public Matrix FireWorld { get; private set; }
    
    public Spaceship(Model ship, Model fire, Vector3 offset)
    {
        shipModel = ship;
        fireModel = fire;
        exhaustOffset = offset; 
        
        shipTransforms = new Matrix[shipModel.Bones.Count];
        shipModel.CopyAbsoluteBoneTransformsTo(shipTransforms);
        
        fireTransforms = new Matrix[fireModel.Bones.Count];
        fireModel.CopyAbsoluteBoneTransformsTo(fireTransforms);
    }
    
    public void Update(GameTime gameTime)
    {
        double totalSeconds = gameTime.TotalGameTime.TotalSeconds;
        float t = (float)totalSeconds * flightSpeed;
        
        float lerpAmount = (float)(Math.Sin(totalSeconds * 2.0) + 1.0) / 2.0f;
        float currentY = MathHelper.Lerp(minHeight, maxHeight, lerpAmount);
        
        float currentX = (float)Math.Sin(t) * pathWidth;
        float currentZ = (float)Math.Sin(t * 2f) * pathLength;
        
        float vx = (float)Math.Cos(t) * pathWidth;
        float vz = (float)Math.Cos(t * 2f) * 2f * pathLength;
        float facingAngle = (float)Math.Atan2(vx, vz);
        
        Matrix shipScale = Matrix.CreateScale(0.2f);
        Matrix shipRotation = Matrix.CreateRotationY(facingAngle);
        Matrix shipTranslation = Matrix.CreateTranslation(new Vector3(currentX, currentY, currentZ));
        Matrix pathRotation = Matrix.CreateRotationY((float)totalSeconds * 0.2f);
        ShipWorld = shipScale * shipRotation * shipTranslation * pathRotation;


        float flicker = (float)(Math.Sin(totalSeconds * 40.0) * 0.5 + 1.5); 
        
        Matrix localFireRotation = Matrix.CreateRotationX(MathHelper.PiOver2*3);
        Matrix localFireScale = Matrix.CreateScale(new Vector3(1f, flicker, 1f));
        Matrix localExhaustOffset = Matrix.CreateTranslation(exhaustOffset);
        
        FireWorld = localFireScale * localFireRotation * localExhaustOffset * ShipWorld;
    }

    public void Draw(Matrix view, Matrix projection)
    {
        DrawModel(shipModel, shipTransforms, ShipWorld, view, projection, Vector3.One, false);
        
        DrawModel(fireModel, fireTransforms, FireWorld, view, projection, new Vector3(1f, 0.5f, 0f), true);
    }

    private void DrawModel(Model model, Matrix[] transforms, Matrix world, Matrix view, Matrix projection, Vector3 colorTint, bool isFire)
    {
        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = transforms[mesh.ParentBone.Index] * world;
                effect.View = view;
                effect.Projection = projection;
                effect.LightingEnabled = false; 
                
                effect.TextureEnabled = false; 
                if (isFire)
                {
                    effect.VertexColorEnabled = false; 
                    effect.DiffuseColor = new Vector3(1f, 0.5f, 0f); 
                }
                else
                {
                    effect.VertexColorEnabled = true;
                    effect.AmbientLightColor = Vector3.Zero; 
                    effect.EmissiveColor = Vector3.Zero;
                }
            }
            mesh.Draw();
        }
    }
}
    