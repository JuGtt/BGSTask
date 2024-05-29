using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    #region Animation Methods
    public void OnAnimationFootstep()
    {
        AudioManager.Instance.PlaySound("Footstep", 0.2f);
    }

    public void OnAnimationRunningFootstep()
    {
        AudioManager.Instance.PlaySound("Footstep", 0.4f);
    }
    #endregion
}
