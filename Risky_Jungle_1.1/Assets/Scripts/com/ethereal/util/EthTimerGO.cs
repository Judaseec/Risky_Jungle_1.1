using System;
using UnityEngine;

namespace Assets.Scripts.com.ethereal.util
{
    /** 
	*	@author    EtherealGF <www.etherealgf.com> 
	* 	@version   1.0 
	* 	@date      Julio 28 2014
	* 
	*	@class 	EthTimerGO
	*   @brief 	Clase usada como referencia para usar un timer.
	*
	*/
    public class EthTimerGO : MonoBehaviour
    {
        /**
        *	@brief Tiempo inicial en el que sera efectuado un metodo especificado.
        */
        public float startTime;

        /**
        *	@brief Tiempo que pasará para repetirse el metodo especificado.
        */
        public float repeatingTime;

        /**
        *	@brief EthTimer padre al cual se le va a referenciar este EthTimerGO.
        */
        private EthTimer _parent = null;

        /**
        *	@brief Asignación de las propiedades de lectura y escritura para la variable _parent.
         *	
         *  @return El valor d ela variable _parent.
        */
        public EthTimer Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        /**
        *	@brief Método usado para iniciar el EthTimerGO.
        */
        public void Start()
        {
        }

        /**
        *	@brief Método usado para actualizar esta clase.
        */
        public void Update()
        {
        }

        /**
        *	@brief Método usado para inicializar el timer.
        *
        *	Se invoca un método especificado en un momento determinado (startTime), el cual se repetirá cada tiempo acordado (repeatingTime).
        */
        public void InitTimer()
        {
            InvokeRepeating("RepeatingMethod", startTime, repeatingTime);
        }

        /**
        *	@brief Método usado para destruir el gameObject actual.
        *
        *	Al detruir el gameObject tambien destruye todos sus componentes que esten relacionados con este.
        */
        public void DestroyTimer()
        {
            Destroy(gameObject);
        }
        
        /**
        *	@brief Método usado para ejecutar el método TimerExecuted de la clase EthTimer.
        *	
        *	Por medio de este metodo se ejecuta el evento referenciado al objeto especificado, cuando se ha cumplido el tiempo determinado.
        */
        public void RepeatingMethod()
        {
            if (_parent != null)
            {
                _parent.TimerExecuted();
            }
        }
    }
}