using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct PieceDescription
{
    public string Name;
    public Sprite Image;
    public int ID;
    public List<string> Parts;
}
