using UnityEngine;


public class ShadowSense : MonoBehaviour
{

    UnityEngine.Rendering.Universal.Light2D[] lights;

    float[] intensities;

    IntEvent ShadowSenseActiveEvent = new IntEvent();

    bool isSenseActive = false;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddInvoker(INTEVENTS.SHADOWSENSEACTIVEEVENT, ShadowSenseActiveEvent);
        lights = GameObject.FindObjectsOfType<UnityEngine.Rendering.Universal.Light2D>();
        intensities = new float[lights.Length];
        for (int i = 0; i <= lights.Length - 1; i++)
        {
            intensities[i] = lights[i].intensity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("ShadowSense") != 0)
        {
            for (int i = 0; i <= lights.Length - 1; i++)
            {
                lights[i].intensity = .05f;
            }
            ShadowSenseActiveEvent.Invoke(1);
        }
        else if (Input.GetButtonUp("ShadowSense"))
        {
            for (int i = 0; i <= lights.Length - 1; i++)
            {
                lights[i].intensity = intensities[i];
            }
            ShadowSenseActiveEvent.Invoke(-1);
        }
    }

}
