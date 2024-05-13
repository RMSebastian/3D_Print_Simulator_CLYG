using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [ContextMenuItem("ActivateMainCamera", "ActivateMainCamera")]
    [SerializeField] private GameObject mainCamera;
    [ContextMenuItem("ActivateMinigameCamera", "ActivateMinigameCamera")]
    [SerializeField] private GameObject minigameCamera;
    [SerializeField] private Camera cam;
    private CinemachineVirtualCamera cmMinigameCamera;

    [Header("PreHeatAnimation Configuration")]
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float maxTime = 0.95f;
    private void Start()
    {
        cmMinigameCamera = minigameCamera.GetComponent<CinemachineVirtualCamera>();
        ActivateMainCamera();
    }
    public void ActivateMinigameCamera()
    {
        cam.orthographic = true;
        mainCamera.SetActive(false);
        minigameCamera.SetActive(true);

    }

    public void ActivateMainCamera()
    {
        cam.orthographic = false;
        mainCamera.SetActive(true);
        minigameCamera.SetActive(false);

    }
    public void SetMinigameCameraTransformOffset(Vector3 position)
    {
        minigameCamera.transform.position += position;
    }
    public void SetMinigameCameraTransform(Vector3 position, Quaternion rotation, float ortographicSize)
    {
        
        minigameCamera.transform.position = position;
        minigameCamera.transform.rotation = rotation;
        if (cmMinigameCamera != null) cmMinigameCamera.m_Lens.OrthographicSize = ortographicSize;
    }
    public void SetMinigameCameraLookAt(GameObject go)
    {
        if (cmMinigameCamera != null) cmMinigameCamera.LookAt = go.transform;
    }
    public void SetMinigameCameraMovement(Vector3 position, Quaternion rotation, float ortographsize)
    {
        StopAllCoroutines();
        StartCoroutine(StartMovement(minigameCamera, position,rotation));
        if (cmMinigameCamera != null) cmMinigameCamera.m_Lens.OrthographicSize = ortographsize;
    }
    private IEnumerator StartMovement(GameObject go, Vector3 finalPosition, Quaternion finalRotation)
    {
        float time = 0;
        Vector3 initialPosition = go.transform.position;
        Quaternion initialRotation = go.transform.rotation;
        while (time < maxTime)
        {
            go.transform.position = Vector3.Lerp(initialPosition, finalPosition, curve.Evaluate(time));
            go.transform.rotation = Quaternion.Lerp(initialRotation, finalRotation, curve.Evaluate(time));
            time = (time + Time.deltaTime) / maxTime;
            yield return null;
        }
        yield return null;
    }
}
