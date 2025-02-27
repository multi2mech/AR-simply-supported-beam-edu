using System.Collections.Generic;
using UnityEngine;

public static class EarClipping
{
public static List<int> Triangulate(Vector3[] vertices, Vector3 normal)
{
    List<int> triangles = new List<int>();
    List<int> indices = new List<int>();

    for (int i = 0; i < vertices.Length; i++)
    {
        indices.Add(i);
    }

    while (indices.Count > 2)
    {
        bool earFound = false;

        for (int i = 0; i < indices.Count; i++)
        {
            int prevIndex = indices[(i - 1 + indices.Count) % indices.Count];
            int currentIndex = indices[i];
            int nextIndex = indices[(i + 1) % indices.Count];

            Vector3 prev = vertices[prevIndex];
            Vector3 current = vertices[currentIndex];
            Vector3 next = vertices[nextIndex];

            //Debug.Log($"Testing ear: {prevIndex}, {currentIndex}, {nextIndex}");

            if (IsEar(prev, current, next, vertices, indices, normal))
            {
                triangles.Add(nextIndex);    // Inizia dal vertice successivo
                triangles.Add(currentIndex); // Poi il vertice corrente
                triangles.Add(prevIndex);    // Infine il vertice precedente
                //Debug.Log($"Ear found: {prevIndex}, {currentIndex}, {nextIndex}");

                indices.RemoveAt(i);
                earFound = true;
                break;
            }
        }

        if (!earFound)
        {
            Debug.LogWarning("Impossibile trovare un'orecchia. Il poligono potrebbe essere non valido.");
            break;
        }
    }

    return triangles;
}
private static bool IsEar(Vector3 prev, Vector3 current, Vector3 next, Vector3[] vertices, List<int> indices, Vector3 normal)
{
    // Controlla se il triangolo è in senso orario
    if (!IsClockwise(prev, current, next, normal))
    {
        //Debug.Log($"Non è un'orecchia (non in senso orario): {prev}, {current}, {next}");
        return false;
    }

    // Controlla se un altro punto si trova all'interno del triangolo
    for (int i = 0; i < vertices.Length; i++)
    {
        if (!indices.Contains(i)) continue;

        Vector3 point = vertices[i];
        if (point == prev || point == current || point == next) continue;

        if (IsPointInTriangle(point, prev, current, next))
        {
            //Debug.Log($"Non è un'orecchia (vertice interno trovato): {point}");
            return false;
        }
    }

    return true;
}   
private static bool IsClockwise(Vector3 a, Vector3 b, Vector3 c, Vector3 normal)
{
    Vector3 cross = Vector3.Cross(b - a, c - a);
    return Vector3.Dot(cross, normal) < 0; // Adjust sign as per Unity's CCW convention
}
    private static bool IsPointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 v0 = c - a;
        Vector3 v1 = b - a;
        Vector3 v2 = p - a;

        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        return (u >= 0) && (v >= 0) && (u + v < 1);
    }
}