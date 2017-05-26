using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.Components;
using AnimationContentClasses;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// A system that is used for drawing height maps
    /// </summary>
    public class HeightmapSystemTexture : IRender
    {
        private GraphicsDevice device;

        /// <summary>
        /// A constructor that takes a GrapicsDevice as a parameter
        /// </summary>
        /// <param name="device"></param>
        public HeightmapSystemTexture(GraphicsDevice device)
        {
            this.device = device;
        }

        /// <summary>
        /// Method for drawing the heightmap
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            var ents = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();

            List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>();
            WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(temp.First());

            var cameraIds = ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>();
            var cameraComp = ComponentManager.GetEntityComponent<CameraComponent>(cameraIds.FirstOrDefault());

            foreach (int heightMapId in ents)
            {
                var heightMap = ComponentManager.GetEntityComponent<HeightmapComponentTexture>(heightMapId);
                if (heightMap.IsVisible)
                {
                    var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(heightMapId);

                    foreach (var chunk in heightMap.HeightMapChunks)
                    {
                        chunk.Effect.Projection = cameraComp.Projection;
                        chunk.Effect.View = cameraComp.View;
                        chunk.Effect.World = transformComp.ObjectMatrix * Matrix.CreateTranslation(chunk.OffsetPosition);

                        if (world != null && world.IsSunActive)
                        {
                            FlareComponent flare = ComponentManager.GetEntityComponent<FlareComponent>(world.ID);
                            chunk.Effect.LightingEnabled = true;
                            //chunk.Effect.DiffuseColor = flare.Diffuse;
                            //chunk.Effect.AmbientLightColor = flare.AmbientLight;
                            chunk.Effect.DirectionalLight0.Enabled = true;
                            chunk.Effect.DirectionalLight0.DiffuseColor = flare.Diffuse;
                            chunk.Effect.DirectionalLight0.Direction = flare.LightDirection;
                        }

                        BoundingBox3D box = new BoundingBox3D(ConvertBoundingBoxToWorldCoords(chunk.BoundingBox, chunk.Effect.World));

                        if (box.Intersects(cameraComp.CameraFrustrum))
                        {
                            device.Indices = chunk.IndexBuffer;
                            device.SetVertexBuffer(chunk.VertexBuffer);
                            foreach (EffectPass p in chunk.Effect.CurrentTechnique.Passes)
                            {
                                p.Apply();
                                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.IndicesDiv3);
                            }
                        }
                    }
                }

            }
        }

        // todo borde bara behöva göras en gång, kanske när de skapas
        /// <summary>
        /// Recalculates/moves the boudning box to its correct placemnet in the world
        /// </summary>
        /// <param name="box"> The boundingBox of the object </param>
        /// <param name="world"> The WorldMatrix of an object </param>
        /// <returns></returns>
        private BoundingBox ConvertBoundingBoxToWorldCoords(BoundingBox box, Matrix world)
        {
            Vector3 pos = Vector3.Transform(Vector3.Zero, Matrix.Invert(world));
            BoundingBox b = new BoundingBox(box.Min - pos, box.Max - pos);
            return b;
        }
    }
}
