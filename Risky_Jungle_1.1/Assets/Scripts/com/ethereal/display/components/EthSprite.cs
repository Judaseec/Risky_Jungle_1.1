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
    *   @class  EthSprite
    *   @brief  Esta clase se encarga de crear un sprite, el cual se usa como un personaje.
    *
    */
    public class EthSprite : EthComponent
    {

        /**
        *   @brief Estilo del EthSprite.
        */
        protected GUIStyle guiBot;

        /**
        *   @brief Textura del EthSprite.
        */
        EthTexture textura;

        /**
        *   @brief Nombre de la textura del EthSprite.
        */
        string nomTextura;

        /**
        *   @brief Frame actual en el que se encuentra el EthSprite.
        */
        public int frameAct;

        /**
        *   @brief Cantidad de frames de ancho que tiene el EthSprite.
        */
        public int framesAncho;

        /**
        *   @brief Cantidad de frames de alto que tiene el EthSprite.
        */
        public int framesAlto;

        /**
        *   @brief Cantidad total de frames que ocupa el EthSprite.
        */
        public int totalFrames;

        /**
        *   @brief Velocidad de movimiento del EthSprite.
        */
        public int velocidad = 3;

        /**
        *   @brief Intervalo de movimiento del EthSprite.
        */
        private int frameInt;

        /**
        *	@brief constructor de la clase EthSprite.
        *
        *	Este método permite crear una instancia de la clase EthSprite.
        *
        *	@param args 		Parametros con los cuales se creará el objeto.
        *	@param parentGUI 	EthComponentManager al cual pertenecerá el objeto.
        */
        public EthSprite(EthArguments args, EthComponentManager parentGUI)
            : base(args, parentGUI)
        {
            ReloadArguments(args);
        }

        /**
        *	@brief Método para recargar los argumentos del componente.
        *
        *	@param args Argumentos que tendrá el componente.
        */
        public override void ReloadArguments(EthArguments args)
        {

            EthComponentManager compThis = new EthComponentManager(null);
            compThis.HRatio = _gui.HRatio;
            compThis.WRatio = _gui.WRatio;

            frameAct = 0;
            frameInt = 0;

            if (args["hSprite"] != null)
            {
                Hei=float.Parse(args["hSprite"]);
            }
            if (args["wSprite"] != null)
            {
                Wid=float.Parse(args["wSprite"]);
            }

            args["x"] = "0";
            args["y"] = "0";
            textura = new EthTexture(args, compThis);

            framesAncho = Eth.ToInt(textura.Wid / Wid);
            framesAlto = Eth.ToInt(textura.Hei / Hei);
            totalFrames = framesAncho * framesAlto;
        }

        /**
        *	@brief Método para dibujar una EthSprite.
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

            frameInt++;

            if (frameInt / velocidad > 1)
            {
                frameInt = 0;
                frameAct++;

                if (frameAct >= totalFrames)
                {
                    frameAct = 0;
                }

                textura.X = (-(frameAct % framesAncho) * Wid);
                textura.Y = (-Eth.ToInt(frameAct / framesAncho) * Hei);

            }

            Rect groupRect = new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio);
            GUI.BeginGroup(groupRect);

            textura.Draw();

            GUI.EndGroup();
        }

        /**
        *	@brief Método ToString que retorna el nombre de la clase.
        *
        *	@return Nombre de la clase, EthSprite.
        */
        public override string ToString()
        {
            return "EthSprite (" + Name + ")";
        }
    }
}
