using GameEngine.Source.Systems;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using AnimationContentClasses;

namespace GameEngine.Tools
{
    public class BoundingBoxRenderer : IRender
    {
        #region Fields

        VertexPositionColor[] verts = new VertexPositionColor[8];
        IndexBuffer iBuffer;
        VertexBuffer vBuffer;
        short[] indices = new short[]
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

        BasicEffect effect;
        VertexDeclaration vertDecl;
        GraphicsDevice graphicsDevice;

        public BoundingBoxRenderer(GraphicsDevice device)
        {
            graphicsDevice = device;
            iBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);
            iBuffer.SetData(indices);
            vBuffer = new VertexBuffer(device, VertexPositionColor.VertexDeclaration, 8, BufferUsage.WriteOnly);
        }

        public override void Draw(GameTime gameTime)
        {
            var dict = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<ModelComponent>();

            foreach (var modEnt in dict)
            {
                ModelComponent modC = (ModelComponent)modEnt.Value;
                if (modC.BoundingVolume != null)
                {
                    if (modC.BoundingVolume.Bounding is BoundingBox3D)
                    {
                        DrawBox((BoundingBox3D)modC.BoundingVolume.Bounding, Color.Red);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Renders the bounding box for debugging purposes.
        /// </summary>
        /// <param name="box">The box to render.</param>
        /// <param name="graphicsDevice">The graphics device to use when rendering.</param>
        /// <param name="view">The current view matrix.</param>
        /// <param name="projection">The current projection matrix.</param>
        /// <param name="color">The color to use drawing the lines of the box.</param>
        public void DrawBox(
            BoundingBox3D box,
            Color color)
        {
            int ent = ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>()[0];
            CameraComponent cc = ComponentManager.GetEntityComponent<CameraComponent>(ent);
            if (effect == null)
            {
                effect = new BasicEffect(graphicsDevice);
                effect.VertexColorEnabled = true;
                effect.LightingEnabled = false;
                //effect.EnableDefaultLighting();
            }

            Vector3[] corners = box.Box.GetCorners();
            for (int i = 0; i < 8; i++)
            {
                verts[i].Position = corners[i];
                verts[i].Color = color;
            }
            vBuffer.SetData(verts);
            effect.View = cc.View;
            effect.Projection = cc.Projection;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                graphicsDevice.SetVertexBuffer(vBuffer);
                graphicsDevice.Indices = iBuffer;
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.LineList,
                    0,
                    0,
                    indices.Length / 2);
            }
        }
    }
}
