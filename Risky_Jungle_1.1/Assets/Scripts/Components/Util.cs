using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.com.ethereal.audio;
using Assets.Scripts.com.ethereal.util;

/** 
*   @author    EtherealGF <www.etherealgf.com> 
*   @version   1.0 
*   @date      Septiembre 22 2015
* 
*   @class  Util
*   @brief  Clase que se encarga de las diferentes funcionalidades que ayudan a la aplicacion.
*/
public class Util {

    /*
    *   @brief Guarda el identificador unico de cada dispositivo.
    */
    public static string imei;
    
    /*
    *   @brief Guarda el id de sesion para cada jugada.
    */
    public static string sessionId;

    /*
    *   @brief Ultima respuesta.
    */
    public static string lastResponse;

    /*
    *   @brief Constantes que indica la orientacion horizontal de las fichas que se van a lanzar.
    */
    public const int HORIZONTAL = 0;
    /*
    *   @brief Constantes que indica la orientacion vertical de las fichas que se van a lanzar.
    */
    public const int VERTICAL = 1;

    /*
    *   @brief Direccion"izquierda" para el algoritmo de busqueda de jugadas.
    */
    public const int LEFT  = 0;
    /*
    *   @brief Direccion "derecha" para el algoritmo de busqueda de jugadas.
    */
    public const int RIGHT = 1;
    /*
    *   @brief Direccion "arriba" para el algoritmo de busqueda de jugadas.
    */
    public const int UP    = 2;
    /*
    *   @brief Direccion "abajo" para el algoritmo de busqueda de jugadas.
    */
    public const int DOWN  = 3;

    /**
    *   @brief sonido del cannon.
    */
    private static AudioSource _cannonSound = null;

    /**
    *   @brief velocidades de la oveja.
    */
    private static List<float> _velocities = new List<float>();

    /*
    *   @brief Retorna un numero aleatorio entre 0(incluyendo) y max(excluyendo).
    *
    *   @param int max, Numero maximo del intervalo a crear el intervalo para el numero aleatorio.
    *
    *   @return Numero aleatorio entre 0 y el numero maximo ingresado.
    */
    public static int GetRand(int max) {

        return Random.Range(0, max);
    }

    
    /*
    *   @brief Metodo que activa o desactiva el sonido, true lo activa false desactiva.
    *
    *   @param bool state, estado del sonido (encendido o apagado).
    *
    *   @return false si el sonido esta prendido, de lo contrario true
    */
    public static bool ToggleSound(bool state) {

        float musicVolume = state ? 100f : 0f;
        EthAudio.GetInstance(null).SetMusicVolume(musicVolume);
        EthAudio.GetInstance(null).SetEffectsVolume(musicVolume);
        return !state;
    }

    /*
    *   @brief Metodo que obtiene el idioma en que esta configurado el dispositivo, solo retorna español o ingles
    *   porque son los idiomas que se estan manejando en los juegos.
    *
    *   @return idioma del systema.
    */
    public static string GetSystemLanguage() {
        if(Application.systemLanguage == SystemLanguage.Spanish) {

            return "es";
        }
        return "en";
    }

    /*
    *   @brief Metodo que obtiene un objeto hijo por el nombre.
    *
    *   @param GameObject gameObject,  objeto padre.
    *   @param string name, nombre del hijo.
    *
    *   @return Referencia del hijo o nulo si no encuentra hijos con ese nombre.
    */
    public static GameObject GetChildByName(GameObject gameObject, string name) {

        Transform[] trans = gameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform trAct in trans) {

            if (trAct.gameObject.name == name) {

                return trAct.gameObject;
            }
        }
        return null;
    }

    /*
    *   @brief Metodo que destruye todos los hijos de un gameobject.
    *
    *   @param GameObject gameObject,  objeto padre.
    */
    public static void DestroyChildren( GameObject go ){
        int childs = go.transform.childCount;
 
        for (int i = childs - 1; i >= 0; i--){
            GameObject.Destroy(go.transform.GetChild(i).gameObject);
        }
    }

    /*
    *   @brief Metodo que halla el angulo formado por un vector.
    *
    *   @param Vector2 vector2,  vector que forma el angulo a hallar.
    */
    public static float Angle(Vector2 vector2) {
        
        if (vector2.y < 0) {

            return 360 - (Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg * -1);
        } else {

            return Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;
        }
    }

    /**
    *   @brief Método para reproducir un sonido en un ciclo.
    *
    *   @param MonoBehaviour parent,    MonoBehaviour en el que se instancia el audio.
    *   @param string loop,     sonido a reproducir en un ciclo 
    *
    */
    public static void PlayLoop(MonoBehaviour parent, string loop) {

        EthAudio.GetInstance(parent).PlayMusic("Audio/"+loop);
    }

    /**
    *   @brief Método para reproducir un efecto.
    *
    *   @param string effect,   efecto a reproducir. 
    */
    public static void PlayEffect(string effect) {

        EthAudio.GetInstance(null).PlayEffect("Audio/"+effect);
    }

    /**
    *   @brief Método para reproducir un efecto cuando se modifica un objeto especifico.
    *
    *   @param string effect,   efecto a reproducir. 
    *   @param GameObject obj,  objeto que es modificado por lo tanto se reproduce su efecto correspondiente.
    *
    *   @return Game object requerido.
    */
    public static void PlayLoopEffectTiedToObject(string effect, GameObject obj) {

        effect = "Audio/"+effect;
        GameObject go = new GameObject();
        go.transform.parent = obj.transform;
        Vector3 tmp = go.transform.position;
        tmp.z = Camera.main.gameObject.transform.position.z + 1;
        go.transform.position = tmp;
        AudioSource effectSource = go.AddComponent<AudioSource>();// as AudioSource;
        effectSource.loop = true;

        AudioClip efecto = (AudioClip)Resources.Load(effect);
        effectSource.clip = efecto;
        effectSource.volume = EthAudio.GetInstance(null).GetEffectsVolume()/100f;
        effectSource.Play();
    }

    /**
    *   @brief Método para "refrescar" el audio, es decir, reiniciarlo.
    *
    *   @param MonoBehaviour parent,    MonoBehaviour del cual sera asignado el audio.
    */
    public static void RefreshAudioPos(MonoBehaviour parent) {

        EthAudio.GetInstance(parent).RefreshPos();
    }
}

    
    
