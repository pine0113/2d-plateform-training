using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPHud : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp_max;
    public int hp_current;

    public int hp_per_heart;

    public Sprite emptyImage;
    public Sprite heartImage;

    public Image[] hearts;

    private RectTransform frame;
    void Start()
    {  
        frame = GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        setFrameSize();
    }

    void setFrameSize()
    {   
        int max_heart_num = hp_max / hp_per_heart;
        int current_heart_num = hp_current / hp_per_heart;
        frame.sizeDelta = new Vector2 (max_heart_num*32 + 48, 32);

        for(int i=0; i< hearts.Length;i++)
        {
            if(i<max_heart_num)
            {
                hearts[i].enabled=true;
            }
            else
            {
                hearts[i].enabled=false;
            }

            if(i<current_heart_num)
            {
                hearts[i].sprite = heartImage;
            }else
            {
                hearts[i].sprite = emptyImage;
            }

        }

    }


}
