using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    [Tooltip("The text that appears when the player touches the trigger")]
    public string dialogue;
    [Tooltip("The time (in seconds) that passes after a character in the sentence has been displayed before the next character gets displayed")]
    public float timeBetweenCharacters = 0.05f;
    [Tooltip("The two TextMeshPro objects that will display the dialogue text")]
    public TMP_Text dialogueText, dialogueTextDuplicate;
    [Tooltip("The two SpriteRenderers that displays the dialogue boxes")]
    public SpriteRenderer dialogueBox, dialogueBoxDuplicate;
    [Tooltip("Determines how long (in seconds) the text stays on screen. If set to 0, the text never disappears.")]
    public float lifeTime = 0.0f;

    // Use this for initialization
    void Start()
    {
        dialogueText.text = "";
        if (dialogueTextDuplicate != null)
            dialogueTextDuplicate.text = "";
        if (dialogueBox != null && dialogueBoxDuplicate != null)
        {
            dialogueBox.enabled = false;
            dialogueBoxDuplicate.enabled = false;
        }
    }

    IEnumerator TypeScentence(string sentence)
    {
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            if (dialogueTextDuplicate != null)
                dialogueTextDuplicate.text += letter;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && dialogueText.text == "")
        {
            if (dialogueBox != null && dialogueBoxDuplicate != null)
            {
                dialogueBox.enabled = true;
                dialogueBoxDuplicate.enabled = true;
            }
            StartCoroutine(TypeScentence(dialogue));
            if (lifeTime > 0.0f)
                StartCoroutine(HideText());
        }
    }

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(lifeTime);
        dialogueText.text = " ";
        if (dialogueTextDuplicate != null)
            dialogueTextDuplicate.text = " ";
        if (dialogueBox != null && dialogueBoxDuplicate != null)
        {
            dialogueBox.enabled = false;
            dialogueBoxDuplicate.enabled = false;
        }
    }
}
