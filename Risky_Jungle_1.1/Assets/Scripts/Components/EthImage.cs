using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts.com.ethereal.util;

/** 
*   @author    EtherealGF <www.etherealgf.com> 
*   @version   1.0 
*   @date      Octubre 16 2015
* 
*   @class  EthImage
*   @brief  Esta clase se encarga de administrar las propiedades de los elementos de tipo imagen.
*/
[Serializable]
public class EthImage : Image{

    /**
    *   @brief Variable que define si la imagen soporta otro lenguaje.
    */
    public bool languageSupport;

    /**
    *   @brief URL de la imagen en el directorio Resources sin la parte del "en" o "es" del lenguaje.
    */
    public string urlImage;
    
    /**
    *   @brief Abreviación representativa del lenguaje, ej: "es" para español o "en" para inglés.
    */
    private string curLanguage;
    
    /**
    *   @brief Método para iniciar el soporte de idiomas.
    */
    public void Start(){
    	base.Start();

        if ( languageSupport ){
            SetImageLang();
        }
    }

    /**
    *   @brief Método para cargar la imagen del idioma actual.
    */
    private void SetImageLang(){
        curLanguage = EthLang.LangAct;

        Sprite img = Resources.Load<Sprite>( urlImage + "_" + curLanguage );
        this.sprite = img;

        SetNativeSize();
    }

    /**
    *   @brief Método para mostrar en pantalla la imagen del idioma adecuado.
    */
    public void OnGUI(){
    	if ( languageSupport ){
            if ( curLanguage != EthLang.LangAct ){
                SetImageLang();
            }
    	}
    }
}
