using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonsStepManager : MonoBehaviour
{
    [SerializeField] private List<IButtonActivation> buttonStep;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite selectecSprite;
    private int index = 1;
    private void Awake()
    {
        buttonStep = new List<IButtonActivation>(GetComponentsInChildren<IButtonActivation>());
        buttons = new List<Button>(GetComponentsInChildren<Button>());
    }
    public void ActivateNextButton(UnityAction action = null)
    {
        buttonStep[index].Animating(action);
        index++;
    }
    public void SelectedButton(Button btn)
    {
        foreach(Button button in buttons)
        {
            button.image.sprite = defaultSprite;
        }
        btn.image.sprite = selectecSprite;
    }
}
