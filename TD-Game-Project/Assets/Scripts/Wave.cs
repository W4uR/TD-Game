using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
    public bool IsEmpty => waveObjects.Count == 0;

    internal void DeleteWaveObject(int index)
    {
        int occurrence = -1;
        foreach (var wo in waveObjects)
        {
            if (wo.SpawnerIndex == WaveEditor.SelectedSpawner)
                occurrence++;
            if (occurrence == index)
            {
                waveObjects.Remove(wo);
                return;
            }
        }
    }

    internal List<WaveObject> GetWaveObjects()
    {
        throw new NotImplementedException();
    }
}
