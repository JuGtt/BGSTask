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
    private void Start()
    {
        UpdateEquipment();
    }

    private void OnEnable()
    {
        //EquipmentSlotUI.OnEquipmentSlotChanged += UpdateEquipment;
    }

    private void OnDisable()
    {
        //EquipmentSlotUI.OnEquipmentSlotChanged -= UpdateEquipment;
    }

    private void UpdateEquipment()
    {
        if (_hatSlot.EquippedItem != null)
        {
            _hair.gameObject.SetActive(false);
        }
        else
        {
            _hair.gameObject.SetActive(true);
            _hair.runtimeAnimatorController = _hairSlot.EquippedItem ? _hairSlot.EquippedItem.Animator : null;
        }   

        _underwear.runtimeAnimatorController = _underwearSlot.EquippedItem ? _underwearSlot.EquippedItem.Animator : null;
        _clothes.runtimeAnimatorController = _clothesSlot.EquippedItem ? _clothesSlot.EquippedItem.Animator : null;
        _hat.runtimeAnimatorController = _hatSlot.EquippedItem ? _hatSlot.EquippedItem.Animator : null;
    }
    #endregion
}
