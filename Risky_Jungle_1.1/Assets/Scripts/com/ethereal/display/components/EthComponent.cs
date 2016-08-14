using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.com.ethereal.util;
using Assets.Scripts.com.ethereal.display.components.ComponentAnimator;

namespace Assets.Scripts.com.ethereal.display.components
{

    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Octubre 16 2014
    * 
    *	@class 	EthComponent
    *   @brief 	De esta clase se derivarán todos los componentes.
    *
    */
    public class EthComponent
    {

        /**
        *	@brief Variable que define si el EthComponent es visible en la GUI.
        */
        protected bool _visible = true;


        /**
        *	@brief Asignación de las propiedades de lectura y escritura de la variable _visible.
        *	
        *   @return El valor de la variable _visible.
        */
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
            }
        }

        /**
        *	@brief Nombre del EthComponent.
        */
        private string _name = "EthComponent";

        /**
		*	@brief Asignación de las propiedades de lectura y escritura del atributo name.
        *	
        *  @return El valor de la variable _name.
		*/
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /**
		*	@brief Posición en X del EthComponent.
		*/
        private float _x;

        /**
		*	@brief Asignación de las propiedades de lectura y escritura del atributo x.
        *	
        *  @return El valor de la variable _x.
		*/
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        /**
		*	@brief Posición en Y del EthComponent.
		*/
        private float _y;

        /**
		*	@brief Asignación de las propiedades de lectura y escritura de la variable _y.
		*
		*	@return El valor de la variable  _y.
		*/
        public float Y
        {
            get { return _y; }
            set
            {
                _y = value;
                if (_gui != null)
                {
                    _gui.EvaluateMinAndMax(this.Y, this._hei);
                }
            }
        }

        /**
		*	@brief Variable que define el grado de rotación del componente.
		*/
        private float _rotation = 0;

        /**
        *   @brief Asignación de laS propiedadES de lectura y escritura de la variable _rotation
        *   
        *   @return El valor de la variable _rotation.
        */
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }
        /**
        *	@brief Variable que define la altura del componente.
        */
        private float _hei;

        /**
		*	@brief Asignación de las propiedades de lectura y escritura de la variable _hei.
		*
		*	@return El valor de la variable  _hei.
		*/
        public float Hei
        {
            get
            {
                return _hei;
            }
            set
            {
                _hei = value;
                if (_gui != null)
                {
                    _gui.EvaluateMinAndMax(_y,_hei);
                }
            }
        }
        /**
        *	@brief Variable que define el anchor del componente.
        */
        private float _wid;

        /**
		*	@brief Asignación de las propiedades de lectura y escritura de la variable _wid.
		*
		*	@return El valor de la variable  _wid.
		*/
        public float Wid
        {
            get
            {
                return _wid;
            }
            set
            {
                _wid = value;
            }
        }
        /**
        *	@brief EthComponentManager al que pertenece el EthComponent.
        */
        protected EthComponentManager _gui;

        /**
        *	@brief Variable que define sí se usara idioma.
        */
        protected bool _useLang = true;

        /**
		*	@brief Asignación de la propiedad de lectura de la variable _useLang.
		*
		*	@return El valor de la variable _useLang.
		*/
        public bool UseLang
        {
            get
            {
                return _useLang;
            }
        }

        /**
        *	@brief Variable que define sí el EthComponent estará habilitado.
        */
        private bool _enable = true;

        /**
        *	@brief Asignación de las propiedades de lectura y escritura del atributo _enable.
        *
        *	@return El valor de la variable _enable.
        */
        public bool Enable
        {
            get
            {
                return (_gui == null) ? _enable : _gui.Enable && _enable;
            }
            set
            {
                _enable = value;
            }
        }

        /**
        *	@brief Variable que define sí se usara imagen para el idioma.
        */
        protected bool _useLangImgs = false;

        /**
        *	@brief Último idioma usado.
        */
        protected string _lastLangUsed = "";

        /**
        *	@brief Variable para asignar un testValue al EthComponent.
        */
        private int _testValue;

        /**
        *	@brief Lista de animaciones disponible spara el EthComponent.
        */
        private List<EthComponentAnimator> _animators;

        /**
        *	@brief Asignación de la propiedad de lectura de la variable _animators.
        *
        *	@return El valor de la variable _animators.
        */
        public List<EthComponentAnimator> Animators
        {
            get
            {
                return _animators;
            }
        }
                
        /**
        *	@brief Constructor de la clase EthComponent.
        *
        *	Este método permite crear una instancia vacía de la clase EthComponent.
        */
        public EthComponent()
        {
            Enable = true;
        }

        /**
        *	@brief Constructor de la clase EthComponent.
        *
        *	Este método permite crear una instancia de la clase EthComponent especificando la GUI.
        *
        *	@param	gui	GUI sobre la cual se creará el componente.
        *
        */
        public EthComponent(EthComponentManager gui)
        {
            this._gui = gui;
            _testValue = 2;
            Enable = true;
        }

        /**
        *	@brief Constructor de la clase EthComponent.
        *
        *	Este método permite crear una instancia de la clase EthComponent especificando la GUI y argumentos.
        *
        *	@param gui		GUI sobre la cual se creará el componente.
        *	@param args 	Argumentos que tendrá el componente.
        *
        */
        public EthComponent(EthArguments args, EthComponentManager gui)
        {

            this._gui = gui;
            _x = Convert.ToSingle(args["x"]);
            _y = Convert.ToSingle(args["y"]);
            _rotation = (float)Eth.GetVal(Convert.ToSingle(args["rotation"]), 0);

            _name = args["name"];

            if (args["useLang"] == "false")
            {
                _useLang = false;
            }
            if (args["useLangImgs"] == "true")
            {
                _useLangImgs = true;
            }

            if (args["visibility"] != null)
            {
                Visible = Convert.ToBoolean(args["visibility"]);
            }
            Enable = true;
        }

        /**
        *	@brief Método que agrega una animación al componente.
        *
        *	@param animAct	Animación que tendrá el componente.
        *
        */
        public void AddAnimator(EthComponentAnimator animAct)
        {
            if (_animators == null)
            {
                _animators = new List<EthComponentAnimator>();
            }

            _animators.Add(animAct);
            animAct.SetComponent(this);
            animAct.StartAnimation();
        }

        /**
        *	@brief Método para recargar los argumentos del componente.
        *
        *	@param args Argumentos que tendrá el componente.
        *
        */
        public void ReloadArguments(string args)
        {
            EthArguments argsNew = new EthArguments(args);
            ReloadArguments(argsNew);
        }

        /**
        *	@brief Método para recargar los argumentos del componente, que podrá ser sobreescrito por las clases hijas.
        *
        *	@param args Argumentos que tendrá el componente.
        *
        */
        public virtual void ReloadArguments(EthArguments args)
        {
        }

        /**
        *	@brief Método para eliminar un EthComponent de su EthComponentManager.
        *
        *	@param callParent Parámetro que define si se remueve el EthComponent, si no se ingresa se tomará true por defecto.
        *
        */
        public virtual void Remove(bool callParent = true)
        {
            if (callParent)
            {
                _gui.RemoveComponent(this);
            }
        }

        /**
        *	@brief Método para dibujar un EthComponent.
        *
        *	@param offset Parámetro de tipo Vector2 para ser dibujado, el cual tambien puede ser nulo.
        *
        */
        public virtual void Draw(Vector2? offset = null)
        {
        }

        /**
        *	@brief Método para mandar el componente al final de la lista de componentes.
        *
        */
        public void ToBack()
        {
            _gui.ToBack(this);
        }

        /**
        *	@brief Método para mandar el componente al inicio de la lista de componentes.
        *
        */
        public void ToFront()
        {
            _gui.ToFront(this);
        }

        /**
        *	@brief Método para saber donde está el ancla del texto.
        *
        *	@param str Nombre del ANcla del texto.
        *
        *	@return TextAnchor del componente.
        */
        public TextAnchor GetTextAnchor(string str)
        {
            switch (str)
            {
                case "UpperLeft":
                    return TextAnchor.UpperLeft;
                case "UpperCenter":
                    return TextAnchor.UpperCenter;
                case "UpperRight":
                    return TextAnchor.UpperRight;
                case "MiddleLeft":
                    return TextAnchor.MiddleLeft;
                case "MiddleRight":
                    return TextAnchor.MiddleRight;
                case "LowerLeft":
                    return TextAnchor.LowerLeft;
                case "LowerCenter":
                    return TextAnchor.LowerCenter;
                case "LowerRight":
                    return TextAnchor.LowerRight;
                default:
                    return TextAnchor.MiddleCenter;
            }
        }

        /**
        *	@brief Método ToString que retorna el nombre de la clase.
        *
        *	@return Nombre de la clase, EthComponent.
        */
        public override string ToString()
        {
            return "EthComponent (" + Name + ")";
        }

    }
}
