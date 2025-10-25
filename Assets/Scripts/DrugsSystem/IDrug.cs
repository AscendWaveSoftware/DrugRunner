using System;
using UnityEngine;

public interface IDrug
{
    public DrugType Type { get; }
    public float Duration { get; }
    public bool IsActive { get; }

    public void Begin(DrugsContext _ctx);
    public void End(DrugsContext _ctx);
}
