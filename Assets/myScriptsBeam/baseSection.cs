using UnityEngine;

public class BaseSection
{
    
    public static Vector3[] GetSection(SectionType sectionType, float extrusionLength, int beamRatio, BeamPositioning beamPositioning)
    {
        float width = extrusionLength / beamRatio;
        float height = extrusionLength / beamRatio;
        // Debug.Log("width: " + width+ " height: "+height);
        if (sectionType == SectionType.Squared)
        {
            return GetSquareSection(width, beamPositioning);
        }

        return GetRectangleSection(width, height, beamPositioning);
    }

    private static Vector3[] GetSquareSection(float size, BeamPositioning beamPositioning)
    {
        float halfSize = size / 2;

        Vector3[] square = new Vector3[]
        {
            new Vector3(-halfSize, -halfSize, 0), // Bottom left
            new Vector3(halfSize, -halfSize, 0),  // Bottom right
            new Vector3(halfSize, halfSize, 0),   // Top right
            new Vector3(-halfSize, halfSize, 0)   // Top left
        };

        return TransformSection(square, beamPositioning);
    }

    private static Vector3[] GetRectangleSection(float width, float height, BeamPositioning beamPositioning)
    {
        float halfWidth = width / 2;
        float halfHeight = height / 2;

        Vector3[] rectangle = new Vector3[]
        {
            new Vector3(-halfWidth, -halfHeight, 0), // Bottom left
            new Vector3(halfWidth, -halfHeight, 0),  // Bottom right
            new Vector3(halfWidth, halfHeight, 0),   // Top right
            new Vector3(-halfWidth, halfHeight, 0)   // Top left
        };

        return TransformSection(rectangle, beamPositioning);
    }

    private static Vector3[] TransformSection(Vector3[] section, BeamPositioning beamPositioning)
    {
        Vector3 center = beamPositioning.GetBaseCenter();
        Vector3 normal = beamPositioning.GetNormal();

        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, normal);

        for (int i = 0; i < section.Length; i++)
        {
            section[i] = rotation * section[i];
            section[i] += center;
        }

        return section;
    }
}



