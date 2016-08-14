using UnityEngine;
using System.Xml;
using System.IO;
using System;

namespace Assets.Scripts.com.ethereal.data
{

    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Julio 1 2014
    * 
    *	@class 	EthXML
    *   @brief 	Esta clase se encarga de trabajar con los documentos XML permitiendo leerlos e identificar sus etiquetas.
    *
    *	Esta clase es utilizada para la lectura de XML por medio de un XMLReader el cual sera cargado en un documento de xml (XMLNode),
    *	además es utilizada para la interpretacion del XML y obtener las diferentes etiquetas de este.
    *
    */
    public class EthXML
    {
        /**
        *	@brief Nodo que representa una de las etiquetas en XML.
        */
        private XmlNode node;

        /**
        *	@brief Método usado para crear un EthXML.
        *	
        *	Este método esta encargado de crear un EthXML asignando un nodo correspondiente a alguna etiqueta XML.
        *	
        *	@param node Nodo que representa una de las etiquetas del XML.
        */
        public EthXML(XmlNode node)
        {
            this.node = node;
        }

        /**
        *	@brief	Método usado para leer un XML.
        *	
        *	Método encargado de leer un XML ubicado en la direccion indicada(url), obteniendo el texto por medio de un TextAsset para luego 
        *	ser leído por un XMLReader, el cual sera cargado en un documento.
        *	
        *	@param url 	Direccion en donde se encuentra ubicado el XML a leer.
        *	
        *	@return Nuevo XML formado por la primera etiqueta del documento creado.
        */
        public static EthXML ReadXML(string url)
        {
            TextAsset text = (TextAsset)Resources.Load(url, typeof(TextAsset));
            XmlDocument doc = new XmlDocument();
            XmlReader reader = XmlReader.Create(new StringReader(text.text));
            doc.Load(reader);

            return new EthXML(((XmlNode)doc).FirstChild);
        }

        /**
        *	@brief	Método usado para conocer la cantidad de Nodos hijos del nodo actual.
        *	
        *	Método encargado de determinar la cantidad de nodos hijos que posee el nodo actual. 
        *	
        *	@return Cantidad de nodos hijos que posee el nodo actual.
        */
        public int GetAmountChildren()
        {
            return node.ChildNodes.Count;
        }

        /**
        *	@brief	Getters y setters del atributo node referenciados con respecto a esta clase.
        *	
        *	Método encargado de obtener y modificar la variable.  
        *	
        *	@param index Identificador numero del atributo del nodo que se desea obtener.
        *
        *	@return Valor de los atributos del nodo.
        */
        public string this[string index]
        {
            get
            {
                if (node.Attributes[index] == null)
                {
                    return null;
                }
                return node.Attributes[index].Value;
            }
            // La funcionalidad de setear posiciones de un EthXML no esta implementada
            set
            {
                Debug.Log("The EthXML positions setting funcionality isn't yet implemented");
            }
        }

        /**
        *	@brief	Método usado para conocer algun nodo hijo del nodo actual.
        *	
        *	Método encargado de obtener uno de los nodos hijos que tenga el nodo actual, especificado por el numero de hijo ingresado
        *	en el parámetro. 
        *
        *	@param index Número del nodo hijo que desea ser obtenido.
        *	
        *	@return Nodo hijo obtenido del nodo actual.
        */
        public EthXML GetChildAt(int index)
        {
            return new EthXML(node.ChildNodes[index]);
        }
		
		/**
	    *	@brief	Método usado para conocer el tipo de nodo del XML.
	    *	
	    *	Método encargado de obtener el tipo del nodo actual, por ejemplo si es nodo o comentario
		*
		*	@return String indica el tipo de nodo actual.
	    */
		public String GetNodeType () {			
			return "" + node.NodeType;
		} 

        /**
        *	@brief	Método usado para obtener un array que contiene los nodos hijos del nodo actual.
        *	
        *	Método encargado de obtener todos los nodos hijos del nodo actual y agregarlos a un array de EthXML. 
        *	
        *	@return Array EthXML con todos lso hijos del nodo actual.
        */
        public EthXML[] GetChildren()
        {
            EthXML[] returnArray = new EthXML[node.ChildNodes.Count];

            for (int j = 0; j < node.ChildNodes.Count; j++)
            {
                XmlNode node1 = node.ChildNodes[j];
                returnArray[j] = new EthXML(node1);
            }

            return returnArray;
        }

        /**
        *	@brief	Método usado para obtener un array que contiene un hijo del nodo actual.
        *	
        *	Método encargado de obtener un nodo hijo, especificado por el index (nombre completo del nodo hijo) que entra 
        *	por parámetro, del nodo actual. 
        *	
        *	@param index Nombre completo del nodo hijo a ser obtenido.
        *
        *	@return Array EthXML con el nodo hijo requerido.
        */
        public EthXML[] GetChildren(string index)
        {

            EthXML[] returnArray = new EthXML[0];

            for (int j = 0; j < node.ChildNodes.Count; j++)
            {
                XmlNode node1 = node.ChildNodes[j];
                if (node1.Name == index)
                {
                    Array.Resize(ref returnArray, returnArray.Length + 1);
                    returnArray[returnArray.Length - 1] = new EthXML(node1);
                }
            }

            return returnArray;
        }

        /**
        *	@brief	Método usado para establecer los valores concatenados del nodo actual y todos sus elementos secundarios.
        *	
        *	Método encargado de obtener todos los valores en un texto, tanto del nodo actual como de sus valores secundarios. 
        *	
        *	@return Concatenación de todos los valores del nodo actual y sus elementos secundarios.
        */
        public string GetContent()
        {
            return node.InnerText;
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
            string returnString = "";

            returnString = returnString + node.Name;

            foreach (XmlAttribute obj in node.Attributes)
            {
                returnString = string.Format("{0} {1}='{2}'", returnString, obj.Name, obj.Value);
            }

            return string.Format("EthXML <{0}> Hijos({1})", returnString, node.ChildNodes.Count);
        }
    }
}
