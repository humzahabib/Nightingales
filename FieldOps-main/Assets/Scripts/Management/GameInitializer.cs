using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Awake()
    {
        EventManager.Initialize();
    }
}
