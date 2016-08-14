using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Boomlagoon.JSON;
using System.Text;
using Assets.Scripts.com.ethereal.util;

/** 
* @author    Carlos Andres Carvajal <ccarvajal@etherealgf.com> 
* @version   1.0 
* @date      Marzo 27 2014
* 
* @class SceneHandler 
* @brief Esta clase se encarga de cargar las escenas y de manejar el evento de atras.
* 
*  Carga las escenas y llama el evento cuando se le da back a las escenas, las escenas son de tipo IGui.
*/
public class SceneHandler: MonoBehaviour{

    /**
    *  @brief Instancia de la clase
    */
    private static SceneHandler _instance = null;

    /**
    *  @brief Escena actual.
    */
    private IGui _currentGui;

    /**
    *  @brief Obtiene la instancia de la clase.
    *
    *  @return la instancia de la clase.
    */
    public static SceneHandler GetInstance(){
        if(_instance == null){
            ObjectFactory.CreateObject<SceneHandler>("SceneHandler");   
        }
        return _instance;
    }

    /**
    *   @brief Función que se llama cuando se "instancia" la clase.
    */
    void Awake(){
        DontDestroyOnLoad( gameObject );
        SceneHandler._instance = this;
    }

    /**
    *   @brief Registra una pantalla de gui para que luego se pueda llamar si se oprime el boton de atras de android.
    *
    *   @param IGui gui GUI a registrar.
    */
    public void RegisterIGui( IGui gui ){
        this._currentGui = gui;
    }

    /**
    *   @brief Función que carga la escena enviada por parametro.
    *
    *   @param string name          Nombre de la escena a cargar.
    *   @param GameObject canvas    Canvas de la escena a cargar.
    */
    public static void LoadScene( string name, GameObject canvas = null ){
        if ( canvas != null ){
            EthPanel.ShowLoader( canvas );
        }
        Application.LoadLevel( name );
    }

    /**
    *   @brief Función que se llama una vez cada frame, y esta pendiente de cuando se oprima el boton de atras de android para avisarle a la pantalla que tenga registrada.
    */
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if ( EthPanel.count == 0 ){
                if ( _currentGui != null ){
                    _currentGui.BackButtonPressed();                
                }
            }
            else{
                EthPanel.currentPanel.Destroy();
            }
            
        }
    }
}
