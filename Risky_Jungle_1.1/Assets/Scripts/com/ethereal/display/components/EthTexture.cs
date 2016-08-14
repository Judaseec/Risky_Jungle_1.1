using UnityEngine;



using System;
using Assets.Scripts.com.ethereal.util;

/*
Ejemplo de uso:
											x  y
EthTexture textur = gui.AddTexture("Textu",10,10,"img:fondoDialog,w:500,h:300"); 

useLangImgs -> indica si para la imagen se usa lang o no (true o false), se utiliza en ese caso img_es o img_en
img->la imagen a mostrar
w-> Ancho del cuadro
h-> Alto del cuadro
*/

namespace Assets.Scripts.com.ethereal.display.components
{

    /** 
    *   @author    EtherealGF <www.etherealgf.com> 
    *   @version   1.0 
    *   @date      Octubre 16 2014
    * 
    *   @class  EthTexture
    *   @brief  Esta clase se encarga de crear una textura.
    *
    */
    public class EthTexture : EthComponent
    {

        /**
        *   @brief Estilo de la EthTextiure.
        */
        protected GUIStyle guiBot;

        
        private Texture2D _texture2D;

        /**
		*	@brief Asignación de las propiedades de lectura y escritura del atributo _texture2D.
		*/
        public Texture2D Texture2D
        {
            get { return _texture2D; }
            set { _texture2D = value; }
        }

        /**
        *   @brief Textura renderizada. 
        */
        RenderTexture texturaRender;

        /**
        *   @brief Nombre de la EthTexture.
        */
        string _textureName;

        /**
        *   @brief Textura flotante.
        */
        Texture2D texturaHover;

        /**
        *   @brief Vector que define el movimiento de la EthTexture.
        */
        Vector2 offset2;

        /**
        *   @brief Variable que define sí hay una bandera activa.
        */
        bool flag = true;


        private bool _detectHover = false;

        /**
		*	@brief Asignación de las propiedades de lectura y escritura del atributo _detectHover.
		*/
        public bool DetectHover
        {
            get { return _detectHover; }
            set { _detectHover = value; }
        }

        /**
		*	@brief constructor de la clase EthTexture.
		*
		*	Este método permite crear una instancia de la clase EthTexture.
		*
		*	@param args 		Parametros con los cuales se creará el objeto.
		*	@param parentGUI 	EthComponentManager al cual pertenecerá el objeto.
		*/
        public EthTexture(EthArguments args, EthComponentManager parentGUI)
            : base(args, parentGUI)
        {
            ReloadArguments(args);
        }

        /**
        *	@brief Método para recargar los argumentos (EthArguments) de la EthTexture.
        *
        *	@param args Nuevos argumentos para la EthTexture.
        */
        public override void ReloadArguments(EthArguments args)
        {
            if (args[Eth.IMG] != null)
            {
                _textureName = args[Eth.IMG];

                if (_useLangImgs)
                {
                    _lastLangUsed = EthLang.LangAct;
                    Texture2D = Resources.Load(_textureName + "_" + _lastLangUsed) as Texture2D;
                }
                else
                {
                    Texture2D = Resources.Load(_textureName) as Texture2D;
                    texturaHover = Resources.Load(_textureName + "_pressed") as Texture2D;
                }

                if (Texture2D == null)
                {
                    throw new System.ArgumentException("No se encuentra la imagen " + _textureName, Eth.IMG);
                }

                Wid=Texture2D.width;
                Hei=Texture2D.height;

                if (args["h"] != null)
                {
                    Hei=float.Parse(args["h"]);
                }
                if (args["w"] != null)
                {
                    Wid=float.Parse(args["w"]);
                }

                if (args["flipX"] != null && args["flipX"] == "true")
                {
                    Wid=Wid * -1;
                }

                if (args["flipY"] != null && args["flipY"] == "true")
                {
                    Hei=Hei * -1;
                }
            }

        }

        /**
        *	@brief Método para configurar la textura cruda.
        *
        *	@param args Propiedades de altura y anchor de la textura.
        *	@param text Texture2D con sus respectivos atributos.
        */
        public void SetRawTexture(EthArguments args, Texture2D text)
        {
            Texture2D = text;
            Wid=Texture2D.width;
            Hei=Texture2D.height;

            if (args["h"] != null)
            {
                Hei=float.Parse(args["h"]);
            }
            if (args["w"] != null)
            {
                Wid=float.Parse(args["w"]);
            }
        }

        /**
        *	@brief Método para configurar la textura renderizada.
        *
        *	@param args Propiedades de altura y anchor de la textura.
        *	@param text Texture2D con sus respectivos atributos.
        */
        public void SetRenderTexture(EthArguments args, RenderTexture text)
        {
            texturaRender = text;

            Wid=texturaRender.width;
            Hei=texturaRender.height;

            if (args["h"] != null)
            {
                Hei=float.Parse(args["h"]);
            }
            if (args["w"] != null)
            {
                Wid=float.Parse(args["w"]);
            }
        }

        /**
        *	@brief Método para dibujar una EthTexture.
        *
        *	@param offset Parámetro de tipo Vector2 para ser dibujado, el cual támbien puede ser nulo.
        *
        *	@see com.ethereal.display.components.EthComponent
        */
        public override void Draw(Vector2? offset = null)
        {

            if (!_visible)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0) && IsOver())
            {
                flag = false;
            }
            if (Input.GetMouseButtonUp(0))
            {
                flag = true;
            }

            offset2 = offset ?? Vector2.zero;
            float xTemp = X + offset2.x;
            float yTemp = Y + offset2.y;

            if (Texture2D != null && _useLangImgs && _lastLangUsed != EthLang.LangAct)
            {
                _lastLangUsed = EthLang.LangAct;
                Texture2D = Resources.Load(_textureName + "_" + _lastLangUsed) as Texture2D;
            }

            if (Texture2D != null)
            {

                if ((flag && Input.GetMouseButton(0) && IsOver() && texturaHover != null) || (DetectHover && IsOver()))
                {
                    GUI.DrawTexture(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), texturaHover);
                }
                else
                {
                    GUI.DrawTexture(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), Texture2D);
                }
            }
            else
            {
                if (texturaRender != null)
                {
                    GUI.DrawTexture(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), texturaRender);
                }
            }

        }

        /**
        *   @brief Método para modificar el valor de la variable _texture2D.
        *
        *   @param txt Nuevo valor de la variable _texture2D.
        */
        public void setTextureHover(Texture2D txt)
        {
            this.Texture2D = txt;
        }

        /**
        *	@brief Método para modificar la textura, y si se indica, redimencionarla.
        *
        *	@param textureName 	Nombre de la textura a modificar.	
        *	@param restore 		indíca si se desea redimencionar, si no se indíca este parametro se toma como true.
        */
        public void setTexture(String textureName, bool restore = true)
        {

            Texture2D texture = Resources.Load(textureName) as Texture2D;

            if (texture == null)
            {
                throw new System.ArgumentException("No se encuentra la imagen " + textureName, "img");
            }

            this.Texture2D = texture;

            if (restore)
            {
                Wid=texture.width;
                Hei=texture.height;
            }
        }

        /**
        *	@brief Método ToString que retorna el nombre de la clase.
        *
        *	@return Nombre de la clase, EthTexture.
        */
        public override string ToString()
        {
            return "EthTexture (" + Name + ")";
        }

        /**
        *	@brief Método que indica si el puntero se encuentra sobre la textura.
        */
        public bool IsOver()
        {

            Rect rctScroll = new Rect(((offset2.x + X) * _gui.WRatio) + _gui.WOffset, ((offset2.y + Y) * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio);

            if (Input.mousePosition.x + offset2.x > rctScroll.x && Input.mousePosition.x + offset2.x < (rctScroll.width + rctScroll.x))
            {
                if ((Screen.height - Input.mousePosition.y + offset2.y - 1) >= rctScroll.y && (Screen.height - Input.mousePosition.y + offset2.y - 1) <= (rctScroll.height + rctScroll.y))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
