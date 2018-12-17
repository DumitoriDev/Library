using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{

    [SerializeField] private GameObject[] backgrounds;
    private int backgroundId = 0;
    [SerializeField] private Sprite[] buttonSprites;
    private Image btnImage;
    void Start()
    {
        btnImage = GetComponent<Image>();
        backgrounds[backgroundId].SetActive(true);
        backgroundId++;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeBackground()
    {
        backgrounds[backgroundId - 1].SetActive(false);
        if (backgroundId >= backgrounds.Length) backgroundId = 0;

        backgrounds[backgroundId].SetActive(true);
        btnImage.sprite = buttonSprites[backgroundId];
        backgroundId++;
    }
}
