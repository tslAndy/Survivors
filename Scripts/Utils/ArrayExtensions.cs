namespace Utils;

static class ArrayExtensions
{
    public static T Find<T, U>(this T[] array, Func<T, U, bool> func, U param) =>
        array.Find(func, param, 0, array.Length);

    public static int IndexOf<T, U>(this T[] array, Func<T, U, bool> func, U param) =>
        array.IndexOf(func, param, 0, array.Length);

    // if necessary to use several params, pack structure  into U
    public static T Find<T, U>(
        this T[] array,
        Func<T, U, bool> func,
        U param,
        int start,
        int length
    )
    {
        for (int i = start; i < start + length; i++)
        {
            T elem = array[i];
            if (func(elem, param))
                return elem;
        }
        return default;
    }

    public static int IndexOf<T, U>(
        this T[] array,
        Func<T, U, bool> func,
        U param,
        int start,
        int length
    )
    {
        for (int i = start; i < start + length; i++)
        {
            T elem = array[i];
            if (func(elem, param))
                return i;
        }
        return -1;
    }
}
