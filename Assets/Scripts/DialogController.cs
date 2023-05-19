using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Handle Level Dialog & Narrator's Animations
public class DialogController : MonoBehaviour
{    
    public enum AnimationState
    {
        Idle, Point, Nervous, Surprised, Shrug, Secret
    };

    public string[] dialog;
    public AnimationState[] animState;

    public Animator ManAnim;
    public TextMeshProUGUI UIText;

    private int dialogPosition = 0;
    private IEnumerator coroutine;
    private float initialGravity;

    void Awake() {
        if (dialog.Length > 0){ //Freeze gravity while dialog is still active
            initialGravity = Physics.gravity.y;
            Physics.gravity = Vector3.zero;
        } else {
            Debug.Log("Warning! No dialog entered for Dialog Controller!");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AdvanceDialog();
    }

    public void AdvanceDialog(){ 
        if (dialogPosition < dialog.Length){
            if (coroutine != null)
                StopCoroutine(coroutine);   //Check if previous coroutine is running to prevent weird text animation
            coroutine = _PlayDialogueText(dialog[dialogPosition], 1);
            StartCoroutine(coroutine);

            if (dialogPosition < animState.Length){
                ManAnim.Play("Idle", 0);       //Force Man to Idle first to ensure clean transition in case clicked early
                ManAnim.SetTrigger(animState[dialogPosition].ToString());
            }
                
            dialogPosition++;
        } else {
            Physics.gravity = new Vector3(0, initialGravity, 0);
            Destroy(gameObject);
        }
    }

    private IEnumerator _PlayDialogueText(string text, float duration)
    {
        float timer = 0;
        int separator = 0;
        UIText.text = "";        
    
        while (timer < duration)
        {
            // Find midpoint in string.
            separator = (int)Mathf.Lerp(0, text.Length, timer / duration);
    
            // Divide string in 2 and add color at separator.
            string left = text.Substring(0, separator);
            string right = text.Substring(separator, text.Length - separator);
            UIText.text = left + "<color=#00000000>" + right + "</color>";
    
            timer += Time.deltaTime;
            yield return null;
        }
    
        UIText.text = text;
    }
}
