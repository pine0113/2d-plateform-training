using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public Image image;
    public float time;
    public Color flashColor;
    private Color defaultColor;
    // Start is called before the first frame update
    void Start()
    {
        defaultColor = image.color;
        flashColor = Color.red;
        flashColor.a = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlashScreen()
    {
        StartCoroutine(Flash());

    }

    IEnumerator Flash()
    {
        image.color = flashColor;
        yield return new WaitForSeconds(time);
        image.color = defaultColor;
    }
}
