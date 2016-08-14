using UnityEngine;


using System;
using Assets.Scripts.com.ethereal.util;

/*
Ejemplo de uso:

EthLabel label = gui.AddLabel("Aca va el texto",0,0,"font:fonts/Avenger,fontColor:0_0_0,fontSize:18,useLang:true");

h -> si se desea especificar un alto definido
w -> si se desea especificar un ancho definido
wrap -> Si se coloca en true entonces le hace wrap al texto.
font -> La fuente a usar
fontColor -> El color de la fuente a usar separado por _, por ejemplo para el blanco 1_1_1
fontSize -> El tamaño de la fuente a usar
useLang -> Indica si pasa por cambio debido a lenguaje o no.
style -> bold: se usa negrita
*/

namespace Assets.Scripts.com.ethereal.display.components {
    /** 
	*	@author    EtherealGF <www.etherealgf.com> 
	* 	@version   1.0
	* 	@date      Octubre 30 2014
	* 
	*	@class 	EthProgressBar
	*   @brief 	Esta clase se encarga de crear una barra de progreso, la cual se puede para mostrar 
	*	por ejemplo el estado de la carga de un escenario o de una descarga de archivos.
	*
	*/
	public class EthProgressBar : EthComponent {
		/**
		*	@brief la imagen que mostrará la barra de progreso.
		*/
		Texture2D textura;

		/**
		*	@brief la ruta donde se aloja la textura que se aplicará a la barra de progreso.
		*/
		string nomTextura;

		/**
		*	@brief valor mínimo que puede tomar la barra de progreso.
		*/
		float minLimit = 0;

		/**
		*	@brief valor máximo que puede tomar la barra de progreso.
		*/
		float maxLimit = 100;

		/**
		*	@brief valor actual que toma la barra de progreso, este valor ya está acotado para los valores de la barra.
		*/
		float progressValue = 0f;
		
		/**
		*	@brief define la orientacion del progressbar, true si es horizontal o false si es vertical
		*/
		private bool alignHoriz = true;

		/**
		*	@brief constructor de la clase EthProgressBar.
		*
		*	este método permite crear una instancia de la clase EthProgressBar.
		*
		*	@param args parametros con los cuales se creará la clase.
		*/
		public EthProgressBar (EthArguments args, EthComponentManager parentGUI) : base(args, parentGUI) {
			
			if ( args ["img"] != null ) {
				nomTextura = args ["img"];
				textura = Resources.Load (nomTextura) as Texture2D;	

				Wid=textura.width;
				Hei=textura.height;

				if ( args ["h"] != null ) {
					Hei= float.Parse (args ["h"]);
				}
				if ( args ["w"] != null ) {
					Wid=float.Parse (args ["w"]);
				}
				if ( args ["align"] != null ) {
					alignHoriz = (args ["align"] != "vert" ? true : false);
				}
			}

			minLimit = Eth.ToFloat (Eth.GetVal (args ["min"], 0));
			maxLimit = Eth.ToFloat (Eth.GetVal (args ["max"], 100));
			progressValue = Eth.ToFloat (Eth.GetVal (args ["value"], 0));
			acotarValor ();
		}

		/**
		*	@brief Muestra la barra de progreso en pantalla.
		*
		*	Este método permite mostrar el GuiStyle de la barra de progreso en la pantalla.
		*
		*	@param offSet vector2 que da las cordenadas para ubicar el objeto en un punto específico de la pantalla.
		*/
        public override void Draw(Vector2? offset = null)
        {

            if (!Visible)
            {
                return;
            }

            Vector2 offset2 = offset ?? Vector2.zero;
            float xTemp = X + offset2.x;
            float yTemp = Y + offset2.y;

            float r = (progressValue - minLimit) / (maxLimit - minLimit);
            Rect groupRect;
			if(alignHoriz) {
				groupRect = new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, (Wid * _gui.WRatio) * r, Hei * _gui.HRatio);
			} else {
				float height1 = Hei * _gui.HRatio * (1f - r);
				groupRect = new Rect ((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset + height1, (Wid * _gui.WRatio), Hei * _gui.HRatio * r);
			}
			
            GUI.BeginGroup(groupRect);

			if(alignHoriz) {
				r = 1;
				GUI.DrawTexture(new Rect(-(Wid * _gui.WRatio * (1 - r)), 0, Wid * _gui.WRatio, Hei * _gui.HRatio), textura);
			} else {
				GUI.DrawTexture(new Rect (0, -(Hei * _gui.HRatio * (1 - r)), Wid * _gui.WRatio, Hei * _gui.HRatio), textura);
			}	
            GUI.EndGroup();

        }

		/**
		*	@brief Permite acotar el valor.
		*
		*	Permite acotar el valor de la barra de progreso que es asignado por el usuario, a los valores mínimo y máximo que la barra puede alcazar.
		*/
		public void acotarValor () {	
			progressValue = Mathf.Clamp (progressValue, minLimit, maxLimit);
		}

		/**
		*	@brief Permite retornar el botón a su posición inicial.
		*
		*	Este método permite regresar el botón a su posición inicial luego de haber sido arrastrado.
		*/
		public void setValue (float value) {			
			progressValue = value;
			acotarValor ();
		}

		/**
		*	@brief Permite retornar el botón a su posición inicial.
		*
		*	Este método permite regresar el botón a su posición inicial luego de haber sido arrastrado.
		*/
		public float getValue () {
			return progressValue;
		}

		/**
		*	@brief metodo toString de la clase.
		*
		*	Retorna el nombre de la clase como una cadena de caracteres.
		*
		*	@return la clase representada en un string.
		*/
		public override string ToString () {
			return "EthProgressBar (" + Name + ")";
		}
	}
}
