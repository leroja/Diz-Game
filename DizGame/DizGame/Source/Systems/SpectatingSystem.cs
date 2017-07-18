using GameEngine.Source.Systems;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using DizGame.Source.Factories;
using Microsoft.Xna.Framework.Input;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// A system that enables the spectating of other players/AIs
    /// probably only works in single player
    /// </summary>
    public class SpectatingSystem : IUpdate
    {
        private KeyboardState prevState;
        private KeyboardState curState;

        /// <summary>
        /// Might not be an elegant solution but it works.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            prevState = curState;
            curState = Keyboard.GetState();

            var ids = ComponentManager.GetAllEntitiesWithComponentType<SpectatingComponent>();

            foreach (var id in ids)
            {
                var SpecComp = ComponentManager.GetEntityComponent<SpectatingComponent>(id);
                var keyboardComp = ComponentManager.GetEntityComponent<KeyBoardComponent>(id);

                if (SpecComp.Time < 0)
                {
                    var available = ComponentManager.GetAllEntitiesWithComponentType<PlayerComponent>();
                    var availableAis = ComponentManager.GetAllEntitiesWithComponentType<AIComponent>();

                    available.AddRange(availableAis);

                    if (keyboardComp.GetState("SpectateUp") == ButtonStates.Pressed)
                    {
                        int index = available.FindIndex(a => a == SpecComp.SpectatedEntity);
                        if (index + 1 == available.Count)
                        {
                            SpecComp.SpectatedEntity = available.ElementAt(0);
                        }
                        else
                        {
                            SpecComp.SpectatedEntity = available.ElementAt(index + 1);
                        }
                        EntityFactory.Instance.RemoveCam();
                        EntityFactory.Instance.AddChaseCamToEntity(SpecComp.SpectatedEntity, new Vector3(0, 10, 25));
                        var hud = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HudComponent>().FirstOrDefault();
                        var hudComp = (HudComponent)hud.Value;
                        hudComp.TrackedEntity = SpecComp.SpectatedEntity;
                    }

                    if (keyboardComp.GetState("SpectateDown") == ButtonStates.Pressed)
                    {
                        int index = available.FindIndex(a => a == SpecComp.SpectatedEntity);
                        if (index == 0)
                        {
                            SpecComp.SpectatedEntity = available.ElementAt(available.Count - 1);
                        }
                        else
                        {
                            SpecComp.SpectatedEntity = available.ElementAt(index - 1);
                        }
                        EntityFactory.Instance.RemoveCam();
                        EntityFactory.Instance.AddChaseCamToEntity(SpecComp.SpectatedEntity, new Vector3(0, 10, 25));
                        var hud = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HudComponent>().FirstOrDefault();
                        var hudComp = (HudComponent)hud.Value;
                        hudComp.TrackedEntity = SpecComp.SpectatedEntity;
                    }
                }
                else
                {
                    SpecComp.Time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (SpecComp.Time < 0)
                    {
                        var available = ComponentManager.GetAllEntitiesWithComponentType<PlayerComponent>();
                        var availableAis = ComponentManager.GetAllEntitiesWithComponentType<AIComponent>();

                        List<int> availableEnts = available.Concat(availableAis).ToList();

                        SpecComp.SpectatedEntity = availableEnts.FirstOrDefault();
                        EntityFactory.Instance.RemoveCam();
                        EntityFactory.Instance.AddChaseCamToEntity(SpecComp.SpectatedEntity, new Vector3(0, 10, 25));

                        var hud = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HudComponent>().FirstOrDefault();
                        var hudComp = (HudComponent)hud.Value;
                        hudComp.TrackedEntity = SpecComp.SpectatedEntity;

                        var textComp = ComponentManager.GetEntityComponent<Texture2DComponent>(hud.Key);
                        ComponentManager.RemoveComponentFromEntity(hud.Key, textComp); // removes the AK texture
                    }
                }
            }
        }
    }
}