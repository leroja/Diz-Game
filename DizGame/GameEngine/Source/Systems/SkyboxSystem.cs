using System;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// A system for rendering skyboxes
    /// </summary>
    public class SkyboxSystem : IRender
    {
        WorldComponent world;
        CameraComponent defaultCam;
        int defaultCamID;
        GraphicsDevice device;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="device"></param>
        public SkyboxSystem(GraphicsDevice device)
        {
            this.device = device;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>();
            world = ComponentManager.GetEntityComponent<WorldComponent>(temp.First());
            //Check for all entities with a camera
            List<int> entitiesWithCamera = ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>();
            defaultCamID = entitiesWithCamera.First();
            //pick one
            defaultCam = ComponentManager.GetEntityComponent<CameraComponent>(defaultCamID);

            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<SkyBoxComponent>())
                RenderSkyBox(entityID);
        }

        /// <summary>
        /// Renders a skybox
        /// </summary>
        /// <param name="EntityID"> The entityID of the skybox </param>
        private void RenderSkyBox(int EntityID)
        {
            SkyBoxComponent skybox = ComponentManager.GetEntityComponent<SkyBoxComponent>(EntityID);
            Effect skyBoxEffect = skybox.SkyboxEffect;
            TransformComponent tcp = ComponentManager.GetEntityComponent<TransformComponent>(EntityID);
            TransformComponent trandXomp = ComponentManager.GetEntityComponent<TransformComponent>(defaultCam.ID);

            var old_rs = device.RasterizerState;
            device.RasterizerState = new RasterizerState { CullMode = CullMode.CullClockwiseFace };

            // Go through each pass in the effect, but we know there is only one...
            foreach (EffectPass pass in skyBoxEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                // Draw all of the components of the mesh, but we know the cube really
                // only has one mesh
                foreach (ModelMesh mesh in skybox.SkyboxModel.Meshes)
                {
                    // Assign the appropriate values to each of the parameters
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = skyBoxEffect;
                        part.Effect.Parameters["World"].SetValue(
                            Matrix.CreateScale(skybox.Size) * Matrix.CreateTranslation(trandXomp.Position));
                        part.Effect.Parameters["View"].SetValue(defaultCam.View);
                        part.Effect.Parameters["Projection"].SetValue(defaultCam.Projection);
                        part.Effect.Parameters["SkyBoxTexture"].SetValue(skybox.SkyboxTextureCube);
                        part.Effect.Parameters["CameraPosition"].SetValue(trandXomp.Position);
                    }

                    // Draw the mesh with the skybox effect
                    mesh.Draw();
                }
            }

            device.RasterizerState = old_rs;
        }
    }
}