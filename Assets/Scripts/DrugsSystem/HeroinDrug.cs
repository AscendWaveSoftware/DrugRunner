using System;
using UnityEngine;

public class HeroinDrug : DrugBase
{
    public static event EventHandler OnHeroinEnabled;
    public static event EventHandler OnHeroinDisabled;

    public HeroinDrug(float _duration) : base(_duration)
    {

    }

    public override DrugType Type => DrugType.Heroin;

    protected override void OnBegin(DrugsContext _ctx)
    {
        //TODO: Collision von Spieler ausschalten (unbesiegbar) Player Script
        _ctx.Vfx.SetHeroin(true);

        OnHeroinEnabled?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnEnd(DrugsContext _ctx)
    {
        //TODO: Collison wieder aktivieren Player Script
        _ctx.Vfx.SetHeroin(false);

        OnHeroinDisabled?.Invoke(this, EventArgs.Empty);
    }
}
