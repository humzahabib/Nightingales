using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FOW))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        FOW fow = (FOW)target;

        Handles.color = Color.red;
        Handles.DrawWireArc(fow.transform.position, Vector3.back, Vector3.up, 360, fow.viewRadius);
        Vector2 angleA = MyMath.DirFromAngle(fow.transform, -fow.viewAngle / 2, false);
        Vector2 angleB = MyMath.DirFromAngle(fow.transform, fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, (Vector2)fow.transform.position + angleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, (Vector2)fow.transform.position + angleB * fow.viewRadius);
    }
}
