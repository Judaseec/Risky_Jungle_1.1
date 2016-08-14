using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.com.ethereal.util
{
    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Septiembre 5 2014
    * 
    *	@class 	URLLoader
    *   @brief 	Clase encargada de manejar las peticiones http de manera asincrona con respecto al ciclo normal del juego
    *
    */
    class URLLoader
    {
        /**
        *	@brief Clase MonoBehaviour padre.
        */
        private MonoBehaviour parent;

        /**
        *	@brief Método para Instanciar un URLLoader.
        *	
        *	AL crear un URLLoader se requiere referenciar un MonoBehaviour padre a esta clase desde la cual se llamara a esta.
        *
        *	@param refParent	MonoBehaviour que sera referenciado como padre de esta clase.
        *
        */
        public URLLoader(MonoBehaviour refParent)
        {
            parent = refParent;
        }

        /**
        *	@brief Método usado para obtener una petición http.
        *	
        *	Se recibe por parametro una url de la que se espera una petición http.
        *
        *	@param url Página web de la que se espera una petición http.
        *
        *	@return Retorna un object WWW el cual puede poseer datos de un servidor web asi como lo son las listas de mejores puntuaciones.
        */
        public WWW GET(string url)
        {
            WWW www = new WWW(url);
            parent.StartCoroutine(WaitForRequest(www));
            return www;
        }

        /**
        *	@brief Método usado para enviar una petición http por GET.
        *	
        *	Se recibe por parametro una url a la que se envia una petición http, eniando por medio del diccionario de datos todos los atributos
        *	que se necesitan enviar.
        *
        *	@param url Página web a la que se envia una petición http.
        *
        *	@return Retorna un object WWW el cual puede poseer datos de un servidor web.
        */
        public WWW GET(string url, Dictionary<string, string> getData)
        {
            if (getData != null)
            {
                url = url + "?";
            }

            foreach (KeyValuePair<String, String> get_arg in getData)
            {
                url = url + get_arg.Key + "=" + get_arg.Value + "&";
            }

            Debug.Log("envia " + url);
            WWW www = new WWW(url);
            parent.StartCoroutine(WaitForRequest(www));
            return www;

        }


        /**
        *	@brief Método usado para enviar una petición http por POST.
        *	
        *	Se recibe por parametro una url a la que se envia una petición http, se utiliza un formulario en el cual se agregan todos los 
        *	datos requeridos encontrados en el diccionario de datos que entra por parámetro.
        *
        *	@param url Página web a la que se envia una petición http.
        *
        *	@return Retorna un object WWW el cual puede poseer datos de un servidor web.
        */
        public WWW POST(string url, Dictionary<string, string> post)
        {

            WWWForm form = new WWWForm();
            foreach (KeyValuePair<String, String> post_arg in post)
            {
                form.AddField(post_arg.Key, post_arg.Value);
            }

            WWW www = new WWW(url, form);

            parent.StartCoroutine(WaitForRequest(www));
            return www;
        }

        /**
        *	@brief Método usado para esperar una petición http.
        *	
        *	Espera las peticiones http sin acaparar la CPU por medio del uso de la interfaz Ienumerator y al retornar el tipo de retorno 
        *	especial yield, permite que la próxima vez que se llame al método continue despues de la linea de código del retorno especial. 
        *
        *	@param www Datos obtenidos de un servidor web.
        *
        *	@return Retorno especial que indica que la proxima vez que se llame al metodo inicie despues de donde se encuentra la línea de 
        *	código de retorno.
        */
        private IEnumerator WaitForRequest(WWW www)
        {
            yield return www;

            // check for errors
            if (www.error == null)
            {
                if (OnRespCB != null)
                {
                    OnRespCB(true, www.text);
                }
            }
            else
            {
                if (OnRespCB != null)
                {
                    OnRespCB(false, "ERROR");
                }
            }
        }

        // Delegate
        /**
        *	@brief Método ejecutado cuando se encuentra una respuesta a un evento, que luego sera modificado con 
        *	respecto a como se desee utilizar manteniendo su estructura.
        *	
        */
        public delegate void OnRespEvent(bool success, string resp);

        // The event
        /**
        *	@brief	Evento llamado cuando ocurre una petición, que luego sera modificado.
        */
        public event OnRespEvent OnRespCB;
    }
}