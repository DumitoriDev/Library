using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBtnsPanel : MonoBehaviour {

    [SerializeField] private GameObject newBooksPanel;
    [SerializeField] private GameObject overdueBooksPanel;
    [SerializeField] private GameObject searchBooksPanel;
	void Start () {
		
	}
	
	public void SetNewBooksPanel()
    {
        newBooksPanel.SetActive(true);

        overdueBooksPanel.SetActive(false);
        searchBooksPanel.SetActive(false);
    }

    public void SetOverdueBooksPanel()
    {
        overdueBooksPanel.SetActive(true);

        newBooksPanel.SetActive(false);
        searchBooksPanel.SetActive(false);
    }

    public void SetSearchBooksPanel()
    {
        searchBooksPanel.SetActive(true);
        searchBooksPanel.GetComponent<BooksListCtrl>().WahtSearchShow();

        newBooksPanel.SetActive(false);
        overdueBooksPanel.SetActive(false);
    }
}
