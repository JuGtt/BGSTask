using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Hair")]
    [SerializeField]
    private Animator _hair;
    [SerializeField]
    private EquipmentSlotUI _hairSlot;

    [Header("Hat")]
    [SerializeField]
    private Animator _hat;
    [SerializeField]
    private EquipmentSlotUI _hatSlot;

    [Header("Clothes")]
    [SerializeField]
    private Animator _clothes;
    [SerializeField]
    private EquipmentSlotUI _clothesSlot;

    [Header("Underwear")]
    [SerializeField]
    private Animator _underwear;
    [SerializeField]
    private EquipmentSlotUI _underwearSlot;
    #endregion

    #region Private Fields
    private Animator _mainAnimator;
    #endregion

    #region Private Methods
    private void Start()
    {
        _mainAnimator = GetComponent<Animator>();
        UpdateEquipment();
    }

    private void OnEnable()
    {
        EquipmentSlotUI.OnEquipmentChanged += UpdateEquipment;
    }

    private void OnDisable()
    {
        EquipmentSlotUI.OnEquipmentChanged -= UpdateEquipment;
    }

    private void UpdateEquipment()
    {
        bool hatEquipped = _hatSlot.EquippedItem != null;
        bool clothesEquipped = _clothesSlot.EquippedItem != null;

        _hair.gameObject.GetComponent<SpriteRenderer>().enabled = !hatEquipped;
        if (!hatEquipped)
        {
            _hair.runtimeAnimatorController = _hairSlot.EquippedItem ? _hairSlot.EquippedItem.Animator : null;
            ResetAnimator(_hair);
        }

        _underwear.gameObject.GetComponent<SpriteRenderer>().enabled = !clothesEquipped;
        if (!clothesEquipped)
        {
            _underwear.runtimeAnimatorController = _underwearSlot.EquippedItem ? _underwearSlot.EquippedItem.Animator : null;
            ResetAnimator(_underwear);
        }


        _clothes.runtimeAnimatorController = _clothesSlot.EquippedItem ? _clothesSlot.EquippedItem.Animator : null;
        ResetAnimator(_clothes);

        _hat.runtimeAnimatorController = _hatSlot.EquippedItem ? _hatSlot.EquippedItem.Animator : null;
        ResetAnimator(_hat);

        ResetAnimator(_mainAnimator);
    }

    private void ResetAnimator(Animator animator)
    {
        animator.Rebind();
        animator.Update(0f);
    }
    #endregion
}
