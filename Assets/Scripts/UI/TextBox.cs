using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour {

    public TextAsset Dialogue;
    string[] linesInFile;
    string[] line = new string[2];
    int count = 0;

    public Text Name;
    public Text TextBody;

    bool IsTyping;
    bool CancelTyping;
    public float TypeSpeed;


	// Use this for initialization
	void Start () {
        linesInFile = Dialogue.text.Split('\n');
        line = linesInFile[count].Split(':');
        Name.text = line[0];
        StartCoroutine(TextScroll(line[1]));
    }
	
	// Update is called once per frame
	void Update () {
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
                    //termina
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
