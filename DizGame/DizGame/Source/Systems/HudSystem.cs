﻿using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// Updates the texture components of the entity which has the hud
    /// </summary>
    public class HudSystem : IUpdate
    {
        /// <summary>
        /// Updates the tecture components of the hud
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var entIDs = ComponentManager.GetAllEntitiesWithComponentType<HudComponent>();
            
            foreach (var entityId in entIDs)
            {
                var hudComp = ComponentManager.GetEntityComponent<HudComponent>(entityId);
                var healthComp = ComponentManager.GetEntityComponent<HealthComponent>(hudComp.TrackedEntity);
                var AmmoComp = ComponentManager.GetEntityComponent<AmmunitionComponent>(entityId); // todo ändra sen när vi börjar använda ammo comp

                var textComp = ComponentManager.GetEntityComponent<TextComponent>(entityId);
                textComp.Children.First().Value.Text = healthComp.Health.ToString();
            }
        }
    }
}