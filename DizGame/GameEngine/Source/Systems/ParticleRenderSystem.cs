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
                Effect effect = emitter.particleEffect;
                LoadEffectParameters(i);
                if (emitter.vertexBuffer.IsContentLost)
                {
                    emitter.vertexBuffer.SetData(emitter.particles);
                }
                if (emitter.firstNewParticle != emitter.firstFreeParticle)
                {
                    AddNewParticlesToVertexBuffer(i);
                }

                if (emitter.firstActiveParticle != emitter.firstFreeParticle)
                {
                    device.BlendState = setings.BlendState;
                    device.DepthStencilState = DepthStencilState.DepthRead;

                    device.SetVertexBuffer(emitter.vertexBuffer);
                    device.Indices = emitter.indexBuffer;

                   
                    foreach (EffectPass pass in emitter.particleEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        if (emitter.firstActiveParticle < emitter.firstFreeParticle)
                        {
                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, (setings.MaxParticles - emitter.firstActiveParticle) * 2);
                        }
                        else
                        {
                            
                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, (setings.MaxParticles - emitter.firstActiveParticle) * 2);

                            if (emitter.firstFreeParticle > 0)
                            {
                                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, emitter.firstFreeParticle * 2);
                            }
                        }
                    }

                    device.DepthStencilState = DepthStencilState.Default;
                }

                emitter.drawCounter++;
            }
        }

        private void LoadEffectParameters(int id)
        {
            ParticleSettingsComponent setings = ComponentManager.GetEntityComponent<ParticleSettingsComponent>(id);
            ParticleEmitterComponent emitter = ComponentManager.GetEntityComponent<ParticleEmitterComponent>(id);

            EffectParameterCollection parameters =  emitter.particleEffect.Parameters;

            
            //emitter.effectViewportScaleParameter = parameters["ViewportScale"];


            // Set the values of parameters that do not change.
            parameters["View"].SetValue(defcame.View);
            parameters["Projection"].SetValue(defcame.Projection);
            parameters["CurrentTime"].SetValue(emitter.currentTime);
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
            parameters["Texture"].SetValue(setings.texture);
        }
    

        void AddNewParticlesToVertexBuffer(int id)
        {
            ParticleSettingsComponent setings = ComponentManager.GetEntityComponent<ParticleSettingsComponent>(id);
            ParticleEmitterComponent emitter = ComponentManager.GetEntityComponent<ParticleEmitterComponent>(id);
            int stride = ParticleVertex.sizeInBytes;

            if (emitter.firstNewParticle < emitter.firstFreeParticle)
            {
                // If the new particles are all in one consecutive range,
                // we can upload them all in a single call.
                emitter.vertexBuffer.SetData(emitter.firstNewParticle * stride * 4, emitter.particles,
                                     emitter.firstNewParticle * 4,
                                     (emitter.firstFreeParticle - emitter.firstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);
            }
            else
            {
                // If the new particle range wraps past the end of the queue
                // back to the start, we must split them over two upload calls.
                emitter.vertexBuffer.SetData(emitter.firstNewParticle * stride * 4, emitter.particles,
                                     emitter.firstNewParticle * 4,
                                     (setings.MaxParticles - emitter.firstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);

                if (emitter.firstFreeParticle > 0)
                {
                    emitter.vertexBuffer.SetData(0, emitter.particles,
                                         0, emitter.firstFreeParticle * 4,
                                         stride, SetDataOptions.NoOverwrite);
                }
            }

            // Move the particles we just uploaded from the new to the active queue.
            emitter.firstNewParticle = emitter.firstFreeParticle;
        }

    }
}