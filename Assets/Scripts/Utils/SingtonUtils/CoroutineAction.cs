using System;
using System.Linq;
using UnityEngine;

public abstract class BaseCoroutineAction
{
    public float Progress { get; internal set; }

    protected Run run;
    protected Coroutine coroutine;

    internal BaseCoroutineAction(Run run, Coroutine coroutine)
    {
        this.run = run;
        this.coroutine = coroutine;
    }

    public abstract void Stop(bool invokeComplete);
}

public class CoroutineAction : BaseCoroutineAction
{
    public Action OnComplete;

    internal CoroutineAction(Run run, Coroutine coroutine, Action OnComplete) : base(run, coroutine)
    {
        this.OnComplete = OnComplete;
    }

    public override void Stop(bool invokeComplete)
    {
        run.StopCoroutine(coroutine);

        if (invokeComplete)
        {
            if (OnComplete != null)
            {
                OnComplete();
            }
        }
    }
}

public class CoroutineAction<T> : BaseCoroutineAction
{
    public Action<T> OnComplete;

    internal CoroutineAction(Run run, Coroutine coroutine, Action<T> OnComplete) : base(run, coroutine)
    {
        this.OnComplete = OnComplete;
    }

    public override void Stop(bool invokeComplete)
    {
        run.StopCoroutine(coroutine);

        if (invokeComplete)
        {
            if (OnComplete != null)
            {
                OnComplete(default(T));
            }
        }
    }
}
