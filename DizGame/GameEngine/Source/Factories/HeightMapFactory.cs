using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.RandomStuff;

namespace GameEngine.Source.Factories
{
    public class HeightMapFactory
    {
        private GraphicsDevice graphicsDevice;
        
        private Texture2D heightMap;
        private Texture2D heightMapTexture;
        public VertexPositionNormalTexture[] VerticesTexture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private int fractions_per_side;
        private int chunk_width;
        private int chunk_height;

        //private BasicEffect Effect;
        private int[] Indices;
        
        private float[,] heightMapData;

        //private VertexBuffer VertexBuffer;
        //private IndexBuffer IndexBuffer;
        

        public HeightMapFactory(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

        }

        public HeightmapComponentTexture CreateTexturedHeightMap(Texture2D heightMap, Texture2D heightMapTexture, int fractions_per_side)
        {
            HeightmapComponentTexture heightMapComponent = new HeightmapComponentTexture();
            this.heightMap = heightMap;
            this.heightMapTexture = heightMapTexture;
            this.fractions_per_side = fractions_per_side;
            SetHeightMapData(ref heightMapComponent);
            //heightMapComponent.Effect = Effect;
            //heightMapComponent.IndexBuffer = IndexBuffer;
            heightMapComponent.Indices = Indices;
            //heightMapComponent.VertexBuffer = VertexBuffer;
            return heightMapComponent;
        }
        

        private void SetHeightMapData(ref HeightmapComponentTexture comp)
        {
            Width = heightMap.Width;
            Height = heightMap.Height;
            chunk_height = Height / fractions_per_side;
            chunk_width = Width / fractions_per_side;
            SetHeights();
            SetVerticesTexture();
            SetIndices();
            CalculateNormals();
            comp.HeightMapData = heightMapData;

            SetUpHeightMapChunks(ref comp);
        }

        private void SetHeights()
        {
            Color[] greyValues = new Color[Width * Height];
            heightMap.GetData(greyValues);
            heightMapData = new float[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    heightMapData[x, y] = greyValues[x + y * Width].R * 0.2f;
                }
            }
        }

        private void SetIndices()
        {
            Indices = new int[(Width - 1) * (Height - 1) * 6];
            int counter = 0;
            for (int y = 0; y < Height - 1; y++)
            {
                for (int x = 0; x < Width - 1; x++)
                {
                    int lowerLeft = (x + y * Width);
                    int lowerRight = ((x + 1) + y * Width);
                    int topLeft = (x + (y + 1) * Width);
                    int topRight = ((x + 1) + (y + 1) * Width);

                    Indices[counter++] = topLeft;
                    Indices[counter++] = lowerRight;
                    Indices[counter++] = lowerLeft;

                    Indices[counter++] = topLeft;
                    Indices[counter++] = topRight;
                    Indices[counter++] = lowerRight;
                }
            }
        }
                
        private void SetVerticesTexture()
        {
            VerticesTexture = new VertexPositionNormalTexture[Width * Height];
            Vector2 texturePosition;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    texturePosition = new Vector2((float)x / Width, (float)y / Height);
                    VerticesTexture[x + y * Width] = new VertexPositionNormalTexture(new Vector3(x, heightMapData[x, y], -y), Vector3.One, texturePosition);
                }
            }
        }
        
        private void CalculateNormals()
        {
            for (int i = 0; i < VerticesTexture.Length; i++)
                VerticesTexture[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < Indices.Length / 3; i++)
            {
                int index1 = Indices[i * 3];
                int index2 = Indices[i * 3 + 1];
                int index3 = Indices[i * 3 + 2];

                Vector3 side1 = VerticesTexture[index1].Position - VerticesTexture[index3].Position;
                Vector3 side2 = VerticesTexture[index1].Position - VerticesTexture[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                VerticesTexture[index1].Normal += normal;
                VerticesTexture[index2].Normal += normal;
                VerticesTexture[index3].Normal += normal;
            }
            for (int i = 0; i < VerticesTexture.Length; i++)
                VerticesTexture[i].Normal.Normalize();
        }

        private void SetUpHeightMapChunks(ref HeightmapComponentTexture heightMapComp)
        {
            for (int x = 0; x < Width - chunk_width; x += chunk_width)
            { 
                for (int y = 0; y < Height - chunk_height; y += chunk_height)
                {
                    
                    Rectangle clipRect = new Rectangle(x, y, chunk_width + 1, chunk_height + 1);
                    var offsetpos = new Vector3(x, 0, -y);

                    // Uncomment in för att se att det verkligen är chunks
                    //HeightMapChunk chunk = CreateHeightMapChunk(heightMap, new Rectangle(x, y, chunk_width, chunk_height),
                    //new Vector3(x, 0, -y), GetVertexTextureNormals(new Rectangle(x, y, chunk_width, chunk_height)), heightMapTexture);

                    HeightMapChunk chunk = CreateHeightMapChunk(heightMap, clipRect, offsetpos, GetVertexTextureNormals(clipRect), heightMapTexture);
                    
                    heightMapComp.HeightMapChunks.Add(chunk);
                }
            }
        }

        private VertexPositionNormalTexture[] GetVertexTextureNormals(Rectangle rect)
        {
            VertexPositionNormalTexture[] terrainVerts = new VertexPositionNormalTexture[rect.Width * rect.Height];

            for (int x = rect.X; x < rect.X + rect.Width; x++)
            {
                for (int y = rect.Y; y < rect.Y + rect.Height; y++)
                {
                    terrainVerts[(x - rect.X) + (y - rect.Y) * rect.Height].Normal = VerticesTexture[x + y * Height].Normal;
                }
            }
            return terrainVerts;
        }
        
        private HeightMapChunk CreateHeightMapChunk(Texture2D terrainMap, Rectangle terrainRect, Vector3 offsetPosition, VertexPositionNormalTexture[] vertexNormals, Texture2D texture)
        {
            var chunk = new HeightMapChunk()
            {
                OffsetPosition = offsetPosition
            };

            var heightinfo = CreateHightmap(terrainMap, terrainRect);
            var chunkVertices = InitTerrainVertices(heightinfo, terrainRect);
            var boundingBox = CreateBoundingBox(chunkVertices);

            var effect = new BasicEffect(graphicsDevice)
            {
                FogEnabled = true,
                FogStart = 10f,
                FogColor = Color.LightGray.ToVector3(),
                FogEnd = 400f,
                TextureEnabled = true,
                Texture = texture
            };

            var indices = InitIndices(terrainRect);
            chunk.indicesDiv3 = indices.Length / 3; // för att slipa göra den här divisionen flera gånger

            CopyNormals(vertexNormals, chunkVertices);

            PrepareBuffers(ref chunk, indices, chunkVertices);
            
            chunk.Effect = effect;
            chunk.BoundingBox = boundingBox;
            chunk.Vertices = chunkVertices;
            chunk.Rectangle = terrainRect;
            chunk.Width = terrainRect.Width;
            chunk.Height = terrainRect.Height;
            return chunk;
        }

        private void CopyNormals(VertexPositionNormalTexture[] vertexNormals, VertexPositionNormalTexture[] vertices)
        {
            for (int i = 0; i < vertices.Length; ++i)
            {
                vertices[i].Normal = vertexNormals[i].Normal;
            }
        }

        private float[,] CreateHightmap(Texture2D terrainMap, Rectangle terrainRect)
        {
            var width = terrainMap.Width;
            var height = terrainMap.Height;
            
            Color[] colors = new Color[width * height];
            terrainMap.GetData(colors);
            
            var heightInfo = new float[terrainRect.Width, terrainRect.Height];
            for (int x = terrainRect.X; x < terrainRect.X + terrainRect.Width; ++x)
            {
                for (int y = terrainRect.Y; y < terrainRect.Y + terrainRect.Height; ++y)
                {
                    heightInfo[x - terrainRect.X, y - terrainRect.Y] = colors[x + y * width].R * 0.2f;
                }
            }
            return heightInfo;
        }

        private int[] InitIndices(Rectangle terrainRect)
        {
            var width = terrainRect.Width;
            var height = terrainRect.Height;
            var indices = new int[(width - 1) * (height - 1) * 6];
            int indicesCount = 0; ;

            for (int y = 0; y < height - 1; ++y)
            {
                for (int x = 0; x < width - 1; ++x)
                {
                    int botLeft = x + y * width;
                    int botRight = (x + 1) + y * width;
                    int topLeft = x + (y + 1) * width;
                    int topRight = (x + 1) + (y + 1) * width;

                    indices[indicesCount++] = topLeft;
                    indices[indicesCount++] = botRight;
                    indices[indicesCount++] = botLeft;

                    indices[indicesCount++] = topLeft;
                    indices[indicesCount++] = topRight;
                    indices[indicesCount++] = botRight;
                }
            }
            return indices;
        }

        private VertexPositionNormalTexture[] InitTerrainVertices(float[,] heightInfo, Rectangle terrainRect)
        {
            var width = terrainRect.Width;
            var height = terrainRect.Height;
            VertexPositionNormalTexture[] terrainVerts = new VertexPositionNormalTexture[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    terrainVerts[x + y * height].Position = new Vector3(x, heightInfo[x, y], -y);
                    terrainVerts[x + y * height].TextureCoordinate.X = (float)x / (width - 1.0f);
                    terrainVerts[x + y * height].TextureCoordinate.Y = (float)y / (height - 1.0f);
                }
            }
            return terrainVerts;
        }

        private void PrepareBuffers(ref HeightMapChunk chunk, int[] indices, VertexPositionNormalTexture[] vertices)
        {
            chunk.IndexBuffer = new IndexBuffer(graphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            chunk.IndexBuffer.SetData(indices);

            chunk.VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.WriteOnly);
            chunk.VertexBuffer.SetData(vertices);
        }

        private BoundingBox CreateBoundingBox(VertexPositionNormalTexture[] vertexArray)
        {
            List<Vector3> points = new List<Vector3>();

            foreach (VertexPositionNormalTexture v in vertexArray)
            {
                points.Add(v.Position);
            }
            BoundingBox b = BoundingBox.CreateFromPoints(points);
            return b;
        }

        private BoundingSphere CreateBoundingSphere(VertexPositionNormalTexture[] vertexArray)
        {
            var first = vertexArray.First();
            var last = vertexArray.Last();
            return BoundingSphere.CreateFromPoints(new List<Vector3> { first.Position, last.Position });
        }
    }
}
