using UnityEngine;
using System.Collections;

public class CameraTransition : MonoBehaviour
{
    [Header("Camera Views")]
    // Assign the first-person camera position/rotation (could be the player's head)
    public Transform fpvTransform;
    // Assign the overhead camera position/rotation for the tower defence view
    public Transform overheadTransform;
    
    [Header("Transition Settings")]
    // Duration of the camera transition in seconds
    public float transitionDuration = 2.0f;

    private bool isOverhead = false;
    private bool transitioning = false;
    private Camera mainCamera;

    void Start()
    {
        // Get the main camera; ensure your main camera has the "MainCamera" tag
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Make sure your camera has the MainCamera tag.");
        }
    }

    // Call this method to toggle between FPV and overhead view
    public void ToggleView()
    {
        if (transitioning || mainCamera == null)
            return;

        // Determine the starting and target transforms based on current view
        Transform startTransform = isOverhead ? overheadTransform : fpvTransform;
        Transform targetTransform = isOverhead ? fpvTransform : overheadTransform;
        
        // Start the smooth transition
        StartCoroutine(TransitionCamera(startTransform, targetTransform));

        // Toggle the state for next call
        isOverhead = !isOverhead;
    }

    // Coroutine to interpolate the camera's position and rotation
    IEnumerator TransitionCamera(Transform start, Transform target)
    {
        transitioning = true;
        float elapsedTime = 0f;

        // Cache start values
        Vector3 startPos = start.position;
        Quaternion startRot = start.rotation;
        Vector3 targetPos = target.position;
        Quaternion targetRot = target.rotation;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            // Optionally, you can apply an easing function to t here for a smoother feel
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure the camera finishes exactly at the target transform
        mainCamera.transform.position = targetPos;
        mainCamera.transform.rotation = targetRot;
        transitioning = false;
    }
}
