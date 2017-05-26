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
        /// <param name="gd"> Graphics device for drawing Primatives </param>
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

            var comp = ComponentManager.GetAllEntitiesWithComponentType<ParticleEmiterComponent>();
            foreach (var i in comp)
            {
                ParticleEmiterComponent emiter = ComponentManager.GetEntityComponent<ParticleEmiterComponent>(i);
                TransformComponent tran = ComponentManager.GetEntityComponent<TransformComponent>(i);
                TransformComponent camtran = ComponentManager.GetEntityComponent<TransformComponent>(ComponentManager.GetEntityIDByComponent<CameraComponent>(defcame));
                Effect effect = emiter.Effect;

                if (buff == null && indexBuff == null)
                {
                    buff = new VertexBuffer(device, typeof(ParticleVertex), emiter.NumberOfParticles * 4, BufferUsage.WriteOnly);
                    indexBuff = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, emiter.NumberOfParticles * 6, BufferUsage.WriteOnly);
                }

                UpdateBuffers(gameTime, i);

                device.SetVertexBuffer(buff);
                device.Indices = indexBuff;

                effect.Parameters["ParticleTexture"].SetValue(emiter.Texture);
                effect.Parameters["View"].SetValue(defcame.View);
                effect.Parameters["Projection"].SetValue(defcame.Projection);
                effect.Parameters["Time"].SetValue((float)gameTime.ElapsedGameTime.TotalSeconds);
                effect.Parameters["Lifespan"].SetValue(emiter.LifeTime);
                effect.Parameters["wind"].SetValue(emiter.Direction);
                effect.Parameters["Size"].SetValue(new Vector2(tran.Scale.X, tran.Scale.Y));
                effect.Parameters["Up"].SetValue(camtran.Up);
                effect.Parameters["Side"].SetValue(camtran.Right);
                effect.Parameters["FadeInTime"].SetValue(emiter.FadeInTime);

                device.BlendState = BlendState.Additive;
                device.DepthStencilState = DepthStencilState.DepthRead;

                effect.CurrentTechnique.Passes[0].Apply();

                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, emiter.NumberOfParticles * 4);

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
        /// <param name="id"> the entitys ID</param>
        public void UpdateBuffers(GameTime gameTime, int id)
        {
            ParticleEmiterComponent emiter = ComponentManager.GetEntityComponent<ParticleEmiterComponent>(id);

            int start = emiter.StartIndex;
            int end = emiter.NumberOfActiveParticles;
            for (int i = start; i < end; i++)
            {
                emiter.Particles[i].StartTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (emiter.Particles[i].StartTime == 0)
                {
                    emiter.StartIndex++;
                    emiter.NumberOfActiveParticles--;
                }
                if (emiter.StartIndex == emiter.Particles.Length)
                    emiter.StartIndex = 0;
            }
            buff.SetData<ParticleVertex>(emiter.Particles.ToArray<ParticleVertex>());
            indexBuff.SetData<int>(emiter.Indices.ToArray<int>());
        }
    }
}