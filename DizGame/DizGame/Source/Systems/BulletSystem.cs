using GameEngine.Source.Systems;
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
    // temporär
    public class BulletSystem : IUpdate
    {
        public override void Update(GameTime gameTime)
        {
            var compIds = ComponentManager.GetAllEntitiesWithComponentType<BulletComponent>();
            foreach (var id in compIds)
            {
                var bulletComponent = ComponentManager.GetEntityComponent<BulletComponent>(id);
                var mouseComp = ComponentManager.GetEntityComponent<MouseComponent>(id);
                var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(id);

                //System.Console.WriteLine(mouseComp.MouseDeltaPosition);



                //transformComp.Rotation = /*transformComp.Rotation +*/ new Vector3(-mouseComp.MouseDeltaPosition.Y, -mouseComp.MouseDeltaPosition.X, 0) * mouseComp.MouseSensitivity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                var rot = transformComp.Rotation;
                rot.X = 0;
                rot.Y = 0;
                rot.Z = 0;

                if (mouseComp.MouseDeltaPosition.X > 0)
                {
                    rot.X += 0.01f;
                }
                if (mouseComp.MouseDeltaPosition.X < 0)
                {
                    rot.X -= 0.01f;
                }
                if (mouseComp.MouseDeltaPosition.Y > 0)
                {
                    rot.Y += 0.01f;
                }
                if (mouseComp.MouseDeltaPosition.Y < 0)
                {
                    rot.Y -= 0.01f;
                }

                transformComp.Rotation = rot;
            }
        }
    }
}
