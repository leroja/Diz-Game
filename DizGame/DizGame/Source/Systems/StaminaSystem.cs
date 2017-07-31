using GameEngine.Source.Systems;
using System;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// 
    /// </summary>
    public class StaminaSystem : IUpdate
    {
        //TODO: refine the replenish and drain of stamina
        private const float StaminaDrain = 5;
        private const float StaminaReplenish = 15;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var ids = ComponentManager.GetAllEntitiesWithComponentType<StaminaComponent>();

            foreach (var id in ids)
            {
                var staminaComp = ComponentManager.GetEntityComponent<StaminaComponent>(id);

                if (staminaComp.IsRunning)
                {
                    // decrease stamina
                    // drains faster when the stamina is low
                    var stamina = (StaminaDrain / staminaComp.StaminaRatio) * gameTime.ElapsedGameTime.TotalSeconds;
                    staminaComp.CurrentStamina -= (float)stamina;
                    if (staminaComp.CurrentStamina < 0)
                        staminaComp.CurrentStamina = 0.9f;

                }
                else if (!staminaComp.IsRunning && staminaComp.CurrentStamina < staminaComp.MaximumStamina)
                {
                    // increase stamina
                    // increases faster when then stamina is higher
                    var stamina = (StaminaReplenish * staminaComp.StaminaRatio) * gameTime.ElapsedGameTime.TotalSeconds;
                    staminaComp.CurrentStamina += (float)stamina;

                    if (staminaComp.CurrentStamina > staminaComp.MaximumStamina)
                        staminaComp.CurrentStamina = staminaComp.MaximumStamina;
                }

                staminaComp.StaminaRatio = staminaComp.CurrentStamina / staminaComp.MaximumStamina;
            }
        }
    }
}