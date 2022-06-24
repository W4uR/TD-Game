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
    public bool IsEmpty => waveObjects.Count == 0;
}
