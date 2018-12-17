using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Authentication : MonoBehaviour {

    [SerializeField] private Canvas atheneumCanvas;

    [SerializeField] private InputField login;
    [SerializeField] private InputField password;
    [SerializeField] private Button rememberMe;
    [SerializeField] private Sprite tickPlace;
    [SerializeField] private Sprite tick;
    [SerializeField] private GameObject connectionErrorPanel;
    [SerializeField] private GameObject authenticationErrorPanel;
    private bool rememberUser = false;

    public string rememberKey;
    private int defaultValue = -1;
    private int UserId = -1;

    private GameController gameController;
    private LibraryClass.ReaderRepository readerRepository=new LibraryClass.ReaderRepository();

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }
    void Start () {
        if(!LibraryClass.DataBaseContext.CheckConnection())
        {
            connectionErrorPanel.SetActive(true);
        }

        UserId = PlayerPrefs.GetInt(rememberKey, defaultValue);
        if(UserId!=defaultValue)
        {
            ActivateNextFunctional();
        }
	}

    public void RemeberUserClick()
    {
        rememberUser = !rememberUser;
        if (rememberUser) rememberMe.image.sprite = tick;
        else rememberMe.image.sprite = tickPlace;
    }

    public void LogInUser()
    {
        if (CheckDataRight())
        {
            if (rememberUser)
            {
                PlayerPrefs.SetInt(rememberKey, int.Parse(login.text));
            }
            else
            {
                PlayerPrefs.DeleteKey(rememberKey);
            }
            ActivateNextFunctional();
        }
        else
        {
            authenticationErrorPanel.SetActive(true);
        }
    }

    public void TryLoginInAgain()
    {
        authenticationErrorPanel.SetActive(false);
    }

    public bool CheckDataRight()
    {
        if (!int.TryParse(login.text, out UserId)) return false;
        LibraryClass.Reader reader = readerRepository.Get(UserId);
        if (reader == null) return false;
        if (reader.Password != password.text) return false;

        return true;
    }

    public void ActivateNextFunctional()
    {
        atheneumCanvas.gameObject.SetActive(true);
        gameController.Loged(UserId);
        gameObject.SetActive(false);
    }

    public bool CheckConnection()
    {
        return LibraryClass.DataBaseContext.CheckConnection();
    }

    public void LogOut()
    {
        PlayerPrefs.DeleteKey(rememberKey);
        atheneumCanvas.gameObject.SetActive(false);
        login.text =password.text= "";
    }
}
