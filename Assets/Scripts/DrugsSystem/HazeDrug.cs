using UnityEngine;

public class HazeDrug : DrugBase
{
    private float prevScale;
    private float prevFixed;

    public HazeDrug(float _durationSec) : base(_durationSec) 
    {

    }

    public override DrugType Type => DrugType.Haze;

    protected override void OnBegin(DrugsContext _ctx)
    {
        prevScale = Time.timeScale;
        prevFixed = Time.fixedDeltaTime;
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.5f * 0.02f;
        _ctx.Vfx.SetHaze(true);
    }

    protected override void OnEnd(DrugsContext _ctx)
    {
        Time.timeScale = prevScale;
        Time.fixedDeltaTime = prevFixed;
        _ctx.Vfx.SetHaze(false);
    }
}
