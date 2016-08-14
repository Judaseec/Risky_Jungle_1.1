using System;
using UnityEngine;


namespace Assets.Scripts.com.ethereal.util
{

    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Julio 22 2014
    * 
    *	@class 	EthTimer
    *   @brief 	Clase utilizada para ejecutar acciones o eventos despues de un tiempo determinado.
    *
    */
    public class EthTimer
    {

        /**
        *	@brief Método ejecutado cuando se presenta un evento de esta clase que será modificado con respecto a como se desee 
        *	utilizar manteniendo su estructura. 
        *
        *	@param obj Objeto que recibe la función que sera modificado.
        */
        public delegate void OnTimeEvent(object obj);

        /**
        *	@brief	Evento llamado cuando un pasa un tiempo determinado, que luego sera modificado.
        */
        public event OnTimeEvent OnTimeExecuted;

        /**
        *	@brief Instancia de EthTimerGO en esta clase.
        */
        private EthTimerGO timerInt;

        /**
        *	@brief Objeto que es recibido por una función.
        */
        private object obj;

        /**
        *	@brief Total de repeticiones que sera ejecutada la pausa.
        */
        private int totalRepeats;

        /**
        *	@brief Número de la actual repetición.
        */
        private int currentRepeat;

        /**
        *	@brief Indicador cuando el tiempo de duración sea infinito.
        */
        private bool infinite = false;

        /**
        *	@brief tiempo que pasará antes de ser ejecutada una función.
        */
        private float time;

        /**
        *	@brief Método para Instanciar un EthTimer.
        *	
        *	Para crear un EthTimer se requieren el tiempo y la función que debe llamar despues de haber terminado ese tiempo.
        *
        *	@param timeMs	Tiempo que debera pasar para ejecutarse la función.
        *	@param fn 		Función a ser ejecutada después de terminado el tiempo determinado.
        *
        */
        public EthTimer(int timeMs, OnTimeEvent fn)
            : this(timeMs, fn, null, 1)
        {

        }

        /**
        *	@brief Método para Instanciar un EthTimer.
        *	
        *	Para crear un EthTimer se requieren el tiempo, la función que debe llamar despues de haber terminado ese tiempo, el objeto que 
        *	recibe la función y las repeticiones de las veces que sera ejecutado.
        *
        *	@param timeMs		Tiempo que debera pasar para ejecutarse la función.
        *	@param fn 			Funcion a ser ejecutada despues de terminado el tiempo determinado.
        *	@param obj 			Objeto que recibe la función.
        *	@param repeticiones Repeticiones que sera ejecutada la función.
        *
        */
        public EthTimer(int timeMs, OnTimeEvent fn, object obj, int repeticiones)
        {
            OnTimeExecuted += fn;
            this.obj = obj;

            if (repeticiones == 0)
            {
                infinite = true;
            }

            totalRepeats = repeticiones;
            currentRepeat = 0;

            this.time = timeMs / 1000f;

            ContinueTimerCurrentScene();

        }

        /**
        *	@brief Método usado para crear un game object para que pueda ser utilizado el timer teniendo como referencia el EthTimerGO.
        *	
        */
        public void ContinueTimerCurrentScene()
        {
            timerInt = (EthTimerGO)ObjectFactory.CreateObject<EthTimerGO>("ethTimer");
            timerInt.startTime = time;
            timerInt.repeatingTime = time;
            timerInt.Parent = this;

            StartTimer();
        }

        /**
        *	@brief Método usado para inicializar el timer del EthTimerGO de esta clase.
        *	
        *	Se ejecuta el método encontrado en la clase EthTimerGO para iniciar con la realizacion de las funciones manejadas en 
        *	un tiempo determinado por esta clase.
        *
        */
        public void StartTimer()
        {
            timerInt.InitTimer();
        }

        /**
        *	@brief Método usado para ejecutar el evento referenciado al objeto especificado, cuando se ha cumplido el tiempo determinado.
        *	
        *	Siempre y cuando el tiempo se haya cumplido, hayan repeticiones restantes y no sea infinito el numero de repeticiones, se ejecutara
        *	el evento recibiendo un objeto. De lo contrario se destruye el timer apenas terminen las repeticiones.
        *
        */
        public void TimerExecuted()
        {
            currentRepeat++;

            if (currentRepeat >= totalRepeats && !infinite)
            {
                timerInt.CancelInvoke();
                timerInt.DestroyTimer();
            }

            if (OnTimeExecuted != null)
            {
                OnTimeExecuted(obj);
            }

        }
		
		/**
        *	@brief Método usado para cancelar el timer.
        */
		public void CancelTimer()
		{
			timerInt.CancelInvoke();
			timerInt.DestroyTimer();
		}
    }
}