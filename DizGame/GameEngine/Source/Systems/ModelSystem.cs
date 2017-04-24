﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Systems
{
    public class ModelSystem : IRender
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<ModelComponent>())
            {
                DrawModel(entityID);
            }
        }
        /// <summary>
        /// Updates all the models and transforms and places the bones on right positions using CopyAbsoluteBoneTranformsTo
        /// applies properties to the effects and then draw the parts.
        /// </summary>
        /// <param name="entityID"></param>
        private void DrawModel(int entityID)
        {
            List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>();
            WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(temp.First());
           

                ModelComponent model = ComponentManager.GetEntityComponent<ModelComponent>(entityID);
                TransformComponent transform = ComponentManager.GetEntityComponent<TransformComponent>(entityID);
                CameraComponent camera = ComponentManager.GetEntityComponent<CameraComponent>(entityID);

            //if (camera.CameraFrustrum.Intersects(model.BoundingSphere))
            //{

                if (model.MeshWorldMatrices == null || model.MeshWorldMatrices.Length < model.Model.Bones.Count)
                    model.MeshWorldMatrices = new Matrix[model.Model.Bones.Count];

                model.Model.CopyAbsoluteBoneTransformsTo(model.MeshWorldMatrices);
                foreach (ModelMesh mesh in model.Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = model.MeshWorldMatrices[mesh.ParentBone.Index] * transform.ObjectMatrix * world.World;

                        effect.View = camera.View;
                        effect.Projection = camera.Projection;

                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                        {
                            pass.Apply();
                            mesh.Draw();
                        }
                    }
                }
            //}
        }
    }
}