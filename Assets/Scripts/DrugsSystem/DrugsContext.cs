using UnityEngine;

public class DrugsContext
{
    public DrugsManager Manager { get; }
    public PlayerEffects Player { get; }
    public VisualEffectsController Vfx { get; }

    public DrugsContext(DrugsManager _manager, PlayerEffects _player, VisualEffectsController _vfx)
    {
        Manager = _manager;
        Player = _player;
        Vfx = _vfx;
    }
}
