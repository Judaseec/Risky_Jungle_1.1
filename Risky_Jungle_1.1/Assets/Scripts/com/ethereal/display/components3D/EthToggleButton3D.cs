using UnityEngine;



using System;

namespace Assets.Scripts.com.ethereal.display.components3D
{

    /** 
    *   @author    EtherealGF <www.etherealgf.com> 
    *   @version   1.0 
    *   @date      Abril 13 2015
    * 
    *   @class  EthButton3D
    *   @brief  Esta clase esta encargada de la configuracion del Los botones de palanca en 3D.
    *	
    *	Este boton hereda de EthButton3D
    */
    public class EthToggleButton3D : EthButton3D
    {

        //true -> pressed, false -> unpressed
        /**
        *   @brief Estado que indica si el botÃ³n esta presionado o no.
        */
        public bool state = false;

        /**
        *	@brief Instancia de la clase EthToggleButtonGroup3D.
        */
        private EthToggleButtonGroup3D togGroup;

        /**
        *   @brief Constructor de la clase EthToggleButton3D.
        *
        *   Este mÃ©todo permite crear una instancia de la clase EthToggleButton3D.
        *
        */
        public EthToggleButton3D()
            : base()
        {
        }

        /**
        *   @brief MÃ©todo que modifica la textura demas caracteristicas dependiendo de si el boton a sido presionado.
        *
        */
        public void _click()
        {

            if (state && togGroup != null && !togGroup.CanUnselect)
            {
                return;
            }

            state = !state;
            click();

            if (state)
            {
                SetTexture("hover");
            }
            else
            {
                SetTexture("normal");
            }

            if (togGroup != null)
            {
                togGroup.ReportClick(this);
            }
        }

        /**
        *   @brief Cuando el boton a sido soltado se modifican las texturas y el estado del boton asi como 
        *	caracteristicas de la posiciÃ³n.
        *
        */
        public override void _EthOnMouseUp()
        {
            if (clickState && !ignoreClicks)
            {
                clickState = false;
                EthOnMouseUp();
                if (!state)
                {
                    SetTexture("normal");
                }
                else
                {
                    SetTexture("hover");
                }
            }

            if (scroll != null)
            {
                scroll.SetNewPositionY(Input.mousePosition.y - lastPosY);
            }
        }

        /**
        *	@brief MÃ©todo para establecer el evento a suceder al cabo de ser presionado el botÃ³n.
        *
        *	Este mÃ©todo permite obtener el metodo _EthOnMouseDown de la clase padre.
        */
        public void OnMouseDown()
        {
            _EthOnMouseDown();
        }

        /**
        *	@brief MÃ©todo para establecer lo que debe suceder al arrastrar el mouse.
        *	Este mÃ©todo permite obtener el metodo _EthOnMouseDrag de la clase padre.
        */
        public void OnMouseDrag()
        {
            _EthOnMouseDrag();
        }

        /**
        *	@brief MÃ©todo para llamar a la funciÃ³n que se debe ejecutar cuando se presiona un botÃ³n o el mouse actua como botÃ³n.
        *
        *	Este mÃ©todo permite obtener el metodo _EthOnMouseUpAsButton de la clase padre.
        */
        public void OnMouseUpAsButton()
        {
            _EthOnMouseUpAsButton();
        }

        /**
        *	@brief Cuando se suelta el mouse sobre un boton se renderiza este boton en su estado despresionado.
        *
        */
        public override void _EthOnMouseUpAsButton()
        {
            if (clickState && !ignoreClicks)
            {
                clickState = false;
                _EthOnMouseUpAsButton();
                if (gameObject.GetComponent<Renderer>().enabled)
                {
                    _click();
                }
            }
        }

        /**
        *	@brief Metodo para dejar el boton deseleccionado.
        */
        public virtual void unselect()
        {
            state = false;
            SetTexture("normal");
        }

        /**
        *	@brief Metodo para asignar a la variable EthToggleButtonGroup3D el valor de este tipo que lleva por parÃ¡metro.
        */
        public void setEthToggleButtonGroup(EthToggleButtonGroup3D togGroup)
        {
            this.togGroup = togGroup;
        }

        /**
        *   @brief  Sobreescritura del mÃ©todo toString().
        *   
        *   MÃ©todo encargado de representar la clase en forma de texto de una manera coherente. 
        *   
        *   @return Cadena de caracteres representando la clase actual.
        */
        public override string ToString()
        {
            return "EthToggleButton3D (" + name + ")";
        }
    }
}
