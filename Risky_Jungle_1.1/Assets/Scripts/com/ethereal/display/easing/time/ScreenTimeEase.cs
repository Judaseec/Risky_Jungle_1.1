using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.com.ethereal.display.easing.time {

		/** 
    	*	@author    EtherealGF <www.etherealgf.com> 
    	* 	@version   1.0 
   		* 	@date      Octubre 16 2014
    	* 
    	*	@class 	ScreenTimeEase
    	*   @brief 	Esta clase representa la pantalla de animación.
    	*
    	*/
		public class ScreenTimeEase {
		
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
        		*	@brief constructor de la clase ScreenTimeEase.
        		*
        		*	Este método permite crear una instancia de la clase ScreenTimeEase
        		*
        		*	@param wScreen 		Ancho de la pantala de la animacióm.
        		*	@param hScreen 		Alto de la pantala de la animacióm.
        		*	@param duration 	Duración de la animación.
        		*	@param direction	Dirección de la animación.
        		*	@param easeType 	Tipo de animación.
        		*/
				public ScreenTimeEase (float wScreen, float hScreen, int duration, string direction, string easeType) {	
			
						switch (direction)
						{
								case "left":
										arrayAnims.Add (CreateEase (wScreen, 0, duration, easeType)); //X
										arrayAnims.Add (null); //Y
										arrayAnims.Add (new TimeEase (1, 1, duration, easeType)); //Width
										arrayAnims.Add (null); //Height								
										break;

								case "right":
										arrayAnims.Add (CreateEase (-wScreen, 0, duration, easeType)); //X
										arrayAnims.Add (null); //Y
										arrayAnims.Add (new TimeEase (1, 1, duration, easeType)); //Width
										arrayAnims.Add (null); //Height								
										break;

								case "top":
										arrayAnims.Add (null); //X
										arrayAnims.Add (CreateEase (-hScreen, 0, duration, easeType)); //Y
										arrayAnims.Add (null); //Width
										arrayAnims.Add (new TimeEase (1, 1, duration, easeType)); //Height								
										break;

								case "bottom":
										arrayAnims.Add (null); //X
										arrayAnims.Add (CreateEase (hScreen, 0, duration, easeType)); //Y
										arrayAnims.Add (null); //Width
										arrayAnims.Add (new TimeEase (1, 1, duration, easeType)); //Height								
										break;
						}
				}

				/**
        		*	@brief Método que crea un TimeEase, le adiciona el evento de terminación, y lo cuenta.
        		*
        		*	@param x 	Ancho de la animacióm.
        		*	@param y 	Alto de la animacióm.
        		*	@param d 	Duración de la animación.
        		*	@param t 	Tipo de animación.
        		*
        		*	@return TimeEase creado con los parametros indicados.
        		*/
				public TimeEase CreateEase (float x, float y, int d, string t) {			
						TimeEase easeTemp = new TimeEase (x, y, d, t);
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
			
						float[] dataFrame = new float[4] {0,0,1,1};

						for (int i = 0; i < 4; i++) {
								if (arrayAnims [i] != null) {
										dataFrame [i] = arrayAnims [i].getStep ();
								}
						}
						
						return dataFrame;
				}

				/**
        		*	@brief Método para iniciar los cuatro pasos de la animación.
        		*/
				public void Start () {
						for (int i = 0; i < 4; i++) {
								if (arrayAnims [i] != null) {
										arrayAnims [i].start ();
								}
						}			
				}

				/**
        		*	@brief Método que actualiza la cantidad de animaciones terminadas, en caso de haber terminado todas dispara el evento
        		*	OnFinish.
        		*/
				public void OnFinishAnimProp () {
						animTerminadas++;

						if (animTerminadas == canAnims) {
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
