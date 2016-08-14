using UnityEngine;

namespace Assets.Scripts.com.ethereal.util
{
    /** 
*	@author    EtherealGF <www.etherealgf.com> 
* 	@version   1.0 
* 	@date      Septiembre 5 2014
* 
*	@class 	ObjectFactory
*   @brief 	Clase encargada de implementar varias funciones con respectto al gameObject tenendo como por ejemplo la creaciÃƒÂ³n de este, asi 
*	como la asignaciÃƒÂ³n de componentes y obtenciÃƒÂ³n de estos.
*
*/
    public class ObjectFactory
    {
        /**
        *	@brief MÃƒÂ©todo para crear un objeto.
        *	
        *	Se crea un nuevo gameObject con el nombre que se ingresa por parametro, adicionandole ademÃƒÂ¡s un componente especificado tambien 
        *	por parÃƒÂ¡metro.
        *
        *	@param name Nombre del objeto a crear.
        *	@param component Componente a ser asignado al nuevo game object.
        *
        *	@return Nuevo gameObject creado con su componente agregado
        */
        public static object CreateObject<T>(string name) where T:Component
        {
            GameObject newEmptyGameObject = new GameObject(name);
            //UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (newEmptyGameObject, "Assets/Scripts/com/ethereal/util/ObjectFactory.cs (31,13)", component);
            newEmptyGameObject.AddComponent<T>();
            return newEmptyGameObject.GetComponent<T>();
        }

        /**
        *	@brief MÃƒÂ©todo para asignar un componente a un gameObject.
        *	
        *	Ingresa un gameObject por parÃƒÂ¡metro al cual se le asignara el componente requerido ingresado por parÃƒÂ¡metro.
        *
        *	@param gameObj GameObject ingresado por parÃƒÂ¡metro al cual se le agregara un componente.
        *	@param component Componente a ser agregado al gameObject.
        */
        public static object CreateObject(GameObject gameObj, string component)
        {
            // UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(gameObj, "Assets/Scripts/com/ethereal/util/ObjectFactory.cs (45,13)", component);
            return gameObj.GetComponent(component);
        }

        /**
        *	@brief MÃƒÂ©todo para obtener un component de un gameObject determinado.
        *
        *	Se especifica el gameObject que se desea obtener por medio del nombre de este, el cual se manda por parametro. Posteriormente se 
        *	obtiene un component de este gameObject.
        *
        *	@param gameObjectName nombre del gameObject que se desea obtener.
        *
        *	@return component del gameObject requerido.
        */
        public static T GetObject<T>(string gameObjectName) where T : Component
        {
            GameObject go = GameObject.Find(gameObjectName);
            T obj = go.GetComponent<T>();
            return obj;
        }

        /**
        *	@brief MÃƒÂ©todo para obtener un gameObject determinado.
        *
        *	Se especifica el gameObject que se desea obtener por medio del nombre de este, el cual se manda por parametro. 
        *
        *	@param gameObjectName nombre del gameObject que se desea obtener.
        *
        *	@return GameObject obtenido.
        */
        public static GameObject GetGameObject(string gameObjectName)
        {
            GameObject go = GameObject.Find(gameObjectName);
            return go;
        }
    }
}