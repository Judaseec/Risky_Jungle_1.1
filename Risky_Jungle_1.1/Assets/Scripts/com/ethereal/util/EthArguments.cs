using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.com.ethereal.util
{
    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Julio 22 2014
    * 
    *	@class 	EthArguments
    *   @brief 	
    *
    *	Esta clase se encarga de organizar los datos como "h:200, w:400, ... , " con la cual se obtendran cada pareja de datos como un 
    *	argumento, posteriormente se almacenan en un diccionario de datos, el cual luego sera usado para adquirir la informacion requerida 
    *	para ubicar los objetos en la GUI.
    *
    */
    public class EthArguments
    {

        /**
        *	@brief Diccionario de datos en el que se almacenarán todos los argumentos.
        */
        private Dictionary<string, string> componentParameters = new Dictionary<string, string>();

        /**
        *	@brief Método para Instanciar un EthArguments.
        *	
        *	Este método es el encargado de crear un nuevo EthArguments sin necesidad de atributos haciendo referencia a su propio contructor 
        *	mandando como parametro una cadena vacía.
        */
        public EthArguments()
            : this("")
        {
        }

        /**
        *	@brief Método para Instanciar un EthArguments.
        *	
        *	Este método es el encargado de crear un nuevo EthArguments, el cual recibe como parametro una cadena para de ésta sacar todos los 
        *	argumentos contenidos.
        *
        *	@param parameters Parámetros en los que están contenidos los argumentos.
        */
        public EthArguments(string parameters)
        {

            if (parameters != "")
            {
                // Split string on spaces. This will separate all the words in a string
                string[] parametersList = parameters.Split(',');

                foreach (string parameter in parametersList)
                {
                    if (parameter == "")
                    {
                        continue;
                    }
                    string[] datas = parameter.Split(':');
                    componentParameters[datas[0]] = datas[1];
                }
            }
        }

        /**
        *	@brief Método usado para conocer si el diccionario de datos ya posee argumentos almacenados.
        *
        *	@return True si el diccionario de datos ya posee argumentos, de los contrario false. 
        */
        public bool HasArguments()
        {
            return componentParameters.Count > 0;
        }

        /**
        *	@brief	Getters y setters del atributo componentParameters referenciados con respecto a esta clase.
        *	
        *	Método encargado de obtener y modificar la variable.  
        *	
        *	@param index Identificador del parámetro que desea obtener.
        *
        *	@return Valor de los atributos del nodo.
        */
        public string this[string index]
        {
            get
            {
                if (componentParameters.ContainsKey(index))
                {
                    return componentParameters[index];
                }

                return null;
            }

            set
            {
                componentParameters[index] = value;
            }
        }

        /**
        *	@brief	Método usado para obtener los argumentos almacenados en la cadena.
        *	
        *	Método encargado de obtener todos los argumentos que hay en una cadena, organizarlos, separando cada argumento por comas, para luego 
        *	retornar todo en una sola cadena.
        *	
        *	@param agrs Cadena en donde se encuentran almacenados todos los argumentos.
        *
        *	@return Cadena con los argumentos separados por comas.
        */
        public string GetArgumentsAsStr(string args)
        {
            string[] parameters = args.Split(',');

            bool pasHasParam = false;

            string strRet = "";

            for (int i = 0; i < parameters.Length; i++)
            {
                if (pasHasParam)
                {
                    strRet += ",";
                    pasHasParam = false;
                }

                string parAct = this[parameters[i]];

                if (parAct != null)
                {
                    strRet += parameters[i] + ":" + parAct;
                    pasHasParam = true;
                }
            }

            return strRet;
        }

        /**
        *	@brief	Método usado para obtener los argumentos almacenados en la cadena.
        *	
        *	Método encargado de obtener todos los argumentos que hay en una cadena, organizarlos, separando cada argumento por comas, para luego 
        *	retornar todo en una sola cadena.
        *	
        *	@param agrs 	Cadena en donde se encuentran almacenados todos los argumentos.
        *	@param newNames Nuevos nombres de las etiquetas de los argumentos.
        *
        *	@return Cadena con los argumentos separados por comas.
        */
        public string GetArgumentsAsStr(string args, string newNames)
        {
            string[] parameters = args.Split(',');
            string[] names = newNames.Split(',');

            bool pasHasParam = false;

            string strRet = "";

            for (int i = 0; i < parameters.Length; i++)
            {
                if (pasHasParam)
                {
                    strRet += ",";
                    pasHasParam = false;
                }

                string parAct = this[parameters[i]];

                if (parAct != null)
                {
                    strRet += names[i] + ":" + parAct;
                    pasHasParam = true;
                }
            }

            return strRet;
        }

        /**
        *	@brief	Sobreescritura del método toString().
        *	
        *	Método encargado de representar la clase en forma de texto de una manera coherente. 
        *	
        *	@return Cadena de caracteres representando la clase actual.
        */
        public override string ToString()
        {
            string str = "";

            foreach (KeyValuePair<string, string> args in componentParameters)
            {
                str += args.Key + "=" + args.Value + " -- ";
            }

            return str;
        }
    }
}