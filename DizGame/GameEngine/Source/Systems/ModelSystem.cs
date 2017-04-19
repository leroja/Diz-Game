using GameEngine.Source.Systems.Abstract_classes;
using System;
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
            DrawModel();
        }
        /// <summary>
        /// Updates all the models and transforms and places the bones on right positions using CopyAbsoluteBoneTranformsTo
        /// applies properties to the effects and then draw the parts.
        /// </summary>
        private void DrawModel()
        {
            WorldComponent world = (WorldComponent)ComponentManager.GetAllEntitiesAndComponentsWithComponentType<WorldComponent>()[0];
           
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<ModelComponent>())
            {
                ModelComponent model = ComponentManager.GetEntityComponent<ModelComponent>(entityID);
                TransformComponent transform = ComponentManager.GetEntityComponent<TransformComponent>(entityID);
                CameraComponent camera = ComponentManager.GetEntityComponent<CameraComponent>(entityID);

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
            }
        }
    }
}
