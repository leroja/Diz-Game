using GameEngine.Source.Systems;
using System.Linq;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// Updates the texture components of the entity which has the HUD
    /// </summary>
    public class HudSystem : IUpdate
    {
        /// <summary>
        /// Updates the texture components of the HUD
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var entIDs = ComponentManager.GetAllEntitiesWithComponentType<HudComponent>();

            foreach (var entityId in entIDs)
            {
                var hudComp = ComponentManager.GetEntityComponent<HudComponent>(entityId);
                var healthComp = ComponentManager.GetEntityComponent<HealthComponent>(hudComp.TrackedEntity);
                var AmmoComp = ComponentManager.GetEntityComponent<AmmunitionComponent>(hudComp.TrackedEntity);

                var textComp = ComponentManager.GetEntityComponent<TextComponent>(entityId);
                textComp.Children.First().Value.Text = healthComp.Health.ToString();
                textComp.Children["Ammunition"].Text = AmmoComp.CurrentAmmoInMag + "/" + AmmoComp.MaxAmmoInMag + " " + " Clips left: " + AmmoComp.AmmountOfActiveMagazines;
            }
        }
    }
}
