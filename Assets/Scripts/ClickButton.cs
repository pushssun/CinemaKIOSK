using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour
{
    public TextMeshProUGUI phoneNumber;
    public Button[] number;
    public Button cancelButton;
    public Button clearButton;

    private string phoneNumberstr;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < number.Length; i++)
        {
            int index = i;
            number[i].onClick.AddListener(() => ClickNumber(index));
        }
        cancelButton.onClick.AddListener(CancelNumber);
        clearButton.onClick.AddListener(ClearNumber);

    }

    public void ClearNumber()
    {
        phoneNumberstr = null;
        UpdateNumber();
    }
    private void ClickNumber(int index)
    {
        phoneNumberstr += number[index].transform.Find("Text").GetComponent<TextMeshProUGUI>().text;
        UpdateNumber();
    }
    private void CancelNumber()
    {
        if (phoneNumberstr.Length > 0)
        {
            phoneNumberstr = phoneNumberstr.Substring(0, phoneNumberstr.Length - 1);
            UpdateNumber();
        }
    }

    private void UpdateNumber()
    {
        phoneNumber.text = phoneNumberstr;
    }
}
