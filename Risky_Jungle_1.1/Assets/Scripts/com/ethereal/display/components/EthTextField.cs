using UnityEngine;

using System;
using Assets.Scripts.com.ethereal.util;

namespace Assets.Scripts.com.ethereal.display.components {
    
    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Octubre 16 2014
    * 
    *	@class 	EthTextField
    *   @brief 	Esta clase se encarga de crear un campo de texto, para poder ingresar datos.
    *
    */
	public class EthTextField : EthComponent {
	
		/**
        *   @brief Estilo del EthTextField.
        */
		protected GUIStyle guiBot;

		/**
        *   @brief Estilo transparente del EthTextField.
        */
		private GUIStyle stlTransparent;

		/**
        *   @brief Texto del EthTextField.
        */
		private string _text;

		/**
		*	@brief Asignación de las propiedades de lectura y escritura del atributo _text.
		*
		*	@return El valor de la variable _text.
		*/
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /**
        *   @brief Variable que define sí el EthTextField es de contraseña.
        */
		private bool isPassword;

		/**
        *   @brief Variable que define sí el EthTextField está habilitado para ingresar texto.
        */
		private bool activeEthTextField = false;

		/**
        *   @brief Variable que representa una cadena vacía.
        */
		private string empty;

		/**
        *   @brief Número máximo de caracteres que pueden ser ingresados en el EthTextField.
        */
		private int maxLength;

		/**
		*	@brief Método para invocar el evento OnActive.
		*/
		public delegate void OnActiveEvent (string name);

		/**
        *	@brief Evento para detectar actividad en el EthTextField.
        */
		public event OnActiveEvent OnActive;

		/**
		*	@brief Método para invocar el evento OnEnter.
		*/
		public delegate void OnEnterEvent (string name);

		/**
        *	@brief Evento para detectar el ingreso de texto en el EthTextField.
        */
		public event OnEnterEvent OnEnter;

		/**
		*	@brief constructor de la clase EthTextField.
		*
		*	Este método permite crear una instancia de la clase EthTextField.
		*
		*	@param args 		Parametros con los cuales se creará el objeto.
		*	@param parentGUI 	EthComponentManager al cual pertenecerá el objeto.
		*/
		public EthTextField (EthArguments args, EthComponentManager parentGUI) : base(args, parentGUI) {
			
			guiBot = new GUIStyle ();
			
			if ( args [Eth.IMG] != null ) {
                string nomTextura = args[Eth.IMG];

				Texture2D textura = Resources.Load (nomTextura) as Texture2D;
				
				guiBot.normal.background = textura;

				Wid =textura.width;
				Hei =textura.height;
			}            

			if ( args ["h"] != null ) {
				Hei=float.Parse (args ["h"]);
			}
			if ( args ["w"] != null ) {
				Wid=float.Parse (args ["w"]);
			}

			_text = "";

			if ( args ["font"] != null ) {
				guiBot.font = Resources.Load (args ["font"]) as Font;                       
			}

			if ( args ["align"] != null ) {
				guiBot.alignment = GetTextAnchor (args ["align"]);
			}
               
			if ( args ["fontColor"] != null ) {
				string[] colorBot = args ["fontColor"].Split ('_');
				guiBot.normal.textColor = new Color (float.Parse (colorBot [0]), float.Parse (colorBot [1]), float.Parse (colorBot [2]));                       
				guiBot.hover.textColor = new Color (float.Parse (colorBot [0]), float.Parse (colorBot [1]), float.Parse (colorBot [2]));                       
			}

			if ( args ["fontSize"] != null ) {
				guiBot.fontSize = int.Parse (args ["fontSize"]);    
				Debug.Log ("asuiasfd; " + args ["fontSize"]);                   
			}

			if ( args ["text"] != null ) {
				_text = args ["text"];
			}
			
			guiBot.clipping = TextClipping.Clip;
			if ( args ["textClipping"] != null ) {
				if ( args ["textClipping"] == "false" ) {
					guiBot.clipping = TextClipping.Overflow;
				}
			}

			isPassword = false;
			if ( args ["isPassword"] != null ) {
				if ( args ["isPassword"] == "true" ) {
					isPassword = true;
				}
			}

			maxLength = 0;
			if ( args ["maxLength"] != null ) {
				maxLength = int.Parse (args ["maxLength"]);
			}

			guiBot.fontSize = (int) (guiBot.fontSize * Math.Min (_gui.WRatio, _gui.HRatio));

			empty = "";
			if ( args ["emptyString"] != null ) {
				empty = args ["emptyString"];

				stlTransparent = new GUIStyle ();
				stlTransparent.normal.background = null;

				stlTransparent.font = guiBot.font;
				stlTransparent.normal.textColor = guiBot.normal.textColor;
				stlTransparent.hover.textColor = guiBot.hover.textColor;
				stlTransparent.fontSize = guiBot.fontSize;

				if ( args ["alignEmptyString"] != null ) {
					stlTransparent.alignment = GetTextAnchor (args ["alignEmptyString"]);
				}
			}											
		}

		/**
		*	@brief Método para dibujar una EthTextField.
		*
		*	@param offset Parámetro de tipo Vector2 para ser dibujado, el cual támbien puede ser nulo.
		*
		*	@see com.ethereal.display.components.EthComponent
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

            if (OnEnter != null)
            {
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
                {
                    if (GUI.GetNameOfFocusedControl().CompareTo(Name) == 0)
                    {
                        click();
                    }
                }
            }

            GUI.SetNextControlName(Name);
            if (isPassword)
            {
                if (maxLength > 0)
                {
                    _text = GUI.PasswordField(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), _text, "*"[0], maxLength, guiBot);
                }
                else
                {
                    _text = GUI.PasswordField(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), _text, "*"[0], guiBot);
                }
            }
            else
            {
                if (maxLength > 0)
                {
                    _text = GUI.TextField(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), _text, maxLength, guiBot);
                }
                else
                {
                    _text = GUI.TextField(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), _text, guiBot);
                }
            }

            if (empty != "")
            {
                string textTemp = "";

                textTemp = EthLang.GetEntry(empty, UseLang);

                if (_text.CompareTo("") == 0 && GUI.GetNameOfFocusedControl().CompareTo(Name) != 0)
                {
                    GUI.Label(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), textTemp, stlTransparent);
                }
            }

            if (OnActive != null)
            {

                if (!activeEthTextField && GUI.GetNameOfFocusedControl().CompareTo(Name) == 0)
                {
                    activeEthTextField = true;
                    OnActive(Name);
                }
                else if (activeEthTextField && GUI.GetNameOfFocusedControl().CompareTo(Name) != 0)
                {
                    activeEthTextField = false;
                }
            }
        }

		/**
		*	@brief Método para modificar el valor de la variable _text.
		*
		*	@param texto Nuevo valor de la variable _text.
		*/
		public void setText (string texto) {
			_text = texto;
		}

		/**
		*	@brief Método para suscribirse al evento OnActive.
		*/
		public void setActiveFunction (OnActiveEvent fn) {
			OnActive += fn;
		}

		/**
		*	@brief Método para suscribirse al evento OnEnter.
		*/
		public void setFunction (OnEnterEvent fn) {
			OnEnter += fn;
		}

		/**
		*	@brief Método para invocar el evento OnEnter.
		*/
		public virtual void click () {						
			if ( OnEnter != null ) {
				OnEnter (_text);
			}
		}

		/**
		*	@brief Método ToString que retorna el nombre de la clase.
		*
		*	@return Nombre de la clase, EthTextField.
		*/
		public override string ToString () {
			return "EthTextField (" + Name + ")";
		}
	}
}
