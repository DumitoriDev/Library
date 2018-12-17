using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookCell : MonoBehaviour {

    [SerializeField] protected Text nameText;
    [SerializeField] protected Text authorText;

    protected BookInfoPanel bookInfoPanel;
    protected FullBookInfo fullBook;


    public string GetName() { return nameText.text; }
    public string GetAuthor() { return authorText.text; }
    


	void Start () {
    }

    public virtual void SetValues(FullBookInfo fullBookInfo)
    {
        nameText.text = fullBookInfo.bookName;
        authorText.text = fullBookInfo.author;
        this.fullBook = fullBookInfo;
    }

    public void BookInfo()
    {
        bookInfoPanel.Show();
        bookInfoPanel.SetValues(fullBook);
    }
}

