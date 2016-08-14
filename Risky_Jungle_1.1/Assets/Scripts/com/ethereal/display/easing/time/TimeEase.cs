using UnityEngine;
using Assets.Scripts.com.ethereal.display.easing;

namespace Assets.Scripts.com.ethereal.display.easing.time {
    
    	/** 
    	*	@author    EtherealGF <www.etherealgf.com> 
    	* 	@version   1.0 
    	* 	@date      Octubre 16 2014
    	* 
    	*	@class 	TimeEase
   	 	*   @brief 	Esta clase representa el tiempo que tomará una animación de un componente.
    	*
    	*/
		public class TimeEase {
		
			/**
       		*	@brief Variable que define la duración de la animación (milisegundos).
        	*/
			protected float duration;

			/**
       		*	@brief Variable que define el inicio de la animación.
        	*/
			protected float ini;

			/**
       		*	@brief Variable que define el final de la animación.
        	*/
			protected float fin;

			/**
       		*	@brief Variable que define el tipo de la animación.
        	*/
			protected string easeType;

			/**
       		*	@brief Variable que define el tiempo en que se inica la animación.
        	*/
			protected float tIni;

            /**
       		*	@brief Variable que define el tiempo actual de la animación.
        	*/
			protected float tActual;

			/**
       		*	@brief Variable que define sí la animación es cíclica.
        	*/
			protected bool loop = false;

			/**
        	*	@brief constructor de la clase TimeEase.
        	*
        	*	Este método permite crear una instancia de la clase TimeEase
        	*
        	*	@param ini 		Inicio de la animación.
        	*	@param fin 		Final de la animación.
        	*	@param duration Duración de la animación.
        	*	@param easeType Tipo de animación.
        	*/
			public TimeEase (float ini, float fin, float duration, string easeType) {			
					this.duration = duration;
					this.ini = ini;
					this.fin = fin;			
					this.easeType = easeType;			
			}

			/**
       		*	@brief Método para acceder al valor de la variable loop.
            *	
            *   @param loop True o false que indica si hay ciclo o no.
       		*
       		*	@return Valor de la variable loop.
        	*/
			public void isLoop (bool loop) {
					this.loop = loop;
			}
		
			/**
       		*	@brief Método para indicar el inicio de la animación.
        	*/
			public void start () {
					tIni = Time.realtimeSinceStartup;
			}

			/**
       		*	@brief Método para ejecutar un ciclo de la animación.
       		*
       		*	@return La duración del ciclo.
        	*/
			public float getStep () {
					float value = (Time.realtimeSinceStartup - tIni) / (duration / 1000);
					if (value >= 1) {
							if (OnFinishAnimation != null) 
									OnFinishAnimation ();
			
							if (!loop) {
									value = 1;
							} else {
									float temp = ini;
									ini = fin;
									fin = temp;
									value = 1;
									start ();
									return ini;
							}
					}
					
					return functions.applyFunction (easeType, ini, fin, value);
			}

			/**
        	*	@brief Método para invocar el evento OnFinish.
        	*/
			public delegate void OnFinishEvent ();
		
			/**
        	*   @brief Evento para detectar el final de una animación.
        	*/
			public event OnFinishEvent OnFinishAnimation;
		}
}
