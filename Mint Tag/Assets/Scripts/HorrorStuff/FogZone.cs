using UnityEngine;

public class FogZone : MonoBehaviour
{
    public GameObject player;
    public float fogDensity = 0.05f;
    public Color fogColor = Color.gray; // Default fog color is gray
    private float initialFogDensity;
    private Color initialFogColor;

    private void Start()
    {
        // Store the initial fog density and color for later use
        initialFogDensity = RenderSettings.fogDensity;
        initialFogColor = RenderSettings.fogColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            // Enable fog and set fog properties when the player enters the trigger area
            RenderSettings.fog = true;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.fogColor = fogColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            // Disable fog and reset fog properties when the player exits the trigger area
            RenderSettings.fog = false;
            RenderSettings.fogDensity = initialFogDensity;
            RenderSettings.fogColor = initialFogColor;
        }
    }
}
