using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(LineRenderer))]
public class splineScript : MonoBehaviour
{
    public SplineContainer spline;

    LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (spline == null) return;

        var s = spline.Spline;
        int knotCount = s.Count;

        // Get resolution based on knots
        int samplePoints = knotCount * 20; // smoother curve
        line.positionCount = samplePoints;

        for (int i = 0; i < samplePoints; i++)
        {
            float t = i / (float)(samplePoints - 1);
            Vector3 pos = s.EvaluatePosition(t);
            line.SetPosition(i, pos);
        }
    }
}
