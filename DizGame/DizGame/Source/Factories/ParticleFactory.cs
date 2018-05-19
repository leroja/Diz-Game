using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Factories
{
    /// <summary>
    /// 
    /// </summary>
    public class ParticleFactory
    {
        private ContentManager Content;
        private Dictionary<string, Texture2D> Texture2dDic;

        /// <summary>
        /// 
        /// Constructor
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="Texture2dDi"></param>
        public ParticleFactory(ContentManager Content, Dictionary<string, Texture2D> Texture2dDi)
        {
            this.Content = Content;
            this.Texture2dDic = Texture2dDi;
        }


        #region particles
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="typeOfParticle"></param>
        /// <param name="EmitterLifeTime"></param>
        public void CreateParticleEmitter(Vector3 Position, string typeOfParticle, float EmitterLifeTime)
        {
            switch (typeOfParticle)
            {
                case "Smoke":
                    ParticleSettingsComponent setting = CreateSmokeSettings();
                    ParticleEmitterComponent emitter = new ParticleEmitterComponent(GameOne.Instance.GraphicsDevice, 6000)
                    {
                        ParticleEffect = Content.Load<Effect>("Effects//ParticleEffect"),
                        LifeTime = 30,
                        ParticleType = "Smoke"
                    };
                    TransformComponent tran = new TransformComponent()
                    {
                        Position = Position
                    };
                    setting.Duration = TimeSpan.FromSeconds(EmitterLifeTime - 1);
                    var a = ComponentManager.Instance.CreateID();
                    ComponentManager.Instance.AddComponentToEntity(a, setting);
                    ComponentManager.Instance.AddComponentToEntity(a, emitter);
                    ComponentManager.Instance.AddComponentToEntity(a, tran);
                    break;
                case "Blood":
                    ParticleSettingsComponent settingB = CreateBloodSettings();
                    ParticleEmitterComponent emitterB = new ParticleEmitterComponent(GameOne.Instance.GraphicsDevice, 600)
                    {
                        ParticleEffect = Content.Load<Effect>("Effects//ParticleEffect"),
                        LifeTime = 0.3f,
                        ParticleType = "Blood"
                    };
                    TransformComponent tranB = new TransformComponent()
                    {
                        Position = Position
                    };
                    settingB.Duration = TimeSpan.FromSeconds(EmitterLifeTime - 1);
                    var aB = ComponentManager.Instance.CreateID();
                    ComponentManager.Instance.AddComponentToEntity(aB, settingB);
                    ComponentManager.Instance.AddComponentToEntity(aB, emitterB);
                    ComponentManager.Instance.AddComponentToEntity(aB, tranB);
                    break;

                default:
                    break;
            }


        }
        /// <summary>
        /// Sets settings for Smoke Particles
        /// </summary>
        /// <returns>settings for smoke particles</returns>
        private ParticleSettingsComponent CreateBloodSettings()
        {
            ParticleSettingsComponent setting = new ParticleSettingsComponent()
            {
                Texture = Texture2dDic["Smoke"],
                MaxParticles = 600,
                Duration = TimeSpan.FromSeconds(10),
                MinHorizontalVelocity = -20,
                MaxHorizontalVelocity = 20,
                MinVerticalVelocity = -5,
                MaxVerticalVelocity = 5,
                Gravity = new Vector3(0, 0, 0),
                EndVelocity = 1,
                MaxColor = Color.Red,
                MinColor = Color.Red,
                MinRotateSpeed = -1,
                MaxRotateSpeed = 1,
                MinStartSize = 4,
                MaxStartSize = 7,
                MinEndSize = 10,
                MaxEndSize = 50
            };
            return setting;
        }

        /// <summary>
        /// sets settings for blood particles
        /// </summary>
        /// <returns>settings for blood particles</returns>
        private ParticleSettingsComponent CreateSmokeSettings()
        {
            ParticleSettingsComponent setting = new ParticleSettingsComponent()
            {
                Texture = Texture2dDic["Smoke"],
                MaxParticles = 6000,
                Duration = TimeSpan.FromSeconds(10),
                MinHorizontalVelocity = 0,
                MaxHorizontalVelocity = 15,
                MinVerticalVelocity = 10,
                MaxVerticalVelocity = 20,
                Gravity = new Vector3(-20, -5, 0),
                EndVelocity = 1,
                MinRotateSpeed = -1,
                MaxRotateSpeed = 1,
                MinStartSize = 4,
                MaxStartSize = 50,
                MinEndSize = 35,
                MaxEndSize = 140
            };
            return setting;
        }
        #endregion

    }
}
