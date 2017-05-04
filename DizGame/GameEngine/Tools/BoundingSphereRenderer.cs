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

        private Game game = null;

        protected VertexBuffer buffer;
        protected VertexDeclaration vertexDecl;

        BasicEffect basicEffect;

        private const int CIRCLE_NUM_POINTS = 32;
        private IndexBuffer _indexBuffer;
        private VertexPositionNormalTexture[] _vertices;

        public BoundingSphereRenderer(Game game)
        {
            this.game = game;
            basicEffect = new BasicEffect(game.GraphicsDevice);
            CreateShape();
        }


        public void CreateShape()
        {
            double angle = MathHelper.TwoPi / CIRCLE_NUM_POINTS;

            _vertices = new VertexPositionNormalTexture[CIRCLE_NUM_POINTS+1];

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
            //for (int i = 33; i <= CIRCLE_NUM_POINTS*2; i++)
            //{
            //    float y = (float)Math.Round(Math.Cos(angle * i), 4);
            //    float z = (float)Math.Round(Math.Sin(angle * i), 4);
            //    Vector3 point = new Vector3(
            //                     0.0f,
            //                     y,
            //                     z);



            //    _vertices[i] = new VertexPositionNormalTexture(
            //        point,
            //        Vector3.Forward,
            //        new Vector2());
            //}

                // Initialize the vertex buffer, allocating memory for each vertex
                buffer = new VertexBuffer(game.GraphicsDevice,
                VertexPositionNormalTexture.VertexDeclaration,
                _vertices.Length,
                BufferUsage.None);


            // Set the vertex buffer data to the array of vertices
            buffer.SetData<VertexPositionNormalTexture>(_vertices);

            InitializeLineStrip();
        }

        private void InitializeLineStrip()
        {
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
                game.GraphicsDevice,
                IndexElementSize.SixteenBits,
                lineStripIndices.Length,
                BufferUsage.None
                );

            // Set the data in the index buffer to our array
            _indexBuffer.SetData<short>(lineStripIndices);

        }

        public void Draw(TransformComponent transComp, BoundingSphere bs, Color color)
        {
            GraphicsDevice device = game.GraphicsDevice;
            if (bs != null)
            {
                Matrix translateMat = Matrix.CreateTranslation(bs.Center);
                Matrix scaleMat = Matrix.CreateScale(bs.Radius-1);
                Matrix rotateZMatrix = Matrix.CreateRotationZ(RADIANS_FOR_90DEGREES);
                Matrix rotateXMatrix = Matrix.CreateRotationX(RADIANS_FOR_90DEGREES);

                RasterizerState rState = new RasterizerState();
                rState.CullMode = CullMode.None;
                rState.FillMode = FillMode.Solid;
                game.GraphicsDevice.BlendState = BlendState.Opaque;
                game.GraphicsDevice.RasterizerState = rState;

                // effect is a compiled effect created and compiled elsewhere
                // in the application
                int ent = ComponentManager.Instance.GetAllEntitiesWithComponentType<CameraComponent>()[0];
                CameraComponent cc = ComponentManager.Instance.GetEntityComponent<CameraComponent>(ent);

                basicEffect.EnableDefaultLighting();
                basicEffect.View = cc.View;
                basicEffect.Projection = cc.Projection;
                basicEffect.World = translateMat * rotateZMatrix * scaleMat;
                basicEffect.DiffuseColor = color.ToVector3();

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    device.SetVertexBuffer(buffer);
                    device.Indices = _indexBuffer;

                    basicEffect.Alpha = ((float)color.A / (float)byte.MaxValue);

                    device.DrawIndexedPrimitives(
                        PrimitiveType.LineStrip,
                        0,  // vertex buffer offset to add to each element of the index buffer
                        0,  // first index element to read
                        CIRCLE_NUM_POINTS); // number of primitives to draw

                    //basicEffect.World = rotateZMatrix * scaleMat * translateMat;
                    //basicEffect.DiffuseColor = color.ToVector3() * 0.5f;

                    //device.DrawIndexedPrimitives(
                    //    PrimitiveType.LineStrip,
                    //    0,  // vertex buffer offset to add to each element of the index buffer
                    //    0,  // first index element to read
                    //    CIRCLE_NUM_POINTS); // number of primitives to draw

                    //basicEffect.World = rotateXMatrix * scaleMat * translateMat;
                    //basicEffect.DiffuseColor = color.ToVector3() * 0.5f;

                    //device.DrawIndexedPrimitives(
                    //    PrimitiveType.LineStrip,
                    //    0,  // vertex buffer offset to add to each element of the index buffer
                    //    0,  // first index element to read
                    //    CIRCLE_NUM_POINTS); // number of primitives to draw
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            List<int> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<ModelComponent>();
            foreach (int ent in entities)
            {

                ModelComponent mc = ComponentManager.Instance.GetEntityComponent<ModelComponent>(ent);
                TransformComponent tc = ComponentManager.Instance.GetEntityComponent<TransformComponent>(ent);
                foreach (ModelMesh mm in mc.Model.Meshes)
                {
                    List<Vector3> verticePositions = new List<Vector3>();
                    foreach (ModelMeshPart mp in mm.MeshParts)
                    {
                        Vector3[] temp = new Vector3[mp.NumVertices];
                        mp.VertexBuffer.GetData<Vector3>(temp);
                        verticePositions.AddRange(temp);
                    }
                    BoundingSphere sphere = BoundingSphere.CreateFromPoints(verticePositions);
                    sphere = new BoundingSphere(Vector3.Transform(sphere.Center, mc.MeshWorldMatrices[mm.ParentBone.Index] * tc.ObjectMatrix), sphere.Radius);
                    mm.BoundingSphere = sphere;
                    Draw(tc, mm.BoundingSphere, new Color(new Vector3(255, 0, 0)));
                }
            }
        }
    }
}
