using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/** 
*   @author    EtherealGF <www.etherealgf.com> 
*   @version   1.0 
*   @date      Octubre 16 2015
* 
*   @class  EthUIProgressbar
*   @brief  Esta clase se encarga de administrar las propiedades de los elementos de tipo barra de progreso.
*/
public class EthUIProgressbar : MonoBehaviour {

    /**
    *   @brief Barra de progreso de color verde.
    */
    private Scrollbar _green;

    /**
    *   @brief Barra de progreso de color naranja.
    */
    private Scrollbar _orange;

    /**
    *   @brief Variable que define si se esta permitido realizar animación.
    */
    private bool _animAuthorized = true;

	/**
    *   @brief Método para inicializar las variables.
    */
	void Start () {
	   
        _green  = Util.GetChildByName(gameObject, "ProgressGreen").GetComponent<Scrollbar>();
        _orange = Util.GetChildByName(gameObject, "ProgressOrange").GetComponent<Scrollbar>();
        _green.size = 1f;
        if(_orange != null) {

            _orange.size = 1f;
        }
	}
	
	/**
    *   @brief Método para actualizar, el cual es llamado una vez por frame.
    */
	void Update () {
	   
        if(_orange != null && _animAuthorized && _orange.size > _green.size) {

            _orange.size -= 0.6f * Time.deltaTime;
            if(_orange.size < _green.size) {

                _orange.size = _green.size;
            }
        }
	}

    /**
    *   @brief Método para modificar la salud en la barra.
    *
    *   @param float health     Nuevo valor de salud.
    *   @param float maxHealth  Valor máximo de salud.
    */
    public void SetHealth(float health, float maxHealth) {

        float tmp = health / maxHealth;
        _green.size  = tmp < 0.04f ? 0.04f : tmp ;
        _animAuthorized = false;
        Invoke("AuthorizeAnim", 0.8f);
    }

    /**
    *   @brief Método para autorizar las animaciones.
    */
    public void AuthorizeAnim() {

        _animAuthorized = true;
    }
}
