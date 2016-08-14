using UnityEngine;

using Assets.Scripts.com.ethereal.audio;
using System;
using Assets.Scripts.com.ethereal.util;

/*
Ejemplo de uso:

EthButton bot = gui.AddButton("Bot0",0,0,"text:Hola,font:fonts/Avenger,fontColor:0_0_0,fontSize:18,useLang:true");

h -> si se desea especificar un alto definido
w -> si se desea especificar un ancho definido
font -> La fuente a usar
fontColor -> El color de la fuente a usar separado por _, por ejemplo para el blanco 1_1_1
fontColorHover -> El color en estado hover de la fuente a usar separado por _, por ejemplo para el blanco 1_1_1
fontSize -> El tamaño de la fuente a usar
useLang -> Indica si pasa por cambio debido a lenguaje o no.
useLangImgs -> indica si para la imagen se usa lang o no, se utiliza en ese caso img_es o img_en
img -> La ruta de la imagen a usar de fondo, la misma pero con _pressed al final aplica para el estado hover
backColor -> si en vez de una imagen se quiere un color de fondo, podria ser 1_1_1, debe ser usado con w y h
backColorHover -> si se quiere un cambio de color para el hover, podria ser 1_1_1, debe ser usado con w y h
offset -> si se quiere colocar offset al texto, por ejemplo -100_0, lo corre -100 en X y 0 en Y
align -> la alineacion del texto con respecto al centro, pueden ser los valores de TextAnchor de unity, si no se provee entonces es MiddleCenter
imgHover -> si se quiere una imagen en especial para cuando esta en hover
*/

namespace Assets.Scripts.com.ethereal.display.components
{
   /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Octubre 16 2014
    * 
    *	@class 	EthButton
    *   @brief 	Esta clase se encarga de crear un único botón que ejecutará una acción al momento de ser presionado.
    *
    */
    public class EthButton : EthComponent
    {
        /**
        *	@brief Constante para indícar en el log cuando se presiona una textura.
        */
        private const string TXT_PRESSED = "_pressed";

        /**
        *	@brief Constante para indícar que no existe la imagen a cargar.
        */
        private const string IMG_NOT_FOUND = "No se encuentra la imagen ";

        /**
        *	@brief Constante para indícar el sufijo imagen.
        */
        private const string IMG = "img";

        /**
        *	@brief Constante para indícar la propiedad de color de fondo en una textura.
        */
        private const string BACK_COLOR = "backColor";

        /**
        *	@brief Constante para indícar el guíon bajo al separar la cadena de propiedades.
        */
        private const char UNDERSCORE = '_';

        /**
        *	@brief Constante para indícar la propiedad de alto en una textura.
        */
        private const string HEIGHT = "h";

        /**
        *	@brief Constante para indícar la propiedad de ancho en una textura.
        */
        private const string WIDTH = "w";

        /**
        *	@brief Constante para indícar la propiedad del color de fondo al seleccionar la textura.
        */
        private const string BACK_COLOR_HOVER = "backColorHover";

        /**
        *	@brief Constante para indícar la propiedad dela escala de la cuadricula de la textura.
        */
        private const string SCALE_GRID = "scaleGrid";

        /**
        *	@brief Constante para indícar la propiedad de relleno en una textura.
        */
        private const string PADDING = "padding";

        /**
        *	@brief Constante para indícar la imagen al pasar el raton por la textura.
        */
        private const string IMG_HOVER = "imgHover";

        /**
        *	@brief Constante para indícar la alineación de textura.
        */
        private const string ALIGN = "align";

        /**
        *	@brief Constante para indícar como envuelve la textura.
        */
        private const string WRAP = "wrap";

        /**
        *	@brief Constante para comparar los valores true de la cadena de propiedades.
        */
        private const string TRUE = "true";

        /**
        *	@brief Constante para indícar la propiedad de la fuente.
        */
        private const string FONT = "font";

        /**
        *	@brief Constante para indícar la propiedad del color de la fuente.
        */
        private const string FONT_COLOR = "fontColor";

        /**
        *	@brief Constante para indícar la propiedad del color de la fuente al pasar el ratón encima.
        */
        private const string FONT_COLOR_HOVER = "fontColorHover";

        /**
        *	@brief Constante para indícar la propiedad del tamaño de la fuente.
        */
        private const string FONT_SIZE = "fontSize";

        /**
        *	@brief Constante para indícar el texto.
        */
        private const string TEXT = "text";

        /**
        *	@brief Constante para indícar la propiedad audio clic.
        */
        private const string AUDIO_CLIC = "audioClic";

        /**
        *	@brief Constante para indícar la propiedad del desplazamiento.
        */
        private const string OFFSET = "offset";

        /**
        *	@brief Constante para indícar la propiedad del desplazamiento al pasar el ratón encima.
        */
        private const string OFFSET_HOVER = "offsetHover";

        /**
        *	@brief Constante para indícar la alineación al paso del ratón.
        */
        private const string ALIGN_HOVER = "alignHover";

        /**
        *	@brief Constante para indícar la propiedad de autoresize.
        */
        private const string AUTO_RESIZE = "autoResize";

        /**
        *	@brief Constante para indícar la propiedad de autoresize del ancho.
        */
        private const string AUTO_RESIZE_WIDTH = "autoResizeWidth";

        /**
        *	@brief Constante para indícar la propiedad de permitir multiple touch.
        */
        private const string AVOID_MULTITOUCH_CLICK = "avoidMultitouchClick";

        /**
        *	@brief Constante para comparar los valores false de la cadena de propiedades.
        */
        private const string FALSE = "false";

        /**
        *	@brief Constante a mostrar cuando un botón tenga una animación establecida y se intente asignarle otra.
        */
        private const string CHECK_TWO_ANIMATORS = "check if the button has two animators.";

        /**
        *	@brief Variable que le dará el estílo al botón.
        */
        protected GUIStyle _guiBot;

        /**
        *	@brief texto que mostrará el botón.
        */
        private string _text;

        /**
		*	@brief Asignación de descriptores de lectura y escritura para la variable _text.
        *
        *   @return El valor de la variable _text.
		*/
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /**
        *	@brief Variable para indicar si se detecta profundidad.
        */
        private bool _detectDeep = false;

        /**
		*	@brief Asignación de descriptores de lectura y escritura para la variable _detectDeep.
        *
        *   @return El valor de la variable _detectDeep.
		*/
        public bool DetectDeep
        {
            get { return _detectDeep; }
            set { _detectDeep = value; }
        }

        /**
        *	@brief Define si el botón está siendo presionado por el mouse o no
        */
        private bool _mouseDown = false;

        /**
        *	@brief Define si el botón se puede arrastrar o no.
        */
        private bool _dragButton = false;

        /**
        *	@brief Evento para detectar cuando el botón es presionado.
        */
        public delegate void OnClickEvent(string name);

        /**
        *	@brief Evento para detectar cuando el botón es arrastrado. 
        */
        public delegate void OnDragEvent(EthButton boton);

        /**
        *	@brief Evento para detectar el click sobre el botón.
        */
        public event OnClickEvent OnClick;

        /**
        *	@brief Evento para detectar cuando el botón es arrastrado.
        */
        public event OnDragEvent OnDrag;

        /**
        *	@brief Evento OnClick para cuando el botón es liberado.
        */
        public event OnClickEvent OnMouseUp;

        /**
        *	@brief Posición inicial del botón en el eje x.
        */
        private float _xInicial;

        /**
        *	@brief Posición inicial del botón en el eje y.
        */
        private float _yInicial;

        /**
        *	@brief Posición actual del botón en el eje x.
        */
        private float _xM;

        /**
        *	@brief Posición actual del botón en el eje y.
        */
        private float _yM;

        /**
        *	@brief Método para definir el evento al dar clic en un objeto.
        */
        public delegate void OnClickObjEvent(string name, object obj);

        /**
        *	@brief Evento al dar clic en un objeto.
        */
        public event OnClickObjEvent OnClickObj;

        /**
        *	@brief Vector que guarda las coordenadas del la posición del mouse.
        */
        private Vector2 _mousePos;

        /**
        *	@brief Objeto que representa el boton.
        */
        private object _obj = null;

        /**
        *	@brief Ancho al cual se ajustará el botón automaticamente.
        */
        private float _autoResizeWidth;

        /**
        *	@brief Altura a la cual se ajustará el botón automaticamente.
        */
        private float _autoResizeHeight;

        /**
        *	@brief Esta variable define sí el botón ajusta su tamaño automaticamente o no.
        */
        private bool _autoResize = false;

        /**
        *	@brief Alfa para el color del botón.
        */
        private float _alpha = 1f;

        /**
		*	@brief Asignación de descriptores de lectura y escritura para la variable _alpha.
        *
        *   @return El valor de la variable _alpha.
		*/
        public float Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        /**
        *	@brief define si se puede cambiar el alfa para el color del botón o no.
        */
        protected bool _changeAlfa = false;

        //Variables para el offset

        /**
        *	@brief Vector para definir el alcance del ratón.
        */
        protected Vector2 _offHover;

        /**
        *	@brief Vector desplazamiento.
        */
        protected Vector2 _off;

        /**
        *	@brief nombre de la textura para el boton.
        */
        protected string _nomTextura;

        /**
        *	@brief Variable para definir el anclaje normal del texto.
        */
        protected TextAnchor _alignNormal;

        /**
        *   @brief Variable para definir el anclaje flotante del texto.
        */
        protected TextAnchor _alignHover;

        /**
        *	@brief ruta donde se aloja el audio que se reproduce cuando se hace clic en el botón.
        */
        private string _audioClic;

        //chanes made in the multimedia
        /**
        *	@brief Variable para definir si se usarán los valores de forma flotante.
        */
        protected bool _useValueHovers = false;

        /**
        *   @brief Variable para definir si se usarán animaciones.
        */
        private bool _canAnimate = false;

        /**
        *   @brief Asignación de las propiedades de lectura y escritura de la variable _canAnimate.
        *   
        *   @return El valor de la variable _canAnimate.
        */
        public bool CanAnimate
        {
            get
            {
                return _canAnimate;
            }
            set
            {
                _canAnimate = value;
            }
        }
        /**
        *	@brief vector que guarda las coordenadas del offset
        */
        private Vector2 _offsetScreen;

        /**
        *	@brief Variable para definir si ya se realizo una primera animación.
        */
        private bool _yaanime = false;

        /**
        *	@brief Variable que cambia cada que se realiza una animación, para si, la siguiente sea diferente a la anterior.
        */
        private bool _yaanimeclick = false;

        /**
        *	@brief Variable para definir si se evitan los clicks multitouch.
        */
        private bool _avoidMultitouchClick = true;
		
		/**
		*	@brief Variable que contiene la ruta por defecto para el sonido de cualquier boton en el juego
		*/
		public static string defaultSoundPath = "";


        /**
        *	@brief constructor de la clase EthButton.
        *
        *	Este método permite crear una instancia de la clase ethButton
        *
        *	@param args parametros con los cuales se creará la clase.
        */
        public EthButton(EthArguments args, EthComponentManager parentGUI)
            : base(args, parentGUI)
        {

            _guiBot = new GUIStyle();

            ReloadArguments(args);
        }

        /**
        *	@brief Permite asignar el evento onDrag al botón.
        *
        *	Este método permite asignar el evento onDrag al botón.
        *
        *	@param onDrag envento onDrag que se va a asignar al botón.
        *
        *	@return el onDrag del botón con el evento asignado.
        */
        public EthButton SetOnDragFuction(OnDragEvent onDrag)
        {
            this.OnDrag += onDrag;
            return this;
        }

        /**
        *	@brief Método para asignar una textura al botón.
        *
        *	Permite cambiar la apariencia del botón asignandole una textura.
        *	
        *	@param omTextura nombre de la textura.
        *	@param restore define si el botón se ajustará o no al tamaño de la textura, por defecto si lo hará.
        */
        public virtual void SetTexture(String nomTextura, bool restore = true)
        {

            Texture2D textura = Resources.Load(nomTextura) as Texture2D;
            Texture2D texturaHover = Resources.Load(nomTextura + TXT_PRESSED) as Texture2D;

            if (textura == null)
            {
                throw new System.ArgumentException(IMG_NOT_FOUND + nomTextura, IMG);
            }

            _guiBot.hover.background = (Texture2D)Eth.GetVal(texturaHover, textura);
            _guiBot.normal.background = textura;

            if (restore)
            {
                Wid=textura.width;
                Hei=textura.height;
            }
        }

        /**
        *	@brief Método que carga los atributos de la clase con los argumentos.
        *
        *	Este método permite recuperar los valores ingresados como argumentos que están almacenados en un diccionario de 
        *	datos y asignarlos a la variable indicada. Dichos argumentos ingresan al método como un objeto de la clase EthArguments.
        *	
        *	@param args objeto de la clase EthArguments, el cual posee el diccionario de datos con la información de los atributos de la clase.
        */
        public override void ReloadArguments(EthArguments args)
        {

            if (args["changeAlfa"] != null)
            {
                _changeAlfa = bool.Parse(args["changeAlfa"]);
                Alpha = float.Parse(args["alfa"]);
            }
            if (args[IMG] != null)
            {
                _nomTextura = args[IMG];
				
				try
				{
					Texture2D textura;
					Texture2D texturaHover;
					
					if ( _useLangImgs ) {
						_lastLangUsed = EthLang.LangAct;
						textura = Resources.Load (_nomTextura + "_" + _lastLangUsed) as Texture2D;
						texturaHover = Resources.Load (_nomTextura + "_" + _lastLangUsed + TXT_PRESSED) as Texture2D;						
					} else {
						textura = Resources.Load (_nomTextura) as Texture2D;
						texturaHover = Resources.Load (_nomTextura + TXT_PRESSED) as Texture2D;
					}
										
					if (textura == null)
					{
						throw new System.ArgumentException(IMG_NOT_FOUND + _nomTextura, IMG);
					}

					_guiBot.hover.background = (Texture2D)Eth.GetVal(texturaHover, textura);
					_guiBot.normal.background = textura;

					Wid = textura.width;
					Hei = textura.height;
				}
				catch
				{
					Texture2D textura = Resources.Load (_nomTextura + ".png") as Texture2D;
				}
                
            }
            else if (args[BACK_COLOR] != null)
            {

                string[] colorBack = args[BACK_COLOR].Split(UNDERSCORE);

                Color colorBkg;

                if (colorBack.Length == 3)
                {
                    colorBkg = new Color(float.Parse(colorBack[0]), float.Parse(colorBack[1]), float.Parse(colorBack[2]));
                }
                else
                {
                    colorBkg = new Color(float.Parse(colorBack[0]), float.Parse(colorBack[1]), float.Parse(colorBack[2]), float.Parse(colorBack[3]));
                }


                Texture2D textura = new Texture2D(1, 1);
                Texture2D texturaHover = null;
                textura.SetPixel(0, 0, colorBkg);
                textura.Apply();

                if (args[HEIGHT] != null)
                {
                    Hei=float.Parse(args[HEIGHT]);
                }
                if (args[WIDTH] != null)
                {
                    Wid=float.Parse(args[WIDTH]);
                }

                if (args[BACK_COLOR_HOVER] != null)
                {

                    colorBack = args[BACK_COLOR_HOVER].Split(UNDERSCORE);

                    if (colorBack.Length == 3)
                    {
                        colorBkg = new Color(float.Parse(colorBack[0]), float.Parse(colorBack[1]), float.Parse(colorBack[2]));
                    }
                    else
                    {
                        colorBkg = new Color(float.Parse(colorBack[0]), float.Parse(colorBack[1]), float.Parse(colorBack[2]), float.Parse(colorBack[3]));
                    }

                    texturaHover = new Texture2D(1, 1);
                    texturaHover.SetPixel(0, 0, colorBkg);
                    texturaHover.Apply();
                }

                _guiBot.hover.background = (Texture2D)Eth.GetVal(texturaHover, textura);
                _guiBot.normal.background = textura;
                _guiBot.stretchHeight = true;
            }

            if (args[SCALE_GRID] != null)
            {
                string data = args[SCALE_GRID];

                RectOffset rectGrid = new RectOffset();

                if (data.IndexOf("_") > -1)
                {
                    string[] dataGridValues = args[SCALE_GRID].Split(UNDERSCORE);
                    rectGrid = new RectOffset(int.Parse(dataGridValues[0]), int.Parse(dataGridValues[1]), int.Parse(dataGridValues[2]), int.Parse(dataGridValues[3]));
                }
                else
                {
                    int scalegrid = int.Parse(args[SCALE_GRID]);
                    rectGrid = new RectOffset(scalegrid, scalegrid, scalegrid, scalegrid);
                }

                _guiBot.border = rectGrid;
            }

            if (args[PADDING] != null)
            {
                int padding = int.Parse(args[PADDING]);
                _guiBot.padding = new RectOffset(padding, padding, padding, padding);
            }


            if (args[IMG_HOVER] != null)
            {
                Texture2D texturaHover = Resources.Load(args[IMG_HOVER]) as Texture2D;
                _guiBot.hover.background = texturaHover;
            }

            _alignNormal = _guiBot.alignment = TextAnchor.MiddleCenter;

            if (args[ALIGN] != null)
            {
                _alignNormal = _guiBot.alignment = GetTextAnchor(args[ALIGN]);
            }

            if (args[WRAP] == TRUE)
            {
                _guiBot.wordWrap = true;
            }

            if (args[HEIGHT] != null)
            {
                Hei=float.Parse(args[HEIGHT]);
            }
            if (args[WIDTH] != null)
            {
                Wid=float.Parse(args[WIDTH]);
            }

            Text = "";

            if (args[FONT] != null)
            {
                _guiBot.font = Resources.Load(args[FONT]) as Font;
            }


            if (args[FONT_COLOR] != null)
            {
                string[] colorBot = args[FONT_COLOR].Split(UNDERSCORE);
                _guiBot.normal.textColor = new Color(float.Parse(colorBot[0]), float.Parse(colorBot[1]), float.Parse(colorBot[2]));
                _guiBot.hover.textColor = new Color(float.Parse(colorBot[0]), float.Parse(colorBot[1]), float.Parse(colorBot[2]));
            }

            if (args[FONT_COLOR_HOVER] != null)
            {
                string[] colorBot = args[FONT_COLOR_HOVER].Split(UNDERSCORE);
                _guiBot.hover.textColor = new Color(float.Parse(colorBot[0]), float.Parse(colorBot[1]), float.Parse(colorBot[2]));
            }

            if (args[FONT_SIZE] != null)
            {
                _guiBot.fontSize = int.Parse(args[FONT_SIZE]);
            }

            if (args[TEXT] != null)
            {
                Text = args[TEXT];
            }

            if (args[AUDIO_CLIC] != null)
            {
                _audioClic = args[AUDIO_CLIC];
            } else if(defaultSoundPath != "") {
			    _audioClic = defaultSoundPath;
			}

            if (args[OFFSET] != null)
            {
                string[] offsetStr = args[OFFSET].Split(UNDERSCORE);
                _off = new Vector2(float.Parse(offsetStr[0]) * _gui.WRatio, float.Parse(offsetStr[1]) * _gui.HRatio);
                _guiBot.contentOffset = _off;
            }

            if (args[OFFSET_HOVER] != null)
            {
                _useValueHovers = true;
                string[] offsetStr = args[OFFSET_HOVER].Split(UNDERSCORE);
                _offHover = new Vector2(float.Parse(offsetStr[0]) * _gui.WRatio, float.Parse(offsetStr[1]) * _gui.HRatio);
                _alignHover = GetTextAnchor(args[ALIGN_HOVER]);
            }

            _guiBot.fontSize = (int)(_guiBot.fontSize * Math.Min(_gui.WRatio, _gui.HRatio));

            if (args[AUTO_RESIZE] == TRUE && args[AUTO_RESIZE] == TRUE)
            {
                _autoResize = true;
                _autoResizeWidth = int.Parse(args[AUTO_RESIZE_WIDTH]);
                Wid=_autoResizeWidth;

                Hei = _guiBot.CalcHeight(new GUIContent(Text), _autoResizeWidth * _gui.WRatio) / _gui.HRatio;
                _autoResizeHeight = Hei;
            }

            if (args[AVOID_MULTITOUCH_CLICK] == FALSE)
            {
                _avoidMultitouchClick = false;
            }
        }

        /**
        *	@brief Método para asignar el descriptor escritura a la variable _text.
        *
        *	@param obj Valor por el que será modificada la variable.
        */
        public EthButton setObject(object obj)
        {
            this._obj = obj;
            return this;
        }

        /**
        *	@brief Método para mostrar el botón en pantalla.
        *
        *	Este método permite mostrar el GuiStyle del botón en la pantalla.
        *
        *	@param offSet vector2 que da las cordenadas para ubicar el objeto en un punto específico de la pantalla.
        */
        public override void Draw(Vector2? offset = null)
        {
            if (!Visible)
            {
                return;
            }
            if (Animators != null && !_canAnimate)
            {
                Animators[0].AnimateComponent();
            }

            _offsetScreen = offset ?? Vector2.zero;
            float xTemp = X + _offsetScreen.x;
            float yTemp = Y + _offsetScreen.y;

            Rect rctScroll = new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio);

            if (_useValueHovers)
            {
                _guiBot.contentOffset = _off;
                _guiBot.alignment = _alignNormal;

                if (Input.mousePosition.x + _offsetScreen.x > rctScroll.x && Input.mousePosition.x + _offsetScreen.x < (rctScroll.width + rctScroll.x))
                {
                    if ((Screen.height - Input.mousePosition.y + _offsetScreen.y - 1) >= rctScroll.y && (Screen.height - Input.mousePosition.y + _offsetScreen.y - 1) <= (rctScroll.height + rctScroll.y))
                    {
                        _guiBot.contentOffset = _offHover;
                        _guiBot.alignment = _alignHover;
                    }
                }
            }

            string textTemp = "";

            if (Text != "")
            {
                textTemp = EthLang.GetEntry(Text, UseLang);
            }

            Color temp = GUI.color;
            if (_changeAlfa)
            {
                Color aux = GUI.color;
                aux.a = Alpha;
                GUI.color = aux;
            }

            if (GUI.Button(rctScroll, textTemp, _guiBot))
            {
                bool contains = true;
                //solucion al problema de clic en la mitad de 2 touches en moviles
                if (Eth.IsMobile() && !_avoidMultitouchClick)
                {
                    Vector2 valuesToAdd = new Vector2(_offsetScreen.x, _offsetScreen.y);
                    if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        valuesToAdd = Vector2.zero;
                    }
                    contains = false;
                    for (int i = 0; i < Input.touchCount; i++)
                    {

                        Vector2 tmVt2 = new Vector2(Input.GetTouch(i).position.x + valuesToAdd.x, Screen.height - Input.GetTouch(i).position.y + valuesToAdd.y);
                        if (rctScroll.Contains(tmVt2))
                        {
                            contains = true;
                            break;
                        }
                    }
                }

                if (contains)
                {
                    if (Enable)
                    {
                        Click();
                    }
                }
            }

            //si se puede arrastrar o no
            if (DetectDeep)
            {
                if (!_mouseDown)
                {
                    if (Input.GetMouseButtonDown(0) && IsOver())
                    {
                        _mouseDown = true;
                        guardarCoordenadas();
                    }
                }
                else
                {
                    if (Input.GetMouseButton(0) && _dragButton)
                    {
                        arrastrarBoton();
                        if (OnDrag != null)
                        {
                            OnDrag(this);
                        }
                    }
                    else
                    {
                        _mouseDown = false;
                        mouseUp();
                    }
                }
            }

            if (_canAnimate)
            {
                if (IsOver())
                {

                    if (Input.GetMouseButtonDown(0) && !_yaanimeclick)
                    {
                        if (Animators[1] != null)
                        {
                            Animators[1].AnimateComponent();
                            _yaanimeclick = true;
                        }
                        else
                        {
                            Debug.Log(CHECK_TWO_ANIMATORS);
                        }
                    }
                    else if (Input.GetMouseButtonUp(0) && _yaanimeclick)
                    {
                        if (Animators[1] != null)
                        {
                            Animators[1].AnimateComponent();
                            _yaanimeclick = false;
                        }
                        else
                        {
                            Debug.Log(CHECK_TWO_ANIMATORS);
                        }
                    }
                    else if (!_yaanime)
                    {
                        if (Animators[0] != null)
                        {
                            Animators[0].AnimateComponent();
                            _yaanime = true;
                        }
                        else
                        {
                            Debug.Log(CHECK_TWO_ANIMATORS);
                        }
                    }
                }
                else
                {
                    if (_yaanime && !Input.GetMouseButtonUp(0))
                    {
                        Animators[0].SetToOriginalSizeY();
                        Animators[0].SetToOriginalSizeX();
                        _yaanime = false;

                    }
                    if (Input.GetMouseButtonUp(0) && _yaanimeclick)
                    {
                        if (Animators[1] != null)
                        {
                            Animators[1].SetToOriginalSizeY();
                            Animators[1].SetToOriginalSizeX();
                            _yaanimeclick = false;
                        }
                        else
                        {
                            Debug.Log(CHECK_TWO_ANIMATORS);
                        }

                    }
                }

            }

            GUI.color = temp;
        }

        /**
        *	@brief Permite retornar el botón a su posición inicial.
        *
        *	Este método permite regresar el botón a su posición inicial luego de haber sido arrastrado.
        */
        public void regresarAOrigen()
        {
            X = _xInicial;
            Y=_yInicial;
        }

        /**
        *	@brief Permite guardar las cordenadas de la posición del mouse.
        *
        *	Permite almacenar en un vector2 las coordenadas de la posición del mouse respecto al tamaño de la pantalla.
        */
        private void guardarCoordenadas()
        {
            _xInicial = X;
            _yInicial = Y;
            _mousePos = Input.mousePosition;
            _mousePos.x += _offsetScreen.x;
            _mousePos.y += _offsetScreen.y;

            _mousePos.x = ((_mousePos.x - _gui.WOffset) / _gui.WRatio) - _xInicial;
            _mousePos.y = (((Screen.height - _mousePos.y) - _gui.HOffset) / _gui.HRatio) - _yInicial;

        }

        /**
        *	@brief Permite retornar el botón a su posición inicial.
        *
        *	Este método permite regresar el botón a su posición inicial luego de haber sido arrastrado.
        */
        public void arrastrarBoton()
        {
            _xM = ((Input.mousePosition.x + _offsetScreen.x - _gui.WOffset) / _gui.WRatio) - _mousePos.x;
            _yM = ((Screen.height - Input.mousePosition.y + _offsetScreen.y - _gui.HOffset) / _gui.HRatio) - _mousePos.y;
            X = _xM;
            Y=_yM;

        }

        /**
        *	@brief Permite determinar si el mouse está sobre el area del botón
        *
        *	Este método permite saber si el puntero de mouse está sobre el area del botón
        *
        *	@return verdadero si el mouse está sobre el area del botón, de lo contrario retorna falso.
        */
        public bool IsOver()
        {

            Rect rctScroll = new Rect(((_offsetScreen.x + X) * _gui.WRatio) + _gui.WOffset, ((_offsetScreen.y + Y) * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio);

            if (Input.mousePosition.x + _offsetScreen.x > rctScroll.x && Input.mousePosition.x + _offsetScreen.x < (rctScroll.width + rctScroll.x))
            {
                if ((Screen.height - Input.mousePosition.y + _offsetScreen.y - 1) >= rctScroll.y && (Screen.height - Input.mousePosition.y + _offsetScreen.y - 1) <= (rctScroll.height + rctScroll.y))
                {
                    return true;
                }
            }

            return false;
        }

        /**
        *	@brief Método para asignar el color del texto.
        *
        *	Permite asignar el color de la fuente en el texto que muestra el botón, dicho color se toma en tres valores RGB (rojo, verde y azul).
        *
        *	@param r valor para el color rojo en RGB.
        *	@param g valor para el verde rojo en RGB.
        *	@param b valor para el azul rojo en RGB.
        */
        public void SetFontColor(float r, float g, float b)
        {
            _guiBot.hover.textColor = new Color(r, g, b);
            _guiBot.normal.textColor = new Color(r, g, b);
        }

        /**
        *	@brief Método que indica cuando el puntero del mouse se encuentra sobre el boton y se da click.
        *
        *	@param fn Evento disparado al dar clic sobre el boton.
        *
        *	@return El boton, sobre el cual se dio click.
        */
        public EthButton setMouseUp(OnClickEvent fn)
        {
            OnMouseUp += fn;
            return this;
        }


        /**
        *	@brief Método para modificar el evento al dar click.
        *
        *	@param Evento al dar click.
        *
        *	@return El boton, sobre el que se dio click.
        */
        public EthButton SetFunction(OnClickEvent fn)
        {
            OnClick = fn;
            return this;
        }


        /**
        *	@brief Método para modificar los eventos al dar click sobre el boton.
        *
        *	@param fn Evento al dar click en el boton.
        *
        *	@return El boton, sobre el que se dio click.
        */
        public EthButton SetFunction(OnClickObjEvent fn)
        {
            OnClickObj += fn;
            return this;
        }

        /**
        *	@brief Método para detectar el click sobre el botón.
        *
        *	Permite detectar el click sobre el botón y ejecutar las acciones consecuentes de ello.
        *
        *	@param launchFunc Determina si el click ejecuta alguna acción o no, por defecto determina que si.
		*	@param makeSound Determina si realiza un sonido o no, por defecto determina que si.
        */
        public virtual void Click(bool launchFunc = true, bool makeSound = true)
        {
            if (!launchFunc)
            {
                return;
            }

            if (_audioClic != null && makeSound)
            {
                EthAudio.GetInstance(null).PlayEffect(_audioClic);
            }

            if (OnClick != null)
            {
                OnClick(Name);
            }

            if (OnClickObj != null)
            {
                OnClickObj(Name, _obj);
            }
        }

        /**
        *	@brief Método que indica que un boton del mouse fue liberado.
        */
        public virtual void mouseUp()
        {
            if (OnMouseUp != null)
            {
                OnMouseUp(Name);
            }
        }

        /**
        *	@brief Metodo toString de la clase.
        *
        *	Retorna el nombre de la clase como una cadena de caracteres.
        *
        *	@return la clase representada en un string.
        */
        public override string ToString()
        {
            return "EthButton (" + Name + ")";
        }


        /**
        *	@brief permite remover el mouseup del botón.
        *	
        *	Permite remover el evento OnClick mouseUp del botón.
        */
        public void RemoveMouseUp()
        {
            OnMouseUp = null;
        }
    }
}
