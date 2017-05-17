using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public ParticleRenderSystem() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gd">Graphiscs device fro drawing Primatives </param>
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
            defcame = ComponentManager.GetEntityComponent<CameraComponent>(entitiesWithCamera.First());

            var comp = ComponentManager.GetAllEntitiesWithComponentType<ParticleEmiterComponent>();
            foreach (var i in comp)
            {
                

                ParticleEmiterComponent emiter = ComponentManager.GetEntityComponent<ParticleEmiterComponent>(i);
                TransformComponent tran = ComponentManager.GetEntityComponent<TransformComponent>(i);
                TransformComponent camtran = ComponentManager.GetEntityComponent<TransformComponent>(ComponentManager.GetEntityIDByComponent<CameraComponent>(defcame));
                Effect effect = emiter.effect;

                if (buff == null && indexBuff == null)
                {
                    buff = new VertexBuffer(device, typeof(ParticleVertex), emiter.nParticles * 4, BufferUsage.WriteOnly);
                    indexBuff = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, emiter.nParticles * 6, BufferUsage.WriteOnly);
                }

                uppdateBuffers(gameTime, i);

                device.SetVertexBuffer(buff);
                device.Indices = indexBuff;

                effect.Parameters["ParticleTexture"].SetValue(emiter.texture);
                effect.Parameters["View"].SetValue(defcame.View);
                effect.Parameters["Projection"].SetValue(defcame.Projection);
                effect.Parameters["Time"].SetValue((float)gameTime.ElapsedGameTime.TotalSeconds);
                effect.Parameters["Lifespan"].SetValue(emiter.lifeTime);
                effect.Parameters["wind"].SetValue(emiter.Direction);
                effect.Parameters["Size"].SetValue(new Vector2(tran.Scale.X ,tran.Scale.Y));
                effect.Parameters["Up"].SetValue(camtran.Up);
                effect.Parameters["Side"].SetValue(camtran.Right);
                effect.Parameters["FadeInTime"].SetValue(emiter.FadeInTime);

                device.BlendState = BlendState.Additive;
                device.DepthStencilState = DepthStencilState.DepthRead;

                effect.CurrentTechnique.Passes[0].Apply();

                device.DrawIndexedPrimitives(PrimitiveType.TriangleList,0, 0, emiter.nParticles * 4);

                device.SetVertexBuffer(null);
                device.Indices = null;

                device.BlendState = BlendState.Opaque;
                device.DepthStencilState = DepthStencilState.Default;
            }
        }


        /// <summary>
        /// Uppdates Buffers for ParticleEmitterComponent
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="id"> the entitys ID</param>
        public void uppdateBuffers(GameTime gameTime, int  id )
        {
            ParticleEmiterComponent emiter = ComponentManager.GetEntityComponent<ParticleEmiterComponent>(id);

            int start = emiter.StartIndex;
            int end = emiter.numberOfActiveParticles;
            for (int i = start; i < end; i++)
            {

                emiter.particle[i].startTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (emiter.particle[i].startTime == 0 )
                {
                    emiter.StartIndex++;
                    emiter.numberOfActiveParticles--;
                }
                if (emiter.StartIndex == emiter.particle.Length)
                    emiter.StartIndex = 0;
            }
           buff.SetData<ParticleVertex>(emiter.particle.ToArray<ParticleVertex>());
           indexBuff.SetData<int>(emiter.indices.ToArray<int>());
        }
    }
}

