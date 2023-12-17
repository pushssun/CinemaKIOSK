using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalPrice;
    [SerializeField] private TextMeshProUGUI _totalDiscount;
    [SerializeField] private TextMeshProUGUI _totalCount;
    [SerializeField] private TextMeshProUGUI _total;

    [SerializeField] private TextMeshProUGUI _movieTheather;
    [SerializeField] private TextMeshProUGUI _movieName;
    [SerializeField] private TextMeshProUGUI _movieDate;
    [SerializeField] private TextMeshProUGUI _movieSeat;
    [SerializeField] private Image _movieImage;

    // Start is called before the first frame update
    void Start()
    {
        _totalCount.text = GameManager.Instance.TotalCount.ToString() + "¸í";
        _totalDiscount.text = GameManager.Instance.TotalDiscount.ToString();
        _totalPrice.text = GameManager.Instance.TotalPrice.ToString();
        _total.text = (GameManager.Instance.TotalPrice - GameManager.Instance.TotalDiscount).ToString();

        Movie movie = GameManager.Instance.SelectedMovie;
        _movieName.text = movie.Name.ToString();    
        _movieTheather.text = movie.Theater.ToString() + "°ü";
        _movieDate.text = movie.Date + movie.Count + "È¸" + movie.Time;
        _movieImage.sprite = movie.Image;

        int i = 0;
        for(i = 0; i < GameManager.Instance.SeatNum.Count-1; i++)
        {
            _movieSeat.text += GameManager.Instance.SeatNum[i].ToString() + ", ";
        }
        _movieSeat.text += GameManager.Instance.SeatNum[i].ToString();
    }
}
