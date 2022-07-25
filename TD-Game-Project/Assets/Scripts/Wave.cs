using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text;

public class Wave
{
    List<WaveObject> waveObjects;
    public Wave()
    {
        waveObjects = new List<WaveObject>();
    }
    public void AddWaveObject(WaveObject wo) { waveObjects.Add(wo); }
    public byte[] ToBytes => waveObjects.Select(x => x.ToBytes).ToArray().To1DArray();

    //Size of the wave in bytes
    public int Size => waveObjects.Count * WaveObject.Size;
    public byte NumberOfWaveObjects => (byte)waveObjects.Count();
    public bool IsEmpty => waveObjects.Count == 0;
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        foreach (var wo in waveObjects)
        {
            sb.Append($"{wo} --- ");
        }

        return sb.ToString();
    }
    public void SetWaveObject(int index,WaveObject newWo)
    {
        int indexOfWO = waveObjects.IndexOf(waveObjects.Where(x => x.SpawnerIndex == newWo.SpawnerIndex).ToArray()[index]);
        waveObjects.RemoveAt(indexOfWO);
        waveObjects.Insert(indexOfWO, newWo);

    }
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

    public List<WaveObject> GetWaveObjects(int spawnerIndex)
    {
        return waveObjects.Where(x => x.SpawnerIndex == spawnerIndex).ToList();
    }
}
