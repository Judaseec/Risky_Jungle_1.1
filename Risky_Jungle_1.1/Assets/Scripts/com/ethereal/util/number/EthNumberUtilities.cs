using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.com.ethereal.util.number
{

    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Julio 22 2014
    * 
    *	@class 	EthNumberUtilities
    *   @brief 	Esta clase esta encargada de diferentes utilidades referentes con los números. 
    *
    */
    public class EthNumberUtilities
    {
        /**
        *	@brief	Esta función sirve para pasar por ejemplo un 3 a 003, recibe el numero
        *	y la cantidad de letras que debe tener el string resultante.
        *
        *	@param num 			Número a ser modificado.
        *	@param charsnumber	Cantidad de letras que debe tener el string a ser retornado.
        */
        public static string GetNumberWithFill(int num, int charsnumber)
        {
            string strRet = "" + num;

            while (strRet.Length < charsnumber)
            {
                strRet = "0" + strRet;
            }

            return strRet;
        }
    }
}