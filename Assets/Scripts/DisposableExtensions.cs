using System;
using R3;

public static class DisposableExtensions
{
    public static IDisposable AssignTo(this IDisposable subscription, SerialDisposable serial)
    {
        serial.Disposable = subscription;
        return subscription;
    }

    public static IDisposable Subscribe<T>(this Observable<T> source, Action action)
    {
        return source.Subscribe(_ => action.Invoke());
    }
}
