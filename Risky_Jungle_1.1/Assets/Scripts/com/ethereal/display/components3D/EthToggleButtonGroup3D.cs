using UnityEngine;
using System.Collections.Generic;
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
    public class EthToggleButtonGroup3D
    {
        /**
        *	@brief Variable que define sí los EthToogleButton pueden ser seleccionados.
        */
        private bool _canUnselect = false;

        /**
        *	@brief Asignación de las propiedades de lectura y escritura de la variable _canUnselect.
        *	
        *   @return El valor de la variable _canUnselect.
        */
        public bool CanUnselect
        {
            get { return _canUnselect; }
            set { _canUnselect = value; }
        }

        /**
        *	@brief Variable que adquiere el valor del índex del EthToggleButton3D que esté seleccionado.
        */
        private int _togSelected = -1;

        /**
        *	@brief Lista o conjunto de EthToggleButton3D .
        */
        private List<EthToggleButton3D> _togButtons;

        /**
        *   @brief Constructor de la clase EthToggleButtonGroup3D.
        *
        *   Este método permite crear una instancia de la clase EthToggleButtonGroup3D.
        *	Se inicializa el la lista de EthToggleButton3D
        */
        public EthToggleButtonGroup3D()
        {
            _togButtons = new List<EthToggleButton3D>();
        }

        /**
        *	@brief Método para agregar un ToggleButton 3D al grupo.
        *
        *	@param togButton ToggleButton 3D a ser agregado.
        */
        public void AddToggleButton(EthToggleButton3D togButton)
        {
            _togButtons.Add(togButton);
            togButton.setEthToggleButtonGroup(this);

            if (!_canUnselect && _togSelected == -1)
            {
                SelectToggle(0);
            }
        }

        /**
        *	@brief Método para deseleccionar el boton del grupo que ha sido presionado 
        *	el cual llega por parámetro.
        *
        *	@param togPressed ToggleButton 3D que ha sido presionado y se desea deselecionar.
        */
        public void ReportClick(EthToggleButton3D togPressed)
        {
            for (int i = 0; i < _togButtons.Count; i++)
            {
                if (_togButtons[i] != togPressed)
                {
                    _togButtons[i].unselect();
                }
            }
        }

        /**
        *	@brief Método para Indicar el numero de ToggleButton3D que ha sido presionado para buscarlo 
        *	en el arreglo y ejecutar el método click de este.
        *
        *	@param togPressed ToggleButton 3D que ha sido presionado y se desea deselecionar.
        */
        public void SelectToggle(int num)
        {
            _togSelected = num;
            _togButtons[num]._click();
        }
    }
}
