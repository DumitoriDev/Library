using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookInfoPanel : MonoBehaviour
{

    [SerializeField] private Text bookName;
    [SerializeField] private Text author;
    [SerializeField] private Text genre;
    [SerializeField] private Text language;
    [SerializeField] private Text rate;
    [SerializeField] private Text desc;
    [SerializeField] private Text price;
    [SerializeField] private Text pages;
    [SerializeField] private Text type;
    [SerializeField] private Image cover;

    private string justBookPriceStr = "День аренды: ";
    private string userBookPriceStr = "К оплате: ";
    private string priceСurrency = " грн";
    void Start()
    {

    }

    public void SetValues(FullBookInfo fullBookInfo)
    {
        switch (fullBookInfo.bookType)
        {
            case FullBookInfo.BookType.UserBook:
                {
                    int days = (DateTime.Now - fullBookInfo.startDate).Days;
                    this.price.text = "Дней: "+ days +" \n"+ userBookPriceStr + fullBookInfo.price * days + priceСurrency;
                }
                break;
            case FullBookInfo.BookType.JustBook:
                {
                    this.price.text = justBookPriceStr + fullBookInfo.price + priceСurrency;
                }
                break;
            default:
                break;
        }

        this.bookName.text = fullBookInfo.bookName;
        this.author.text = fullBookInfo.author;
        this.genre.text = fullBookInfo.genre;
        this.language.text = fullBookInfo.language;
        this.rate.text = fullBookInfo.rate;
        this.desc.text = fullBookInfo.desc;

        this.pages.text = fullBookInfo.pages;
        this.type.text = fullBookInfo.type;
        this.cover.sprite = fullBookInfo.cover;
    }

    public void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
