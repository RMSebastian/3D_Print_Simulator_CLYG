using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
/// <summary>
/// This use InputSystem Component to receive InputActions
/// Shoot a raycast on where the screen its touch, keyboard or mouse
/// Verify if it's hitter its using the IInteractabla and use a Layer Interactable
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class SelectorHandler : MonoBehaviour
{
    #region Variables
    private Camera _cam;
    public LayerMask _mask;
    private AudioSource source;
    #endregion

    #region UnityMethods
    private void Start()
    {
        _mask = 6;
        _cam = Camera.main;
        source = GetComponent<AudioSource>();
    }
    #endregion

    #region Actions Methods
    public void OnClick(InputAction.CallbackContext ctx)
    {
        
        if (!ctx.started) return;
        
        Vector3 position = new Vector3();
        if (ctx.control.device.displayName == "Mouse")
        {
            position = Input.mousePosition;
        }
        else if(Input.touchCount == 1)
        {
            
            position = Input.GetTouch(0).position;
        }
        Ray ray = _cam.ScreenPointToRay(position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,Mathf.Infinity))
        {
            StartCoroutine(CheckPointerOverUI(hit));
            
        }
    }
    private IEnumerator CheckPointerOverUI(RaycastHit hit)
    {
        yield return null;
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isPointerOverUI && hit.transform.gameObject.layer == _mask && hit.transform.gameObject.activeSelf)
        {
            hit.transform.parent.transform.GetComponent<IInteractable>().Interact();
            source.Play();
        }

    }
    #endregion
}
