using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts.com.ethereal.util;

/** 
*   @author    EtherealGF <www.etherealgf.com> 
*   @version   1.0 
*   @date      Octubre 16 2015
* 
*   @class  EthText
*   @brief  Esta clase se encarga de administrar las propiedades de los elementos de tipo texto.
*/
[Serializable]
public class EthText : Text{

    /**
    *   @brief Variable que define si el texto tiene soporte para idiomas.
    */
    public bool languageSupport;

    /**
    *   @brief Texto en el lenguaje actual.
    */
    public string textLanguage;

    /**
    *   @brief Texto en el lenguaje inicial.
    */
    private string _textLanguage;
    
    /**
    *   @brief Método para inicializar las variables.
    */
    public void Start(){
    	base.Start();

    	_textLanguage = textLanguage;
    	if ( languageSupport ){
    		this.text = EthLang.GetEntry(_textLanguage, true);
    	}
    }

    /**
    *   @brief Método para mostrar el texto en pantalla.
    */
    public void OnGUI(){
    	if ( languageSupport ){
    		this.text = EthLang.GetEntry(_textLanguage, true);
    	}
    }
}
