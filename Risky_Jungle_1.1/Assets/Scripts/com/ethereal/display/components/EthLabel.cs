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
	*	@class 	EthLabel
	*   @brief 	Esta clase se encarga en crear un label al cual se le puede asignar un texto o una imagen para que la muestre
	*	en pantalla.
	*
	*/
	public class EthLabel : EthComponent {
		
		/**
		*	@brief  Estilo básico de la clase. Permite manejar el elemento en pantalla y controlar sus opciones de visualización.
		*/
		protected GUIStyle guiBot;
		
		/**
		*	@brief Estilo decorado de la clase. Permite manejar el elemento en pantalla y controlar sus opciones de visualización.
		*/
		protected GUIStyle guiBotDecorated;
		
		/**
		*	@brief el texto del label
		*/
        private string _text;

        /**
		*	@brief Asignación de propiedades de lectura y escritura a la variable _text.
		*/
        public string Text {
            get { return _text; }
            set { _text = value; }
        }
		
		/**
		*	@brief Variable para definir la decoración del desplazamiento.
		*/
        private int _decorateOffSet = 1;

        /**
		*	@brief Asignación de propiedades de lectura y escritura a la variable _decorateOffSet.
		*/
        public int DecorateOffSet {
            get { return _decorateOffSet; }
            set { _decorateOffSet = value; }
        }
		
		/**
		*	@brief define si el label ajusta su tamaño automáticamente o no.
		*/
		private bool autoResize = false;
		
		/**
		*	@brief ancho al cual se ajustará el label automáticamente.
		*/
		private float autoResizeWidth = 0;
		
		/**
		*	@brief alto al cual se ajustará el label automáticamente.
		*/
		private float autoResizeHeight = 0;
		
		/**
		*	@brief define si el texto del label se mostrará todo en mayúsculas o no.
		*/
        private bool _useUpperCase = false;

        /**
		*	@brief Asignación de propiedades de lectura y escritura a la variable _useUpperCase.
		*/
        public bool UseUpperCase {
            get { return _useUpperCase; }
            set { _useUpperCase = value; }
        }

        /**
		*	@brief Constante para acceder al argumento del estilo de fuente del EthDialog.
		*/
        private const string STYLE = "style";

        /**
		*	@brief Constante para acceder al argumento del tamaño de la fuente del EthDialog.
		*/
        private const string FONT_SIZE = "fontSize";

        /**
		*	@brief Constante para acceder al argumento de la fuente del EthDialog.
		*/
        private const string FONT = "font";

        /**
		*	@brief Constante para acceder al argumento del color de la fuente del EthDialog.
		*/
        private const string FONT_COLOR = "fontColor";

        /**
		*	@brief Constante para acceder al argumento de la alineación de la fuente del EthDialog.
		*/
        private const string ALIGN = "align";

        /**
		*	@brief Constante para definir sí la fuente del EthDialog es en mayúscula.
		*/
        private const string UPPER_CASE = "upperCase";

        /**
		*	@brief Constante para definir sí la fuente del EthDialog tiene envoltura.
		*/
        private const string WRAP = "wrap";
        
        /**
		*	@brief Constante para acceder al argumento del color decorativo de la fuente del EthDialog.
		*/
        private const string DECORATE_COLOR = "decorateColor";

		/**
		*	@brief constructor de la clase EthLabel.
		*
		*	Este método permite crear una instancia de la clase EthLabel.
		*
		*	@param args 		Parametros con los cuales se creará el objeto.
		*	@param parentGUI 	EthComponentManager al cual pertenecerá el objeto.
		*/
		public EthLabel (EthArguments args, EthComponentManager parentGUI) : base(args, parentGUI) {
			
			guiBot = new GUIStyle ();			
			guiBot.alignment = TextAnchor.MiddleCenter;

            if (args[FONT_SIZE] != null) {
                guiBot.fontSize = int.Parse(args[FONT_SIZE]);                       
			}

            if (args[ALIGN] != null) {
                guiBot.alignment = GetTextAnchor(args[ALIGN]);
			}

			Text = "";

			if ( args ["h"] != null ) {
				Hei =float.Parse (args ["h"]);
			}
			if ( args ["w"] != null ) {
				Wid=float.Parse (args ["w"]);
			}

            if (args[WRAP] == "true") {
				guiBot.wordWrap = true;                       
			}

            if (args[FONT] != null) {
                guiBot.font = Resources.Load(args[FONT]) as Font;                       
			}

            if (args[UPPER_CASE] == "true") {
				UseUpperCase = true;
			}

            if (args[FONT_COLOR] != null) {
                string[] colorBot = args[FONT_COLOR].Split('_');
				guiBot.normal.textColor = new Color (float.Parse (colorBot [0]), float.Parse (colorBot [1]), float.Parse (colorBot [2]));                       
				guiBot.hover.textColor = new Color (float.Parse (colorBot [0]), float.Parse (colorBot [1]), float.Parse (colorBot [2]));                       
			}
           
			if ( args ["text"] != null ) {
				Text = args ["text"];
			}

			if ( args [STYLE] != null ) {
                if (args[STYLE] == "bold") {
					guiBot.fontStyle = FontStyle.Bold;
                } else if (args[STYLE] == "italic") {
					guiBot.fontStyle = FontStyle.Italic;
                } else if (args[STYLE] == "boldanditalic") {
					guiBot.fontStyle = FontStyle.BoldAndItalic;
				}
			}

            guiBot.fontSize = (int)(guiBot.fontSize * Math.Min(_gui.WRatio, _gui.HRatio));

            if (args[DECORATE_COLOR] != null) {		

				guiBotDecorated = new GUIStyle ();
				guiBotDecorated.alignment = guiBot.alignment;           
				guiBotDecorated.wordWrap = guiBot.wordWrap;
				guiBotDecorated.font = guiBot.font;
				guiBotDecorated.fontSize = guiBot.fontSize;
				guiBotDecorated.fontStyle = guiBot.fontStyle;

                string[] colorBot = args[DECORATE_COLOR].Split('_');
				guiBotDecorated.normal.textColor = new Color (float.Parse (colorBot [0]), float.Parse (colorBot [1]), float.Parse (colorBot [2]));                       
				guiBotDecorated.hover.textColor = new Color (float.Parse (colorBot [0]), float.Parse (colorBot [1]), float.Parse (colorBot [2]));                       
			}

			if ( args ["autoResize"] == "true" ) {
				autoResize = true;       
				autoResizeWidth = int.Parse (args ["autoResizeWidth"]);
				Wid=autoResizeWidth;

                Hei = guiBot.CalcHeight(new GUIContent(_text), autoResizeWidth * _gui.WRatio) / _gui.HRatio;
				autoResizeHeight = Hei;	
			}
		}

		/**
		*	@brief Devuelve el GUIStyle de la barra de progreso.
		*
		*	Retorna el GUIStyle asociado a la barra de progreso.
		*
		*	@return el guiBot de la clase.
		*/
		public GUIStyle GetStyle () {
			return guiBot;
		}

		/**
		*	@brief Muestra el label en pantalla.
		*
		*	Este método permite mostrar el GuiStyle del label en la pantalla.
		*
		*	@param offSet vector2 que da las cordenadas para ubicar el objeto en un punto específico de la pantalla.
		*/
        public override void Draw(Vector2? offset = null)
        {

            string textTemp = "";

            if (Text != "")
            {
                textTemp = EthLang.GetEntry(Text, UseLang);
            }

            if (UseUpperCase)
            {
                textTemp = textTemp.ToUpper();
            }

            Vector2 offset2 = offset ?? Vector2.zero;
            float xTemp = X + offset2.x;
            float yTemp = Y + offset2.y;

            if (guiBotDecorated != null)
            {
                if (!_visible)
                {
                    return;
                }
                GUI.Label(new Rect(((xTemp * _gui.WRatio) + _gui.WOffset) - DecorateOffSet, ((yTemp * _gui.HRatio) + _gui.HOffset) - DecorateOffSet, Wid * _gui.WRatio, Hei * _gui.HRatio), textTemp, guiBotDecorated);
                GUI.Label(new Rect(((xTemp * _gui.WRatio) + _gui.WOffset) - DecorateOffSet, ((yTemp * _gui.HRatio) + _gui.HOffset) + DecorateOffSet, Wid * _gui.WRatio, Hei * _gui.HRatio), textTemp, guiBotDecorated);
                GUI.Label(new Rect(((xTemp * _gui.WRatio) + _gui.WOffset) + DecorateOffSet, ((yTemp * _gui.HRatio) + _gui.HOffset) - DecorateOffSet, Wid * _gui.WRatio, Hei * _gui.HRatio), textTemp, guiBotDecorated);
                GUI.Label(new Rect(((xTemp * _gui.WRatio) + _gui.WOffset) + DecorateOffSet, ((yTemp * _gui.HRatio) + _gui.HOffset) + DecorateOffSet, Wid * _gui.WRatio, Hei * _gui.HRatio), textTemp, guiBotDecorated);
            }

            Matrix4x4 matrix = GUI.matrix;

            if (Rotation != 0)
            {
                GUI.matrix = Matrix4x4.identity;
                GUIUtility.RotateAroundPivot(Rotation, new Vector2((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset));
            }

            if (autoResize)
            {

            }

            if (!Visible)
            {
                return;
            }
            GUI.Label(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), textTemp, guiBot);

            if (Rotation != 0)
            {
                GUI.matrix = matrix;
            }
        }

		/**
		*	@brief metodo toString de la clase.
		*
		*	Retorna el nombre de la clase como una cadena de caracteres.
		*
		*	@return la clase representada en un string.
		*/
		public override string ToString () {
			return "EthLabel (" + Name + ")";
		}
	}
}
