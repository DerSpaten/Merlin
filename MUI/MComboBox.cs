using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TestMod.MUI
{
    /**
    <summary>   A combo box. (incomplete) </summary>
    
    <seealso cref="T:TestMod.MUI.MComponent"/>
    **/

    public class MComboBox : MComponent
    {
        /**
        <summary>   Default constructor. </summary>
        
        <param name="prefab">   (Optional) The prefab. </param>
        **/

        public MComboBox(GameObject prefab = null)
        {
            try
            {
                if (prefab == null)
                {
                    gameobject = GameObject.Instantiate(GameAccess.Prefabs.ComboBox1);
                }
                else
                {
                    gameobject = GameObject.Instantiate(prefab);
                }
                gameobject.name = "MComboBox";
                RectTransform = gameobject.GetComponentInChildren<RectTransform>();
            }catch(Exception e)
            {
                Debug.Log(e);
            }
        }                      
    }
}
