using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public readonly Vector3[] lookPoints;
    public readonly Line[] turnBoundaries;
    public readonly int finishLineIdx;

    public Path(Vector3[] waypoints, Vector3 start, float turnDst)
    {
        lookPoints = waypoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishLineIdx = turnBoundaries.Length - 1;

        Vector2 previousPt = (Vector2)start;
        for(int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = (Vector2)lookPoints[i];
            Vector2 dirToCurrentPoint = (currentPoint - previousPt).normalized;
            Vector2 turnBoundaryPoint = (i == finishLineIdx)?currentPoint : currentPoint - dirToCurrentPoint * turnDst;
            turnBoundaries[i] = new Line(turnBoundaryPoint, previousPt - dirToCurrentPoint * turnDst);
            previousPt = turnBoundaryPoint;
        }
    }

    public void GizmoDebugger()
    {
        Gizmos.color = Color.black;
        foreach(Vector3 p in lookPoints)
        {
            Gizmos.DrawCube(p + Vector3.up, Vector3.up);
        }
        Gizmos.color = Color.white;
        foreach(Line l in turnBoundaries)
        {
            l.DrawWithGizmos(10);
        }
    }
}
