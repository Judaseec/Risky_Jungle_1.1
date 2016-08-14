using UnityEngine;

namespace Assets.Scripts.com.ethereal.util
{
    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Julio 23 2014
    * 
    *	@class 	EthFileExist
    *   @brief 	Ésta clase esta encargada de la verificación de la existencia de los archivos.
    *
    */
    public class EthFileExist
    {

        /**
        *	@brief Método para Instanciar un EthFileExist.
        *	
        *	Para crear un EthFileExist no hay necesidad de parametros.
        */
        public EthFileExist()
        {

        }

        /**
        *	@brief	Método usado para verificar si un archivo existe o no.
        *	
        *	Éste metodo busca el archivo especificado por la url y lo carga en un TextAsset y si este es diferente de null es por que si existe.
        *	
        *	@param url Dirección especificada en donde se encuentra el archivo a verificar.
        *
        *	@return True si el archivo existe, de lo contrario false.
        */
        public static bool TextAssetExist(string url)
        {
            TextAsset text = (TextAsset)Resources.Load(url, typeof(TextAsset));

            if (text != null)
            {
                return true;
            }

            return false;
        }
    }
}