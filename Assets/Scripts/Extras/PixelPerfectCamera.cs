/* A Basic Orthographic camera script
 * To get the correct orthosize, you must import all of your textures as sprites and
 * you must ensure that all of your sprites are using the same Pixels Per Unit count
 * and you must ensure that the orthoSize in this script is double your Pixels Per Unit
 */
using UnityEngine;
using System.Collections;

public class ThePixelPerfectCamera: MonoBehaviour
{
    public float orthoSize = 32f; // Check your sprite's in the inspector. This should be double the "Pixels Per Unit"
    private float lastPixelWidth = 0f;
    private float lastPixelHeight = 0f;
    private float lastOrtho;
    void Update()
    {
        // If the orthographic size is wrong or the camera's width or height has changed
        if (gameObject.GetComponent<Camera>().orthographicSize != Screen.height / orthoSize ||
             lastPixelWidth != gameObject.GetComponent<Camera>().pixelWidth ||
             lastPixelHeight != gameObject.GetComponent<Camera>().pixelHeight)
        {
            gameObject.GetComponent<Camera>().orthographicSize = Screen.height / orthoSize;
            lastPixelWidth = gameObject.GetComponent<Camera>().pixelWidth;
            lastPixelHeight = gameObject.GetComponent<Camera>().pixelHeight;
        }



    }

}
