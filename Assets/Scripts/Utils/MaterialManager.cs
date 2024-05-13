using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    private MeshRenderer[] m_Materials; 
    // Start is called before the first frame update
    void Start()
    {
        m_Materials = this.transform.GetComponentsInChildren<MeshRenderer>();
    }

    public void ChangeColor(Color newColor)
    {
        for (int i = 0; i < m_Materials.Length; i++)
        {
            m_Materials[i].material.color = newColor;
        }
    }
}
