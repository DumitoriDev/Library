using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchBookCell : BookCell {

    [SerializeField] Text countText;
	void Start () {
        bookInfoPanel = FindObjectOfType<BookInfoPanel>();
	}

    public override void SetValues(FullBookInfo fullBookInfo)
    {
        base.SetValues(fullBookInfo);
        countText.text = fullBookInfo.count.ToString();
    }

}
