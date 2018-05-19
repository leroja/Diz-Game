using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;
using DizGame.Source.Factories;
using System.Threading.Tasks;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// 
    /// </summary>
    public class SmokeSystem : IUpdate
    {
        /// <summary>
        /// Updates all particle components and compares if component should be removed or not
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var a = ComponentManager.GetAllEntitiesWithComponentType<ParticleSettingsComponent>();
            Parallel.ForEach(a, id =>
            {
                var emitter = ComponentManager.GetEntityComponent<ParticleEmitterComponent>(id);
                var settings = ComponentManager.GetEntityComponent<ParticleSettingsComponent>(id);
                var tran = ComponentManager.GetEntityComponent<TransformComponent>(id);
                AddParticle(emitter, settings, tran, Vector3.Zero);
                emitter.LifeTime = emitter.LifeTime - (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (emitter.LifeTime <= 0)
                {
                    switch (emitter.ParticleType)
                    {
                        case "Smoke":
                            Vector3 pos = GetMapPositions(1).FirstOrDefault();
                            EntityFactory.Instance.ParticleFactory.CreateParticleEmitter(pos, "Smoke", 10);
                            ComponentManager.RemoveEntity(id);
                            ComponentManager.RecycleID(id);
                            break;
                        case "Blood":
                            ComponentManager.RemoveEntity(id);
                            ComponentManager.RecycleID(id);
                            break;
                    }
                }
            });
        }

        /// <summary>
        /// Function for adding particles to emitters
        /// </summary>
        /// <param name="emitter"> ParticleEmitterComponent for saving and updating particles </param>
        /// <param name="settings"> ParticleSettingsEmitter </param>
        /// <param name="tran"> transformComponent for particle </param>
        /// <param name="velocity"> velocity of new particle </param>
        public void AddParticle(ParticleEmitterComponent emitter, ParticleSettingsComponent settings, TransformComponent tran, Vector3 velocity)
        {
            Random random = new Random();

            int nextFreeParticle = emitter.FirstFreeParticle + 1;

            if (nextFreeParticle >= settings.MaxParticles)
                nextFreeParticle = 0;

            if (nextFreeParticle == emitter.FirstRetiredParticle)
                return;
            velocity *= settings.EmitterVelocitySensitivity;


            float horizontalVelocity = MathHelper.Lerp(settings.MinHorizontalVelocity,
                                                       settings.MaxHorizontalVelocity,
                                                       (float)random.NextDouble());

            double horizontalAngle = random.NextDouble() * MathHelper.TwoPi;

            velocity.X += horizontalVelocity * (float)Math.Cos(horizontalAngle);
            velocity.Z += horizontalVelocity * (float)Math.Sin(horizontalAngle);

            velocity.Y += MathHelper.Lerp(settings.MinVerticalVelocity,
                                          settings.MaxVerticalVelocity,
                                          (float)random.NextDouble());
            Color randomValues = new Color((byte)random.Next(255),
                                           (byte)random.Next(255),
                                           (byte)random.Next(255),
                                           (byte)random.Next(255));

            for (int i = 0; i < 4; i++)
            {
                emitter.Particles[emitter.FirstFreeParticle * 4 + i].Position = tran.Position;
                emitter.Particles[emitter.FirstFreeParticle * 4 + i].Velocity = velocity;
                emitter.Particles[emitter.FirstFreeParticle * 4 + i].Random = randomValues;
                emitter.Particles[emitter.FirstFreeParticle * 4 + i].Time = emitter.CurrentTime;
            }

            emitter.FirstFreeParticle = nextFreeParticle;
        }

        /// <summary>
        /// Function for geting positions of the heightmap
        /// </summary>
        /// <param name="numberToCreate"> number of position </param>
        /// <returns> list with positions </returns>
        public List<Vector3> GetMapPositions(int numberToCreate)
        {
            List<Vector3> positions = new List<Vector3>();

            Random r = new Random();
            int mapWidht;
            int mapHeight;
            List<int> heightList = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponent>();
            HeightmapComponent heigt = ComponentManager.GetEntityComponent<HeightmapComponent>(heightList[0]);
            mapWidht = heigt.Width;
            mapHeight = heigt.Height;
            for (int i = 0; i < numberToCreate; i++)
            {
                var pot = new Vector3(r.Next(mapWidht - 10), 0, r.Next(mapHeight - 10));
                pot.Y = heigt.HeightMapData[(int)pot.X, (int)pot.Z];
                if (pot.X < 10)
                {
                    pot.X = pot.X + 10;
                }
                if (pot.Z < 10)
                {
                    pot.Z = pot.Z - 10;
                }
                pot.Z = -pot.Z;
                positions.Add(pot);
            }
            return positions;
        }
    }
}