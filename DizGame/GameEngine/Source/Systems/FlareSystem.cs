using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.RandomStuff;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// Class to render flares
    /// </summary>
    public class FlareSystem : IRender
    {
        GraphicsDevice device;
        SpriteBatch spriteBatch;

        /// <summary>
        /// Flare system that draws active flares.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public FlareSystem(SpriteBatch spriteBatch)
        {
            this.device = spriteBatch.GraphicsDevice;
            this.spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Draws the active flares
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<FlareComponent>())
            {
                FlareComponent flare = ComponentManager.GetEntityComponent<FlareComponent>(entityID);
                foreach (int cameraID in ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>())
                {
                    CameraComponent camera = ComponentManager.GetEntityComponent<CameraComponent>(cameraID);
                        if (flare.IsActive)
                        {
                            DrawOcclusion(flare, camera);
                            DrawGlow(flare);
                            if (camera.IsFlareable)
                                DrawFlares(flare);
                            RestoreRenderStates();
                        }
                }
            }
        }

        private void DrawOcclusion(FlareComponent flare, CameraComponent camera)
        {
            KeyboardState s = Keyboard.GetState();
            List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>();
            WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(temp.First());
            float aspectRatio = device.Viewport.AspectRatio;
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                    aspectRatio,
                                                                    0.1f, 500);
            if (camera != null)
            {
                BasicEffect basicEffect = new BasicEffect(device)
                {
                    VertexColorEnabled = true
                };

                // The sun is infinitely distant, so it should not be affected by the
                // position of the camera. Floating point math doesn't support infinitely
                // distant vectors, but we can get the same result by making a copy of our
                // view matrix, then resetting the view translation to zero. Pretending the
                // camera has not moved position gives the same result as if the camera
                // was moving, but the light was infinitely far away. If our flares came
                // from a local object rather than the sun, we would use the original view
                // matrix here.
                Matrix infiniteView = camera.View;

                infiniteView.Translation = Vector3.Zero;

                // Project the light position into 2D screen space.
                Viewport viewport = device.Viewport;

                Vector3 projectedPosition = viewport.Project(-flare.LightDirection, projection,
                                                             infiniteView, world.World);

                // Don't draw any flares if the light is behind the camera.
                if ((projectedPosition.Z < 0) || (projectedPosition.Z > 1))
                {
                    flare.LightBehindCamera = true;
                    return;
                }

                flare.LightPosition = new Vector2(projectedPosition.X, projectedPosition.Y);

                flare.LightBehindCamera = false;

                if (flare.OcclusionQueryActive)
                {
                    // If the previous query has not yet completed, wait until it does.
                    if (!flare.OcclusionQuery.IsComplete)
                        return;

                    // Use the occlusion query pixel count to work
                    // out what percentage of the sun is visible.
                    float queryArea = flare.QuerySize * flare.QuerySize;

                    flare.OcclusionAlpha = Math.Min(flare.OcclusionQuery.PixelCount / queryArea, 1);
                }

                // Set renderstates for drawing the occlusion query geometry. We want depth
                // tests enabled, but depth writes disabled, and we disable color writes
                // to prevent this query polygon actually showing up on the screen.
                device.BlendState = FlareComponent.ColorWriteDisable;
                device.DepthStencilState = DepthStencilState.DepthRead;

                // Set up our BasicEffect to center on the current 2D light position.
                basicEffect.World = Matrix.CreateTranslation(flare.LightPosition.X,
                                                             flare.LightPosition.Y, 0);

                basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0,
                                                                            viewport.Width,
                                                                            viewport.Height,
                                                                            0, 0, 1);
                basicEffect.CurrentTechnique.Passes[0].Apply();
                // Issue the occlusion query.
                flare.OcclusionQuery.Begin();
                device.DrawUserPrimitives(PrimitiveType.TriangleStrip, flare.QueryVertices, 0, 2);
                flare.OcclusionQuery.End();
                flare.OcclusionQueryActive = true;
            }
        }

        /// <summary>
        /// Draws a large circular glow sprite, centered on the sun.
        /// </summary>
        private void DrawGlow(FlareComponent flare)
        {
            if (flare.LightBehindCamera || flare.OcclusionAlpha <= 0)
                return;

            Color color = Color.White * flare.OcclusionAlpha;
            Vector2 origin = new Vector2(flare.GlowSprite.Width, flare.GlowSprite.Height) / 2;
            float scale = flare.GlowSize * 2 / flare.GlowSprite.Width;

            spriteBatch.Begin();

            spriteBatch.Draw(flare.GlowSprite, flare.LightPosition, null, color, 0,
                             origin, scale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        /// <summary>
        /// Draws the lensflare sprites, computing the position
        /// of each one based on the current angle of the sun.
        /// </summary>
        private void DrawFlares(FlareComponent flare)
        {
            if (flare.LightBehindCamera || flare.OcclusionAlpha <= 0)
                return;

            Viewport viewport = device.Viewport;

            // Lensflare sprites are positioned at intervals along a line that
            // runs from the 2D light position toward the center of the screen.
            Vector2 screenCenter = new Vector2(viewport.Width, viewport.Height) / 2;

            Vector2 flareVector = screenCenter - flare.LightPosition;

            // Draw the flare sprites using additive blending.
            spriteBatch.Begin(0, BlendState.Additive);

            foreach (Flare flareEff in flare.Flares)
            {
                // Compute the position of this flare sprite.
                Vector2 flarePosition = flare.LightPosition + flareVector * flareEff.Position;

                // Set the flare alpha based on the previous occlusion query result.
                Vector4 flareColor = flareEff.Color.ToVector4();

                flareColor.W *= flare.OcclusionAlpha;

                // Center the sprite texture.
                Vector2 flareOrigin = new Vector2(flareEff.Texture.Width,
                                                  flareEff.Texture.Height) / 2;

                // Draw the flare.
                spriteBatch.Draw(flareEff.Texture, flarePosition, null,
                                 new Color(flareColor), 1, flareOrigin,
                                 flareEff.Scale, SpriteEffects.None, 0);
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Sets renderstates back to their default values after we finish drawing
        /// the lensflare, to avoid messing up the 3D terrain rendering.
        /// </summary>
        private void RestoreRenderStates()
        {
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            device.SamplerStates[0] = SamplerState.LinearWrap;
        }
    }
}
