using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public float InteractionRadius = 1.5f;
    public LayerMask playermask;
    public UnityEvent OnInteract;
    TMP_Text InteractionText;
    public string interactionText = "Use 'E' to interact with ___";
    Image textImg;
    bool disabledInteractionText;
    // Start is called before the first frame update
    void Start()
    {
        InteractionText = GameObject.Find("InteractionText (TMP)").GetComponent<TMP_Text>();
        textImg = InteractionText.transform.parent.GetComponent<Image>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, InteractionRadius);
    }
    // Update is called once per frame
    void Update()
    {
        if(Physics2D.OverlapCircle((Vector2)transform.position, InteractionRadius, playermask))
        {
            disabledInteractionText = false;
            InteractionText.text = interactionText;
            textImg.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnInteract.Invoke();
            }
        }
        else
        {
            if (!disabledInteractionText)
            {
                textImg.enabled = false;
                InteractionText.text = "";
                disabledInteractionText = true;
            }
        }
    }
}