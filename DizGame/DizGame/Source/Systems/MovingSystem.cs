using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;

namespace DizGame.Source.Systems
{
    public class MovingSystem : IUpdate
    {
        public override void Update(GameTime gameTime)
        {
            Dictionary<int,IComponent> EntityDict = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<KeyBoardComponent>();

            foreach(var entity in EntityDict)
            {
                KeyBoardComponent key = ComponentManager.GetEntityComponent<KeyBoardComponent>(entity.Key);
                TransformComponent trans = ComponentManager.GetEntityComponent<TransformComponent>(entity.Key);

                if (key.State["Forward"] == ButtonStates.Hold)
                {
                    trans.Position += trans.Forward * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (key.State["Backwards"] == ButtonStates.Hold)
                {
                    trans.Position -= trans.Forward * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (key.State["Left"] == ButtonStates.Hold)
                {
                    trans.Position -= trans.Right * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (key.State["Right"] == ButtonStates.Hold)
                {
                    trans.Position += trans.Right * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

            }
        }
    }
}
