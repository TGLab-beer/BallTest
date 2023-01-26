using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Dropdown))]
public class DropdownValueChanged : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    public GameManager manager;
    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    public void ValueChanged()
    {
        manager.ChangeDifficultLevel(dropdown.value);
    }
}
