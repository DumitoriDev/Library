using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullBookInfo {

    public enum BookType { UserBook,JustBook}

    public string bookName;
    public string author;
    public string genre;
    public string language;
    public string rate;
    public string desc;
    public decimal price;
    public string pages;
    public string type;
    public string edition;
    public string date;
    public int count;
    public Sprite cover;
    public DateTime untilDate;
    public DateTime startDate;
    public BookType bookType;

    public FullBookInfo(BookType bookType, string bookName, string author, string genre, string language, string rate, string desc, decimal price, string pages, string type,string edition, string date,Sprite cover)
    {
        this.bookType = bookType;
        this.bookName = bookName;
        this.author = author;
        this.genre = genre;
        this.language = language;
        this.rate = rate;
        this.desc = desc;
        this.price = price;
        this.pages = pages;
        this.type = type;
        this.edition = edition;
        this.date = date;
        this.cover = cover;
    }

    public FullBookInfo() { }
}
