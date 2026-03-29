using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A6;

public class Vine
{
    private List<VineLink>  _links;
    private Texture2D _vineTexture;
    private Vector2 _startPos;
    private Vector2 _gravity;
    private Vector2 _origin;
    private Vector2 _mouseVelocity;
    private MouseState _lastMouseState;
    private float _scale;
    private float _linkSpacing;
    private float _ks = 0.013f;
    private float _kd = 0.22f;
    private bool _adding = true;
    private VineLink _lastLink;
    

    public Vine(Texture2D vineTexture, Vector2 startPos, Vector2 gravity,  float scale)
    {
        _scale = Math.Clamp(scale, 0.1f, 1f);
        _vineTexture = vineTexture;
        _startPos = startPos;
        _gravity = gravity;
        _lastMouseState = Mouse.GetState();
        _origin = new Vector2(_vineTexture.Width / 2, _vineTexture.Height / 2);
        _links = new List<VineLink>();
        _lastLink = new VineLink(new Vector2(_lastMouseState.X, _lastMouseState.Y), 0f, _scale * (1.1f * _links.Count), _vineTexture);
        _links.Add(_lastLink);
        _linkSpacing = (_vineTexture.Height * _scale) - (_vineTexture.Width * _scale);
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
            LinkUpdate(gameTime);
        }
        
        Vector2 currentPos = new Vector2(mouseState.X, mouseState.Y);
        Vector2 lastPos = new Vector2(_lastMouseState.X, _lastMouseState.Y);
        _mouseVelocity = (currentPos - lastPos) / (float)gameTime.ElapsedGameTime.TotalSeconds;
        _lastMouseState = mouseState;
    }


    public void NewLink()
    {
        MouseState mouseState = Mouse.GetState();
        Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
        Vector2 prevMousePosition = new Vector2(_lastMouseState.X, _lastMouseState.Y);
        if (Vector2.Distance(mousePosition, _lastLink.Position) >= _linkSpacing)
        {
            Vector2 direction = mousePosition - _lastLink.Position;
            direction.Normalize();
            Vector2 clampedPosition = _lastLink.Position + (direction * _linkSpacing);
            float angle = (float)Math.Atan2(direction.Y, direction.X) + MathHelper.PiOver2;
            VineLink newLink = new VineLink(clampedPosition, angle, _scale-(_links.Count * 0.01f), _vineTexture);
            _links.Add(newLink);
            if (_links.Count != 2)
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
    

    public void LinkUpdate(GameTime gameTime)
    {
        for (int i = 0; i < _links.Count; i++)
        {
            _links[i].Update(gameTime);
            if (i > 0 && !_links[i].Clicked)
            {

                Vector2 direction = _links[i].Position - _links[i - 1].Position;
                direction.Normalize();
                
                float angle = (float)Math.Atan2(direction.Y, direction.X) + MathHelper.PiOver2;
                _links[i].ApplyForce(_gravity);
                _links[i].ApplySpringForce(_links[i-1].Position, _ks, _kd);
                _links[i].Velocity += _links[i - 1].Velocity * 0.1f;
                _links[i].Rotation = angle;
                _links[i-1].Rotation += MathHelper.WrapAngle(angle - _links[i-1].Rotation) * 0.5f;
                Vector2 clampedPosition = _links[i-1].Position + (direction * _linkSpacing);
                _links[i].Position = clampedPosition;


            }
            if (_links[i].Clicked)
            {
                Vector2 direction2 = _links[i].Position - _links[i - 1].Position;
                direction2.Normalize();
                float angle2 = (float)Math.Atan2(direction2.Y, direction2.X) + MathHelper.PiOver2;
                _links[i].Rotation = angle2;
                for (int z = i - 1; z > 0; z--)
                {
                    Vector2 direction = _links[z].Position - _links[z+1].Position;
                    direction.Normalize();

                    float angle = (float)Math.Atan2(direction.Y, direction.X) + MathHelper.PiOver2;
                    _links[z].ApplySpringForce(_links[z+1].Position, _ks, _kd);
                    _links[z].Rotation = angle;
                    Vector2 clampedPosition = _links[z+1].Position + (direction * _linkSpacing);
                    _links[z].Position = clampedPosition;
                }
            }
            
            
        }
    }

    public void LinkCheck()
    {
        MouseState mouseState =  Mouse.GetState();
        for (int i = 1; i < _links.Count; i++)
        {
            if (_links[i].Bounds.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed)
            {
                _links[i].Clicked = true;
                break;
            }
            else if (_links[i].Clicked)
            {
                _links[i].Clicked = false;
                _links[i].Velocity += _mouseVelocity*4;
            }
        }
    }
    
    

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        for (int i = 0; i < _links.Count; i++)
        {
            spriteBatch.Draw(_vineTexture, _links[i].Position,  null, Color.White, _links[i].Rotation, _origin, _scale, SpriteEffects.None, 0f);
        }
        spriteBatch.End();
        
    }






}