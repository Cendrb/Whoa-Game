using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Cave
{
    public float TargetLength { get; private set; }
    public float MaxHeight { get; private set; }
    public Vector2 BasePosition { get; private set; }
    public float BorderLength { get; private set; }
    public float Length { get; private set; }
    public float Height { get; private set; }
    public bool Symmetry { get; set; }

    private float lastUpperBorderY;
    private float lastLowerBorderY;

    public Cave(float length, float maxHeight, Vector2 basePosition, float borderLength)
    {
        TargetLength = length;
        MaxHeight = maxHeight * borderLength;
        BasePosition = basePosition;
        BorderLength = borderLength;
        lastLowerBorderY = -9;
        lastUpperBorderY = 9;
    }

    public BorderTransformData[] GetBordersPair()
    {
        if (Length < TargetLength)
        {
            BorderTransformData[] borders = new BorderTransformData[2];

            // Beginning expansion
            if (Length < (MaxHeight / BorderLength) / 3)
            {
                borders[0] = GenerateBorderWithAngle(45, BorderType.upper);
                borders[1] = GenerateBorderWithAngle(-45, BorderType.lower);
            }
            // Ending retraction
            else if ((Height / BorderLength) >= (TargetLength - Length))
            {
                if ((TargetLength - Length) == 1)
                {
                }
                else
                {
                    borders[0] = GenerateBorderWithAngle(-45, BorderType.upper);
                    borders[1] = GenerateBorderWithAngle(45, BorderType.lower);
                }
            }
            // Normal generation
            else
            {
                if (Symmetry)
                {
                    int angle = WhoaPlayerProperties.CaveBorderAnglesProbabilities.GetRandomItem();
                    borders[0] = GenerateBorderWithAngle(angle, BorderType.upper);
                    borders[1] = GenerateBorderWithAngle(angle, BorderType.lower);
                }
                else
                {
                    int upperBorderAngle = WhoaPlayerProperties.CaveBorderAnglesProbabilities.GetRandomItem();
                    int lowerBorderAngle = WhoaPlayerProperties.CaveBorderAnglesProbabilities.GetRandomItem();

                    borders[0] = GenerateBorderWithAngle(upperBorderAngle, BorderType.upper);
                    borders[1] = GenerateBorderWithAngle(lowerBorderAngle, BorderType.lower);
                }
            }

            Length++;

            return borders;
        }
        else
            return null;
    }

    private BorderTransformData GenerateBorderWithAngle(float angleDeg, BorderType type)
    {
        BorderTransformData borderData;
        if (type == BorderType.upper)
        {
            // UPPER
            borderData = new BorderTransformData(BorderLength, type, new Vector2(BorderLength * Length, lastUpperBorderY) + BasePosition, angleDeg);
            lastUpperBorderY += borderData.BorderHeight;
            Height += borderData.BorderHeight;
        }
        else
        {
            // LOWER
            borderData = new BorderTransformData(BorderLength, type, new Vector2(BorderLength * Length, lastUpperBorderY) + BasePosition, angleDeg);
            lastLowerBorderY += borderData.BorderHeight;
            Height += borderData.BorderHeight;
        }/*
        TransformData data = new TransformData();
        float angleRad = Mathf.Deg2Rad * angleDeg;
        // Scale
        float cos = Mathf.Cos(angleRad);
        float xScale = 1 / cos;
        data.Scale = new Vector2(xScale, 1);

        // Position
        float tan = Mathf.Tan(angleRad);
        float yPosOffset = tan * BorderLength;
        if (type == BorderType.upper)
        {
            // UPPER
            data.Position = new Vector2(BorderLength * Length, lastUpperBorderY + yPosOffset / 2) + BasePosition;
            lastUpperBorderY += yPosOffset;
            Height += yPosOffset;
        }
        else
        {
            // LOWER
            data.Position = new Vector2(BorderLength * Length, lastLowerBorderY + yPosOffset / 2) + BasePosition;
            lastLowerBorderY += yPosOffset;
            Height -= yPosOffset;
        }

        // Rotation
        data.Rotation = Quaternion.Euler(0, 0, angleDeg);*/

        return borderData;
    }

    public Vector2 GetLastPosition()
    {
        return BasePosition + new Vector2(BorderLength * Length, Mathf.Abs(lastLowerBorderY - lastUpperBorderY));
    }

    public static Cave GetRandomCave(Vector2 basePosition, float borderLength)
    {
        return new Cave(Random.Range(4, 20), Random.Range(3, 7), basePosition, borderLength);
    }
}

