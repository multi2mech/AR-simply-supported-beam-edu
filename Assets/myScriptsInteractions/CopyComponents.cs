using UnityEngine;

public static class ComponentUtility
{
    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        // Add a new component of the same type to the destination
        T copy = destination.AddComponent<T>();

        // Copy all fields from the original to the new component
        System.Type type = typeof(T);
        System.Reflection.FieldInfo[] fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        foreach (var field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }

        return copy;
    }
}