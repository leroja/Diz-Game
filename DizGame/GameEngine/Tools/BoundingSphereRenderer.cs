using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;
using GameEngine.Source.Systems;

namespace GameEngine.Tools
{
    public class BoundingSphereRenderer : IRender
    {
        public static float RADIANS_FOR_90DEGREES = MathHelper.ToRadians(90);//(float)(Math.PI / 2.0);
        public static float RADIANS_FOR_180DEGREES = RADIANS_FOR_90DEGREES * 2;

        private Game _gameInstance = null;

        protected VertexBuffer buffer;
        protected VertexDeclaration vertexDecl;

        private BasicEffect basicEffect;

        private const int CIRCLE_NUM_POINTS = 32;
        private IndexBuffer _indexBuffer;
        private VertexPositionNormalTexture[] _vertices;

        private Matrix[] modelBoneTransforms;

        public BoundingSphereRenderer(Game game)
        {
            _gameInstance = game;
        }

        public void OnCreateDevice()
        {
            IGraphicsDeviceService graphicsService = (IGraphicsDeviceService)_gameInstance.Services.GetService(typeof(IGraphicsDeviceService));

            basicEffect = new BasicEffect(graphicsService.GraphicsDevice);

            CreateShape();
        }

        public void CreateShape()
        {
            IGraphicsDeviceService graphicsService = (IGraphicsDeviceService)_gameInstance.Services.GetService(typeof(IGraphicsDeviceService));

            double angle = MathHelper.TwoPi / CIRCLE_NUM_POINTS;

            _vertices = new VertexPositionNormalTexture[CIRCLE_NUM_POINTS + 1];

            _vertices[0] = new VertexPositionNormalTexture(
                Vector3.Zero, Vector3.Forward, Vector2.One);

            for (int i = 1; i <= CIRCLE_NUM_POINTS; i++)
            {
                float x = (float)Math.Round(Math.Sin(angle * i), 4);
                float y = (float)Math.Round(Math.Cos(angle * i), 4);
                Vector3 point = new Vector3(
                                 x,
                                 y,
                                  0.0f);



                _vertices[i] = new VertexPositionNormalTexture(
                    point,
                    Vector3.Forward,
                    new Vector2());
            }

            // Initialize the vertex buffer, allocating memory for each vertex
            buffer = new VertexBuffer(graphicsService.GraphicsDevice,
                VertexPositionNormalTexture.VertexDeclaration,
                _vertices.Length,
                BufferUsage.None);


            // Set the vertex buffer data to the array of vertices
            buffer.SetData<VertexPositionNormalTexture>(_vertices);

            InitializeLineStrip();
        }

        private void InitializeLineStrip()
        {
            IGraphicsDeviceService graphicsService = (IGraphicsDeviceService)_gameInstance.Services.GetService(typeof(IGraphicsDeviceService));

            // Initialize an array of indices of type short
            short[] lineStripIndices = new short[CIRCLE_NUM_POINTS + 1];

            // Populate the array with references to indices in the vertex buffer
            for (int i = 0; i < CIRCLE_NUM_POINTS; i++)
            {
                lineStripIndices[i] = (short)(i + 1);
            }

            lineStripIndices[CIRCLE_NUM_POINTS] = 1;

            // Initialize the index buffer, allocating memory for each index
            _indexBuffer = new IndexBuffer(
                graphicsService.GraphicsDevice,
                IndexElementSize.SixteenBits,
                lineStripIndices.Length,
                BufferUsage.None
                );

            // Set the data in the index buffer to our array
            _indexBuffer.SetData<short>(lineStripIndices);

        }

        public void Draw(BoundingSphere bs, Color color)
        {

            IGraphicsDeviceService graphicsService = (IGraphicsDeviceService)_gameInstance.Services.GetService(typeof(IGraphicsDeviceService));
            GraphicsDevice device = graphicsService.GraphicsDevice;

            if (bs != null)
            {
                Matrix scaleMatrix = Matrix.CreateScale(bs.Radius);
                Matrix translateMat = Matrix.CreateTranslation(bs.Center);
                Matrix rotateYMatrix = Matrix.CreateRotationY(RADIANS_FOR_90DEGREES);
                Matrix rotateXMatrix = Matrix.CreateRotationX(RADIANS_FOR_90DEGREES);

                //device.RenderState.DepthBufferEnable = true;
                //device.RenderState.DepthBufferWriteEnable = true;
                //device.RenderState.AlphaBlendEnable = true;
                //device.RenderState.SourceBlend = Blend.SourceAlpha;
                //device.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                //device.RenderState.CullMode = CullMode.CullCounterClockwiseFace;

                // effect is a compiled effect created and compiled elsewhere
                // in the application
                int ent = ComponentManager.Instance.GetAllEntitiesWithComponentType<CameraComponent>()[0];
                CameraComponent cc = ComponentManager.Instance.GetEntityComponent<CameraComponent>(ent);
                basicEffect.EnableDefaultLighting();
                basicEffect.View = cc.View;
                basicEffect.Projection = cc.Projection;

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    device.SetVertexBuffer(buffer);
                    device.Indices = _indexBuffer;

                    basicEffect.Alpha = ((float)color.A / (float)byte.MaxValue);

                    basicEffect.World = scaleMatrix * translateMat;
                    basicEffect.DiffuseColor = color.ToVector3();

                    device.DrawIndexedPrimitives(
                        PrimitiveType.LineStrip,
                        0,  // vertex buffer offset to add to each element of the index buffer
                        0,  // first index element to read
                        CIRCLE_NUM_POINTS); // number of primitives to draw

                    basicEffect.World = rotateYMatrix * scaleMatrix * translateMat;
                    basicEffect.DiffuseColor = color.ToVector3() * 0.5f;

                    device.DrawIndexedPrimitives(
                        PrimitiveType.LineStrip,
                        0,  // vertex buffer offset to add to each element of the index buffer
                        0,  // first index element to read
                        CIRCLE_NUM_POINTS); // number of primitives to draw

                    basicEffect.World = rotateXMatrix * scaleMatrix * translateMat;
                    basicEffect.DiffuseColor = color.ToVector3() * 0.5f;

                    device.DrawIndexedPrimitives(
                        PrimitiveType.LineStrip,
                        0,  // vertex buffer offset to add to each element of the index buffer
                        0,  // first index element to read
                        CIRCLE_NUM_POINTS); // number of primitives to draw
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            List<int> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<ModelComponent>();
            foreach(int ent in entities)
            {
                ModelComponent mc = ComponentManager.Instance.GetEntityComponent<ModelComponent>(ent);
                foreach(ModelMesh mm in mc.Model.Meshes)
                {
                    Draw(mm.BoundingSphere, new Color(new Vector3(255, 0, 0)));
                }
            }
        }
    }
}
