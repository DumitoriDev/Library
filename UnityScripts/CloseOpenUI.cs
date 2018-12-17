using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[System.Serializable]
public class CloseOpenUI : MonoBehaviour
{

    [SerializeField] private CloseOpenUI[] linkedUIs;
    private Animator animator;
    [SerializeField] private bool open;
    [SerializeField] private int SpecialID;
    [SerializeField] private int ExitId;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Open", open);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseOpenAll()
    {
        for (int i = 0; i < linkedUIs.Length; ++i)
        {
            linkedUIs[i].GetComponent<Animator>().SetBool("Open", true);
        }
        animator.SetBool("Open", false);
    }

    public void CloseOpenSpecial()
    {
        linkedUIs[SpecialID].GetComponent<Animator>().SetBool("Open", true);
        animator.SetBool("Open", false);
    }
    public void CloseOpenExit()
    {
        linkedUIs[ExitId].GetComponent<Animator>().SetBool("Open", true);
        animator.SetBool("Open", false);
    }
}
