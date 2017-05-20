using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;
using GameEngine.Source.Systems;
using AnimationContentClasses;

namespace GameEngine.Tools
{
    public class BoundingSphereRenderer : IRender
    {
        public static float RADIANS_FOR_90DEGREES = MathHelper.ToRadians(90);//(float)(Math.PI / 2.0);
        public static float RADIANS_FOR_180DEGREES = RADIANS_FOR_90DEGREES * 2;

        private GraphicsDevice graphicsDevice = null;

        protected VertexBuffer buffer;
        protected VertexDeclaration vertexDecl;

        private BasicEffect basicEffect;

        private const int CIRCLE_NUM_POINTS = 32;
        private IndexBuffer _indexBuffer;
        private VertexPositionNormalTexture[] _vertices;
        private int[] lineStripIndices;

        //private Matrix[] modelBoneTransforms;

        public BoundingSphereRenderer(GraphicsDevice device)
        {
            graphicsDevice = device;
            OnCreateDevice();
        }

        public void OnCreateDevice()
        {
            basicEffect = new BasicEffect(graphicsDevice);

            CreateShape();
        }

        /// <summary>
        /// This creates the circles to show how the bounding sphere looks like.
        /// </summary>
        public void CreateShape()
        {
            double angle = MathHelper.TwoPi / CIRCLE_NUM_POINTS;

            _vertices = new VertexPositionNormalTexture[CIRCLE_NUM_POINTS * 3 + 3];
            _vertices[0] = new VertexPositionNormalTexture(Vector3.Zero, Vector3.Forward, Vector2.One);
            //int verticeIndex = 0;
            for (int i = 0; i <= CIRCLE_NUM_POINTS; i++)
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
                    Vector2.Zero);

            }

            //_vertices[CIRCLE_NUM_POINTS + 1] = new VertexPositionNormalTexture(
            //    Vector3.Zero, Vector3.Forward, Vector2.One);
            for (int i = CIRCLE_NUM_POINTS + 1; i <= CIRCLE_NUM_POINTS * 2 + 1; i++)
            {
                float y = (float)Math.Round(Math.Cos(angle * (i - CIRCLE_NUM_POINTS * 2 - 2)), 4);
                float z = (float)Math.Round(Math.Sin(angle * (i - CIRCLE_NUM_POINTS * 2 - 2)), 4);
                Vector3 point = new Vector3(
                                 0.0f,
                                 y,
                                 z);



                _vertices[i] = new VertexPositionNormalTexture(
                    point,
                    Vector3.Forward,
                    new Vector2());
            }

            //_vertices[CIRCLE_NUM_POINTS * 2 + 2] = new VertexPositionNormalTexture(
            //    Vector3.Zero, Vector3.Forward, Vector2.One);
            for (int i = CIRCLE_NUM_POINTS * 2 + 2; i <= CIRCLE_NUM_POINTS * 3 + 2; i++)
            {
                float z = (float)Math.Round(Math.Sin(angle * (i - CIRCLE_NUM_POINTS - 1)), 4);
                float x = (float)Math.Round(Math.Cos(angle * (i - CIRCLE_NUM_POINTS - 1)), 4);
                Vector3 point = new Vector3(
                                 x,
                                 0.0f,
                                 z);



                _vertices[i] = new VertexPositionNormalTexture(
                    point,
                    Vector3.Forward,
                    new Vector2());
            }
       
            
            // Initialize the vertex buffer, allocating memory for each vertex
            buffer = new VertexBuffer(graphicsDevice,
                            VertexPositionNormalTexture.VertexDeclaration,
                            _vertices.Length,
                            BufferUsage.WriteOnly);


            // Set the vertex buffer data to the array of vertices
            buffer.SetData(_vertices);

            InitializeLineStrip();
        }

        /// <summary>
        /// Adds all indices, and initializes and sets the buffer
        /// </summary>
        private void InitializeLineStrip()
        {
            // Initialize an array of indices of type short
            lineStripIndices = new int[CIRCLE_NUM_POINTS * 3 + 1];

            // Populate the array with references to indices in the vertex buffer
            for (int i = 0; i < CIRCLE_NUM_POINTS * 3; i++)
            {
                lineStripIndices[i] = (i + 1);
            }

            lineStripIndices[CIRCLE_NUM_POINTS] = 1;

            // Initialize the index buffer, allocating memory for each index
            _indexBuffer = new IndexBuffer(
                graphicsDevice,
                IndexElementSize.ThirtyTwoBits,
                lineStripIndices.Length,
                BufferUsage.WriteOnly
                );

            // Set the data in the index buffer to our array
            _indexBuffer.SetData(lineStripIndices);

        }
        /// <summary>
        /// Takes in a modelComponent and Color as parameters, translates the boundingsphere 
        /// made in CreateShape to the models position and scales it and sets the effects 
        /// DiffuseColor to the colors Vector3
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="color"></param>
        public void DrawSphere(ModelComponent mc, Color color)
        {
            if (mc.BoundingVolume.Bounding != null)
            {
                BoundingSphere3D sphere = mc.BoundingVolume.Bounding as BoundingSphere3D;
                Matrix scaleMatrix = Matrix.CreateScale((sphere.Sphere.Radius));
                Matrix translateMat = Matrix.CreateTranslation(sphere.Sphere.Center);
                //Matrix rotateYMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(90));
                //Matrix rotateXMatrix = Matrix.CreateRotationX(MathHelper.ToRadians(90));

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
                    basicEffect.AmbientLightColor = color.ToVector3();
                    graphicsDevice.SetVertexBuffer(buffer);
                    graphicsDevice.Indices = _indexBuffer;

                    basicEffect.World = scaleMatrix * translateMat;
                    basicEffect.DiffuseColor = color.ToVector3();

                    graphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.LineStrip,
                        0,  // vertex buffer offset to add to each element of the index buffer
                        0,  // first index element to read
                        lineStripIndices.Length); // number of primitives to draw

                    //basicEffect.World =  scaleMatrix * rotateYMatrix * translateMat;
                    ////basicEffect.World = translateMat * rotateYMatrix * scaleMatrix;
                    //basicEffect.DiffuseColor = color.ToVector3();

                    //graphicsDevice.DrawIndexedPrimitives(
                    //    PrimitiveType.LineStrip,
                    //    0,  // vertex buffer offset to add to each element of the index buffer
                    //    0,  // first index element to read
                    //    lineStripIndices.Length / 3); // number of primitives to draw

                    //basicEffect.World = rotateXMatrix * scaleMatrix * translateMat;
                    //basicEffect.DiffuseColor = color.ToVector3();

                    //graphicsDevice.DrawIndexedPrimitives(
                    //    PrimitiveType.LineStrip,
                    //    0,  // vertex buffer offset to add to each element of the index buffer
                    //    0,  // first index element to read
                    //    lineStripIndices.Length / 3); // number of primitives to draw
                }
            }
        }
        /// <summary>
        /// Simply looping through all modelcomponents and calls the DrawSphere
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            List<int> entities = ComponentManager.GetAllEntitiesWithComponentType<ModelComponent>();
            foreach (int ent in entities)
            {

                ModelComponent mc = ComponentManager.GetEntityComponent<ModelComponent>(ent);
                if (mc.BoundingVolume != null && mc.BoundingVolume.Bounding is BoundingSphere3D)
                    DrawSphere(mc, new Color(new Vector3(255, 0, 0)));

            }
        }
    }
}
