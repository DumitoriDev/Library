using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserBookCell : BookCell {

    [SerializeField] Text dateText;
	void Start () {
        bookInfoPanel = FindObjectOfType<BookInfoPanel>();
	}

    public override void SetValues(FullBookInfo fullBookInfo)
    {
        base.SetValues(fullBookInfo);
        dateText.text = fullBook.date;
    }
}
