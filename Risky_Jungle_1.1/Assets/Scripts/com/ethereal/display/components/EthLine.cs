using System;
using UnityEngine;
using Assets.Scripts.com.ethereal.util;
 
namespace Assets.Scripts.com.ethereal.display.components {
    
    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Octubre 16 2014
    * 
    *	@class 	EthLine
    *   @brief 	Esta clase se encarga de crear una línea.
    *
    */
	public class EthLine : EthComponent {
	
		/**
		*	@brief  Texture2D para representar la textura de la EthLine.
		*/
		private Texture2D lineTex;

		/**
		*	@brief  Vector que representa el inicio de la EthLine.
		*/
		private Vector2 origin;

		/**
		*	@brief  Vector que representa el final de la EthLine.
		*/
		private Vector2 termination;

		/**
		*	@brief  Color que de la línea.
		*/
		private Color color;

		/**
		*	@brief  Anchor que de la línea.
		*/
		private float width;

		/**
		*	@brief  Variable que define si la EthLine tendrá movimiento.
		*/
		private bool useOffset = true;

		/**
		*	@brief constructor de la clase EthLine.
		*
		*	este método permite crear una instancia de la clase EthLine
		*
		*	@param args 		Parametros con los cuales se creará el objeto clase.
		*	@param parentGUI	EthComponentManager al cual pertenecerá el objeto.
		*/
		public EthLine (EthArguments args, EthComponentManager parentGUI) : base(args, parentGUI) {   
			lineTex = new Texture2D (1, 1);
			ReloadArguments (args);
		}

		/**
		*	@brief Método para recargar los argumentos del componente.
		*
		*	@param args Argumentos que tendrá el componente.
		*/
		public override void ReloadArguments (EthArguments args) {

			if ( args ["color"] != null ) {
				string[] colorBack = args ["color"].Split ('_');

				if ( colorBack.Length == 3 ) {
					color = new Color (float.Parse (colorBack [0]), float.Parse (colorBack [1]), float.Parse (colorBack [2]));                                        
				} else {
					color = new Color (float.Parse (colorBack [0]), float.Parse (colorBack [1]), float.Parse (colorBack [2]), float.Parse (colorBack [3]));                                          
				}
			} else {
				color = GUI.color;
			}

			if ( args ["width"] != null ) {
				width = float.Parse (args ["width"]);
			} else {
				width = 3;
			}

			if ( args ["noOffset"] != null ) {
				useOffset = false;
			}

			if ( args ["origin"] != null ) {
				string[] pointData = args ["origin"].Split ('_');
				origin = new Vector2 (float.Parse (pointData [0]), float.Parse (pointData [1]));
			} else {
				origin = new Vector2 (0, 0);
			}

			if ( args ["termination"] != null ) {
				string[] pointData = args ["termination"].Split ('_');
				termination = new Vector2 (float.Parse (pointData [0]), float.Parse (pointData [1]));
			} else {
				termination = new Vector2 (0, 0);
			}            
		}

		/**
		*	@brief Método para modificar el vector origen y retornar el objeto EthLine.
		*
		*	@param newOrigin Nuevo vector de origen.
		*
		*	@return EthLine con un nuevo vector de origen.
		*/
		public EthLine setOrigin (Vector2 newOrigin) {
			origin = newOrigin;
			return this;
		}
        
        /**
		*	@brief Método para modificar el vector de terminación y retornar el objeto EthLine.
		*
		*	@param newOrigin Nuevo vector de terminación.
		*
		*	@return EthLine con un nuevo vector de terminación.
		*/
		public EthLine setTermination (Vector2 newTermination) {
			termination = newTermination;
			return this;
		}

		/**
		*	@brief Método para acceder al vector origen (origin).
		*
		*	@return El vector origen (origin).
		*/
		public Vector2 getOrigin () {
			return origin;
		}
        
        /**
		*	@brief Método para acceder al vector de terminación (termination).
		*
		*	@return El vector de terminación (termination).
		*/
		public Vector2 getTermination () {
			return termination;
		}

		/**
		*	@brief Método para modificar el valor de la variable color.
		*
		*	@param h Nuevo valor de la variable color.
		*/
		public void setColor (Color newColor) {
			color = newColor;
		}

		/**
		*	@brief Método para dibujar una EthLine, el cual decide si ejecuta el método DrawLine.
		*
		*	@param offset Parámetro de tipo Vector2 para ser dibujado, el cual támbien puede ser nulo.
		*
		*	@see com.ethereal.display.components.EthComponent
		*/
		public override void Draw (Vector2? offset = null) {

			if ( !Visible ) {
				return;
			}
            
			DrawLine (offset);

		}

		/**
		*	@brief Método ToString que retorna el nombre de la clase.
		*
		*	@return Nombre de la clase, EthLine.
		*/
		public override string ToString () {
			return "EthLine (" + Name + ")";
		}

		/**
		*	@brief Método para dibujar una EthLine.
		*
		*	@param offset Parámetro de tipo Vector2 para ser dibujado, el cual tambien puede ser nulo.
		*
		*/
		private void DrawLine (Vector2? offset = null) {
			// Save the current GUI matrix, since we're going to make changes to it.
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
     
			// Store current GUI color, so we can switch it back later,
			// and set the GUI color to the color parameter
			Color savedColor = GUI.color;
			GUI.color = color;
     
			Vector2 offset2 = offset ?? new Vector2 (0, 0);

            float offsetW = useOffset ? _gui.WOffset : 0;
            float offsetH = useOffset ? _gui.HOffset : 0;

            float ratioW = useOffset ? _gui.WRatio : 1;
            float ratioH = useOffset ? _gui.HRatio : 1;

			Vector2 pointA = new Vector2 (((origin.x + offset2.x) * ratioW) + offsetW, ((origin.y + offset2.y) * ratioH) + offsetH);
			Vector2 pointB = new Vector2 (((termination.x + offset2.x) * ratioW) + offsetW, ((termination.y + offset2.y) * ratioH) + offsetH);
            
			float angle = Mathf.Atan2 (pointB.y - pointA.y, pointB.x - pointA.x) * 180f / Mathf.PI;
			float length = (pointA - pointB).magnitude;
			GUIUtility.RotateAroundPivot (angle, pointA);
			GUI.DrawTexture (new Rect (pointA.x, pointA.y, length, width), lineTex);
     
			// We're done.  Restore the GUI matrix and GUI color to whatever they were before.
			GUI.matrix = matrix;
			GUI.color = savedColor;
		}
	}
}