using ContentProject;
using ContentProject.Utils;
using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DizGame.Source.Factories
{
    /// <summary>
    /// Factory to create resources
    /// </summary>
    public class ResourceFactory
    {
        private Dictionary<string, Model> ModelDic;

        /// <summary>
        /// Constructor for creating the ResourceFactory
        /// </summary>
        /// <param name="ModelDic"> Dictionary containing the models which should represent 
        /// the different types of resources </param>
        public ResourceFactory(Dictionary<string, Model> ModelDic)
        {
            this.ModelDic = ModelDic;
        }

        /// <summary>
        /// Method for creating health resources
        /// </summary>
        /// <param name="position">This parameter should represent the position in which the 
        /// resource should be spawned</param>
        public void CreateHealthResource(Vector3 position)
        {
            int newEntityId = ComponentManager.Instance.CreateID();

            //adjust the scales differently for the models if needed
            TransformComponent tcp = new TransformComponent(position, new Vector3(0.04f, 0.04f, 0.04f));
            Model model = ModelDic["Heart"];
            BoundingVolume volume = (BoundingVolume)model.Tag;
            Util.ScaleBoundingVolume(ref volume, tcp.Scale.X * 10, tcp.Position, out BoundingVolume scaledVolume);

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
                IsVisible = EntityFactory.Instance.VisibleBullets,
                BoundingVolume = scaledVolume
            };

            List<IComponent> resourceCompList = new List<IComponent>
            {
                new ResourceComponent(ResourceType.Health),
                tcp,
                mcp,
            };

            ComponentManager.Instance.AddAllComponents(newEntityId, resourceCompList);
        }

        /// <summary>
        /// Method for creating the ammunition resources.
        /// </summary>
        /// <param name="position">This parameter should represent the position for which the resource 
        /// should be spawned.</param>
        public void CreateAmmoResource(Vector3 position)
        {
            int newEntityId = ComponentManager.Instance.CreateID();
            Model cart = ModelDic["Cartridge"];
            BoundingVolume volume = (BoundingVolume)cart.Tag;

            Util.ScaleBoundingVolume(ref volume, 1, position, out BoundingVolume scaledVolume);

            var newPosY = position.Y + Math.Abs(((BoundingBox3D)volume.Bounding).Box.Min.Y);
            Vector3 newTotalPos = new Vector3(position.X, newPosY, position.Z);
            //adjust the scales differently for the models if needed
            TransformComponent tcp = new TransformComponent(newTotalPos, new Vector3(1, 1, 1));


            ModelComponent mcp = new ModelComponent(ModelDic["Cartridge"])
            {
                IsVisible = EntityFactory.Instance.VisibleBullets,
                BoundingVolume = scaledVolume

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