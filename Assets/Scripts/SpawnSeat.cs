
using UnityEngine;
using UnityEngine.UI;

public class SpawnSeat : MonoBehaviour
{
    [SerializeField]
    private string _row;
    [SerializeField]
    private int _start;
    [SerializeField]
    private int _end;

    private int _remainSelectSeat;


    // Start is called before the first frame update
    void Start()
    {
        _remainSelectSeat = GameManager.Instance.TotalCount;
        for(int i = _start-1; i < _end; i++)
        {
            int index = i;
            GameObject activeSeat = gameObject.transform.GetChild(i).gameObject;
            activeSeat.SetActive(true);
            activeSeat.transform.GetChild(1).GetComponent<Text>().text = _row + (index + 1).ToString();
            activeSeat.GetComponent<Toggle>().onValueChanged.AddListener((value) => SelectSeat(value, index));
            //activeSeat.GetComponent<Toggle>().onValueChanged.AddListener((value)=>SelectAutoSeat(value, index));
        }



        //Init();
    }

    private void Init()
    {
        if (_remainSelectSeat >= 2) //2�ڸ� �̻��̸�
        {
            //zone�� �� �ڸ��� ������ ������ �� ������
            for (int i = 0; i < gameObject.transform.childCount / 2; i++) //ó���� ������ �� 
            {
                int count = 0;
                for (int j = 0; j < gameObject.transform.childCount; i++)
                {
                    if ((j / 2) == i)
                    {
                        Toggle toggleButton = gameObject.transform.GetChild(j).GetComponent<Toggle>();
                        if (toggleButton.gameObject.activeSelf == true) //Ȱ��ȭ�� ��ư�� 1���� zone�� �ϳ��� �ִٸ� interactable = false
                            count++;
                        if (count < 2)
                        {
                            toggleButton.interactable = false;
                        }
                        else
                        {
                            toggleButton.interactable = true;

                        }
                    }

                }
            }
        }
    }

    //private void SelectAutoSeat(bool isOn, int index)
    //{
    //    int zone = index / 2;
    //    int count = 0;
    //    if (isOn)
    //    { 
    //        for (int i = 0; i < gameObject.transform.childCount; i++)
    //        {   if(_remainSelectSeat == 0) //count����ŭ ������� pay �����ϵ���
    //            {
    //                Debug.Log("Ȯ��");
    //                GameManager.Instance.IsPay = true;
    //            }
    //            if (i / 2 == zone)
    //            {
    //                Toggle toggleButton = gameObject.transform.GetChild(i).GetComponent<Toggle>();
    //                count++;
    //                if(count < 2)
    //                {
    //                    toggleButton.interactable = false;
    //                }
    //                if (_remainSelectSeat == 1) //�ϳ��� ���Ҵٸ� �� ���� ���õǵ���
    //                {
    //                    _remainSelectSeat--;
    //                    toggleButton.isOn = true;
    //                    return;
    //                }
    //                else //�ƴϸ� �� ���� �� ���� ���õǵ���
    //                {
    //                    _remainSelectSeat -= 2;
    //                    toggleButton.interactable = false;

    //                }
    //            }

    //        }
    //    }

    //}

    private void SelectSeat(bool isOn, int index)
    {
        if (isOn)
        {
            GameManager.Instance.CurrentCount++;
            GameManager.Instance.SeatNum.Add(gameObject.transform.GetChild(index).transform.GetChild(1).GetComponent<Text>().text.ToString());
        }
        else
        {
            GameManager.Instance.CurrentCount--;
            GameManager.Instance.SeatNum.Remove(gameObject.transform.GetChild(index).transform.GetChild(1).GetComponent<Text>().text.ToString());
        }
    }
}
