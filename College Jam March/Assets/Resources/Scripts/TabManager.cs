using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class M_ChangeTab : UnityEvent<int>{}

public class TabManager : MonoBehaviour 
{
    public List<GameObject> tabObjects = new List<GameObject>();
    public int currentTab = 0;
    public M_ChangeTab E_ChangeTab;

    private void Awake() 
    {
        E_ChangeTab = new M_ChangeTab();
        for (int i=0; i < tabObjects.Count; i++)
        {
            if (currentTab == i) tabObjects[i].SetActive(true);
            else tabObjects[i].SetActive(false);
        }
    }

    public void SwapTab(int tabIndex)
    {
        if (tabIndex != currentTab)
        {
            tabObjects[currentTab].SetActive(false);
            tabObjects[tabIndex].SetActive(true);
            currentTab = tabIndex;
            E_ChangeTab.Invoke(tabIndex);
        }
    }
}