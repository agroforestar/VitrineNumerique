using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class MenuArbresHaies : MonoBehaviour
{
    
    private string ArbreOuHaie = "arbre";
    
    private GameObject oldGo;
    
    public static int systemChoose = 0;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        
        var panel = GameObject.Find("Canvas").transform.GetChild(0);
        
        for (int j = 0; j < panel.transform.childCount; j++)
        {
            var child = panel.transform.GetChild(j);
            if (Char.IsDigit(child.name[0]))
            {
                int digit = int.Parse(child.name);
                if(digit != systemChoose)
                {
                    ChangeTransp(child.gameObject, 0.1f);
                    print("élément non sélect mis en transparence");
                }
                    
                else
                {
                    ChangeTransp(child.gameObject, 0.7f);
                    print("élément sélect mis en avant");
                    oldGo = child.gameObject;
                }
            }
        }
    }

    public void BtnSystem()
    {
        var go = EventSystem.current.currentSelectedGameObject;
        
        if(go == oldGo)
        
            return;
        
        systemChoose = int.Parse(go.name);
        ChangeTransp(go, 0.7f);
        print(ArbreOuHaie);
        
            
        
        if(oldGo != null)
        {
            ChangeTransp(oldGo, 0.1f);
            if (ArbreOuHaie == "arbre")
            {
                ArbreOuHaie = "haie";  
            }
            else if (ArbreOuHaie == "haie")
            {
                ArbreOuHaie = "arbre"; 
            }

            oldGo = go;
            print(ArbreOuHaie);
        }
            
    }



    void ChangeTransp(GameObject go, float transparency)
    {
        var rawImage = go.transform.GetComponentInChildren<RawImage>();
        var image = go.GetComponent<Image>();

        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, transparency); 
    
        image.color = new Color(image.color.r, image.color.g, image.color.b, transparency);
            
    }

}
