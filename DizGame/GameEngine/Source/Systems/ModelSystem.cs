﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// ModelSystem class is responsible for handling all entities with an component type of modelComponent, 
    /// which are renderable objects. 
    /// In other words this class contains logic for drawing these kind of objects.
    /// </summary>
    public class ModelSystem : IRender
    {
        WorldComponent world;
        CameraComponent defaultCam;
        int defaultCamID;

        GraphicsDevice device;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="device"></param>
        public ModelSystem(GraphicsDevice device)
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

            var ents = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<ModelComponent>();
            foreach (var ent in ents)
            {
                DrawModel(ent);
            }
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<SkyBoxComponent>())
                RenderSkyBox(entityID);

            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<HardwareInstancedComponent>())
                RenderHardwareInstanced(entityID);
        }



        /// <summary>
        /// Renders from one copy in hardware to several places on map.
        /// </summary>
        /// <param name="entityID">The id to render.</param>
        private void RenderHardwareInstanced(int entityID)
        {
            HardwareInstancedComponent hwComponent = ComponentManager.GetEntityComponent<HardwareInstancedComponent>(entityID);
            TransformComponent transform = ComponentManager.GetEntityComponent<TransformComponent>(entityID);

            hwComponent.GraphicsDevice.Indices = hwComponent.IndexBuffer;

            hwComponent.GraphicsDevice.SetVertexBuffers(hwComponent.Bindings);

            hwComponent.Effect.Parameters["World"].SetValue(Matrix.Identity);
            hwComponent.Effect.Parameters["View"].SetValue(defaultCam.View);
            hwComponent.Effect.Parameters["Projection"].SetValue(defaultCam.Projection);
            hwComponent.Effect.Parameters["Texture"].SetValue(hwComponent.Texture);

            hwComponent.GraphicsDevice.RasterizerState = hwComponent.RasterizerState;

            foreach (EffectPass pass in hwComponent.Effect.Techniques[1/*(int)ShaderTechnique.InstancedTechnique*/].Passes)
            {
                pass.Apply();

                hwComponent.GraphicsDevice.DrawInstancedPrimitives(
                    PrimitiveType.LineList, 0, 0, hwComponent.VertexBuffer.VertexCount, 0,
                    hwComponent.IndexBuffer.IndexCount / hwComponent.IndicesPerPrimitive, hwComponent.InstanceCount);

            }
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

        /// <summary>
        /// Updates all the models and transforms and places the bones on right positions using CopyAbsoluteBoneTranformsTo
        /// applies properties to the effects and then draw the parts.
        /// </summary>
        /// <param name="ent"></param>
        private void DrawModel(KeyValuePair<int, IComponent> ent)
        {
            var model = (ModelComponent)ent.Value;
            if (model.IsVisible)
            {
                TransformComponent transform = ComponentManager.GetEntityComponent<TransformComponent>(ent.Key);
                if (!ComponentManager.CheckIfEntityHasComponent<AnimationComponent>(ent.Key))
                {
                    if (defaultCam.CameraFrustrum.Intersects(model.BoundingVolume.Bounding))
                    {
                        if (model.MeshWorldMatrices == null || model.MeshWorldMatrices.Length < model.Model.Bones.Count)
                            model.MeshWorldMatrices = new Matrix[model.Model.Bones.Count];

                        model.Model.CopyAbsoluteBoneTransformsTo(model.MeshWorldMatrices);
                        foreach (ModelMesh mesh in model.Model.Meshes)
                        {
                            foreach (BasicEffect effect in mesh.Effects)
                            {
                                effect.World = model.MeshWorldMatrices[mesh.ParentBone.Index] * transform.ObjectMatrix * world.World;

                                effect.View = defaultCam.View;
                                effect.Projection = defaultCam.Projection;

                                if (world != null && world.IsSunActive)
                                {
                                    FlareComponent flare = ComponentManager.GetEntityComponent<FlareComponent>(world.ID);
                                    effect.LightingEnabled = true;

                                    //effect.DiffuseColor = flare.Diffuse;
                                    //effect.AmbientLightColor = flare.AmbientLight;

                                    effect.DirectionalLight0.Enabled = true;
                                    effect.DirectionalLight0.DiffuseColor = flare.Diffuse;
                                    effect.DirectionalLight0.Direction = flare.LightDirection;
                                }

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
                else
                {
                    DrawAnimation(ent.Key, model);
                }
            }
        }

        /// <summary>
        /// Draws a model with an animation
        /// </summary>
        /// <param name="entityID">  </param>
        /// <param name="model">  </param>
        private void DrawAnimation(int entityID, ModelComponent model)
        {
            AnimationComponent anm = ComponentManager.GetEntityComponent<AnimationComponent>(entityID);
            Matrix[] bones = anm.SkinTransforms;

            foreach (ModelMesh mesh in model.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(anm.SkinTransforms);

                    effect.View = defaultCam.View;
                    effect.Projection = defaultCam.Projection;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;

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