using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    const float verticalGradient = 1e5f;

    float gradient;
    float y_intercept;
    Vector2 pointOnLine_1;
    Vector2 pointOnLine_2;

    float gradientPerp;

    bool approachSide;

    public Line(Vector2 pointOn, Vector2 pointPerp)
    {
        float deltaX = pointOn.x = pointPerp.x;
        float deltaY = pointOn.y - pointPerp.y;

        if(deltaX == 0)
            gradientPerp = verticalGradient;
        else
            gradientPerp = deltaY / deltaX;

        if (gradientPerp == 0)
            gradient = verticalGradient;
        else
            gradient = -1 / gradientPerp;

        y_intercept = pointOn.y - gradient * pointOn.x;
        pointOnLine_1 = pointOn;
        pointOnLine_2 = pointOn + new Vector2(1, gradient);

        approachSide = false;
        approachSide = GetSide(pointPerp);
    }

    bool GetSide(Vector2 pt)
    {
        return (pt.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (pt.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
    }

    public bool HasCrossedLine(Vector2 p)
    {
        return GetSide(p) != approachSide;
    }

    public void DrawWithGizmos(float length)
    {
        Vector3 lineDir = new Vector3(1, 0, gradient).normalized;
        Vector3 lineCenter = new Vector3(pointOnLine_1.x, pointOnLine_1.y, 0) + Vector3.up;
        Gizmos.DrawLine(lineCenter - lineDir * length / 2f, lineCenter + lineDir * length / 2f);
    }
}
