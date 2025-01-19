using PGG;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;

namespace ProcGen.Jobs
{
    [BurstCompile]
    public class ParallelGenerator : MonoBehaviour
    {
        public NativeArray<float> HeightMap;
        
        public void GenerateChunk(Vector2 chunkPos)
        {
            //float2 chunk = chunkPos;
            //Chunk currentChunk = Instantiate(ChunkPrefab, chunkPos, Quaternion.identity, ChunksHolder.transform);
            //
            //
            //
            //currentChunk.CreateMesh(MeshGenerator.GenerateMesh(Noise.GenerateNoiseMap(ChunkSize, GenerationAsset, internSeed, Offset, Vector2.zero), LOD));
        }

        public struct ComputeHeightJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<float> HeightMap;
            [ReadOnly] public float2 SamplePos;

            public void Execute(int index)
            {
                //HeightMap[index] = 
            }
        }

        public struct CreateChunkHeightMapJob : IJob
        {
            private JobHandle HeightMapHandle;

            public void Execute()
            {
                throw new System.NotImplementedException();
            }
        }

        public struct SampleHeightJob : IJob
        {
            [ReadOnly] private float2 worldPos;
            public float height;
            
            public void Execute()
            {
                height = Generated_GenerationStatics.SampleDunes(worldPos.x, worldPos.y);
            }
        }
    }
}