using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_18_assignment4;

public class SwimmingAnimal
{
    public Texture2D body;
    public Texture2D appendage;
    public Vector2 position;
    public float speed;
    public float rotate;
    public Color tint;

    public Vector2 origin;
    public Vector2[] appendageOffset;
    public Vector2 appendageOrigin;
    public float[] appendageRotation;
    public float[] appendagePhases;
    public float scale = 0.1f;

    private float _animationTime;

    public SwimmingAnimal(Texture2D bodyTex, Texture2D appendageTex, Vector2 startPos, float startSpeed)
    {
        body = bodyTex;
        appendage = appendageTex;
        position = startPos;
        speed = startSpeed;
        rotate = 0f;
        tint = Color.SkyBlue;

        origin = new Vector2(body.Width / 2f, body.Height / 2f);
        appendageOffset = new Vector2[1];
        appendagePhases = new float[1] { 0f };
        appendageRotation = new float[1] { 0f };
    }

    public void Update(GameTime gameTime, int screenWidth)
    {
        position.X += speed;
        if (position.X > screenWidth + (body.Width * scale))
        {
            position.X = -(body.Width * scale);
        }

        _animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        // Adjust the 10f (speed) and 0.5f (angle) to change how wild the flapping is
        if (appendageOffset != null)
        {
            if (appendageRotation.Length != appendageOffset.Length)
                appendageRotation = new float[appendageOffset.Length];

            for (int i = 0; i < appendageOffset.Length; i++)
            {
                // Adding the phase shifts the sine wave so limbs move opposite to each other
                appendageRotation[i] = (float)Math.Sin((_animationTime * 10f) + appendagePhases[i]) * 0.5f;
            }
        }
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // 1. Determine the facing direction based on speed.
        // Since the image faces left natively, a positive speed (moving right) requires a negative X scale.
        // 1. Create the Root Matrix (Combines Scale, Rotation, and Translation)
        // This perfectly satisfies: "this Matrix should be composed of at least two transformations"
        Matrix rootMatrix = Matrix.CreateScale(scale) * Matrix.CreateRotationZ(rotate) * Matrix.CreateTranslation(new Vector3(position, 0f));

        // 2. Begin a new SpriteBatch specifically for this object, passing in the Matrix
        spriteBatch.Begin(transformMatrix: rootMatrix);

        // 3. Draw the Root (Body)
        // Because the Matrix handles the placement, we draw at Vector2.Zero with a scale of 1f
        spriteBatch.Draw(body, Vector2.Zero, null, tint, 0f, origin, 1f, SpriteEffects.None, 0f);

        // 4. Draw the Second Level (Appendages)
        // We only need to provide local offsets and local rotations. 
        // The rootMatrix automatically scales them and moves them to the correct screen position!
        if (appendage != null && appendageOffset != null)
        {
            for (int i = 0; i < appendageOffset.Length; i++)
            {
                spriteBatch.Draw(appendage, appendageOffset[i], null, tint, appendageRotation[i], appendageOrigin, 1f,
                    SpriteEffects.None, 0f);
            }
        }
        spriteBatch.End();
    }
    
}