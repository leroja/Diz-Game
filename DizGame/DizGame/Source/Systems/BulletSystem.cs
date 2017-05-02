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
    // TODO temporär, ta bort sen
    public class BulletSystem : IUpdate
    {
        private List<int> toDelete = new List<int>();

        public override void Update(GameTime gameTime)
        {
            var compIds = ComponentManager.GetAllEntitiesWithComponentType<BulletComponent>();
            //System.Console.WriteLine(compIds.Count);
            foreach (var id in compIds)
            {
                var bulletComponent = ComponentManager.GetEntityComponent<BulletComponent>(id);
                var mouseComp = ComponentManager.GetEntityComponent<MouseComponent>(id);
                var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(id);


                //transformComp.Rotation = /*transformComp.Rotation +*/ new Vector3(-mouseComp.MouseDeltaPosition.Y, -mouseComp.MouseDeltaPosition.X, 0) * mouseComp.MouseSensitivity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                //var rot = transformComp.Rotation;
                //rot.X = 0;
                //rot.Y = 0;
                //rot.Z = 0;

                //if (mouseComp.MouseDeltaPosition.X > 0)
                //{
                //    rot.Y += 0.01f;
                //}
                //if (mouseComp.MouseDeltaPosition.X < 0)
                //{
                //    rot.Y -= 0.01f;
                //}
                //if (mouseComp.MouseDeltaPosition.Y > 0)
                //{
                //    rot.Z += 0.05f;
                //}
                //if (mouseComp.MouseDeltaPosition.Y < 0)
                //{
                //    rot.Z -= 0.05f;
                //}
                var curPos = transformComp.Position;

                var curRange = Vector3.Distance(transformComp.Position, bulletComponent.StartPos);
                if (curRange > bulletComponent.MaxRange)
                {
                    toDelete.Add(id);
                }
                
                transformComp.Position += transformComp.Forward *(float)100 *(float)gameTime.ElapsedGameTime.TotalSeconds;
                //transformComp.Rotation = Vector3.Zero;

                //transformComp.Rotation = rot;
            }

            foreach (var id in toDelete)
            {
                ComponentManager.RemoveEntity(id);
                ComponentManager.RecycleID(id);
            }
            toDelete.Clear();
        }
    }
}
