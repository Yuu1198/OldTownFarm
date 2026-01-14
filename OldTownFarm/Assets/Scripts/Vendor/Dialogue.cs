using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public DialogueData[] dialogueData;
    public Button button1;
    public Button button2;
    public float textSpeed;

    private int index;

    public static Dialogue instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void OnDialogueBoxClick()
    {
        if (textComponent.text == dialogueData[index].line)
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = dialogueData[index].line;
            SetupButtons();
        }

    }

    public void StartDialogue(DialogueData[] dialogueLines)
    {
        gameObject.SetActive(true);
        dialogueData = dialogueLines;

        textComponent.text = string.Empty;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);

        foreach (char c in dialogueData[index].line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        SetupButtons();
    }

    void SetupButtons()
    {
        if (dialogueData[index].buttonText1 != string.Empty)
        {
            button1.gameObject.SetActive(true);
            button2.gameObject.SetActive(true);

            button1.GetComponentInChildren<TMP_Text>().text = dialogueData[index].buttonText1;
            button2.GetComponentInChildren<TMP_Text>().text = dialogueData[index].buttonText2;

            if (dialogueData[index].buttonAction1 != null)
                button1.onClick.AddListener(dialogueData[index].buttonAction1);
            if (dialogueData[index].buttonAction2 != null)
                button2.onClick.AddListener(dialogueData[index].buttonAction2);
        }
    }

    void NextLine()
    {
        if (index < dialogueData.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnButtonClick(bool button1clicked)
    {
        Debug.Log((button1clicked ? "I" : "You") + " have been clicked!");
    }
}

[System.Serializable]
public struct DialogueData
{
    public string line;
    public string buttonText1;
    public string buttonText2;
    public UnityAction buttonAction1;
    public UnityAction buttonAction2;

    public DialogueData(string l, string btn1, string btn2, UnityAction btnact1, UnityAction btnact2) : this()
    {
        line = l;
        buttonText1 = btn1;
        buttonText2 = btn2;
        buttonAction1 = btnact1;
        buttonAction2 = btnact2;
    }
}
