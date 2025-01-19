using System;
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
        public ProceduralGenerationManager Manager;
        public JobHandle handle;

        public void Awake()
        {
            Manager = GetComponent<ProceduralGenerationManager>();
        }

        public JobHandle GenerateChunk(Vector2 chunkPos, int LOD)
        {
            NativeArray<float3> vertices = new NativeArray<float3>(Manager.ChunkSize * Manager.ChunkSize, Allocator.Temp);

            var job = new ComputeHeightJob
            {
                Vertices = vertices,
                size = Manager.ChunkSize,
                ChunkWorldPos = chunkPos,
                Simplification = LOD <= 0 ? 1 : LOD * 2,
            };
            JobHandle handle = job.Schedule(vertices.Length, 32);

            vertices.Dispose();

            return handle;
        }

        public struct ComputeHeightJob : IJobParallelFor
        {
            public NativeArray<float3> Vertices;
            [ReadOnly] public int size;
            [ReadOnly] public float2 ChunkWorldPos;
            [ReadOnly] public int Simplification;

            public void Execute(int index)
            {
                int y = (index / size) * size;
                int x = index % size;
                float xPos = ChunkWorldPos.x + x;
                float zPos = ChunkWorldPos.y + y;
                int verticesPerLine = (size - 1) / Simplification + 1;

                Vertices[index] = new float3
                {
                    x = xPos,
                    y = Generated_GenerationStatics.SampleDunes(ChunkWorldPos.x + x, ChunkWorldPos.y + y),
                    z = zPos
                };
            }
        }
    }
}