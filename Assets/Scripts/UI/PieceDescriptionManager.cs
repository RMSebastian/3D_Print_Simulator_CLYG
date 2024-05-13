using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PieceDescriptionManager : MonoBehaviour
{
    [SerializeField] private DescriptionHolder holder;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject boxContent;
    [SerializeField] private Button button;
    private Vector2 currentResolution;
    private Vector2 referenceResolution = new Vector2(1920,1080);
    private float rectWidth;
    private float referenceSpacing = 255f;
    private float currentSpacing;
    private float xMultiplier;
    private List<string> descriptions = new List<string>();
    private List<DescriptionHolder> holderAlive = new List<DescriptionHolder>();
    
    private bool firstTime = false;
    private void Start()
    {
        currentResolution = new Vector2(Screen.width, Screen.height);
        xMultiplier = currentResolution.x/referenceResolution.x;
        currentSpacing = referenceSpacing * xMultiplier;
        
    }
    public void SetButtonAction(UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
    public void SetPosition(Vector3 position)
    {
        Vector3 newPosition = new Vector3(position.x, this.transform.position.y, 0);
        //print(boxContent.transform.position.x);
        if(newPosition.x > currentResolution.x/2) 
        {
            //print("right");
            rectWidth = ((referenceResolution.x / 2) + ((1284f / 2) / 2)) * xMultiplier;
            if (newPosition.x > rectWidth)
            {
                //print("corner right");
                newPosition = new Vector3(position.x - (((1284f / 2) / 2.5f) * xMultiplier),this.transform.position.y, 0);
            }
            else
            {
                //print("corner left");
                newPosition = new Vector3(position.x + (((1284f / 2) / 2.5f) * xMultiplier), this.transform.position.y, 0);
            }
        }
        else
        {
            //print("left");
            rectWidth = ((referenceResolution.x / 2) - ((1284f / 2) / 2)) * xMultiplier;
            if (newPosition.x > rectWidth)
            {
                //print("corner right");
                newPosition = new Vector3(position.x - (((1284f / 2) / 2.5f) * xMultiplier), this.transform.position.y, 0);
            }
            else
            {
                //print("corner left");
                newPosition = new Vector3(position.x + (((1284f / 2) / 2.5f) * xMultiplier), this.transform.position.y, 0);
            }
        }
        boxContent.transform.position = newPosition;

    }
    public void SetListOfDescription(List<string> strings)
    {
        foreach (string s in strings)
        {
            descriptions.Add(s);
        }
        if(!firstTime)
        {
            firstTime = true;
            SetHolderByInstantiate(descriptions);
        }    
        else
        {
            SetHoldersByRedefinition(descriptions);
        }
    }
    private void SetHolderByInstantiate(List<string> strings)
    {
        foreach(string s in strings) 
        {
            DescriptionHolder go = Instantiate(holder,parent.transform);
            go.SetDescription(s);
            holderAlive.Add(go);   
        }
        strings.Clear();
    }
    private void SetHoldersByRedefinition(List<string> strings)
    {
        HideDescriptions();
        int i  = -1;
        bool overfloat = false;
        foreach (string s in strings)
        {
            i++;
            if(i <= holderAlive.Count -1)
            {
                holderAlive[i].gameObject.SetActive(true);
                holderAlive[i].SetDescription(s);
            }
            else
            {
                overfloat = true;
                break;
            }
        }
        if(overfloat)
        {
            strings.RemoveRange(0, i);
            SetHolderByInstantiate(strings);
        }
        else
        {
            strings.Clear();
        }    
    }
    private void HideDescriptions()
    {
        foreach (DescriptionHolder s in holderAlive)
        {
            s.gameObject.SetActive(false);
        }
    }

}
