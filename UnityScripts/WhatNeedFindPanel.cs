using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhatNeedFindPanel : MonoBehaviour {
    
    [SerializeField] InputField nameText;
    [SerializeField] InputField authorText;
    [SerializeField] InputField rateText;
    [SerializeField] Dropdown typeDropDown;
    [SerializeField] InputField genreText;
    [SerializeField] BooksListCtrl searchBooksPanel;
	void Start () {
		
	}

    public void StartSearch()
    {
        searchBooksPanel.ResetBooks();
        gameObject.SetActive(false);
    }

    public string GetAuthor()
    {
        return  authorText.text;
    }

    public string GetName()
    {
        return nameText.text;
    }

    public int GetRate()
    {
        int rate;
        if (int.TryParse(rateText.text, out rate)) return rate;
        else return 0;
    }

    public string GetGenre()
    {
        return genreText.text;
    }

    public string GetBookType()
    {
        return typeDropDown.options[typeDropDown.value].text;
    }
    
}
