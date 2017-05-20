using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using AnimationContentClasses;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Tools
{
    public class BoundingBoxRenderer : IRender
    {
        GraphicsDevice gDevice;

        VertexBuffer vBuffer;
        IndexBuffer iBuffer;
        VertexPositionColor[] verts = new VertexPositionColor[8];
        BasicEffect effect;
        short[] indices;

        public BoundingBoxRenderer(GraphicsDevice device)
        {
            gDevice = device;
            effect = new BasicEffect(device);
            indices = new short[]
            {
            0, 1,
            1, 2,
            2, 3,
            3, 0,
            0, 4,
            1, 5,
            2, 6,
            3, 7,
            4, 5,
            5, 6,
            6, 7,
            7, 4,
            };

            iBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, 24, BufferUsage.WriteOnly);
            iBuffer.SetData(indices);
            vBuffer = new VertexBuffer(device, VertexPositionColor.VertexDeclaration, 8, BufferUsage.WriteOnly);

        }

        public override void Draw(GameTime gameTime)
        {
            List<int> entities = ComponentManager.GetAllEntitiesWithComponentType<ModelComponent>();
            foreach (int ent in entities)
            {
                ModelComponent mc = ComponentManager.GetEntityComponent<ModelComponent>(ent);

                if (mc.BoundingVolume != null && mc.BoundingVolume.Bounding is BoundingBox3D)
                    Draw(mc, new Color(new Vector3(255, 0, 0)));
            }

        }
        private void Draw(ModelComponent mc, Color color)
        {
            if (mc.BoundingVolume.Bounding != null)
            {
                BoundingBox3D box = mc.BoundingVolume.Bounding as BoundingBox3D;
                int ent = ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>()[0];
                CameraComponent cc = ComponentManager.GetEntityComponent<CameraComponent>(ent);
                Vector3[] corners = box.Box.GetCorners();
                for (int i = 0; i < 8; i++)
                {
                    verts[i].Position = corners[i];
                    verts[i].Color = color;
                }
                effect.EnableDefaultLighting();
                effect.View = cc.View;
                effect.Projection = cc.Projection;
                foreach(EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    gDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, indices.Length / 2);
                }
            }
        }   
    }
}
