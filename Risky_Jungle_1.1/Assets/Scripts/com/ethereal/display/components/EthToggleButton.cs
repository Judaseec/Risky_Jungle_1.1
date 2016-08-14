using UnityEngine;



using System;
using Assets.Scripts.com.ethereal.util;

/*
Ejemplo de uso:

EthToggleButtonGroup togGr = new EthToggleButtonGroup();

EthToggleButton tog = gui.AddToggleButton("nomTog0",0,0,"text:Hola,font:fonts/Avenger,fontColor:0_0_0,fontSize:18,useLang:true");
EthToggleButton tog1 = gui.AddToggleButton("nomTog1",0,20,"text:Hola2,font:fonts/Avenger,fontColor:0_0_0,fontSize:18,useLang:true");

togGr.addToggleButton(tog);
togGr.addToggleButton(tog1);


h -> si se desea especificar un alto definido
w -> si se desea especificar un ancho definido
font -> La fuente a usar
fontColor -> El color de la fuente a usar separado por _, por ejemplo para el blanco 1_1_1
fontSize -> El tamaño de la fuente a usar
useLang -> Indica si pasa por cambio debido a lenguaje o no.

*/

namespace Assets.Scripts.com.ethereal.display.components
{

    /** 
    *   @author    EtherealGF <www.etherealgf.com> 
    *   @version   1.0 
    *   @date      Octubre 16 2014
    * 
    *   @class  EthToggleButton
    *   @brief  Esta clase se encarga de crear un Toggle Button, el cual se usa principalmente en los sub-menús.
    *
    */
    public class EthToggleButton : EthButton
    {

        /**
        *   @brief Variable que define sí el EthToggleButton está presionado.
        */
        public bool state = false;

        /**
        *   @brief Textura por defecto del EthToggleButton.
        */
        private Texture2D backUpNormalTexture;

        /**
        *   @brief Color por defecto del EthToggleButton.
        */
        private Color backUpNormalColor;

        /**
        *   @brief Imagen apfa del EthToggleButton.
        */
        private Texture2D imgAlpha;

        /**
        *   @brief Imagen flotante del EthToggleButton.
        */
        private Texture2D imgHover;

        /**
        *   @brief EthToggleButtonGroup al que pertenece el EthToggleButton.
        */
        private EthToggleButtonGroup togGroup;

        /**
        *	@brief constructor de la clase EthToggleButton.
        *
        *	Este método permite crear una instancia de la clase EthToggleButton.
        *
        *	@param args 		Parametros con los cuales se creará el objeto.
        *	@param parentGUI 	EthComponentManager al cual pertenecerá el objeto.
        */
        public EthToggleButton(EthArguments args, EthComponentManager parentGUI)
            : base(args, parentGUI)
        {
            CreateTextureBackUp();
            if (args["alphaHover"] != null)
            {
                _changeAlfa = bool.Parse(args["alphaHover"]);
                imgHover = _guiBot.hover.background;
                imgAlpha = Resources.Load(_nomTextura + "_pressed_alfa") as Texture2D;
                _guiBot.hover.background = imgAlpha;
            }

        }

        /**
        *	@brief Método para modificar la textura del EthToggleButton, y si se indica, redimencionarla.
        *
        *	@param textureName 	Nombre de la textura.	
        *	@param restore 		indíca si se desea redimencionar, si no se indíca este parametro se toma como true.
        */
        public virtual void SetTexture(String nomTextura, bool restore = true)
        {
            base.SetTexture(nomTextura, restore);
            imgHover = _guiBot.hover.background;
            imgAlpha = Resources.Load(nomTextura + "_pressed_alfa") as Texture2D;
            _guiBot.hover.background = imgAlpha;
            CreateTextureBackUp();
        }

        /**
        *	@brief Método que crea un backup de las caracteísticas de la textura del EthToggleButton.
        */
        private void CreateTextureBackUp()
        {

            backUpNormalTexture = _guiBot.normal.background;
            backUpNormalColor = _guiBot.normal.textColor;
        }

        /**
        *	@brief Método Para reportar un click sobre el EthToggleButton y realizar las correspondientes acciones.
        *
        *	@param launchFunc Determina si el click ejecuta alguna acción o no, por defecto determina que si.
		*	@param makeSound Determina si realiza un sonido o no, por defecto determina que si.
        */
        public override void Click(bool launchFunc, bool makeSound = true)
        {

            if ((state && togGroup != null && !togGroup.CanUnselect) || !Enable)
            {
                return;
            }

            state = !state;
            if (togGroup != null)
            {
                togGroup.reportClick(this);
            }

            if (state)
            {
                if (imgHover != null)
                {
                    _guiBot.hover.background = imgHover;
                }
                _guiBot.normal = _guiBot.hover;

                if (_useValueHovers)
                {
                    _guiBot.contentOffset = _offHover;
                    _guiBot.alignment = _alignHover;
                }
                base.Click(launchFunc, makeSound);
            }
            else
            {
                if (imgAlpha != null)
                {
                    _guiBot.hover.background = imgAlpha;
                }

                _guiBot.normal.background = backUpNormalTexture;
                _guiBot.normal.textColor = backUpNormalColor;
                _guiBot.contentOffset = _off;
                _guiBot.alignment = _alignNormal;
                base.Click(launchFunc, makeSound);
            }

        }

        /**
        *	@brief Método para dibujar una EthToggleButton.
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

            Vector2 offset2 = offset ?? Vector2.zero;
            float xTemp = X + offset2.x;
            float yTemp = Y + offset2.y;


            if (_useValueHovers && !Eth.IsMobile())
            {
                if (!state)
                {
                    _guiBot.contentOffset = _off;
                }
                if (!state)
                {
                    _guiBot.alignment = _alignNormal;
                }

                if (isHover(xTemp, yTemp))
                {
                    _guiBot.contentOffset = _offHover;
                    _guiBot.alignment = _alignHover;

                }
            }
            if (isHover(xTemp, yTemp))
            {

                if (!state && _changeAlfa)
                {

                    if (imgAlpha != null)
                    {
                        _guiBot.hover.background = (Texture2D)Eth.GetVal(imgAlpha, _guiBot.normal.background);
                    }
                }
                else
                {

                    if (imgAlpha != null && imgHover != null)
                    {
                        _guiBot.hover.background = imgHover;
                    }
                }
            }

            string textTemp = "";

            if (Text != "")
            {
                textTemp = EthLang.GetEntry(Text, _useLang);
            }

            if (GUI.Button(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), textTemp, _guiBot))
            {
                Click();
            }

        }

        /**
        *	@brief Método que analiza si un EthToggleButton es activable.
        *
        *	@param xTemp Coordenada temporal en x.
        *	@param yTemp Coordenada temporal en y.
        */
        public bool isHover(float xTemp, float yTemp)
        {
            Rect rctScroll = new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio);
            if (Input.mousePosition.x > rctScroll.x && Input.mousePosition.x < (rctScroll.width + rctScroll.x))
            {
                if ((Screen.height - Input.mousePosition.y) > rctScroll.y && (Screen.height - Input.mousePosition.y) < (rctScroll.height + rctScroll.y))
                {
                    return true;
                }
            }
            return false;
        }

        /**
        *	@brief Método que quita la animación de seleccion a un EthToggleButton.
        */
        public void unselect()
        {
            state = false;

            if (_useValueHovers)
            {
                if (!state)
                {
                    _guiBot.contentOffset = _off;
                }
                if (!state)
                {
                    _guiBot.alignment = _alignNormal;
                }
            }

            _guiBot.normal.background = backUpNormalTexture;
            _guiBot.normal.textColor = backUpNormalColor;
        }

        /**
        *   @brief Método para modificar el EthToggleButtonGroup al que pertenece el EthToggleButton.
        *
        *   @param togGroup Nuevo EthToggleButtonGroup del EthToggleButton
        */
        public void setEthToggleButtonGroup(EthToggleButtonGroup togGroup)
        {
            this.togGroup = togGroup;
        }

        /**
        *	@brief Método ToString que retorna el nombre de la clase y el nombre del objeto.
        *
        *	@return Nombre de la clase concatenado con el nombre del objeto.
        */
        public override string ToString()
        {
            return string.Format("EthToggleButton ({0})", Name);
        }
    }
}
