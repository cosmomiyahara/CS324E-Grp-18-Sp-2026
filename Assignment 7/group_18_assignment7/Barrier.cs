using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_18_assignment7;

public class Barrier
{
    private List<BarrierPart>  _parts;
    private Texture2D _barrierTexture;
    private Vector2 _startPos;
    private Vector2 _origin;
    private Vector2 _mouseVelocity;
    private MouseState _lastMouseState;
    private float _scale;
    private float _linkSpacing;
    private bool _adding = true;
    private BarrierPart _lastLink;
    private Player _player;
    public List<BarrierPart> Parts => _parts;

    public Barrier(Texture2D barrierTexture, Vector2 startPos, float scale, Player player)
    {
        _player = player;
        _scale = scale;
        _barrierTexture = barrierTexture;
        _startPos = startPos;
        _lastMouseState = Mouse.GetState();
        _origin = new Vector2(_barrierTexture.Width / 2, _barrierTexture.Height / 2);
        _parts = new List<BarrierPart>();
        _lastLink = new BarrierPart(_startPos, barrierTexture, 0f, 3, 0f);
        _parts.Add(_lastLink);
        _linkSpacing = (_barrierTexture.Height * _scale) - (_barrierTexture.Width * _scale);
    }
    
    public void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        if (_adding)
        {
            NewLink();
            
            if (mouseState.LeftButton == ButtonState.Released)
            {
                _adding = false;
            }
        }
        else
        {
            //PartUpdate(gameTime);
        }
        
        Vector2 currentPos = new Vector2(mouseState.X, mouseState.Y);
        Vector2 lastPos = new Vector2(_lastMouseState.X, _lastMouseState.Y);
        _mouseVelocity = (currentPos - lastPos) / (float)gameTime.ElapsedGameTime.TotalSeconds;
        _lastMouseState = mouseState;
    }
    
    
    
    public void NewLink()
    {
        if (_player.InkCounter > 0)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            Vector2 prevMousePosition = new Vector2(_lastMouseState.X, _lastMouseState.Y);
            while (Vector2.Distance(mousePosition, _lastLink.Position) >= _linkSpacing)
            {
                Vector2 direction = mousePosition - _lastLink.Position;
                direction.Normalize();
                Vector2 clampedPosition = _lastLink.Position + (direction * _linkSpacing);
                float angle = (float)Math.Atan2(direction.Y, direction.X) + MathHelper.PiOver2;
                BarrierPart newLink = new BarrierPart(clampedPosition, _barrierTexture, 0f, 3, angle);
                _player.InkCounter -= 1;
                _parts.Add(newLink);
                if (_parts.Count != 2)
                {
                    _lastLink.Rotation += MathHelper.WrapAngle(angle - _lastLink.Rotation) * 0.5f;
                }
                else
                {
                    _lastLink.Rotation = angle;
                }

                _lastLink = newLink;
                _lastMouseState = mouseState;
            }
        }
        else
        {
            _adding = false;
        }


    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        for (int i = 0; i < _parts.Count; i++)
        {
            spriteBatch.Draw(_barrierTexture, _parts[i].Position,  null, Color.White, _parts[i].Rotation, _origin, _scale, SpriteEffects.None, 0f);
        }
        spriteBatch.End();
        
    }
    
    
}