using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.com.ethereal.display.components
{

    /** 
    *   @author    EtherealGF <www.etherealgf.com> 
    *   @version   1.0 
    *   @date      Octubre 16 2014
    * 
    *   @class  EthToggleButtonGroup
    *   @brief  Esta clase se encarga de albergar varios EthToggleButton.
    *
    */
    public class EthToggleButtonGroup
    {

        /**
        *   @brief Variable que define sí se puede quitar la selección.
        */
        private bool _canUnselect = false;

        /**
		*	@brief Asignación de las propiedades de lectura y escritura de la variable _canUnselect.
		*
		*	@return Valor de la variable _canUnselect.
		*/
        public bool CanUnselect
        {
            get
            {
                return _canUnselect;
            }
            set
            {
                _canUnselect = value;
            }
        }

        /**
        *   @brief Variable que define sí la lista de EthToggleButton empieza con un campo vacío.
        */
        private bool initEmpty = false;

        /**
        *   @brief Valor que define que EthToggleButton esta seleccionado, si no hay alguno seleccionado, su valor es -1.
        */
        private int togSelected = -1;

        /**
        *   @brief Lista que contiene todos los EthToggleButton pertenecientes a el EthToggleButtonGroup.
        */
        private List<EthToggleButton> togButtons;

        /**
        *	@brief Constructor de la clase EthToggleButtonGroup.
        *
        *	Este método permite crear una instancia de la clase EthToggleButtonGroup.
        *
        *	@param initEmpty Define el valor inicial de la variable initEmpty, si no se especifica dicho valor, por defecto es false.
        */
        public EthToggleButtonGroup(bool initEmpty = false)
        {
            togButtons = new List<EthToggleButton>();
            this.initEmpty = initEmpty;
        }

        /**
        *	@brief Método que agrega un EthToggleButton a el EthToggleButtonGroup.
        *
        *	@param togButton EthToggleButton a ser agregado.
        */
        public void AddToggleButton(EthToggleButton togButton)
        {
            togButtons.Add(togButton);
            togButton.setEthToggleButtonGroup(this);

            if (!initEmpty && !CanUnselect && togSelected == -1)
            {
                selectToggle(togButtons.Count - 1);
            }
        }

        /**
        *	@brief Método que dado un EthToggleButton, lo busca e identifica su ubicación para guardar dicho valor en la variable togSelected.
        *
        *	@param togPressed EthToggleButton a ser agregado.
        */
        public void reportClick(EthToggleButton togPressed)
        {
            for (int i = 0; i < togButtons.Count; i++)
            {
                if (togButtons[i] != togPressed)
                {
                    togButtons[i].unselect();
                }
                else
                {
                    togSelected = i;
                }
            }
        }

        /**
        *   @brief Método para acceder a la variable privada togSelected que identifica el EthToggleButton seleccionado.
        *
        *   @return El valor de la variable togSelected.
        */
        public int getSelectedIndex()
        {
            return togSelected;
        }

        /**
        *	@brief Método que dado el valor de la variable togSelected, busca el EthToggleButton correspondiente y lo retorna.
        *
        *	@return El EthToggleButton que actualmente está seleccionado o null si no hay un EthToggleButton seleccionado.
        */
        public EthToggleButton GetSelectedButton()
        {
            if (togSelected >= 0 && togSelected < togButtons.Count)
            {
                return togButtons[togSelected];
            }

            return null;
        }

        /**
        *	@brief Método que dado el índice de un EthToggleButton, si dicho índice existe lo retorna.
        *
        *	@return El EthToggleButton correspondiente al índice o null si no existe.
        */
        public EthToggleButton GetButton(int index)
        {
            if (index >= 0 && index < togButtons.Count)
            {
                return togButtons[index];
            }

            return null;
        }

        /**
        *	@brief Método que cuenta la cantidad de EthToggleButton en el EthToggleButtonGroup y lo retorna.
        *
        *	@return El número de EthToggleButton en el EthToggleButtonGroup.
        */
        public int Length()
        {
            return togButtons.Count;
        }

        /**
        *	@brief Método que selecciona un EthToggleButton dado si índice.
        *
        *	@param num Indice del EthToggleButton a seleccionar.
        */
        public void selectToggle(int num)
        {
            togSelected = num;
            togButtons[num].Click(true, false);
        }

        /**
        *   @brief Método para acceder a los EthToggleButton pertenecientes al EthToggleButtonGroup.
        *
        *   @return Lista con todos los EthToggleButton pertenecientes al EthToggleButtonGroup.
        */
        public List<EthToggleButton> getChildren()
        {
            return togButtons;
        }
    }
}
