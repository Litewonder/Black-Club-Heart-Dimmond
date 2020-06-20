using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToEnter : MonoBehaviour
{

    public GameObject EnterDio;
    public Text GemNumber,toEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if ( Getnumber() <= 1)
            {
                toEnter.text = "You Need Get More Gem";
            }
            else
            {
                toEnter.text = "Press E To Enter";
            }
            EnterDio.SetActive(true);
        }   
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EnterDio.SetActive(false);
        }
    }

    public int Getnumber()
    {
        int gemnumber = int.Parse(GemNumber.text.Replace(":", ""));
        return gemnumber;
    }
}
