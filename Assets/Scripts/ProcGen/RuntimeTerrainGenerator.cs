using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProcGen
{
    public class RuntimeTerrainGenerator : MonoBehaviour
    {
        public float ViewDistance = 300f;
        public Transform ViewTransform;

        public static Vector2 ViewPosition;
        private int chunkSize;
        private int visibleChunks;

        public Dictionary<Vector2Int, ChunkData> ChunksData = new Dictionary<Vector2Int, ChunkData>();
        private List<ChunkData> ChunksVisibleLastUpdate = new List<ChunkData>();

        private ProceduralGenerationManager ProcGenManager;

        public static RuntimeTerrainGenerator Instance { get; private set; }

        public void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
        }


        private void Start()
        {
            if (!ProcGenManager)
            {
                ProcGenManager = GetComponent<ProceduralGenerationManager>();
            }

            Assert.IsNotNull(ProcGenManager);

            chunkSize = ProceduralGenerationManager.Instance.ChunkSize - 1;
            visibleChunks = Mathf.RoundToInt(ViewDistance / chunkSize);
        }

        public void Update()
        {
            var position = ViewTransform.position;
            ViewPosition = new Vector2(position.x, position.z);
            UpdateVisibleChunks();
        }

        private void UpdateVisibleChunks()
        {
            for (int i = 0; i < ChunksVisibleLastUpdate.Count; i++)
            {
                ChunksVisibleLastUpdate[i].SetVisible(false);
            }

            ChunksVisibleLastUpdate.Clear();

            Vector2Int currentViewChunk = new Vector2Int(Mathf.RoundToInt(ViewPosition.x / chunkSize), Mathf.RoundToInt(ViewPosition.y / chunkSize));

            int ViewChunkPosX = Mathf.RoundToInt(ViewPosition.x / chunkSize);
            int ViewChunkPosY = Mathf.RoundToInt(ViewPosition.y / chunkSize);

            for (int yOffset = -visibleChunks; yOffset <= visibleChunks; yOffset++)
            {
                for (int xOffset = -visibleChunks; xOffset <= visibleChunks; xOffset++)
                {
                    Vector2Int ChunkPos = new Vector2Int(ViewChunkPosX + xOffset, ViewChunkPosY + yOffset);

                    if (ChunksData.ContainsKey(ChunkPos))
                    {
                        ChunksData[ChunkPos].UpdateVisibility();
                        if (ChunksData[ChunkPos].IsVisible())
                        {
                            ChunksVisibleLastUpdate.Add(ChunksData[ChunkPos]);
                        }
                    }
                    else
                    {
                        ChunksData.Add(ChunkPos, new ChunkData(ChunkPos, chunkSize));
                    }
                }
            }
        }


        public class ChunkData
        {
            public Chunk Chunk { get; private set; }
            private Vector2 worldPosition;
            private Bounds bounds;


            public ChunkData(Vector2Int position, int size)
            {
                worldPosition = position * size;
                bounds = new Bounds(worldPosition + new Vector2(size / 2f, size / 2f), Vector2.one * size);

                Chunk = GenerateChunk(worldPosition);
                SetVisible(false);
            }

            public void UpdateVisibility()
            {
                float viewerDistanceFromBounds = Mathf.Sqrt(bounds.SqrDistance(ViewPosition));
                bool visible = viewerDistanceFromBounds <= RuntimeTerrainGenerator.Instance.ViewDistance;
                SetVisible(visible);
            }

            public void SetVisible(bool visible)
            {
                Chunk.gameObject.SetActive(visible);
            }

            public bool IsVisible()
            {
                return Chunk.gameObject.activeSelf;
            }

            public Chunk GenerateChunk(Vector2 chunkPos)
            {
                ProceduralGenerationManager generator = ProceduralGenerationManager.Instance;
                Vector3 pos3D = new Vector3(chunkPos.x, 0, chunkPos.y);

                Chunk currentChunk = Instantiate(ProceduralGenerationManager.Instance.ChunkPrefab, pos3D, Quaternion.identity, ProceduralGenerationManager.Instance.ChunksHolder.transform);

                currentChunk.CreateMesh(MeshGenerator.GenerateMesh(Noise.GenerateNoiseMap(generator.ChunkSize, generator.GenerationAsset, generator.internSeed, ProceduralGenerationManager.Instance.Offset, chunkPos), generator.LOD, ProceduralGenerationManager.Instance.ChunkSize));

                return currentChunk;
            }
        }
    }
}