using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Private Fields
    private Animator _baseAnimator;
    private Animator[] _clothingAnimators;
    #endregion

    #region Public Methods
    public void PopulateClothingAnimators()
    {
        int animatorsCount = transform.childCount;
        _clothingAnimators = new Animator[animatorsCount];

        for (int i = 0; i < animatorsCount; i++)
        {
            _clothingAnimators[i] = GetComponent<Animator>();
        }
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        _baseAnimator = GetComponent<Animator>();
        PopulateClothingAnimators();
    }

    private void Update()
    {
        SyncAnimators();
    }

    private void SyncAnimators()
    {
        AnimatorStateInfo stateInfo = _baseAnimator.GetCurrentAnimatorStateInfo(0);

        foreach (Animator animator in _clothingAnimators)
        {
            animator.Play(stateInfo.fullPathHash, 0, stateInfo.normalizedTime);
        }
    }

    public void SetAnimationState(string state)
    {
        _baseAnimator.Play(state);

        foreach (Animator animator in _clothingAnimators)
        {
            animator.Play(state);
        }
    }
    #endregion
}
