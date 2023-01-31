using UnityEngine;

public static class MyMath
{
    public static Vector3 CrossProduct(Vector3 v, Vector3 w)
    {
        float xMult = v.y * w.z - v.z * w.y;
        float yMult = v.z * w.x - v.x * w.z;
        float zMult = v.x * w.y - v.y * w.x;

        return new Vector3(xMult, yMult, zMult);
    }

    public static Vector2 DirFromAngle(Transform transform, float angleInDegrees, bool isAngleGlobal)
    {
        if (!isAngleGlobal)
            angleInDegrees += -transform.eulerAngles.z;
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }



    static float seconds;

    static float minutes;

    static float hours;



    public static string GetTimeString(float _seconds)
    {
        seconds = Mathf.RoundToInt(_seconds);
        minutes = Mathf.RoundToInt(seconds / 60);
        hours = Mathf.RoundToInt(minutes / 60);

        string timeString = hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString();

        return timeString;
    }
}
