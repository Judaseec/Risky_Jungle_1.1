using UnityEngine;
using System;
using Assets.Scripts.com.ethereal.util;

namespace Assets.Scripts.com.ethereal.display.components3D
{

	/** 
	*	@author    EtherealGF <www.etherealgf.com> 
	* 	@version   1.0 
	* 	@date      Abril 11 2015
	* 
	*	@class 	EthButton3D
	*   @brief 	Esta clase esta encargada de generar los componentes en 3D.
	*
	*/
    public class EthComponent3D : MonoBehaviour
    {

    	/**
        *	@brief Instancia de la clase EthGameObject.
        */
        private EthGameObject _ethGameObject;

        /**
        *	@brief Constructor de la clase EthComponent3D.
        *
        *	Este método permite crear una instancia de la clase EthButton3D.
        *
        */
        public EthComponent3D()
        {
        }

        /**
        *	@brief Metodo usado para iniciar el componente.
        *
        */
        void Start()
        {
        }

        /**
        *   @brief Asignación de propiedades de lectura y escritura a la variable _ethGameObject.
        */
        public EthGameObject ethGameObject
        {
            get
            {
                if (_ethGameObject == null)
                {
                    _ethGameObject = new EthGameObject(gameObject);
                }
                return _ethGameObject;
            }
            set
            {
                _ethGameObject = value;
            }
        }

        
		/**
		*	@brief Método para establecer el evento a suceder al cabo de ser presionado el botón.
		*
		*	Método a ser heredado por los hijos de esta clase
		*/
        public virtual void EthOnMouseDown()
        {
        }

        /**
		*	@brief Método para llamar a la función que se debe ejecutar cuando se presiona un botón o el mouse actua como botón.
		*
        *   Método a ser heredado por los hijos de esta clase
		*/
        public virtual void EthOnMouseUpAsButton()
        {
        }
        
		/**
		*	@brief Método para establecer el evento a suceder al cabo de ser soltado el botón.
		*
        *   Método a ser heredado por los hijos de esta clase
		*	
		*/
        public virtual void EthOnMouseUp()
        {
        }

        /**
		*	@brief Método para establecer lo que debe suceder al arrastrar el mouse.
		*   Método a ser heredado por los hijos de esta clase
		*/
        public virtual void EthOnMouseDrag()
        {
        }

        /**
		*	@brief Método para establecer el evento a suceder al cabo de ser presionado el botón.
		*
		*	Este método permite obtener el metodo EthOnMouseDown de esta clase.
		*   Método a ser heredado por los hijos de esta clase
		*/
        public virtual void _EthOnMouseDown()
        {
            EthOnMouseDown();
            OnMouseDown();
        }

        /**
		*	@brief Método para llamar a la función que se debe ejecutar cuando se presiona un botón o el mouse actua como botón.
		*
		*	Este método permite obtener el metodo EthOnMouseUpAsButton de esta clase.
        *   Método a ser heredado por los hijos de esta clase
		*/
        public virtual void _EthOnMouseUpAsButton()
        {
            EthOnMouseUpAsButton();
            OnMouseUpAsButton();
        }

        /**
		*	@brief Método para establecer el evento a suceder al cabo de ser soltado el botón.
		*
		*	Este método permite obtener el metodo EthOnMouseUp de esta clase.
		*   Método a ser heredado por los hijos de esta clase
		*/
        public virtual void _EthOnMouseUp()
        {
            EthOnMouseUp();
            OnMouseUp();
        }

        /**
		*	@brief Método para establecer lo que debe suceder al arrastrar el mouse.
		*
		*	Este método permite obtener el metodo EthOnMouseDrag de esta clase.
        *   Método a ser heredado por los hijos de esta clase
		*/
        public virtual void _EthOnMouseDrag()
        {
            EthOnMouseDrag();
            OnMouseDrag();
        }

        /**
		*	@brief Método para agregar un componente 3D como hijo.
		*
		*	@param comp Componente 3D a ser agregado como hijo.
		*/
        public virtual void AddChild(EthComponent3D comp)
        {
            comp.gameObject.transform.parent = gameObject.transform;
        }

        /**
		*	@brief Método para obtener el padre de esta clase.
		*
		*	@return GameObject padre de tipo EthComponent3D.
		*/
        public EthComponent3D getEthParent()
        {
            return getEthParent(typeof(EthComponent3D));
        }

        /**
		*	@brief Obtiene el Padre de esta clase especificado por el tipo de este.
		*
		*	@param tipoParent 	Tipo de objeto que sera el padre.
		*	
		*	@return GameObject requerido.
		*/
        public EthComponent3D getEthParent(Type tipoParent)
        {

            if (transform.parent == null)
            {
                return null;
            }

            Component[] comps = transform.parent.gameObject.GetComponents(tipoParent);

            if (comps.Length > 0)
            {
                return (EthComponent3D)comps[0];
            }

            return null;
        }

        /**
        *   @brief Método para establecer el evento a suceder al cabo de ser presionado el botón.
        *
        */
        public void OnMouseDown()
        {
        }

        /**
        *   @brief Método para establecer el evento a suceder cuando se presiona un botón o el mouse actua como botón.
        *
        */
        public void OnMouseUpAsButton()
        {
        }

        /**
        *   @brief Método para establecer el evento a suceder al cabo de ser soltado el botón.
        *     
        */
        public void OnMouseUp()
        {
        }

        /**
        *   @brief Método para establecer el evento a suceder al arrastrar el mouse.
        */
        public void OnMouseDrag()
        {
        }

        /**
        *	@brief	Método para asignar argumentos.
        *	
        *	@param args Argunmentos a ser asignados
        *	
        *	@return Cadena de caracteres representando la clase actual.
        */
        public virtual void SetArgs(EthArguments args)
        {

            /*this.gui = gui;
            x = Convert.ToInt32(args["x"]);
            y = Convert.ToInt32(args["y"]);
            name = args["name"];

            if(args["visibility"]!=null) {
                visible = Convert.ToBoolean(args["visibility"]);
            }*/
        }

        /**
        *	@brief	Sobreescritura del método toString().
        *	
        *	Método encargado de representar la clase en forma de texto de una manera coherente. 
        *	
        *	@return Cadena de caracteres representando la clase actual.
        */
        public override string ToString()
        {
            return string.Format("EthComponent ({0})", name);
        }
    }
}
