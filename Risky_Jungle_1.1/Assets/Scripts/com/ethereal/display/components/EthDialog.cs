using System;
using System.Collections.Generic;



using UnityEngine;
using Assets.Scripts.com.ethereal.util;
using com.ethereal.display.components;

/* Ejemplo de uso:  EthDialog dial = gui.AddDialog("win","Este es el texto que sale","img:fondoDialog,bot:ok;cancel,w:400,h:300,font:fonts/Bayday,fontColor:1_1_1,fontSize:16,fontBtn:fonts/Avenger,fontColorBtn:1_1_1,fontSizeBtn:20"); dial.setFunction(clickDialog);  void clickDialog(string bot) {     Debug.Log("el texto del boton presionado es: " + bot); }  img->la imagen de fondo para el dialog, la cortinilla negra sale por defecto por ahora pero esta imagen es el fondo del cuadro imgButton -> es la imagen a usar para los botones, si no se especifica se usa [img]+"_button" img[nombreBoton] -> por ejemplo si tengo un boton ok y especifico el parametro imgok entonces se usa esa como textura del boton bot->los nombres de los botones que van a salir separados por ; si ese parametro se omite se coloca solo un boton de ok, si se coloca un nobots entonces no se colocan botones      usualmente para utilizar getGUI y agregar otras cosas.	 useLang->Indica si se debe usar lang para el texto del dialog, si no se recibe es true useLangBtn -> Indica si se debe usar lang para los botones, si no se recibe es true w-> Ancho del cuadro h-> Alto del cuadro font-> Fuente a usar en el texto del cuadro fontColor-> color de fuente a usar en el texto del cuadro fontSize-> tamaño de fuente a usar en el texto del cuadro fontBtn-> Fuente a usar en los botones del dialog fontColorBtn-> color de fuente a usar en los botones del dialog fontSizeBtn-> tamaño de fuente a usar en los botones del dialog offsetText-> el offset para el texto, por ejemplo 0_20 significa bajarlo 20 en y y en x dejarlo igual */
namespace Assets.Scripts.com.ethereal.display.components
{

    /** 
    *   @author    EtherealGF <www.etherealgf.com> 
    *   @version   1.0 
    *   @date      Octubre 16 2014
    * 
    *   @class  EthDialog
    *   @brief  Esta clase se encarga de crear una ventana de diálogo.
    *
    */
    public class EthDialog : EthComponent
    {

        /**
        *   @brief EthComponentManager al que pertenece el EthDialog.
        */
        new public EthComponentManager gui;

        /**
        *   @brief Evento para detectar cuando se hace click.
        */
        public delegate void OnClickEvent(string name);

        /**
        *   @brief Evento para detectar el click sobre el EthDialog.
        */
        public event OnClickEvent OnClick;

        /**
        *   @brief  Estilo de la ventana. Permite manejar el elemento en pantalla y controlar sus opciones de visualización.
        */
        protected GUIStyle guiWin;

        /**
        *   @brief  Estilo del label. Permite manejar el elemento en pantalla y controlar sus opciones de visualización.
        */
        protected GUIStyle guiLabel;

        /**
        *   @brief  Texto que mostrará la ventana de diálogo.
        */
        public string text;

        /**
        *   @brief  Ventana modal sobre la cual aparecerá el diálogo.
        */
        EthModalWindow modal;

        /**
        *   @brief  Grupo de botones que tendrá el diálogo.
        */
        private EthToggleButtonGroup group;

        /**
        *   @brief  Variable que define sí los botones estarán animados.
        */
        public bool animateButtons = false;

        /**
        *   @brief  Diccionario que contiene los diálogos por defecto.
        */
        public static Dictionary<string, string> defaultsDialogs = new Dictionary<string, string>();

        /**
        *   @brief  Diálogo por defecto, está inclusive antes de llenar el diccionario.
        */
        public static string defaultDialogData;

        /**         *	@brief Constructor de la clase EthDialog.         *         *	Este método permite crear una instancia de la clase EthDialog.         *         *	@param args 		Argumentos del EthDialog.         *	@param parentGUI	Contenedor del EthDialog.         *         */
        public EthDialog(EthArguments args, EthComponentManager parentGUI)
            : base(args, parentGUI)
        {

            gui = new EthComponentManager(parentGUI);

            if (args["hasBackground"] == "false")
            {
                modal = gui.AddModalWindow("win", false, "", false);
            }
            else
            {

                if (args["backImg"] != null)
                {
                    modal = gui.AddModalWindow("win", true, args["backImg"]);
                }
                else
                {
                    modal = gui.AddModalWindow("win");
                }
            }
            guiWin = new GUIStyle();
            guiLabel = new GUIStyle();

            text = "";

            if (args["font"] != null)
            {
                guiLabel.font = Resources.Load(args["font"]) as Font;
            }

            if (args["fontColor"] != null)
            {
                string[] colorBot = args["fontColor"].Split('_');
                guiLabel.normal.textColor = new Color(float.Parse(colorBot[0]), float.Parse(colorBot[1]), float.Parse(colorBot[2]));
                guiLabel.hover.textColor = new Color(float.Parse(colorBot[0]), float.Parse(colorBot[1]), float.Parse(colorBot[2]));
            }

            if (args["fontSize"] != null)
            {
                guiLabel.fontSize = int.Parse(args["fontSize"]);
            }

            if (args["text"] != null)
            {
                text = args["text"];
            }

            Texture2D textTemp = Resources.Load(args["img"]) as Texture2D;

            float wid = float.Parse((string)Eth.GetVal(args["w"], "" + textTemp.width));
            float hei = float.Parse((string)Eth.GetVal(args["h"], "" + textTemp.height));

            guiLabel.wordWrap = true;

            float xVent = (gui.ObjectWidth - (wid)) / 2;
            float yVent = (gui.ObjectHeight - (hei)) / 2;

            //Que tanto espacio ocupa el label dentro del cuadro
            float margenX = 0.7f;
            float margenY = 0.8f;

            float widTemp = wid * gui.WRatio;
            float heiTemp = hei * gui.WRatio;

            float offsetButtonsY = float.Parse((string)Eth.GetVal(args["offsetButtonsY"], "0"));
            float offsetButtonsX = float.Parse((string)Eth.GetVal(args["offsetButtonsX"], "0"));

            modal.gui.AddTexture("Textu", xVent, yVent, "img:" + args["img"] + ",w:" + (widTemp / gui.WRatio) + ",h:" + (heiTemp / gui.HRatio));

            Vector2 offsetText = new Vector2(0, 0);

            if (args["offsetText"] != null)
            {
                string[] offsetStr = args["offsetText"].Split('_');
                offsetText = new Vector2(float.Parse(offsetStr[0]), float.Parse(offsetStr[1]));
            }

            EthLabel lab = modal.gui.AddLabel("Textu", xVent + (((((widTemp / gui.WRatio) - (margenX * widTemp / gui.WRatio)) / 2) + offsetText.x)), yVent + offsetText.y, "w:"
                + (margenX * widTemp / gui.WRatio) + ",h:" + ((heiTemp / gui.HRatio) - ((heiTemp / gui.HRatio) * (1 - margenY))) + ",wrap:true,useLang:" + ("" + _useLang).ToLower()
                + "," + args.GetArgumentsAsStr("font,fontColor,fontSize"));
            lab.Text = text;

            args["bot"] = (string)Eth.GetVal(args["bot"], "ok");

            if (args["bot"] == "nobots")
            {
                return;
            }

            string[] buttons = args["bot"].Split(';');

            args["imgButton"] = (string)Eth.GetVal(args["imgButton"], args["img"] + "_button");

            Texture textureBtn = Resources.Load(args["imgButton"]) as Texture2D;
            if (textureBtn != null)
            {
                args["widBtn"] = (string)Eth.GetVal(args["widBtn"], "" + textureBtn.width);
            }
            else
            {
                args["widBtn"] = args["widBtn"];
            }

            args["sepBtn"] = (string)Eth.GetVal(args["sepBtn"], "" + float.Parse(args["widBtn"]) * 0.1f);

            int cantButtons = buttons.Length;

            if (textureBtn != null)
            {
                args["heiBtn"] = (string)Eth.GetVal(args["heiBtn"], "" + textureBtn.height);
            }

            float heiButton = float.Parse(args["heiBtn"]);
            float widButton = float.Parse(args["widBtn"]);
            float sepButton = float.Parse(args["sepBtn"]);

            float posXFirstBot = (((widTemp / gui.WRatio) - ((cantButtons * widButton) + ((cantButtons - 1) * sepButton))) / 2);

            string useLangBtn = (string)Eth.GetVal(args["useLangBtn"], "true");

            group = new EthToggleButtonGroup(true);
            group.CanUnselect = false;

            for (int i = 0; i < cantButtons; i++)
            {
                string imgButAct = (string)Eth.GetVal(args["img" + buttons[i]], args["imgButton"], "");
                string textBot = buttons[i];

                if (args["useBotText"] == "false")
                {
                    textBot = "";
                }

                EthToggleButton botTemp = modal.gui.AddToggleButton(buttons[i], xVent + posXFirstBot + ((i * widButton) + (i * sepButton)) + offsetButtonsX, yVent + (heiTemp / gui.HRatio)
                    - ((heiTemp / gui.HRatio) * (1 - margenY)) - (heiButton / 2) + offsetButtonsY, "img:" + imgButAct + ",text:" + textBot + ",w:" + (widButton) + ",h:" + (heiButton)
                    + ",wrap:true,useLang:" + useLangBtn + "," + args.GetArgumentsAsStr("fontBtn,fontColorBtn,fontSizeBtn,offsetTextBtn", "font,fontColor,fontSize,offset"));
                if (animateButtons)
                {
                    botTemp.CanAnimate = true;
                    botTemp.AddAnimator(new StretchAnimator(10, 50));
                    botTemp.AddAnimator(new StretchAnimator(-10, 50));
                }
                botTemp.SetFunction(clickBtn);
                group.AddToggleButton(botTemp);
            }

            if (Application.platform == EthVitaInput.vitaRuntime)
            {
                group.GetButton(0).Click(false);
            }

        }

        /**         *	@brief Este método retorna el EthComponentManager correspondiente a la GUI del modal.         *         *	@return EthComponentManager correspondiente a la GUI del modal.         */
        public EthComponentManager getGUI()
        {
            return modal.gui;
        }

        /**         *	@brief Este método retorna la ventana modál (modal).         *         *	@return La ventana modál (EthModalWindow).         */
        public EthModalWindow getModalWindow()
        {
            return modal;
        }

        /**         *	@brief Este método remueve la ventana modal y agrega el evento del click.         *         *	@param str Cadena para registrar el evento.         */
        public void clickBtn(string str)
        {
            gui.RemoveComponent(modal);
            Remove();
            if (OnClick != null)
            {
                OnClick(str);
            }
        }

        /**         *	@brief Este método agrega el vento de un click a OnClick.         *         *	@param str Evento a agregar.         */
        public void setFunction(OnClickEvent fn)
        {
            OnClick += fn;
        }

        /**         *	@brief Método para dibujar un EthDialog.         *         *	@param offset Parámetro de tipo Vector2 para ser dibujado, el cual támbien puede ser nulo.         *         *	@see com.ethereal.display.components.EthComponent         */
        public override void Draw(Vector2? offset = null)
        {
            gui.Draw(offset);

            if (Application.platform == EthVitaInput.vitaRuntime)
            {
                if (EthVitaInput.KEY_RELEASE[EthVitaInput.KEY_X] && !EthVitaInput.KEY_USED[EthVitaInput.KEY_X])
                {
                    EthVitaInput.SetKeyUsed(EthVitaInput.KEY_X);
                    clickBtn(group.GetSelectedButton().Name);
                }
                else
                {
                    EthVitaInput.NavigateGroup(group, 0, 80, EthVitaInput.VITA_INPUT_TIME);
                }
            }
        }

        /**         *	@brief Método ToString que retorna el nombre de la clase.         *         *	@return Nombre de la clase, EthDialog.         */
        public override string ToString()
        {
            return string.Format("EthDialog ({0})", Name);
        }

        /**         *	@brief Este método modifica la variable defaultDialogData.         *         *	@param strParam Nuevo valor d ela variable defaultDialogData.         */
        public static void setDefaultData(string strParam)
        {
            defaultDialogData = strParam;
        }

        /**         *	@brief Método para agregar un diálogo por defecto al diccionario.         *         *	@param name	Llave para agregar al diccionario.         *	@param par 	Valor a agregar al diccionario.         */
        public static void addDefaultDialog(string name, string par)
        {
            defaultsDialogs[name] = par;
        }

        /**         *	@brief Método para usar un diálogo, utilizando el diálogo por defecto agregando diálogos almacenados y texto extra.         *         *	Este método recibe un EthComponentManager, la llave para buscar en el diccionario y el texto, lo que hace es llamar al         *	método con el mismo nombre pero recibe un string adicional, el cual se manda vacío.         *         *	@param gui 	EthComponentManager donde será mostrado el diálogo.         *	@param name Llave para buscar los diálogos almacenados.         *	@param text Texto que será mostrado en el diálogo.         *         *	@return EthDialog con los atributos asignados.         */
        public static EthDialog useDialog(EthComponentManager gui, string name, string text)
        {
            return useDialog(gui, name, text, "");
        }

        /**         *	@brief Método para usar un diálogo, utilizando el diálogo por defecto agregando diálogos almacenados y texto extra.         *         *	@param gui 		EthComponentManager donde será mostrado el diálogo.         *	@param name 	Llave para buscar los diálogos almacenados.         *	@param text 	Texto que será mostrado en el diálogo.         *	@param addParam	Parametro adicional.         *         *	@return EthDialog con los atributos asignados.         */
        public static EthDialog useDialog(EthComponentManager gui, string name, string text, string addParam)
        {
            string strUse = defaultDialogData;
            strUse += "," + defaultsDialogs[name];
            strUse += "," + addParam;
            return gui.AddDialog(name, text, strUse);
        }

    }
}
