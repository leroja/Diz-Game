using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// System to draw 2D
    /// </summary>
    public class _2DSystem : IRender
    {
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Constructor that takes spritebatch as parameter.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public _2DSystem(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Function to draw 2D Textures
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<Texture2DComponent>())
            {
                Texture2DComponent texture = ComponentManager.GetEntityComponent<Texture2DComponent>(entityID);
                if (texture.Active)
                    Draw2DTexture(texture, dt);
            }
        }

        private void Draw2DTexture(Texture2DComponent texture, float dt)
        {
            texture.ElapsedTime += (int)dt;
            if (texture.ElapsedTime > texture.FrameTime)
            {
                texture.CurrentFrame++;
                if (texture.CurrentFrame == texture.Frames)
                    texture.CurrentFrame = 0;
                texture.ElapsedTime = 0;
            }
            texture.SourceRect = new Rectangle(
                texture.CurrentFrame * texture.Width,
                0,
                texture.Width,
                texture.Height);

            spriteBatch.Begin();
            //spriteBatch.Draw(texture.Texture, texture.SourceRect, texture.Color);
            spriteBatch.Draw(texture.Texture, texture.Position, null, texture.Color, texture.Rotation, texture.Origin, texture.Scale, SpriteEffects.None, 0);
            spriteBatch.End();

            spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            spriteBatch.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
