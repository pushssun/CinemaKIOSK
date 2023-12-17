using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyRicket : MonoBehaviour
{
    [SerializeField]
    private Image _movieImage;
    [SerializeField]
    private TextMeshProUGUI _movieName;
    [SerializeField]
    private TextMeshProUGUI _movieTheather;
    [SerializeField]
    private TextMeshProUGUI _movieDate;

    [Header("Count")]
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private GameObject MoviePf;
    [SerializeField]
    private GameObject _agePopup;
    [SerializeField]
    private TextMeshProUGUI _ageText;
    [SerializeField]
    private GameObject _selectMovie;
    [SerializeField]
    private GameObject _selectCount;

    [Header("Seat")]
    [SerializeField]
    private Button _payButton;

    private MovieDatabase _movieDatabase;

    // Start is called before the first frame update
    void Start()
    {
        _movieDatabase = GameManager.Instance.MovieDatabase;
        SpawnMovieButton();
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentCount == GameManager.Instance.TotalCount)
        {
            _payButton.interactable = true;
        }
        else
        {
            _payButton.interactable = false;

        }
        
    }
    private void SpawnMovieButton() //영화 선택
    {
        for (int i = 0; i < _movieDatabase.Movies.Length; i++)
        {
            int index = i;
            MoviePf.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = _movieDatabase.Movies[i].Image;
            MoviePf.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = _movieDatabase.Movies[i].Name;
            MoviePf.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = _movieDatabase.Movies[i].Time;
            GameObject movie = Instantiate(MoviePf, spawnPoint);
            movie.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => OnMovieButton(index));
        }
    }
    private void OnMovieButton(int index)
    {
        _selectMovie.SetActive(false);
        _selectCount.SetActive(true);

        if (_movieDatabase.Movies[index].Age > 0)
        {
            _agePopup.SetActive(true);
            _ageText.text = _movieDatabase.Movies[index].Age.ToString();
        }

        GameManager.Instance.SelectedMovie = _movieDatabase.Movies[index];
        _movieImage.sprite = _movieDatabase.Movies[index].Image;
        _movieName.text = _movieDatabase.Movies[index].Name;
        _movieTheather.text = _movieDatabase.Movies[index].Theater + "관";
        _movieDate.text = _movieDatabase.Movies[index].Date + _movieDatabase.Movies[index].Count +"회" + _movieDatabase.Movies[index].Time;
    }
}
