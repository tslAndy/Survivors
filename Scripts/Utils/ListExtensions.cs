static class ListExtensions
{
    public static void SwapRemove<T>(this List<T> list, int index)
    {
        list[index] = list[^1];
        list.RemoveAt(list.Count - 1);
    }
}
