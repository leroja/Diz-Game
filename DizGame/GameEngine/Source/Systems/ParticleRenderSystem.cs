using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Source.Managers;
using GameEngine.Source.RandomStuff;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// System for rendering particles
    /// </summary>
    public class ParticleRenderSystem : IRender
    {
        private GraphicsDevice device;
        private CameraComponent defcame;
        DateTime CreationTime;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public ParticleRenderSystem() { }

        /// <summary>
        /// Constructor for ParticleRenderSystem
        /// </summary>
        /// <param name="gd"> Graphics device for drawing Primitives </param>
        public ParticleRenderSystem(GraphicsDevice gd)
        {
            CreationTime = DateTime.Now;
            device = gd;
        }

        /// <summary>
        /// Draws particles.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            List<int> entitiesWithCamera = ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>();
            defcame = ComponentManager.GetEntityComponent<CameraComponent>(entitiesWithCamera.FirstOrDefault());

            var comp = ComponentManager.GetAllEntitiesWithComponentType<ParticleSettingsComponent>();
            foreach (var i in comp)
            {
                ParticleSettingsComponent setings = ComponentManager.GetEntityComponent<ParticleSettingsComponent>(i);
                ParticleEmitterComponent emitter = ComponentManager.GetEntityComponent<ParticleEmitterComponent>(i);
                TransformComponent tran = ComponentManager.GetEntityComponent<TransformComponent>(i);
                TransformComponent camtran = ComponentManager.GetEntityComponent<TransformComponent>(ComponentManager.GetEntityIDByComponent<CameraComponent>(defcame));
                Effect effect = emitter.ParticleEffect;
                LoadEffectParameters(i);
                if (emitter.VertexBuffer.IsContentLost)
                {
                    emitter.VertexBuffer.SetData(emitter.Particles);
                }
                if (emitter.FirstNewParticle != emitter.FirstFreeParticle)
                {
                    AddNewParticlesToVertexBuffer(i);
                }

                if (emitter.FirstActiveParticle != emitter.FirstFreeParticle)
                {
                    device.BlendState = setings.BlendState;
                    device.DepthStencilState = DepthStencilState.DepthRead;

                    device.SetVertexBuffer(emitter.VertexBuffer);
                    device.Indices = emitter.IndexBuffer;


                    foreach (EffectPass pass in emitter.ParticleEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        if (emitter.FirstActiveParticle < emitter.FirstFreeParticle)
                        {
                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, (setings.MaxParticles - emitter.FirstActiveParticle) * 2);
                        }
                        else
                        {
                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, (setings.MaxParticles - emitter.FirstActiveParticle) * 2);

                            if (emitter.FirstFreeParticle > 0)
                            {
                                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, emitter.FirstFreeParticle * 2);
                            }
                        }
                    }
                    device.DepthStencilState = DepthStencilState.Default;
                }
                emitter.DrawCounter++;
            }
        }

        private void LoadEffectParameters(int id)
        {
            ParticleSettingsComponent setings = ComponentManager.GetEntityComponent<ParticleSettingsComponent>(id);
            ParticleEmitterComponent emitter = ComponentManager.GetEntityComponent<ParticleEmitterComponent>(id);

            EffectParameterCollection parameters = emitter.ParticleEffect.Parameters;


            //emitter.effectViewportScaleParameter = parameters["ViewportScale"];


            // Set the values of parameters that do not change.
            parameters["View"].SetValue(defcame.View);
            parameters["Projection"].SetValue(defcame.Projection);
            parameters["CurrentTime"].SetValue(emitter.CurrentTime);
            parameters["ViewportScale"].SetValue(new Vector2(0.5f / device.Viewport.AspectRatio, -0.5f));
            parameters["Duration"].SetValue((float)setings.Duration.TotalSeconds);
            parameters["DurationRandomness"].SetValue(setings.DurationRandomness);
            parameters["Gravity"].SetValue(setings.Gravity);
            parameters["EndVelocity"].SetValue(setings.EndVelocity);
            parameters["MinColor"].SetValue(setings.MinColor.ToVector4());
            parameters["MaxColor"].SetValue(setings.MaxColor.ToVector4());

            parameters["RotateSpeed"].SetValue(
                new Vector2(setings.MinRotateSpeed, setings.MaxRotateSpeed));

            parameters["StartSize"].SetValue(
                new Vector2(setings.MinStartSize, setings.MaxStartSize));

            parameters["EndSize"].SetValue(
                new Vector2(setings.MinEndSize, setings.MaxEndSize));
            parameters["Texture"].SetValue(setings.Texture);
        }


        void AddNewParticlesToVertexBuffer(int id)
        {
            ParticleSettingsComponent setings = ComponentManager.GetEntityComponent<ParticleSettingsComponent>(id);
            ParticleEmitterComponent emitter = ComponentManager.GetEntityComponent<ParticleEmitterComponent>(id);
            int stride = ParticleVertex.sizeInBytes;

            if (emitter.FirstNewParticle < emitter.FirstFreeParticle)
            {
                // If the new particles are all in one consecutive range,
                // we can upload them all in a single call.
                emitter.VertexBuffer.SetData(emitter.FirstNewParticle * stride * 4, emitter.Particles,
                                     emitter.FirstNewParticle * 4,
                                     (emitter.FirstFreeParticle - emitter.FirstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);
            }
            else
            {
                // If the new particle range wraps past the end of the queue
                // back to the start, we must split them over two upload calls.
                emitter.VertexBuffer.SetData(emitter.FirstNewParticle * stride * 4, emitter.Particles,
                                     emitter.FirstNewParticle * 4,
                                     (setings.MaxParticles - emitter.FirstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);

                if (emitter.FirstFreeParticle > 0)
                {
                    emitter.VertexBuffer.SetData(0, emitter.Particles,
                                         0, emitter.FirstFreeParticle * 4,
                                         stride, SetDataOptions.NoOverwrite);
                }
            }

            // Move the particles we just uploaded from the new to the active queue.
            emitter.FirstNewParticle = emitter.FirstFreeParticle;
        }
    }
}