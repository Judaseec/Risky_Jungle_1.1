using UnityEngine;

using System;

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

namespace Assets.Scripts.com.ethereal.display.components.ComponentAnimator
{

    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Octubre 16 2014
    * 
    *	@class 	EthComponentAnimator
    *   @brief 	Esta clase se encarga de realizar animaciones a los componentes.
    *
    */
    public class EthComponentAnimator
    {

        /**
        *	@brief Patrón Singleton para mantener la misma instancia de EthComponent en todo el juego.
        */
        protected EthComponent component;

        /**
        *	@brief Constructor de la clase EthComponentAnimator.
        *
        *	Este método permite crear una instancia de la clase EthComponentAnimator
        *
        */
        public EthComponentAnimator()
        {

        }

        /**
        *   @brief Método para modificar el componente a animar.
        *
        *   @param comp Componente a animar.
        */
        public virtual void SetComponent(EthComponent comp)
        {
            this.component = comp;
        }

        /**
        *	@brief Método para definir la animación del componente.
        *
        */
        public virtual void AnimateComponent()
        {

        }

        /**
        *	@brief Método para iniciar la animación del componente.
        *
        */
        public virtual void StartAnimation()
        {

        }

        /**
        *	@brief Método para revertir el tamaño en X del componente al original.
        *
        */
        public virtual void SetToOriginalSizeX()
        {

        }

        /**
        *	@brief Método para revertir el tamaño en Y del componente al original.
        *
        */
        public virtual void SetToOriginalSizeY()
        {

        }

        /**
        *	@brief Método ToString que retorna el nombre de la clase junto con el nombre del componente.
        *
        *	@return Nombre de la clase EthComponentAnimator seguido del nombre del componente.
        *
        */
        public override string ToString()
        {
            return string.Format("EthComponentAnimator ({0})", component);
        }
    }
}
