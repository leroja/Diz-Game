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
    public class PlayerSystem : IUpdate
    {
        public override void Update(GameTime gameTime)
        {
            var PlayerEntityIds = ComponentManager.GetAllEntitiesWithComponentType<PlayerComponent>();

            foreach (var playerId in PlayerEntityIds)
            {
                var playerComp = ComponentManager.GetEntityComponent<PlayerComponent>(playerId);
                var mouseComp = ComponentManager.GetEntityComponent<MouseComponent>(playerId);
                var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(playerId);

                var rot = transformComp.Rotation;
                rot.X = 0;
                rot.Y = 0;
                rot.Z = 0;

                if (mouseComp.MouseDeltaPosition.X > 0)
                {
                    rot.Y += 0.01f;
                }
                if (mouseComp.MouseDeltaPosition.X < 0)
                {
                    rot.Y -= 0.01f;
                }
                if (mouseComp.MouseDeltaPosition.Y > 0)
                {
                    rot.Z += 0.05f;
                }
                if (mouseComp.MouseDeltaPosition.Y < 0)
                {
                    rot.Z -= 0.05f;
                }
            }
        }
    }
}
