using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.com.ethereal.display.easing.time {
   
   		/** 
    	*	@author    EtherealGF <www.etherealgf.com> 
    	* 	@version   1.0 
   		* 	@date      Octubre 16 2014
    	* 
    	*	@class 	AnimationEase
    	*   @brief 	Esta clase representa la animación de los componentes.
    	*
    	*/
		public class AnimationEase {   
		

				/**
       			*	@brief Lista de TimeEase, correspondientes a diferentes animaciones.
        		*/
				List<TimeEase> arrayAnims = new List<TimeEase> ();

				/**
       			*	@brief Cantidad de animaciones.
        		*/
				private int canAnims = 0;

				/**
       			*	@brief Cantidad de animaciones terminadas.
        		*/
				private int animTerminadas = 0;

				/**
       			*	@brief Arreglo con los finales de los pasos de la animación.
        		*/
				private float [] fin;

				/**
       			*	@brief Variable que define si la animación ya terminó.
        		*/
				public bool isEnded;

				/**
        		*	@brief constructor de la clase AnimationEase.
        		*
        		*	Este método permite crear una instancia de la clase AnimationEase
        		*
        		*	@param init		Arreglo con el inicio de los pasos de la animación.
        		*	@param fin 		Arreglo con el final de los pasos de la animación.
        		*	@param duration Duración de la animación.
        		*	@param easeType Tipo de animación.
        		*/
				public AnimationEase (float [] init, float [] fin, int duration, string easeType) {	
						this.fin = fin;

						for (int i = 0; i < init.Length; i++) {
								if (init [i] != fin [i]) {
										arrayAnims.Add (CreateEase (init [i], fin [i], duration, easeType));
								} else {
										arrayAnims.Add (null);
								}
						}

						isEnded = false;
				}

				/**
        		*	@brief Método que crea un TimeEase, le adiciona el evento de terminación, y lo cuenta.
        		*
        		*	@param init Ancho de la animacióm.
        		*	@param fin 	Alto de la animacióm.
        		*	@param d 	Duración de la animación.
        		*	@param t 	Tipo de animación.
        		*
        		*	@return TimeEase creado con los parametros indicados.
        		*/
				public TimeEase CreateEase (float init, float fin, int d, string t) {			
						TimeEase easeTemp = new TimeEase (init, fin, d, t);
						easeTemp.OnFinishAnimation += OnFinishAnimProp;
						canAnims++;
						return easeTemp;
				}

				/**
        		*	@brief Método que obtiene los 4 pasos de la animación a reproducir.
        		*
        		*	@return Arreglo con los 4 pasos de la animación.
        		*/
				public float[] GetStep () {
			
						float[] dataFrame = new float[fin.Length];

						for (int i = 0; i < fin.Length; i++) {
								if (arrayAnims [i] == null) {
										dataFrame [i] = fin [i];
								} else {
										dataFrame [i] = arrayAnims [i].getStep ();
								}
						}

						return dataFrame;
				}

				/**
        		*	@brief Método para iniciar los cuatro pasos de la animación y al terminar invoca el evento de terminación apropiada.
        		*/
				public void Start () {
						if (canAnims > 0) {
								for (int i = 0; i < 4; i++) {
										if (arrayAnims [i] != null) {
												arrayAnims [i].start ();
										}
								}
						} else {
								OnFinishAnimProp ();
						}
						
				}

				/**
        		*	@brief Método que actualiza la cantidad de animaciones terminadas, en caso de haber terminado todas dispara el evento
        		*	OnFinish.
        		*/
				public void OnFinishAnimProp () {
						animTerminadas++;
						isEnded = true;

						if (animTerminadas >= canAnims) {
								if (OnFinish != null)
										OnFinish ();
						}
				}

				/**
        		*	@brief Método para invocar el evento OnFinish.
        		*/
				public delegate void OnFinishEvent ();

				/**
        		*   @brief Evento para detectar el final de las animaciones.
        		*/
				public event OnFinishEvent OnFinish;
		}
}
