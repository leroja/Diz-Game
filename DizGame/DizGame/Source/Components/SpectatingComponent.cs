using DizGame.Source.Factories;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DizGame.Source.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class SpectatingComponent : IComponent
    {
        /// <summary>
        /// The id of the currently spectated entity
        /// </summary>
        public int SpectatedEntity { get; set; }
        /// <summary>
        /// A time value in seconds that acts as a buffer zone
        /// before the ability to spectate kicks in. 
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SpectatingComponent()
        {
            Time = 5;
            //var available = ComponentManager.Instance.GetAllEntitiesWithComponentType<PlayerComponent>();
            //var availableAis = ComponentManager.Instance.GetAllEntitiesWithComponentType<AIComponent>();

            //List<int> availableEnts = available.Concat(availableAis).ToList();

            //SpectatedEntity = availableEnts.FirstOrDefault();
            //EntityFactory.Instance.RemoveCam();
            //EntityFactory.Instance.AddChaseCamToEntity(SpectatedEntity, new Vector3(0, 10, 25));

            //var hud = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<HudComponent>().FirstOrDefault();
            //var hudComp = (HudComponent)hud.Value;
            //hudComp.TrackedEntity = SpectatedEntity;

            //var textComp = ComponentManager.Instance.GetEntityComponent<Texture2DComponent>(hud.Key);
            //ComponentManager.Instance.RemoveComponentFromEntity(hud.Key, textComp);
        }
    }
}