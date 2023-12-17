using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public enum GameStep
{
    None,
    Step1,
    Step2,
    Step3
}

[System.Serializable]
public class GameData
{
    public string member_id;
    public string kiosk_category_id;
    public string play_date;
    public int play_stage;
    public int play_time;
    public int is_success;
    public int is_game;
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public MovieDatabase MovieDatabase;

    public int TotalCount;
    public int TotalPrice;
    public int TotalDiscount;

    public int CurrentCount;
    public List<string> SeatNum = new List<string>();
    public bool IsPay;
    public Movie SelectedMovie;

    //=========Game================
    public GameObject finishUI; //finishUI�� �����鼭 �������� Ȯ��
    public TextMeshProUGUI playTimeTxt; //���� ���� �ð�
    public TextMeshProUGUI randomTxt; //���� ���� ���� text
    public bool isSuccess; //���� ����

    [SerializeField] private GameObject _successPanel;
    [SerializeField] private GameObject _failPanel;

    private GameData _gameData;
    private GameStep _gameStep;
    private int playTime;
    private Stopwatch sw;
    private int random;
    private string _sceneNameType;
    private bool _saveData;

    private string[] step3 = { "����Ƽ�����", "ȯ��", "����Ʈ ����" };
    private bool[] _isStep3Success = new bool[3];

    private void Awake()
    {
        Instance = this;
        _gameData = new GameData();
    }

    private void Start()
    {
        Application.ExternalCall("unityFunction", _gameData.member_id);

        _gameData.play_date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        string sceneName = SceneManager.GetActiveScene().name; //����UI�� None ��ȯ
        _sceneNameType = sceneName.Substring(6, 5); //����UI�� None ��ȯ

        _gameData.kiosk_category_id = sceneName.Substring(0, 5);
        UnityEngine.Debug.Log(sceneName.Substring(10, 1));
        _gameData.play_stage = int.Parse(sceneName.Substring(10, 1));

        if (_sceneNameType.StartsWith("Prac"))
        {
            _gameData.is_game = 0;
        }
        else if (_sceneNameType.StartsWith("Test"))
        {
            _gameData.is_game = 1;
        }
        _gameStep = (GameStep)char.GetNumericValue(sceneName[sceneName.Length - 1]);

        //�ð� ����
        sw = new Stopwatch();
        sw.Start();

        //���� ���� (3�ܰ� 2���� ���� ����)
        if (randomTxt != null)
        {
            random = UnityEngine.Random.Range(0, 3);
            if (_gameStep == GameStep.Step3)
            {
                randomTxt.text = step3[random] + "�� �������ּ���.";
            }
        }

    }
  
    private void Update()
    {
        if (finishUI.activeSelf == true)
        {
            sw.Stop();
            switch (_gameStep)
            {
                case GameStep.Step1:
                    isSuccess = TotalCount > 0;

                    break;
                case GameStep.Step2:
                    isSuccess = TotalCount > 1;
                    break;
                case GameStep.Step3:
                    break;

            }


            if (isSuccess)
            {
                _successPanel.SetActive(true);
            }
            else
            {
                _failPanel.SetActive(true);
            }

            _gameData.is_success = Convert.ToInt32(isSuccess); //������ ��������
            UnityEngine.Debug.Log(_gameData.is_success);
            if (!_saveData)
            {
                SaveData(); //������ ���� ������

            }
            
        }

        //�ð� ���
        if (playTimeTxt != null)
        {
            playTime = (int)sw.ElapsedMilliseconds / 1000;
            int minutes;
            int seconds;

            minutes = playTime / 60;
            seconds = playTime % 60;

            if (minutes > 0)
            {
                playTimeTxt.text = "�ҿ� �ð� : " + minutes.ToString() + "�� " + seconds.ToString() + "��";
            }
            else
            {
                playTimeTxt.text = "�ҿ� �ð� : " + seconds.ToString() + "��";
            }

            _gameData.play_time = playTime;
        }
    }


    public void GameStep3Success(int index)
    {
        _isStep3Success[index] = true;
        if (_sceneNameType.StartsWith("Test"))
        {
            isSuccess = _isStep3Success[random];
        }
        else if (_sceneNameType.StartsWith("Prac"))
        {
            isSuccess = _isStep3Success[0] || _isStep3Success[1] || _isStep3Success[2];

        }

    }

    private void SaveData()
    {
        _saveData = true;
        //����ȭ
        string jsonData = JsonUtility.ToJson(_gameData);

        string url = "https://003operation.shop/kiosk/insertData";

        StartCoroutine(SendDataToWeb(jsonData, url));
    }

    private IEnumerator SendDataToWeb(string jsonData, string url)
    {
        byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest www = UnityWebRequest.PostWwwForm(url, "POST");
        www.uploadHandler = new UploadHandlerRaw(dataBytes);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("withCredentials", "true");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.LogError("Failed to send data to the web server: " + www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Data sent successfully!");
            SetQuit();
        }
    }

    public void ReceiveData(string message)
    {
        _gameData.member_id = message;
        UnityEngine.Debug.Log("Received message from JavaScript: " + message);
    }
    public void SetQuit()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}
