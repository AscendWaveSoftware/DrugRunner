using System;
using UnityEngine;

public class LSDDrug : DrugBase
{
    public static event EventHandler OnLSDEnabled;
    public static event EventHandler OnLSDDisabled;

    public LSDDrug(float _duration) :base(_duration)
    {
        
    }

    public override DrugType Type => DrugType.LSD;

    protected override void OnBegin(DrugsContext _ctx)
    {
        //TODO: CanDoubleJump true in Player Script
        _ctx.Vfx.SetLSD(true);
        
        OnLSDEnabled?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnEnd(DrugsContext _ctx)
    {
        //TODO: CanDoubleJump false in Player Script
        _ctx.Vfx.SetLSD(false);

        OnLSDDisabled?.Invoke(this, EventArgs.Empty);
    }
}
