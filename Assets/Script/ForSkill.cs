using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForSkill : MonoBehaviour
{
    public GameObject Forskill;
    public Text forSkill;
    void Update()
    {
        if (!Playercontrol.Instance.usingSkill  )
        {
            switch (Playercontrol.Instance.skillNumber)
            {
                case 0:
                    forSkill.text = ("Black: Dash");
                    break;
                case 1:
                    forSkill.text = ("Club: Go Back");
                    break;
                case 2:
                    forSkill.text = ("Dimmond: SlowerTime");
                    break;
                case 3:
                    forSkill.text = ("Heart: GetWell");
                    break;
                case 5:
                    forSkill.text = ("You didn't get skill!");
                    break;
                case 6:
                    forSkill.text = ("CD is not allready.");
                    break;
                default:
                    Forskill.SetActive(false);
                    break;
            }
        }
        else
        {
            Forskill.SetActive(false);
        }
    }

    void SetFalse()
    {
        Forskill.SetActive(false);
    }
}
