using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.Globalization;

namespace Assets.Scripts.com.ethereal.util
{
    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Agosto 30 2014
    * 
    *	@class 	Eth
    *   @brief 	Esta clase esta encargada de implementar varias funciones que sirvan a las diferentes funcionalidades que se requieran 
    *	para elaborar una aplicaci�n.
    *
    */
    public class Eth
    {
        /**
        *	@brief Plataforma m�vil en la que se ejecuta la aplicaci�n.
        */
        private static string platformMobile = "unknown";

        /**
         * @brief Constante para imagen
         */
        public static string IMG = "img";

        /**
        *	@brief M�todo encargado de crear un Eth.
        *	
        */
        public Eth()
        {

        }

        /**
        *	@brief M�todo encargado de codificar una cadena de caracteres en base 64.
        *
        *	Para codificar esta cadena de caracteres es necesario obtener los bytes  de �sta en UTF8, para luego por medio del m�todo
        *	ToBase64String( ), codificar el texto en base 64.
        *	
        *	@param words Cadena de caracteres que sera codificada en base 64.
        *
        *	@return Texto codificado.
        */
        public static string EncodeBase64(string words)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(words);
            string encodedText = Convert.ToBase64String(bytesToEncode);

            return encodedText;
        }

        /**
        *	@brief M�todo encargado de obtener el tipo de clase de un objeto.
        *	
        *	@param obj Objeto al que se le identificara el tipo de clase al que pertenece.
        *
        *	@return tipo de clase del objeto requerido.
        */
        public static string GetClassType(object obj)
        {

            string[] types = obj.GetType().ToString().Split('.');
            return types[types.Length - 1];
        }

        /**
        *	@brief M�todo encargado de obtener el tipo de clase de un objeto.
        *	
        *	Este m�todo obtiene el nombre de la clase completa incluyendo el nombre del namespace al que pertenece.
        *
        *	@param obj Objeto al que se le identificara el tipo de clase al que pertenece.
        *
        *	@return tipo de clase al que pertenece el objeto requerido.
        */
        public static string GetFullClassType(object obj)
        {
            return obj.GetType().ToString();
        }

        /**
        *	@brief M�todo encargado de obtener el primer valor del arreglo de argumentos ingresado que se encuentre no nulo.
        *	
        *	@param arguments Parametros recibidos por la funci�n.
        *
        *	@return Object encontrado que no es nulo y si no encuentra ningun existente retorna null.
        */
        public static object GetVal(params object[] arguments)
        {
            foreach (object x in arguments)
            {
                if (x != null)
                {
                    return x;
                }
            }

            return null;
        }

        /**
        *	@brief M�todo encargado de agregar a un array todos los objetos encontrados en el arreglo de argumentos recibido por par�metro.
        *	
        *	@param arguments Parametros recibidos por la funci�n para ser agregados.
        *
        */
        public static void AddObjects(params object[] arguments)
        {
            ArrayList array = (ArrayList)arguments[0];

            for (int i = 1; i < arguments.Length; i++)
            {
                array.Add(arguments[i]);
            }
        }

        /**
        *	@brief M�todo encargado de separar las palabras en una cadena de caracteres y agregarlos en un arreglo.
        *	
        *	Se separan las palabras de acuerdo a un indicador de separaci�n especificado, para as� ser agregadas en un array que las 
        *	contenga.
        *
        *	@param words 		Cadena de caracteres de la que se obtendr�n las palabras a ser separadas.
        *	@param separator 	Indicador de separaci�n para obtener las palabras contenidas en la cadena de car�cteres.
        *
        *	@return Arreglo contenedor de todas las palabras obtenidas de la cadena de caracteres suministrada.
        */
        public static string[] ToArray(string words, string separator)
        {
            string[] array = words.Split(separator.ToCharArray());
            return array;
        }

        /**
        *	@brief M�todo encargado de separar las palabras en una cadena de caracteres y agregarlos en un arreglo de tipo float.
        *	
        *	Se separan las palabras de acuerdo a un indicador de separaci�n especificado, para as� ser agregadas en un array que las 
        *	contenga.
        *
        *	@param words 		Cadena de caracteres de la que se obtendr�n las palabras a ser separadas.
        *	@param separator 	Indicador de separaci�n para obtener las palabras contenidas en la cadena de car�cteres.
        *
        *	@return Arreglo de tipo float contenedor de todas las palabras obtenidas como tipo float de la cadena de caracteres suministrada.
        */
        public static float[] ToFloatArray(string words, string separator)
        {
            string[] array = words.Split(separator.ToCharArray());
            float[] arrayResponse = new float[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                arrayResponse[i] = ToFloat(array[i]);
            }

            return arrayResponse;
        }

        /**
        *	@brief Concatena los elementos del arreglo especificado o los miembros de una colecci�n, utilizando el separador indicado entre 
        *	todos los elementos o miembros.	
        *
        *	@param array 		Arreglo contenedor de los elementos a concatenar.
        *	@param separator 	Cadena usada como separador que sera incluida en la cadena de retorno solo si array tiene mas de 1 elemento.
        *
        *	@return Cadena que contiene todos los elementos unidos, provenientes del arreglo especificado.
        */
        public static string ToString(string[] array, string separator)
        {
            string result = string.Join(separator, array);
            return result;
        }

        /**
        *	@brief M�todo encargado de separar las palabras en una cadena de caracteres y agregarlos en un Arraylist.
        *	
        *	Se separan las palabras de acuerdo a un indicador de separaci�n especificado, para as� ser agregadas en un Arraylist que las 
        *	contenga.
        *
        *	@param words 		Cadena de caracteres de la que se obtendr�n las palabras a ser separadas.
        *	@param separator 	Indicador de separaci�n para obtener las palabras contenidas en la cadena de car�cteres.
        *
        *	@return Arraylist contenedor de todas las palabras obtenidas de la cadena de caracteres suministrada.
        */
        public static ArrayList ParseStringToArraylist(string words, string separator)
        {
            ArrayList array = new ArrayList();
            string[] collection = words.Split(separator.ToCharArray());

            for (int i = 0; i < collection.Length; i++)
            {
                array.Add(collection[i]);
            }

            return array;
        }

        /**
        *	@brief M�todo encargado de unir las palabras contenidas en un array en un string.
        *
        *	Se obtiene un arreglo del array especificado por parametro para as� concatenar las palabras por el m�todo 
        *	Arraylist.Join(String, String[]) en un  string, utilizando el separador especiificado.
        *
        *	@param array 		Arraylist que contene las palabras a ser concatenadas.
        *	@param separator 	Indicador de separaci�n para obtener las palabras contenidas en la cadena de car�cteres.
        *
        *	@return Cadena de caracteres que contiene todas las palabras del array con el separador especificado.
        */
        public static string ArraylistToString(ArrayList array, string separator)
        {
            return string.Join(separator, (string[])array.ToArray(Type.GetType("System.String")));
        }

        /**
        *	@brief M�todo encargado de imprimir en consola un objeto encontrado en una lista.
        *	
        *	Todos los objetos que hereden de la interfaz IEnumerable como las listas, pueden ser recibidos como par�metro de las cuales
        *	se imprimiran en consola los objetos encontrados en estas. Este metodo es usado para la depuraci�n del programa
        *
        *	@param myList Lista de la que se requiere saber los objetos almacenados en esta.
        *
        */
        public static void PrintValues(IEnumerable myList)
        {
            foreach (object obj in myList)
                Debug.Log(obj);
        }

        /**
        *	@brief M�todo encargado de identicar si la plataforma en la que se ejecuta el juego es m�vil o no.
        *	
        *	Si cumple alguna de las opciones del swicth la plataforma es m�vil y de lo contrario no lo es.
        *
        *	@return True si la plataforma es m�vil, de lo contrario false.
        */
        public static bool IsMobile()
        {

            if (platformMobile == "unknown")
            {
                platformMobile = "noMobile";

                switch (Application.platform)
                {
                    case RuntimePlatform.IPhonePlayer:
                    case RuntimePlatform.Android:
                    case RuntimePlatform.WP8Player:
                        platformMobile = "mobile";
                        break;
                }
            }

            return platformMobile == "mobile";
        }

        /**
        *	@brief M�todo encargado de convertir un objeto a Integer.
        *
        *	Antes de convertir el objeto en un integer se verifica si existe algun punto en �l para evitar errores. Posteriormente se convierte
        *	a Integer el objeto.
        *	
        *	@param par Objeto que ingresa por parametro para ser convertido a Integer.
        *
        *	@return Integer obtenido de convertir el objeto ingresado por par�metro.
        */
        public static int ToInt(object par)
        {
            if (par.ToString().IndexOf(".") != -1)
            {
                par = par.ToString().Substring(0, par.ToString().IndexOf("."));
            }

            return Convert.ToInt32(par);
        }

        /**
        *	@brief M�todo encargado de convertir un objeto a float.
        *	
        *	@param Objeto que ingresa por parametro para ser convertido a Float.
        *
        *	@return Float obtenido de convertir el objeto ingresado por par�metro.
        */
        public static float ToFloat(object par)
        {
            return (float)Convert.ChangeType(par, typeof(float));
        }

        /**
        *	@brief M�todo encargado de encriptar a MD5 un string especificado.
        *	
        *	@param strToEncrypt Cadena a ser encriptada.
        *
        *	@return String codificado en MD5 con una longitud de 32 carateres.
        */
        public static string Md5Sum(string strToEncrypt)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            // encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }

        /**
        *	@brief M�todo encargado de convertir un n�mero a string dandole un formato especificado a �ste.
        *
        *	Al n�mero ingresado se le asinga un formato en especifico denotado por la letra N el cual hace que quede ##,###.00
        *	
        *	@param num N�mero a ser convertido a string.
        *
        *	@return N�mero convertido en String con el nuevo formato.
        */
        public static string GetFormatedNumber(int num)
        {
            return num.ToString("N0");
        }

        /**
        *	@brief M�todo encargado de obtener el mes determinado por el n�mero que ingresa por par�metro.
        *
        *	Se obtiene el nombre del mes requerido de un array que contiene  todos los nombres de los meses, del cual son obtenidos por su 
        *	posici�n en el array.
        *	
        *	@param num N�mero del mes que se requiere.
        *
        *	@return Mes retornado de acuerdo al n�mero ingresado por parametro.
        */
        public static string GetNameOfMonth(int numMonth)
        {
            string[] arrayMonths = {
								"Enero",
								"Febrero",
								"Marzo",
								"Abril",
								"Mayo",
								"Junio",
								"Julio",
								"Agosto",
								"Septiembre",
								"Octubre",
								"Noviembre",
								"Diciembre"
						};

            return arrayMonths[numMonth - 1];
        }

        /**
        *	@brief M�todo encargado de mostrar en consola un mensaje requerido o especiicado por par�metro llamando tambi�n un metodo externo,
        *	el cual deja en un log el mensaje especificado.
        *
        *	@param msg Mensaje a ser mostrado en consola.
        */
        public static void Log(string msg)
        {
            Debug.Log(msg);
            Application.ExternalCall("console.log", msg);
        }

        /**
        *	@brief M�todo encargado de cambiar la ubicaci�n de los elementos en un array.
        *
        *	Al ser usado este metodo cumple la funci�n de desordenar un array para que los elementos dentrto de el no sean obtenidos siempre
        *	de la misma posicion en la que antes se encontraban.
        *	
        *	@param originalArray Array que se desea desordenar.
        *
        *	@return nuevo array con los elementos en una posicion diferente del array original.
        */
        public static List<T> ShuffleArray<T>(List<T> originalArray)
        {

            List<int> arrayOrder = CreateArray(originalArray.Count);
            List<T> organizedArray = new List<T>();

            while (arrayOrder.Count > 0)
            {
                int pos = UnityEngine.Random.Range(0, arrayOrder.Count);
                organizedArray.Add(originalArray[arrayOrder[pos]]);
                arrayOrder.RemoveAt(pos);
            }

            return organizedArray;
        }

        /**
        *	@brief M�todo encargado de crear un array al que se le ingresaran una cantidad de n�meros determinada ingresada por par�metro.
        *	
        *	@param pos Tama�o o cantidad de n�meros que se van a agregar al array creado.
        *
        *	@return Nuevo  array con la cantidad de n�meros especificada.
        */
        public static List<int> CreateArray(int pos)
        {
            List<int> arrayRet = new List<int>();

            for (int i = 0; i < pos; i++)
            {
                arrayRet.Add(i);
            }

            return arrayRet;
        }

        /**
        *	@brief M�todo encargado de obtener las interfaces asignadas a un gameObject.
        *	
        *	Se obtienen las interfaces del gameObject recibido por parametro, para luego agrearlos en un array y retornarlo por par�metros 
        *	referenciados.
        *
        *	@param objectToSearch Objeto del cual se obtendr�n las interfaces.
        *
        */
        public static void GetInterfaces<T>(out List<T> resultList, GameObject objectToSearch) where T : class
        {
            MonoBehaviour[] list = objectToSearch.GetComponents<MonoBehaviour>();
            resultList = new List<T>();
            foreach (MonoBehaviour mb in list)
            {
                if (mb is T)
                {
                    resultList.Add((T)((System.Object)mb));
                }
            }
        }

        /**
        *	@brief M�todo encargado de encontrar una probabilidad.
        *	
        *	Se recibe una probabilidad por par�metro para luego ser comparada con un numero aleatorio entre 1 a 100 y si el numero aleatorio
        *	esta en un rango menor o igual a la probabilidad ingresada, se identifica la probabilidad como cierta.
        *
        *	@param probability probabilidad que sera recibida para la comparaci�n.
        *	@param log Registro de los n�meros aleatorios.
        *
        *	@return .
        */
        public static bool IsMeetingProbability(int probability, bool log = false)
        {
            float number = UnityEngine.Random.Range(1, 100);
            if (log)
            {
                Debug.Log("number: " + number);
            }

            if (number <= probability)
            {
                return true;
            }

            return false;
        }

        /*
        *	@brief M�todo para asignar un valor de visualizaci�n al game object que llega por parametro
        *	
        *	@param go Game Object a asignar un valor de visualizaci�n.
        *	@param value Valor de visualizacion que trae el game object.
        */
        public static void SetVisibleGameObject(GameObject go, bool value)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                r.enabled = value;
            }
        }

        /*
        *	@brief M�todo para asignar un valor de transparencia al game object que llega por parametro
        *	
        *	@param go Game Object a asignar un valor de transparencia.
        *	@param value Valor de transparencia a asignar al game object.
        */
        public static void SetAlphaGameObject(GameObject go, float alpha)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
            Color temp;
            foreach (Renderer r in renderers)
            {
                temp = r.material.color;
                temp.a = alpha;
                r.material.color = temp;
                if (alpha < 1f)
                {
                    r.material.shader = Shader.Find("Transparent/Diffuse");
                }
                else
                {
                    r.material.shader = Shader.Find("Diffuse");
                }
            }
        }

        /*
        *	@brief M�todo para asignar un valor de tinte al game object que llega por parametro
        *	
        *	@param go Game Object a asignar un valor de tinte.
        *	@param value Valor de tinte a asignar.
        */
        public static void TintGameObject(GameObject go, Color colorTint)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

            foreach (Renderer r in renderers)
            {
                for (int i = 0; i < r.materials.Length; i++)
                {
                    r.materials[i].color = colorTint;
                }

            }
        }

        /*
        *	@brief M�todo para contar cuantas veces se repite determinada letra en un string.
        *	
        *	@param stringToCount String en el que se inspeccionara la repetici�n de una letra requerida.
        *	@param character Caracter del que se desea obtener su numero de repeticiones en el string.
        *
        *	@return Numero de repeticiones del caracter especificado.
        */
        public static int CountCharInString(string stringToCount, char character)
        {
            int acount = 0;
            for (int i = 0; i < stringToCount.Length; i++)
            {
                if (stringToCount[i] == character)
                {
                    acount++;
                }
            }

            return acount;
        }
    }
}