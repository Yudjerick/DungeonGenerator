using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClassChooseMenu : MonoBehaviour
{
    [SerializeField] private List<string> classNames = new List<string>();
    [SerializeField] private int selectedIdx = 0;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
         text.text = classNames[selectedIdx];
    }

    public void NextClass()
    {
        selectedIdx++;
        if (selectedIdx == classNames.Count)
        {
            selectedIdx = 0;
        }
        text.text = classNames[selectedIdx];
    }

    public void PreviousClass() {
        selectedIdx--;
        if (selectedIdx == -1)
        {
            selectedIdx = classNames.Count - 1;
        }
        text.text = classNames[selectedIdx];
    }

}
