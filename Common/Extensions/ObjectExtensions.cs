using ServiceStack.Text;

public static class ObjectExtensions {
    public static string ToSerialized<T>(this T instance) {
        return JsonSerializer.SerializeToString<T>(instance);
    }
}
