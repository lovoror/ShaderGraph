/*
 *  @CreatedTime 2021年7月1日 16:49:31
 *  @FileName Boid_Manager.cs
 *  @Version 1.00
 *  @Author  Jacob.zhang
 *
*/

using Unity.Entities;

public struct Boid_Manager 
{
    public float cohesionBias;
    public float separationBias;
    public float alignmentBias;
    public float targetBias;
    public float perceptionRadius;
    public float step;
    public int cellSize;
    public float fieldOfView;
    public int maxPercived;
}

public struct BoidManagerBLOB
{
    public BlobArray<Boid_Manager> blobManagerArray;
}