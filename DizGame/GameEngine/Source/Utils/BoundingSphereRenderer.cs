using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;
using GameEngine.Source.Systems;
using ContentProject;

namespace GameEngine.Source.Utils
{
    /// <summary>
    /// A system for rendering BoundingSpheres
    /// </summary>
    public class BoundingSphereRenderer : IRender
    {
        private GraphicsDevice graphicsDevice = null;

        /// <summary>
        /// VertexBuffer
        /// </summary>
        protected VertexBuffer buffer;

        private BasicEffect basicEffect;

        private const int CIRCLE_NUM_POINTS = 32;
        private IndexBuffer _indexBuffer;
        private VertexPositionNormalTexture[] _vertices;

        //private Matrix[] modelBoneTransforms;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="device"></param>
        public BoundingSphereRenderer(GraphicsDevice device)
        {
            graphicsDevice = device;
            OnCreateDevice();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnCreateDevice()
        {
            basicEffect = new BasicEffect(graphicsDevice);

            CreateShape();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateShape()
        {
            double angle = MathHelper.TwoPi / CIRCLE_NUM_POINTS;

            _vertices = new VertexPositionNormalTexture[CIRCLE_NUM_POINTS * 3 + 4];

            _vertices[0] = new VertexPositionNormalTexture(Vector3.Zero, Vector3.Forward, Vector2.One);

            for (int i = 1; i <= CIRCLE_NUM_POINTS; i++)
            {
                float x = (float)Math.Round(Math.Sin(angle * i), 4);
                float y = (float)Math.Round(Math.Cos(angle * i), 4);
                Vector3 point = new Vector3(x, y, 0);

                _vertices[i] = new VertexPositionNormalTexture(point, Vector3.Forward, new Vector2());
            }

            for (int i = CIRCLE_NUM_POINTS + 1; i <= CIRCLE_NUM_POINTS * 2 + 1; i++)
            {
                float z = (float)Math.Round(Math.Sin(angle * i), 4);
                float x = (float)Math.Round(Math.Cos(angle * i), 4);
                Vector3 point = new Vector3(x, 0, z);

                _vertices[i] = new VertexPositionNormalTexture(point, Vector3.Forward, new Vector2());
            }

            for (int i = CIRCLE_NUM_POINTS * 2 + 2; i <= CIRCLE_NUM_POINTS * 3 + 3; i++)
            {
                float y = (float)Math.Round(Math.Sin(angle * i), 4);
                float z = (float)Math.Round(Math.Cos(angle * i), 4);
                Vector3 point = new Vector3(0, y, z);

                _vertices[i] = new VertexPositionNormalTexture(point, Vector3.Forward, new Vector2());
            }

            // Initialize the vertex buffer, allocating memory for each vertex
            buffer = new VertexBuffer(graphicsDevice,
                VertexPositionNormalTexture.VertexDeclaration,
                _vertices.Length,
                BufferUsage.None);


            // Set the vertex buffer data to the array of vertices
            buffer.SetData(_vertices);

            InitializeLineStrip();
        }

        private void InitializeLineStrip()
        {
            // Initialize an array of indices of type short
            short[] lineStripIndices = new short[CIRCLE_NUM_POINTS * 3 + 1];

            // Populate the array with references to indices in the vertex buffer
            for (int i = 0; i < CIRCLE_NUM_POINTS * 3; i++)
            {
                lineStripIndices[i] = (short)(i + 1);
            }

            lineStripIndices[CIRCLE_NUM_POINTS] = 1;

            // Initialize the index buffer, allocating memory for each index
            _indexBuffer = new IndexBuffer(
                graphicsDevice,
                IndexElementSize.SixteenBits,
                lineStripIndices.Length,
                BufferUsage.None
                );

            // Set the data in the index buffer to our array
            _indexBuffer.SetData<short>(lineStripIndices);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="color"></param>
        public void DrawSpheres(BoundingVolume volume, Color color)
        {
            if (volume != null)
            {

                Matrix scaleMatrix = Matrix.CreateScale(((BoundingSphere3D)volume.Bounding).Sphere.Radius);
                Matrix translateMat = Matrix.CreateTranslation(((BoundingSphere3D)volume.Bounding).Sphere.Center);

                // effect is a compiled effect created and compiled elsewhere
                // in the application
                int ent = ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>()[0];
                CameraComponent cc = ComponentManager.GetEntityComponent<CameraComponent>(ent);
                basicEffect.EnableDefaultLighting();
                basicEffect.View = cc.View;
                basicEffect.Projection = cc.Projection;
                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.SetVertexBuffer(buffer);
                    graphicsDevice.Indices = _indexBuffer;

                    basicEffect.Alpha = ((float)color.A / (float)byte.MaxValue);

                    basicEffect.World = scaleMatrix * translateMat;
                    basicEffect.DiffuseColor = color.ToVector3();

                    graphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.LineStrip,
                        0,  // vertex buffer offset to add to each element of the index buffer
                        0,  // first index element to read
                        CIRCLE_NUM_POINTS * 3 + 1); // number of primitives to draw
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            var entities = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<ModelComponent>();
            foreach (var ent in entities)
            {
                var mc = (ModelComponent)ent.Value;
                if (mc.BoundingVolume != null)
                {
                    if (mc.BoundingVolume.Bounding is BoundingSphere3D)
                    {
                        DrawSpheres(mc.BoundingVolume, Color.Red);
                    }
                }
            }
        }
    }
}