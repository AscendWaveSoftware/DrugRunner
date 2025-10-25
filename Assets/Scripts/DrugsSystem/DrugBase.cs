using System.Collections;
using UnityEngine;

public abstract class DrugBase : IDrug
{
    protected float defaultDuration;

    public abstract DrugType Type { get; }
    public virtual float Duration => defaultDuration;
    public bool IsActive { get; private set; }

    protected DrugBase(float _duration) => defaultDuration = _duration;

    public void Begin(DrugsContext _ctx)
    {
        if (IsActive)
            return;
        IsActive = true;
        OnBegin(_ctx);

        if (Duration > 0)
            _ctx.Manager.StartCoroutine(AutoStop(Duration, _ctx));
    }

    public void End(DrugsContext _ctx)
    {
        if (!IsActive)
            return;
        IsActive = false;
        OnEnd(_ctx);
    }

    protected abstract void OnBegin(DrugsContext _ctx);
    protected abstract void OnEnd(DrugsContext _ctx);

    private IEnumerator AutoStop(float _sec,DrugsContext _ctx)
    {
        yield return new WaitForSeconds(_sec);
        End(_ctx);
    }
}
