using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Wave
{
    List<WaveObject> waveObjects;
    public Wave()
    {
        waveObjects = new List<WaveObject>();
    }
    public void AddWaveObject(WaveObject wo) { waveObjects.Add(wo); }
    public byte[] ToBytes => waveObjects.Select(x => x.ToBytes).ToArray().To1DArray();
    public int Size => waveObjects.Count * WaveObject.Size;
    public byte NumberOfWaveObjects => (byte)waveObjects.Count();
}

public class WaveObject
{
    byte enemyId;
    byte numberOfEnemies;
    byte spawnTime;
    byte spawnerIndex;

    public WaveObject(byte enemyId, byte numberOfEnemies, byte spawnTime, byte spawnerIndex)
    {
        this.enemyId = enemyId;
        this.numberOfEnemies = numberOfEnemies;
        this.spawnTime = spawnTime;
        this.spawnerIndex = spawnerIndex;
    }

    public byte[] ToBytes => new byte[] { enemyId, numberOfEnemies, spawnTime, spawnerIndex };
    public static int Size => 4;
}
