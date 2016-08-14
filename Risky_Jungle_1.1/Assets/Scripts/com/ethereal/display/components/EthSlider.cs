using UnityEngine;
using System;
using com.ethereal.display.components;
using Assets.Scripts.com.ethereal.util;

/*
Ejemplo de uso:
gui.AddSlider("sli",10,10,"img:barra,min:20,max:200");

img -> La ruta de la imagen a usar de barra, la misma pero con _bot al final aplica para el boton de arrastre
dir-> si es vertical entonces el slider sera vertical, de lo contrario sera horizontal
min-> este parametro representa el minimo valor de la barra
max-> este parametro representa el maximo valor de la barra
*/
namespace Assets.Scripts.com.ethereal.display.components
{

    /** 
    *   @author    EtherealGF <www.etherealgf.com> 
    *   @version   1.0 
    *   @date      Octubre 16 2014
    * 
    *   @class  EthSlider
    *   @brief  Esta clase se encarga de crear una slider para mostrar información al usuario.
    *
    */
    public class EthSlider : EthComponent
    {

        /**
        *	@brief Método para invocar el evento OnChange.
        */
        public delegate void OnChangeEvent(string name, float value);

        /**
        *   @brief Evento para detectar un cambio en el EthSlider.
        */
        public event OnChangeEvent OnChange;

        /**
        *   @brief Variable para definir si el EthSlider es vertical, de lo contrario es horizontal.
        */
        bool isVerticalSlider = false;

        /**
        *   @brief Textura que posee el EthSlider.
        */
        Texture2D textura;

        /**
        *   @brief Textura alta y baja que posee el EthSlider.
        */
        Texture2D texturaBot;

        /**
        *   @brief Textura alta que posee el EthSlider.
        */
        Texture2D texturaUp;

        /**
        *   @brief Textura baja que posee el EthSlider.
        */
        Texture2D texturaDown;

        /**
        *   @brief Valor actual del EthSlider.
        */
        float sliderValue = 0f;

        /**
        *   @brief VÚltimo vlor del EthSlider.
        */
        float lastValue = 0f;

        /**
        *   @brief Valor mínimo del EthSlider.
        */
        float minLimit = 0;

        /**
        *   @brief Valor máximo del EthSlider.
        */
        float maxLimit = 100;

        /**
        *	@brief constructor de la clase EthSlider.
        *
        *	este método permite crear una instancia de la clase EthSlider.
        *
        *	@param args parametros con los cuales se creará el objeto.
        *	@param parentGUI EthComponentManager al cual pertenecerá el objeto.
        */
        public EthSlider(EthArguments args, EthComponentManager parentGUI)
            : base(args, parentGUI)
        {

            if (args["dir"] == "vertical")
            {
                isVerticalSlider = true;
            }

            minLimit = Convert.ToInt32(Eth.GetVal(args["min"], 0));
            maxLimit = Convert.ToInt32(Eth.GetVal(args["max"], 100));

            if (args[Eth.IMG] != null)
            {
                string nomTextura = args[Eth.IMG];

                textura = Resources.Load(nomTextura) as Texture2D;
                texturaBot = Resources.Load(nomTextura + "_bot") as Texture2D;


                Wid=textura.width;
                Hei=textura.height;

                if (args["h"] != null)
                {
                    Hei=float.Parse(args["h"]);
                }
                if (args["w"] != null)
                {
                    Wid=float.Parse(args["w"]);
                }
            }
        }

        /**
        *	@brief Método para suscribirse al evento OnChange y retornar el objeto.
        *
        *	@return El objeto EthSlider.
        */
        public EthSlider SetFunction(OnChangeEvent fn)
        {
            OnChange += fn;
            return this;
        }

        /**
        *   @brief Método para modificar el valor de la variable sliderValue.
        *
        *   @param value Nuevo valor de la variable sliderValue.
        */
        public void setValue(float value)
        {
            sliderValue = value;
        }

        /**
        *   @brief Método para acceder a la variable privada sliderValue.
        *
        *   @return El valor de la variable sliderValue.
        */
        public float getValue()
        {
            return sliderValue;
        }

        /**
        *	@brief Método para dibujar una EthSlider.
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

            GUISkin customSkin = GUI.skin;

            if (isVerticalSlider)
            {
                if (textura != null)
                {
                    customSkin.verticalSlider.normal.background = textura;
                    customSkin.verticalSlider.fixedWidth = textura.width;
                }

                if (texturaBot)
                {
                    customSkin.verticalSliderThumb.normal.background = texturaBot;
                    customSkin.verticalSliderThumb.hover.background = texturaBot;
                    customSkin.verticalSliderThumb.active.background = texturaBot;
                    customSkin.verticalSliderThumb.fixedWidth = texturaBot.width;
                }

                sliderValue = GUI.VerticalSlider(new Rect(xTemp, yTemp, Wid, Hei), sliderValue, minLimit, maxLimit);
            }
            else
            {

                if (textura != null)
                {
                    customSkin.horizontalSlider.normal.background = textura;
                    customSkin.horizontalSlider.fixedHeight = textura.height * _gui.WRatio * 1.2f;//*0.85f;
                    customSkin.horizontalSlider.fixedWidth = textura.width * _gui.WRatio;//*gui.wRatio;				
                }

                if (texturaBot)
                {
                    customSkin.horizontalSliderThumb.normal.background = texturaBot;
                    customSkin.horizontalSliderThumb.hover.background = texturaBot;
                    customSkin.horizontalSliderThumb.active.background = texturaBot;
                    customSkin.horizontalSliderThumb.fixedHeight = textura.height * _gui.WRatio * 1.2f;//*0.80f;//*gui.hRatio;	
                    customSkin.horizontalSliderThumb.fixedWidth = texturaBot.width * _gui.WRatio;//*gui.wRatio;
                }
                else
                {
                    customSkin.horizontalSliderThumb = GUIStyle.none;
                }

                sliderValue = GUI.HorizontalSlider(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), sliderValue, minLimit, maxLimit);
            }

            if (sliderValue != lastValue)
            {
                FnChange();
            }
        }

        /**
        *	@brief Método para notificar un cambio en el slider.
        */
        public void FnChange()
        {
            lastValue = sliderValue;
            if (OnChange != null)
            {
                OnChange(Name, sliderValue);
            }
        }

        /**
        *	@brief Método ToString que retorna el nombre de la clase.
        *
        *	@return Nombre de la clase, EthSlider.
        */
        public override string ToString()
        {
            return "EthSlider (" + Name + ")";
        }
    }
}
