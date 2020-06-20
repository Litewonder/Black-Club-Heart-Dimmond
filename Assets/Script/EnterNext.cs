using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterNext : MonoBehaviour
{
    public Text GemNumber;
    void Update()
    {
        
        if (Input .GetKeyDown(KeyCode.E) && Getnumber()>=2 )
        {
            SoundManage.soundmanage.OpendoorAudio();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public int Getnumber()
    {
        int gemnumber = int.Parse(GemNumber.text.Replace(":", ""));
        return gemnumber;
    }
}
