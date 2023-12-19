using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] Camera maincam;
    [SerializeField] CanvasGroup group;
    [SerializeField] TMP_Text Text;
    public static Tooltip instance;
    [SerializeField] bool Move;
    [SerializeField] bool DynamicSize;
    public RectTransform transform_rect;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        if (group != null)
        {
            group.blocksRaycasts = false;
            group.interactable = false;
            group.alpha = 0;
        }
        else
        {
            Debug.LogWarning("Warning: You forgot to assign a CanvasGroup!");
        }
        if(transform_rect == null) transform_rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Move)
        {
            transform.position = Input.mousePosition;
            //If our rect isn't null
            if (transform_rect != null)
            {
                //If our mouse is at the right side of the screen
                if (transform_rect.position.x > Screen.width / 2)
                {
                    transform_rect.pivot = new(1.05f, transform_rect.pivot.y);
                }
                else //If our mouse is at the left side of the screen
                {
                    transform_rect.pivot = new(-0.05f, transform_rect.pivot.y);
                }

                //If our mouse is at the top side of the screen
                if (transform_rect.position.y > Screen.height / 2)
                {
                    transform_rect.pivot = new(transform_rect.pivot.x, 1.05f);
                }
                else //If our mouse is at the bottom side of the screen
                {
                    transform_rect.pivot = new(transform_rect.pivot.x, -0.05f);
                }
                    
            }
            else Debug.LogWarning("Warning: You forgot to assign a RectTransform");

        }
        if (DynamicSize)
        {
            
            if (transform_rect != null)
                transform_rect.sizeDelta = Text.rectTransform.sizeDelta; //set the size of the background to the text size.
            else Debug.LogWarning("Warning: You forgot to assign a RectTransform");
        }
    }
    /// <summary>
    /// Opens the tooltip and displays any string of text you choose.
    /// </summary>
    public void OpenTooltip(string text)
    {
        group.alpha = 1;
        Text.text = text;

    }
    /// <summary>
    /// Closes the tooltip.
    /// </summary>
    public void CloseTooltip()
    {
        group.alpha = 0;
    }
}