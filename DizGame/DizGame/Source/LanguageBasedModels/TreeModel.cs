using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DizGame.Source.LanguageBasedModels
{
    /// <summary>
    /// A simple model of a tree drawn with lines.
    /// </summary>
    public class TreeModel
    {
        private struct Pos_or
        {
            public Vector3 Position { get; set; }
            public Vector3 Direction { get; set; }
            public float Scale { get; set; }
        }

        private Stack<VertexPositionNormalTexture> treeStack;
        private Stack<VertexPositionNormalTexture> storeStack;
        private Stack<Pos_or> turtleStack;

        private int replicationDepth;

        static string dna_example = "F[LF]F[RF]F";

        private char[] originalDna;
        private char[] dna;
        private char[] currentString;
        private float unitLength;
        private float rotAroundZAxis;
        private float scaleReplication;
        private string[] recursiveStrings;
        private int countVertices;

        private VertexPositionNormalTexture[] vertices;
        private int[] indices;
        public VertexBuffer VertexBuffer { get; private set; }
        public IndexBuffer IndexBuffer { get; private set; }

        private Vector3 startPosition;
        //private Vector3 segmentVector;
        private Vector3 normal;
        private Vector2 textureCoordinate;
        private Vector3 directionF;
        //private Vector3 directionL;
        //private Vector3 directionR;

        /// <summary>
        /// Example : TreeModel(gd, 1f, MathHelper.PiOver4 - 0.4f, "F[LF]F[RF]F", 0, 1f, new string[] { "F" });
        /// </summary>
        /// <param name="gd">The device to render to.</param>
        /// <param name="unitLength">The length of one unit of the tree.</param>
        /// <param name="rotAroundZAxis">Rotate branches around the stem.</param>
        /// <param name="dna">The formula to build the tree from. F = Forward, [ = push onto stack,
        /// L = Left, ] = pop from the stack, R = Right.</param>
        /// <param name="replicationDepth">How many times the tree will be subdivided and create branches on branches</param>
        /// <param name="scaleReplication">Set to 1f</param>
        /// <param name="recursiveStrings">The letter(s) that will be replaced by the dna string.</param>
        public TreeModel(GraphicsDevice gd, float unitLength, float rotAroundZAxis,
                    string dna, int replicationDepth,
                    float scaleReplication, string[] recursiveStrings)
        {
            treeStack = new Stack<VertexPositionNormalTexture>();
            storeStack = new Stack<VertexPositionNormalTexture>();
            turtleStack = new Stack<Pos_or>();

            this.replicationDepth = replicationDepth;

            this.originalDna = dna.ToArray();

            this.dna = dna.ToArray();

            this.unitLength = unitLength;

            this.rotAroundZAxis = rotAroundZAxis;

            this.scaleReplication = scaleReplication;

            this.recursiveStrings = recursiveStrings;

            startPosition = new Vector3(0, 0, 0);
            //segmentVector = new Vector3(0, 1, 0);
            //normal = new Vector3(0.5f, 0.3f, 0.7f);
            textureCoordinate = new Vector2(0, 0);

            directionF = new Vector3(0, 1, 0);
            //directionL = new Vector3(0, 1, 0);
            //directionR = new Vector3(0, 1, 0);


            BuildDna(ref this.dna, 0);

            //System.Diagnostics.Debug.WriteLine(currentString);

            CountRecursiveStrings();

            BuildStructure();

            CreateIndices();

            SetVertexAndIndexBuffer(gd, vertices, indices);
        }


        private void SetVertexAndIndexBuffer(GraphicsDevice gd, VertexPositionNormalTexture[] vertices, int[] indices)
        {
            VertexBuffer = new VertexBuffer(gd, typeof(VertexPositionNormalTexture), vertices.Count(), BufferUsage.None);
            IndexBuffer = new IndexBuffer(gd, typeof(int), indices.Count(), BufferUsage.None);

            VertexBuffer.SetData<VertexPositionNormalTexture>(vertices);
            IndexBuffer.SetData<int>(indices);
        }


        private void BuildStructure()
        {
            Random rnd = new Random();
            Vector3 direction = directionF;
            Vector3 currentVector = Vector3.Zero;
            Pos_or turtleValues;

            int counter = 1;
            float scale = 1f;

            treeStack.Push(new VertexPositionNormalTexture(startPosition, normal, textureCoordinate));

            foreach (char x in currentString)
            {
                switch (x)
                {
                    case 'F':
                        treeStack.Push(new VertexPositionNormalTexture(F(this.unitLength, currentVector, direction, scale),
                                                                   normal, textureCoordinate));
                        currentVector = treeStack.Peek().Position;

                        counter++;
                        break;

                    case 'L':
                        direction = Vector3.Transform(direction, Matrix.CreateRotationZ(rotAroundZAxis));
                        //                                         
                        break;

                    case 'R':
                        direction = Vector3.Transform(direction, Matrix.CreateRotationZ(-rotAroundZAxis));
                        break;

                    case '[':
                        scale = this.scaleReplication * scale;
                        turtleStack.Push(new Pos_or()
                        {
                            Position = currentVector,
                            Direction = direction,
                            Scale = scale
                        });

                        break;

                    case ']':
                        turtleValues = turtleStack.Pop();
                        currentVector = turtleValues.Position;
                        direction = turtleValues.Direction;
                        scale = turtleValues.Scale;
                        break;


                }
            }

            vertices = treeStack.Reverse().ToArray();
        }


        private void CreateIndices()
        {
            Stack<int> indices = new Stack<int>();
            Stack<int> leftIndices = new Stack<int>();
            Stack<int> topIndex = new Stack<int>();

            leftIndices.Push(0);
            int leftIndex = 0;
            int rightIndex = 1;

            foreach (char x in currentString)
            {
                switch (x)
                {
                    case 'F':
                        indices.Push(leftIndices.Peek());
                        indices.Push(rightIndex);
                        rightIndex++;
                        break;

                    case '[':
                        leftIndex = indices.Peek();
                        leftIndices.Push(leftIndex);

                        break;

                    case ']':
                        leftIndex = leftIndices.Pop(); // (indices.Peek());
                        break;
                }
            }

            this.indices = indices.Reverse().ToArray();
        }


        private void CountRecursiveStrings()
        {
            countVertices = currentString.Count(x => x.Equals(recursiveStrings[0].ToCharArray()[0]));
        }


        public Vector3 F(float unitLength, Vector3 currentVector, Vector3 direction, float scale)
        {
            Vector3 segment = (currentVector + direction);
            //segment.Normalize();

            return scale * unitLength * segment;
        }


        private void BuildDna(ref char[] buildString, int replicationDepth)
        {

            if (replicationDepth > this.replicationDepth)
                return;

            foreach (string str in recursiveStrings)
                currentString = new string(buildString).Replace(str, new string(dna)).ToArray();

            BuildDna(ref currentString, ++replicationDepth);

            dna = currentString;
        }
    }
}
