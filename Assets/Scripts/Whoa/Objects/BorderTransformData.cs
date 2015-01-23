using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum BorderType
{
    upper,
    lower
}

/// <summary>
/// Keeps same width by scaling
/// </summary>
public class BorderTransformData
{
    public TransformData Transform { get; private set; }
    /// <summary>
    /// Angle in degrees
    /// </summary>
    public float Angle { get; private set; }
    public BorderType Type { get; private set; }

    public float BorderHeight { get; private set; }
    public float BorderWidth { get; private set; }

    Vector2 basePosition;

    public BorderTransformData(float borderWidth, BorderType type)
    {
        basePosition = new Vector2();
        Angle = 0;
        this.BorderWidth = borderWidth;
        Type = type;

        calculateTransform();
    }
    public BorderTransformData(float borderWidth, BorderType type, Vector2 basePosition, float angle)
    {
        this.basePosition = basePosition;
        Angle = angle;
        this.BorderWidth = borderWidth;
        Type = type;

        calculateTransform();
    }

    public void SetPosition(Vector2 position)
    {
        basePosition = position;
        calculateTransform();
    }


    /// <summary>
    /// Sets angle of the border
    /// </summary>
    /// <param name="angle">Angle in degrees</param>
    public void SetAngle(float angle)
    {
        Angle = angle;
        calculateTransform();
    }

    private void calculateTransform()
    {
        TransformData data = new TransformData();
        float angleRad = Mathf.Deg2Rad * Angle;
        // Scale
        float cos = Mathf.Cos(angleRad);
        float xScale = 1 / cos;
        data.Scale = new Vector2(xScale, 1);

        // Position
        float tan = Mathf.Tan(angleRad);
        float yPosOffset = tan * BorderWidth;
        data.Position = new Vector2(0, yPosOffset / 2) + basePosition;
        BorderHeight = yPosOffset;

        // Rotation
        data.Rotation = Quaternion.Euler(0, 0, Angle);

        Transform = data;
    }
}

