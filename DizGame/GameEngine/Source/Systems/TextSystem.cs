﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// System to draw text on the screen
    /// derived from IRender
    /// </summary>
    public class TextSystem : IRender
    {
        private SpriteBatch spriteBatch;
        /// <summary>
        /// Constructor which take spriteBatch as in parameter.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public TextSystem(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }
        /// <summary>
        /// Function that loops throught the TextComponents and then drawing them.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
           foreach(int entityID in ComponentManager.GetAllEntitiesWithComponentType<TextComponent>())
            {
                TextComponent text = ComponentManager.GetEntityComponent<TextComponent>(entityID);
                if(text.Children != null)
                    foreach(var child in text.Children)
                            DrawText(child.Value);
                else
                    DrawText(text);
            }
        }
        /// <summary>
        /// Function to draw the text 
        /// </summary>
        /// <param name="text"></param>
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
