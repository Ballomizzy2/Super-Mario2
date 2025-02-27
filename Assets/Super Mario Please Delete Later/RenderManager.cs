using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderManager : MonoBehaviour
{
    public Transform cameraTransform;  // Assign the camera's transform in the inspector
    public float maxDistance = 50f;    // Maximum distance to keep objects enabled
    private GameObject[] allObjects;   // Array to store all GameObjects with renderers
    
    [SerializeField]
    private LayerMask layerToIgnore;

    void Start()
    {
        // Find all objects in the scene that have a Renderer component
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        allObjects = new GameObject[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            if(renderers[i].gameObject.layer != LayerMask.NameToLayer(layerToIgnore.ToString()))
                allObjects[i] = renderers[i].gameObject;  // Store the GameObjects that have Renderers
        }
    }

    void Update()
    {
        foreach (GameObject obj in allObjects)
        {
            if (obj != null)  // Ensure the object still exists
            {
                float distance = Vector3.Distance(cameraTransform.position, obj.transform.position);
                // Enable GameObject if within maxDistance, otherwise disable it
                if (distance <= maxDistance && !obj.activeSelf)
                {
                    obj.SetActive(true);
                }
                else if (distance > maxDistance && obj.activeSelf)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
