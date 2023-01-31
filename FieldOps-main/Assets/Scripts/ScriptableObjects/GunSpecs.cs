using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Configuration", menuName = "Configuration/GunSpecs")]
public class GunSpecs : ScriptableObject
{
    public Sprite icon;

    public bool automatic;

    public int maximumBulletsInPocket;

    public int magSize;

    public int remainingBulletsInMag;

    public int remainingBulletsInPocket;

    public bool reloading = false;

    public bool cooledDown = true;

    public float coolDownSeconds;

    public float reloadTime;

    public float elapsedSeconds;

    public int noise;




}

