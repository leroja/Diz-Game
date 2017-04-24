﻿using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame
{
    public class EntityFactory
    {
        private ContentManager Content;
        public EntityFactory(ContentManager Content)
        {
            this.Content = Content;
        }
        /// <summary>
        /// Method for creating one of the many (maybe) player models. 
        /// </summary>
        public void CreateSexyWomanSoldier()
        {
            int entityID = ComponentManager.Instance.CreateID();
            Model sonya = Content.Load<Model>("sonya/Sonya");


            List<IComponent> components = new List<IComponent>
            {
                new TransformComponent(new Vector3(0,0,0), new Vector3(0.07f,0.07f,0.07f)),
                new ModelComponent(sonya),
                new WorldComponent(Matrix.Identity),
                new CameraComponent(CameraType.Chase)
            };

            //TODO: need to add keyboard components and such to assign controllers to the model.

            ComponentManager.Instance.AddAllComponents(entityID, components);
        }

        public void CreateBullet(Model model)
        {
            int BulletEntity = ComponentManager.Instance.CreateID();

            List<IComponent> componentList = new List<IComponent>()
            {

            };
            
        }
    }
}