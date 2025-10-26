using UnityEngine;

public class DrugsContext
{
    public DrugsManager Manager { get; }
    public VisualEffectsController Vfx { get; }

    public DrugsContext(DrugsManager _manager, VisualEffectsController _vfx)
    {
        Manager = _manager;
        Vfx = _vfx;
    }
}
