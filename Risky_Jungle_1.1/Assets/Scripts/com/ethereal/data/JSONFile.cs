using UnityEngine;
using Boomlagoon.JSON;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace com.ethereal.data.JSONFile
{

    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Julio 3 2014
    * 
    *	@class 	JSONFile
    *   @brief 	Esta clase se encarga de convertir un archivo de texto a JSONFile.
    */
    public class JSONFile
    {

        /**
        *	@brief	Metodo usado para convertir un archivo de texto a JSONFile.
        *	
        *	Obtiene un objeto JSONObject con la información cargada del archivo según la url indicada.
        *	Esta función usa la libreria Boomlagoon.JSON.
        *	
        *	@param url Url del archivo a cargar sin extension, esta comienza en la carpeta Resources de Unity, por ejemplo una url
        *	podría ser 'anims/samsung/animSamsung' para referirse a un archivo Resources/anims/samsumg/animSamsung.txt.
        *	
        *	@return JSONObject con la información del archivo.
        */
        public static JSONObjectBoom GetFile(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            TextAsset text = (TextAsset)Resources.Load(url, typeof(TextAsset));

            return JSONObjectBoom.Parse(text.text);
        }

        //guarda un archivo especificado en url con el contenido de str
        //solo funciona en el editor
        //ejemplo url "Assets/Resources/config/Levels.json"
        public static void WriteFile( string url, string str) {
            #if UNITY_EDITOR
                using (FileStream fs = new FileStream( url, FileMode.Create)){
                    using (StreamWriter writer = new StreamWriter(fs)){
                        writer.Write(str);
                    }
                }
            #endif
        }
    }
}
