// Zaviyan Tharwani 
// zt3245 
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_18_assignment4;

public class GroundAnimal
{
    private readonly Texture2D _bodyTex;
    private readonly Texture2D _faceTex;
    private readonly Texture2D _tailTex;

    private Vector2 _pos;
    private readonly float _speed;

    private float _t;
    private float _tailAngle;
    private float _headAngle;
    private float _bounceY;

    private readonly float _scale;

    private Vector2 _faceOffset;
    private Vector2 _tailOffset;

    private Vector2 _tailOrigin;
    private Vector2 _headOrigin;

    public GroundAnimal(Texture2D body, Texture2D face, Texture2D tail, Vector2 startPos, float speed,
        float targetBodyWidth = 350f)
    {
        _bodyTex = body;
        _faceTex = face;
        _tailTex = tail;

        _pos = startPos;
        _speed = speed;

        _scale = targetBodyWidth / _bodyTex.Width;

        _tailOffset = new Vector2(0.03f * _bodyTex.Width, 0.02f * _bodyTex.Height);
        _faceOffset = new Vector2(0.63f * _bodyTex.Width, 0.03f * _bodyTex.Height);

        

        _tailOrigin = new Vector2(10f, _tailTex.Height * 0.8f);
        _headOrigin = new Vector2(20f, _faceTex.Height * 0.75f);
    }

    public void Update(GameTime gameTime, int screenWidth)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _t += dt;

        _pos.X += _speed * dt;

        float bodyWidthOnScreen = _bodyTex.Width * _scale;
        if (_pos.X > screenWidth + bodyWidthOnScreen)
            _pos.X = -bodyWidthOnScreen;

        _bounceY = 12f * (float)Math.Sin(_t * 4f);
        _tailAngle = 0.35f * (float)Math.Sin(_t * 6f);
        _headAngle = 0.15f * (float)Math.Sin(_t * 3f);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 bob = new Vector2(0f, _bounceY);

        Matrix root =
            Matrix.CreateScale(_scale) *
            Matrix.CreateTranslation(new Vector3(_pos, 0f));

        spriteBatch.Begin(transformMatrix: root);

        spriteBatch.Draw(
            _tailTex,
            bob + _tailOffset + _tailOrigin,
            null,
            Color.White,
            _tailAngle,
            _tailOrigin,
            1f,
            SpriteEffects.None,
            0f
        );

        spriteBatch.Draw(
            _bodyTex,
            bob,
            null,
            Color.White,
            0f,
            Vector2.Zero,
            1f,
            SpriteEffects.None,
            0f
        );

        spriteBatch.Draw(
            _faceTex,
            bob + _faceOffset + _headOrigin,
            null,
            Color.White,
            _headAngle,
            _headOrigin,
            1f,
            SpriteEffects.None,
            0f
        );

        spriteBatch.End();
    }


}

