using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts.com.ethereal.util;
using Assets.Scripts.com.ethereal.audio;

/** 
*   @author    EtherealGF <www.etherealgf.com> 
*   @version   1.0 
*   @date      Octubre 16 2015
* 
*   @class  EthUIButton
*   @brief  Esta clase se encarga de administrar las propiedades de los elementos de tipo botón.
*/
[Serializable]
public class EthUIButton : Button{

    /**
    *   @brief Variable que define si el botón tiene soporte para idiomas.
    */
    public bool languageSupport;

    /**
    *   @brief Texto en el lenguaje actual.
    */
    public string textLanguage;

    /**
    *   @brief URL que contiene el audio a reproducir al presionar el botón.
    */
    public string urlAudio = "";

    /**
    *   @brief Variable que define si el audio se reproduce por defecto.
    */
    public bool playDefaultAudio = true;
    
    /**
    *   @brief Texto en el lenguaje inicial.
    */
    private string _textLanguage;
    
    /**
    *   @brief Método para inicializar las variables.
    */
    public void Start(){
    	base.Start();

    	if ( languageSupport ){
            EthText text = gameObject.GetComponentInChildren<EthText>();
            text.text = EthLang.GetEntry(_textLanguage, true);
    		text.languageSupport = languageSupport;
            text.textLanguage = textLanguage;
    	}

        AddListener();
    }

    /**
    *   @brief Método para agregar el listener al botón.
    */
    private void AddListener(){
        this.onClick.AddListener(() => ClickOnButton() );
    }

    /**
    *   @brief  Método que se llama cuando se le da clic al botón.
    */
    private void ClickOnButton( ){
        if ( playDefaultAudio ){
            if ( EthAudio.GetInstance(null).GetAudioButtonDefault() != "" ){
                EthAudio.GetInstance(null).PlayEffect( EthAudio.GetInstance(null).GetAudioButtonDefault() );
            }
        }
        else if ( urlAudio != "" ){
            EthAudio.GetInstance(null).PlayEffect( urlAudio );
        }
    }

}
