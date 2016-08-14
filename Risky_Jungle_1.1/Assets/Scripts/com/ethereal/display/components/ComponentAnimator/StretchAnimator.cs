using UnityEngine;


using System;

/*
Ejemplo de uso:

EthButton bot = gui.AddButton("Bot0",0,0,"text:Hola,font:fonts/Avenger,fontColor:0_0_0,fontSize:18,useLang:true");
bot.addAnimator(new SqueezeAnimator());

*/

namespace com.ethereal.display.components {

	/** 
	*	@author    EtherealGF <www.etherealgf.com>
	* 	@version   1.0 
	* 	@date      Noviembre 7 2014
	* 
	*	@class	StretchAnimator
	*   @brief 	Esta clase se encarga de realizar animacion de estiramiento.
	*	
	*	Esta clase hereda de la clase SqueezeAnimator.
	*/
	public class StretchAnimator : SqueezeAnimator {
	
		/**
		*	@brief Constructor de la clase StretchAnimator.
		*
		*	Este método permite crear una instancia de la clase StretchAnimator, pero previamente crea una instancia de SqueezeAnimator.
		*
		*	@param stretchPixels	Píxeles que tomara la animación.
		*	@param velocity			Velocidad de la animación.
		*
		*/
		public StretchAnimator (float stretchPixels, float velocity) : base( stretchPixels,  velocity) {								
		}

		/**
		*	@brief Constructor de la clase StretchAnimator.
		*
		*	Este método permite crear una instancia vacía de la clase StretchAnimator.
		*
		*/
		public StretchAnimator () {
		}

		/**
		*	@brief Método para iniciar la animación de estiramiento.
		*
		*	@see  <namespace.SqueezeAnimator>.
		*/
		public override void StartAnimation () {
			
			timeX = new Assets.Scripts.com.ethereal.display.easing.time.TimeEase (100 - stretchPixels, 100 + stretchPixels, velocity, "linear");				
			timeX.isLoop (true);
			timeX.start ();
			
			timeY = new Assets.Scripts.com.ethereal.display.easing.time.TimeEase (100 - stretchPixels, 100 + stretchPixels, velocity, "linear");				
			timeY.isLoop (true);
			timeY.start ();
		}

		/**
		*	@brief Método ToString que retorna el nombre de la clase junto con el nombre del componente.
		*
		*	@return Nombre de la clase StretchAnimator seguido del nombre del componente.
		*
		*/
		public override string ToString () {
			return "StretchAnimator (" + component + ")";
		}
	}
}
