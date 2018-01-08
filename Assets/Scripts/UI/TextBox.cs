using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextBox : MonoBehaviour {

    public GameLoader Loader;

    TextAsset Dialogue;
    string[] linesInFile;
    string[] line = new string[2];
    int count = 0;

    public Text Name;
    public Text TextBody;

    bool IsTyping;
    bool CancelTyping;
    public float TypeSpeed;

    private void Awake()
    {
        Dialogue = Loader.SelectedLevel.dialogue;
    }

    // Use this for initialization
    private void Start () {
        linesInFile = Dialogue.text.Split('\n');
        line = linesInFile[count].Split(':');
        Name.text = line[0];
        StartCoroutine(TextScroll(line[1]));
    }
	
	// Update is called once per frame
	private void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (!IsTyping)
            {
                count++;
                if (count < linesInFile.Length)
                {
                    line = linesInFile[count].Split(':');
                    Name.text = line[0];
                    StartCoroutine(TextScroll(line[1]));
                }
                else
                {
                    print("load scene");
                    SceneManager.LoadScene("BaseLevel");
                }
            }
            else if (IsTyping && !CancelTyping)
            {
                CancelTyping = true;
            }
        }
	}

    private IEnumerator TextScroll(string line)
    {
        int letter = 0;
        TextBody.text = "";
        IsTyping = true;
        CancelTyping = false;
        while (IsTyping && !CancelTyping && (letter < line.Length - 1))
        {
            //GetComponent<AudioSource>().Play();
            TextBody.text += line[letter];
            letter += 1;
            yield return new WaitForSeconds(TypeSpeed);
        }
        TextBody.text = line;
        IsTyping = false;
        CancelTyping = false;
    }

}
