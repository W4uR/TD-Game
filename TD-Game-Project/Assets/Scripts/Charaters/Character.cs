using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Charater", menuName = "Charater")]
public class Character : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public Mesh model;
}
