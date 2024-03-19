using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveIcon : MonoBehaviour
{
    [SerializeField] private bool showAlways = true;
    [SerializeField] private Sprite icon;
    
    public bool isShowAlways => showAlways;
    public Sprite Icon => icon;
}
