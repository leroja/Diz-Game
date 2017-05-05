using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Systems
{
    public class TextSystem : IRender
    {
        SpriteBatch spriteBatch;
        public TextSystem(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }
        public override void Draw(GameTime gameTime)
        {
           foreach(int entityID in ComponentManager.GetAllEntitiesWithComponentType<TextComponent>())
            {
                TextComponent text = ComponentManager.GetEntityComponent<TextComponent>(entityID);
                DrawText(text);
            }
        }

        private void DrawText(TextComponent text)
        {
            if (spriteBatch != null)
            {
                if (text.IsVisible)
                {
                    spriteBatch.Begin();
                    spriteBatch.DrawString(text.Font, text.Text, text.Position, text.Color);
                    spriteBatch.End();
                }
                spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                spriteBatch.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
        }
    }
}
