using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HideLoading : MonoBehaviour
{
    [SerializeField] float waitTime = 0.1f;
    private Image panel;
    // Start is called before the first frame update
    void Awake()
    {
        panel = GetComponent<Image>();
        panel.color = Color.black;
        StartCoroutine(WaitForLoading());

    }
    IEnumerator WaitForLoading()
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
    }
  
}
