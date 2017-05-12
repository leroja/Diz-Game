using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.Components;

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
        /// Methos for drawing the heightmaps
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            var ents = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();

            var cameraIds = ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>();
            var cameraComp = ComponentManager.GetEntityComponent<CameraComponent>(cameraIds[0]);

            foreach (int heightMapId in ents)
            {
                var heightMap = ComponentManager.GetEntityComponent<HeightmapComponentTexture>(heightMapId);
                if (heightMap.IsVisable)
                {
                    var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(heightMapId);

                    foreach (var chunk in heightMap.HeightMapChunks)
                    {
                        chunk.Effect.Projection = cameraComp.Projection;
                        chunk.Effect.View = cameraComp.View;
                        chunk.Effect.World = transformComp.ObjectMatrix * Matrix.CreateTranslation(chunk.OffsetPosition);


                        BoundingBox box = ConvertBoundingBoxToWorldCoords(chunk.BoundingBox, chunk.Effect.World);

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
