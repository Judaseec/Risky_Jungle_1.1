using System;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

/** 
*	@author    EtherealGF <www.etherealgf.com> 
* 	@version   1.0 
* 	@date      Julio 3 2014
* 
*	@class 	EthSerializador
*   @brief 	Serializa la información para poderla transportar a través de la red y para hacer persistente la información
*
*/
public class EthSerializer
{

    /**
    *	@brief Método usado para serializar la información.
    *
    *	Para serializar la información se requiere crear una secuencia que sera almacenada en la memoria, posteriormente se serializa la 
    *	información en formato binario para luego ser convertida en un string por medio del metodo Convert.ToBase64String(Byte[]).
    *
    *	@param list Lista de objetos a serializar.
    *
    *	@return String con la información serializada.
    */
    public static string Serialize(object list)
    {
        MemoryStream o = new MemoryStream(); //Create something to hold the data

        BinaryFormatter bf = new BinaryFormatter(); //Create a formatter
        bf.Serialize(o, list); //Save the list
        string data = Convert.ToBase64String(o.GetBuffer()); //Convert the data to a string

        return data;
    }

    /**
    *	@brief Método usado para deserializar la información para convertirla en objetos.
    *
    *	Para deserializar se requiere crear una secuencia, de acuerdo con la información que llega como un string codificada en Base64,
    *	la cual sera almacenada en la memoria, para posteriormente dar formato a la información por medio del BinaryFormatter.
    *
    *	@param data Información a ser deserializada.
    *
    *	@return Información deserializada.
    */
    public static object Deserialize(string data)
    {
        MemoryStream ins = new MemoryStream(Convert.FromBase64String(data)); //Create an input stream from the string
        BinaryFormatter bf = new BinaryFormatter(); //Create a formatter
        //Read back the data
        return bf.Deserialize(ins);
    }
}