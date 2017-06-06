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
        private VertexBuffer buff;
        private IndexBuffer indexBuff;
        private CameraComponent defcame;
        DateTime CreationTime;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public ParticleRenderSystem() { }

        /// <summary>
        /// 
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

            var comp = ComponentManager.GetAllEntitiesWithComponentType<ParticleEmitterComponent>();
            foreach (var i in comp)
            {
                ParticleEmitterComponent emitter = ComponentManager.GetEntityComponent<ParticleEmitterComponent>(i);
                TransformComponent tran = ComponentManager.GetEntityComponent<TransformComponent>(i);
                TransformComponent camtran = ComponentManager.GetEntityComponent<TransformComponent>(ComponentManager.GetEntityIDByComponent<CameraComponent>(defcame));
                Effect effect = emitter.Effect;

                if (buff == null && indexBuff == null)
                {
                    buff = new VertexBuffer(device, typeof(ParticleVertex), emitter.NumberOfParticles * 4, BufferUsage.WriteOnly);
                    indexBuff = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, emitter.NumberOfParticles * 6, BufferUsage.WriteOnly);
                }

                UpdateBuffers(gameTime, i);

                device.SetVertexBuffer(buff);
                device.Indices = indexBuff;

                effect.Parameters["ParticleTexture"].SetValue(emitter.Texture);
                effect.Parameters["View"].SetValue(defcame.View);
                effect.Parameters["Projection"].SetValue(defcame.Projection);
                effect.Parameters["Time"].SetValue((float)gameTime.ElapsedGameTime.TotalSeconds);
                effect.Parameters["Lifespan"].SetValue(emitter.LifeTime);
                effect.Parameters["wind"].SetValue(emitter.Direction);
                effect.Parameters["Size"].SetValue(new Vector2(tran.Scale.X, tran.Scale.Y));
                effect.Parameters["Up"].SetValue(camtran.Up);
                effect.Parameters["Side"].SetValue(camtran.Right);
                effect.Parameters["FadeInTime"].SetValue(emitter.FadeInTime);

                device.BlendState = BlendState.Additive;
                device.DepthStencilState = DepthStencilState.DepthRead;

                effect.CurrentTechnique.Passes[0].Apply();

                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, emitter.NumberOfParticles * 4);

                device.SetVertexBuffer(null);
                device.Indices = null;

                device.BlendState = BlendState.Opaque;
                device.DepthStencilState = DepthStencilState.Default;
            }
        }

        /// <summary>
        /// Updates Buffers for ParticleEmitterComponent
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="id"> the entities ID</param>
        public void UpdateBuffers(GameTime gameTime, int id)
        {
            ParticleEmitterComponent emitter = ComponentManager.GetEntityComponent<ParticleEmitterComponent>(id);

            int start = emitter.StartIndex;
            int end = emitter.NumberOfActiveParticles;
            for (int i = start; i < end; i++)
            {
                emitter.Particles[i].StartTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (emitter.Particles[i].StartTime == 0)
                {
                    emitter.StartIndex++;
                    emitter.NumberOfActiveParticles--;
                }
                if (emitter.StartIndex == emitter.Particles.Length)
                    emitter.StartIndex = 0;
            }
            buff.SetData<ParticleVertex>(emitter.Particles.ToArray<ParticleVertex>());
            indexBuff.SetData<int>(emitter.Indices.ToArray<int>());
        }
    }
}