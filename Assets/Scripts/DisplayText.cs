using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayText : MonoBehaviour
{
    [SerializeField] private TMP_Text displayControls;
    [SerializeField] private float displayDuration;
    void Start()
    {
        StartCoroutine(ShowandHideText());
    }

    IEnumerator ShowandHideText()
    {
        displayControls.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        displayControls.gameObject.SetActive(false);

    }
    
}
