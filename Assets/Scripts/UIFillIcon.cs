using System;
using UnityEngine;
using UnityEngine.UI;

public class UIFillIcon : MonoBehaviour
{
    [SerializeField] private GameObject fillImg;

    public void SetFilled(bool newIsActive) => fillImg.gameObject.SetActive(newIsActive);
}