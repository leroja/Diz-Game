﻿using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DizGame.Source.Components.ResourceComponent;

namespace DizGame.Source.Factories
{
    /// <summary>
    /// Factory to create resources
    /// </summary>
    public class ResourceFactory
    {
        private ContentManager Content;
        private Dictionary<string, Model> ModelDic;
        private bool VisibleBullets;

        public ResourceFactory(ContentManager Content, Dictionary<string, Model> ModelDic, bool VisibleBullets)
        {
            this.Content = Content;
            this.ModelDic = ModelDic;
            this.VisibleBullets = VisibleBullets;
        }
        public void CreateHealthResource(Vector3 position)
        {
            int newEntityId = ComponentManager.Instance.CreateID();

            //adjust the scales differently for the models if needed
            TransformComponent tcp = new TransformComponent(position, new Vector3(0.04f, 0.04f, 0.04f));
            Model model = ModelDic["Heart"];
            foreach (var modelpart in model.Meshes)
            {
                BasicEffect effect = (BasicEffect)modelpart.Effects[0];
                effect.EnableDefaultLighting();
                effect.DiffuseColor = Color.DeepPink.ToVector3();
                effect.AmbientLightColor = Color.AntiqueWhite.ToVector3();
                effect.FogEnabled = true;
                effect.FogColor = Color.LightGray.ToVector3();
                effect.FogStart = 10;
                effect.FogEnd = 400;
            }
            ModelComponent mcp = new ModelComponent(model)
            {
                IsVisible = VisibleBullets
            };

            List<IComponent> resourceCompList = new List<IComponent>
            {
                new ResourceComponent(ResourceType.Health),
                tcp,
                mcp,
            };

            ComponentManager.Instance.AddAllComponents(newEntityId, resourceCompList);
        }

        public void CreateAmmoResource(Vector3 position)
        {
            int newEntityId = ComponentManager.Instance.CreateID();
            var newPosY = position.Y + 5;
            Vector3 newTotalPos = new Vector3(position.X, newPosY, position.Z);
            //adjust the scales differently for the models if needed
            TransformComponent tcp = new TransformComponent(newTotalPos, new Vector3(1, 1, 1));

            ModelComponent mcp = new ModelComponent(ModelDic["Cartridge"])
            {
                IsVisible = VisibleBullets
            };
            foreach (var modelpart in mcp.Model.Meshes)
            {
                BasicEffect effect = (BasicEffect)modelpart.Effects[0];
                effect.EnableDefaultLighting();
                effect.FogEnabled = true;
                effect.FogColor = Color.LightGray.ToVector3();
                effect.FogStart = 10;
                effect.FogEnd = 400;
            }
            List<IComponent> resourceCompList = new List<IComponent>
            {
                new ResourceComponent(ResourceType.Ammo),
                tcp,
                mcp,
            };

            ComponentManager.Instance.AddAllComponents(newEntityId, resourceCompList);
        }
    }
}
