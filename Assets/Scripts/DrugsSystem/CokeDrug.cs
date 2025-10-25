using System;

public class CokeDrug : DrugBase
{
    public static event EventHandler OnCokeEnabled;
    public static event EventHandler OnCokeDisabled;

    public CokeDrug(float _duration) :base(_duration)
    {
        
    }

    public override DrugType Type => DrugType.Coke;

    protected override void OnBegin(DrugsContext _ctx)
    {
        _ctx.Vfx.SetCoke(true);
        OnCokeEnabled?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnEnd(DrugsContext _ctx)
    {
        _ctx.Vfx.SetCoke(false);
        OnCokeDisabled?.Invoke(this, EventArgs.Empty);
    }
}
