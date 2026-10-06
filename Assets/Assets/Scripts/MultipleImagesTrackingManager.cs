using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImagesTrackingManager : MonoBehaviour
{
    //prefabs to spawn
    [Header("Prefabs to Spawn")]
    [SerializeField] private List<GameObject> prefabsToSpawn = new List<GameObject>();

    //reference to ARTrackedImageManager
    private ARTrackedImageManager trackedImageManager;

    //dictionary of prefabs to spawn
    private Dictionary<string, GameObject> prefabCatalog = new Dictionary<string, GameObject>();

    //dictionary of instances of prefabs that have been spawned
    private Dictionary<TrackableId, GameObject> activeInstances = new Dictionary<TrackableId, GameObject>();

    //camera to check if image is still in view
    private Camera arCamera;

    //camera padding for camera check
    [Header("Camera Visibility Bounds")]
    [SerializeField] private float screenPadding = 0.05f;

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        arCamera = Camera.main;

        // get all the prefabs that can be spawned into the dictionary
        foreach (var prefab in prefabsToSpawn)
        {
            if (prefab != null) prefabCatalog[prefab.name.Trim()] = prefab;
        }
    }

    // enable/disable listeners
    private void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        // when image is first detected
        foreach (var trackedImg in eventArgs.added)
        {
            ProcessTrackedImage(trackedImg);
        }

        // when position or other changes
        foreach (var trackedImg in eventArgs.updated)
        {
            ProcessTrackedImage(trackedImg);
        }

        // when image detection is completely removed
        foreach (var trackedImg in eventArgs.removed)
        {
            // destroy the instance that had been spawned and remove it from the spawned dictionary
            if (activeInstances.TryGetValue(trackedImg.Value.trackableId, out GameObject instance))
            {
                Destroy(instance);
                activeInstances.Remove(trackedImg.Value.trackableId);
            }
        }
    }

    private void ProcessTrackedImage(ARTrackedImage trackedImage)
    {
        string imgName = trackedImage.referenceImage.name;

        // check if name is null
        if (string.IsNullOrEmpty(imgName)) return;

        //Debug.Log($"[AR] Detected image with name: '{imgName}'");

        // if object now spawned --> spawn it
        if (!activeInstances.ContainsKey(trackedImage.trackableId))
        {
            // find object to spawn using the name
            if (prefabCatalog.TryGetValue(imgName, out GameObject prefabToInstantiate))
            {
                //spawn object as child of tracked image 
                GameObject instance = Instantiate(prefabToInstantiate, trackedImage.transform);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.identity;

                activeInstances[trackedImage.trackableId] = instance;
                //Debug.Log($"[AR] Successfully spawned '{instance.name}' for image '{imgName}'");
            }
            else
            {
                //Debug.LogWarning($"[AR] Found image '{imgName}', but no prefab named '{imgName}' is in Prefabs To Spawn list!");
            }
        }

        if (activeInstances.TryGetValue(trackedImage.trackableId, out GameObject activeObj))
        {
            //check if tracking is gone or if card has left the camera view
            if (trackedImage.trackingState == TrackingState.None || !isInCameraView(trackedImage.transform.position))
            {
                activeObj.SetActive(false);
            }
            else activeObj.SetActive(true);
        }
    }

    // check if image is within camera views 
    private bool isInCameraView(Vector3 imgPosition)
    {
        Vector3 viewPos = arCamera.WorldToViewportPoint(imgPosition);

        // Z must be in front of the camera (> 0)
        // X and Y must be within the viewport bounds [0.0, 1.0]
        if (viewPos.z > 0 && viewPos.x >= screenPadding &&
            viewPos.x <= (1.0f - screenPadding) &&
            viewPos.y >= screenPadding &&
            viewPos.y <= (1.0f - screenPadding)) return true;
        else return false;
    }
}
