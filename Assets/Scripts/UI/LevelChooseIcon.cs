using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelChooseIcon : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _lockPanel;
    [SerializeField] private TMP_Text _numberText;

    public void SetLevelData(int index, bool locked, UnityAction<int> onChoose)
    {
        _numberText.text = index.ToString();
        _lockPanel.SetActive(locked);
        if (locked)
        {
            _button.interactable = false;
        }
        else
        {
            _button.onClick.AddListener(() => onChoose.Invoke(index));
        }
    }
}
