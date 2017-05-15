﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.Components;
using AnimationContentClasses;

namespace GameEngine.Source.Systems
{
    public class HeightmapSystemTexture : IRender
    {
        private GraphicsDevice device;
        

        public HeightmapSystemTexture(GraphicsDevice device)
        {
            this.device = device;
        }

        public override void Draw(GameTime gameTime)
        {
            var ents = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();

            var cameraIds = ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>();
            var cameraComp = ComponentManager.GetEntityComponent<CameraComponent>(cameraIds[0]);

            foreach (int heightMapId in ents)
            {
                var heightMap = ComponentManager.GetEntityComponent<HeightmapComponentTexture>(heightMapId);
                var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(heightMapId);

                foreach (var chunk in heightMap.HeightMapChunks)
                {
                    chunk.Effect.Projection = cameraComp.Projection;
                    chunk.Effect.View = cameraComp.View;
                    chunk.Effect.World = transformComp.ObjectMatrix * Matrix.CreateTranslation(chunk.OffsetPosition);


                    BoundingBox3D box = ConvertBoundingBoxToWorldCoords(chunk.BoundingBox, chunk.Effect.World);

                    if (box.Intersects(cameraComp.CameraFrustrum))
                    {
                        device.Indices = chunk.IndexBuffer;
                        device.SetVertexBuffer(chunk.VertexBuffer);
                        foreach (EffectPass p in chunk.Effect.CurrentTechnique.Passes)
                        {
                            p.Apply();
                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.indicesDiv3);
                        }
                    }
                }
            }
        }
        
        private BoundingBox3D ConvertBoundingBoxToWorldCoords(BoundingBox box, Matrix world)
        {
            Vector3 pos = Vector3.Transform(Vector3.Zero, Matrix.Invert(world));
            BoundingBox3D b = new BoundingBox3D(new BoundingBox(box.Min - pos, box.Max - pos));
            return b;
        }
    }
}
