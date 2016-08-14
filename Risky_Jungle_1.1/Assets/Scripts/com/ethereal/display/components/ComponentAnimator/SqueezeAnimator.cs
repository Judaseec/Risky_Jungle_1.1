using UnityEngine;





using System;

/*
Ejemplo de uso:

EthButton bot = gui.AddButton("Bot0",0,0,"text:Hola,font:fonts/Avenger,fontColor:0_0_0,fontSize:18,useLang:true");
bot.addAnimator(new SqueezeAnimator());

*/

namespace com.ethereal.display.components {

	public class SqueezeAnimator : Assets.Scripts.com.ethereal.display.components.ComponentAnimator.EthComponentAnimator {
	
		/**
		*	@brief Variable que representa el tiempo de animación en el eje X.
		*/
		protected Assets.Scripts.com.ethereal.display.easing.time.TimeEase timeX;

		/**
		*	@brief Variable que representa el tiempo de animación en el eje Y.
		*/
		protected Assets.Scripts.com.ethereal.display.easing.time.TimeEase timeY;

		/**
		*	@brief Variable que representa el ancho inicial de la animación.
		*/
		protected float widIni;

		/**
		*	@brief Variable que representa el punto en el eje X inicial de la animación.
		*/
		protected float xIni;

		/**
		*	@brief Variable que representa la altura inicial de la animación.
		*/
		protected float heiIni;

		/**
		*	@brief Variable que representa el punto en el eje Y inicial de la animación.
		*/
		protected float yIni;

		/**
		*	@brief Variable que representa cuantos píxeles se estirará la animación.
		*/
		protected float stretchPixels;

		/**
		*	@brief Variable que representa la velocidad de ejecución de la animación.
		*/
		protected float velocity;

		/**
		*	@brief Variable utilizada para especificar el tipo de TimeEase en las variables timeX y timeY.
		*/
        private const string LINEAR = "linear";

        /**
		*	@brief Constructor de la clase SqueezeAnimator.
		*
		*	Este método permite crear una instancia vacía de la clase SqueezeAnimator.
		*
		*/
		public SqueezeAnimator () {
		}

		/**
		*	@brief Constructor de la clase SqueezeAnimator.
		*
		*	Este método permite crear una instancia de la clase SqueezeAnimator.
		*
		*	@param	stretchPixels	Píxeles de apretamiento que tomara la animación.
		*	@param	velocity		Velocidad de la animación.
		*
		*/
		public SqueezeAnimator (float stretchPixels, float velocity) {		
			this.stretchPixels = stretchPixels;
			this.velocity = velocity;				
		}

		/**
		*	@brief Método que dependiendo del componente que se ingrese, modifica las variables de tamaño.
		*
		*	@param	comp Componente a animar.
		*
		*	@see  <namespace.EthComponentAnimator>.
		*/
        public override void SetComponent(Assets.Scripts.com.ethereal.display.components.EthComponent comp)
        {
            this.component = comp;
            widIni = comp.Wid;
            xIni = comp.X;
            heiIni = comp.Hei;
            yIni = comp.Y;
        }

		/**
		*	@brief Método para iniciar la animación de apretón.
		*
		*	@see  <namespace.EthComponentAnimator>.
		*/
		public override void StartAnimation () {
            timeX = new Assets.Scripts.com.ethereal.display.easing.time.TimeEase(100 - stretchPixels, 100 + stretchPixels, velocity, LINEAR);				
			timeX.isLoop (true);
			timeX.start ();

            timeY = new Assets.Scripts.com.ethereal.display.easing.time.TimeEase(100 + stretchPixels, 100 - stretchPixels, velocity, LINEAR);				
			timeY.isLoop (true);
			timeY.start ();
		}

		/**
		*	@brief Método para iniciar la animación de apretón.
		*
		*	@see  <namespace.EthComponentAnimator>.
		*/
        public override void AnimateComponent()
        {
            float valActX = timeX.getStep();
            component.Wid=widIni * (valActX / 100);
            component.X = (xIni + ((widIni - component.Wid) / 2));

            float valActY = timeY.getStep();
            component.Hei=heiIni * (valActY / 100);
            component.Y = (yIni + ((heiIni - component.Hei) / 2));

        }

		/**
		*	@brief Método ToString que retorna el nombre de la clase junto con el nombre del componente.
		*
		*	@return Nombre de la clase SqueezeAnimator seguido del nombre del componente.
		*
		*/
		public override string ToString () {
			return "SqueezeAnimator (" + component + ")";
		}
	}
}
