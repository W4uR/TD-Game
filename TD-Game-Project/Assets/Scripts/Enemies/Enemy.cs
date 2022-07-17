using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public byte id;
    public int health;
    public new string name;
    public Sprite icon;
    public Mesh model;

}
