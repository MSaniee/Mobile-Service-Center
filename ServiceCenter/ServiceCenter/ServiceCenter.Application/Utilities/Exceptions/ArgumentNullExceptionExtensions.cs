namespace ServiceCenter.Application.Utilities.Exceptions;

public static class CheckNullArgument
{
    public static T ThrowIfNull<T>(this T argument)
    {
        ArgumentNullException.ThrowIfNull(argument);

        return argument;
    }
}