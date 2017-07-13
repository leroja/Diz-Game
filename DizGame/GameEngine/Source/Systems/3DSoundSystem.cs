using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Audio;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// 
    /// </summary>
    public class _3DSoundSystem : IUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //var temp = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<_3DAudioListenerComponent>();
            //var ListenerID = temp.First().Key;
            //var listenerTransComp = ComponentManager.GetEntityComponent<TransformComponent>(ListenerID);

            //var soundEffectCompIDs = ComponentManager.GetAllEntitiesWithComponentType<_3DSoundEffectComponent>();

            //foreach (var Id in soundEffectCompIDs)
            //{
            //    var soundEffectComponent = ComponentManager.GetEntityComponent<_3DSoundEffectComponent>(Id);
            //    var transComp = ComponentManager.GetEntityComponent<TransformComponent>(Id);
            //    var physComp = ComponentManager.GetEntityComponent<PhysicsComponent>(Id);

            //    foreach (var soundEffect in soundEffectComponent.SoundEffectsToBePlayed)
            //    {

            //    }
            //    soundEffectComponent.SoundEffectsToBePlayed.Clear();
            //}
        }
    }
}
