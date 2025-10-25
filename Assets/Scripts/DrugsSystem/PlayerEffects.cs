using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [Header("Abilities")]
    public bool canDoubleJump;

    private int extraJumpsUsed = 0;

    public void EnableDoubleJump(bool _enable)
    {
        canDoubleJump = _enable;
        extraJumpsUsed = 0;
    }

    /// <summary>
    /// Hook im Jump-System
    /// </summary>
    public bool TryConsumeExtraJump()
    {
        if (!canDoubleJump)
            return false;
        if (extraJumpsUsed >= 1)
            return false;
        extraJumpsUsed++;
        return true;
    }

    public void ResetGround()
    {
        extraJumpsUsed = 0;
    }
}
