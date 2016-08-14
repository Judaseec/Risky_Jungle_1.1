using UnityEngine;
using System;
using Assets.Scripts.com.ethereal.display.components3D;

namespace com.ethereal.display.components3D {

	/** 
	*	@author    EtherealGF <www.etherealgf.com> 
	* 	@version   1.0 
	* 	@date      Abril 8 2014
	* 
	*	@class 	EthChildObject3D
	*   @brief 	Esta clase se encarga de las funcionalidades de obtencion del padre de los componentes 3D
	*
	*/
	public class EthChildObject3D : MonoBehaviour {	

		/**
		*	@brief Constructor de la clase EthChildObject3D.
		*
		*	Este método permite crear una instancia de la clase EthChildObject3D
		*
		*/
		public EthChildObject3D () {				
		}

		/**
		*	@brief Método para establecer el evento a suceder al cabo de ser soltado el botón.
		*
		*	Este método permite obtener el metodo EthOnMouseUp del padre.
		*	
		*/
		public void OnMouseUp () {
			getEthParent ()._EthOnMouseUp ();
		}

		/**
		*	@brief Método para establecer el evento a suceder al cabo de ser presionado el botón.
		*
		*	Este método permite obtener el metodo _EthOnMouseDown del padre.
		*	
		*/
		public void OnMouseDown () {			
			getEthParent ()._EthOnMouseDown ();
		}

		/**
		*	@brief Método para llamar a la función que se debe ejecutar cuando se presiona un botón o el mouse actua como botón.
		*
		*	Este método permite obtener el metodo _EthOnMouseUpAsButton del padre.
		*/
		public void OnMouseUpAsButton () {
			getEthParent ()._EthOnMouseUpAsButton ();
		}

		/**
		*	@brief Método para establecer lo que debe suceder al arrastrar el mouse.
		*
		*	Este método permite obtener el metodo _EthOnMouseDrag del padre.
		*/
		public void OnMouseDrag () {
			getEthParent ()._EthOnMouseDrag ();
		}
        
		/**
		*	@brief Método para obtener el padre de esta clase.
		*
		*	@return GameObject padre de tipo EthComponent3D.
		*/
		public EthComponent3D getEthParent () {
			return getEthParent (gameObject, typeof(EthComponent3D));
		}

		/**
		*	@brief Obtiene el Padre de esta clase especificado por el tipo de este.
		*
		*	@param go 			Game object del que se quiere obtener el padre.
		*	@param tipoParent 	Tipo de objeto que sera el padre.
		*	
		*	@return GameObject requerido.
		*/
		public EthComponent3D getEthParent (GameObject go, Type tipoParent) {
			Component[] comps = go.transform.parent.gameObject.GetComponents (tipoParent);

			if ( comps.Length > 0 ) {
				return (EthComponent3D) comps [0];
			} else {
				return getEthParent (go.transform.parent.gameObject, tipoParent);
			}   		
		}

	}
}
