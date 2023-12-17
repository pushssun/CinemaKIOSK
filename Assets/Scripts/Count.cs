using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Count : MonoBehaviour
{
    [SerializeField]
    private ToggleGroup[] _toggleGroup;
    [SerializeField]
    private TextMeshProUGUI _totalCount;
    [SerializeField]
    private TextMeshProUGUI _seatTotalCount;
    [SerializeField]
    private TextMeshProUGUI _seatTotalPrice;
    [SerializeField] 
    private Button _countButton;

    private int _maxCount;
    private int _count;

    // Start is called before the first frame update
    void Start()
    {
        _maxCount = 8;
        _countButton.interactable = true;
        _countButton.onClick.AddListener(() => GameManager.Instance.TotalCount = _count);
        foreach (var toggle in FindObjectsOfType<Toggle>())
        {
            toggle.onValueChanged.AddListener(OnChangeCount);
        }
    }

    private void OnChangeCount(bool isChanged)
    {
        if (isChanged)
        {
            _count = 0;
            for (int i = 0; i < _toggleGroup.Length; i++)
            {
                Toggle toggle = _toggleGroup[i].ActiveToggles().FirstOrDefault();
                _count += int.Parse(toggle.transform.GetChild(1).GetComponent<Text>().text);
            }

            _totalCount.text = string.Format(_count + "명");
            GameManager.Instance.TotalCount = _count;
            if (_count > _maxCount)
            {
                //버튼 비활성화
                _countButton.interactable = false;
            }
            else
            {
                _countButton.interactable = true;
            }
        }
        _seatTotalCount.text = string.Format(GameManager.Instance.TotalCount + "명");
        GameManager.Instance.TotalPrice = GameManager.Instance.TotalCount * 12000;
        _seatTotalPrice.text = string.Format(GameManager.Instance.TotalPrice + "원");
    }
}