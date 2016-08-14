using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.com.ethereal.display
{

    /** 
        *   @author    EtherealGF <www.etherealgf.com> 
        *   @version   1.0 
        *   @date      Abril 24 2015
        * 
        *   @class  EthGameObject
        *   @brief  Esta clase representa un objeto de juego, el cual a su vez contiene ma sobjetos de juego que descienden de Ã©l.
        *
        */
    public class EthGameObject
    {

        /**
        *   @brief Patrón singleton para mantener la misma instancia de GameObject durante el juego.
        */
        public GameObject gameObject;

        /**
        *   @brief PDiccionario que contiene los EthGameObject que corresponden al EthGameObject actual.
        */
        private Dictionary<string, EthGameObject> children = new Dictionary<string, EthGameObject>();

        /**
        *   @brief Esta variable se usa para hacer un backup de la escala cuando el objeto se ponga invisible, pues esto lo pone con escala en 0 y tiene
        *   que recuperarse luego
        */
        private Vector3 backUpScale;

        /**
        *   @brief Constructor de la clase AnimationEase.
        *
        *   Este mÃ©todo recibe un EthGameObject ya creado.
        *
        *   @param gameObject 
        */
        public EthGameObject(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        /**
        *   @brief Constructor de la clase EthGameObject.
        *
        *   Este mÃ©todo recibe el path para realizar la creaciÃ³n del EthGameObject en dicho path.
        *
        *   @param objPath Path del EthGameObject a crear.
        */
        public EthGameObject(string objPath)
        {
            this.gameObject = GameObject.Find(objPath);
        }

        /**
        *	@brief Propiedades de lectura y escritura de la clase EthGameObject.
        *	
        *   @param index Índice de la clase.
        *
        *	@return El objeto EthGameObject.
        */
        public EthGameObject this[string index]
        {
            get
            {
                EthGameObject objRet;
                if (!children.TryGetValue(index, out objRet))
                {
                    objRet = GetChildByName(index);
                    children[index] = objRet;
                }

                return objRet;
            }

            set
            {

            }
        }

        /**
        *   @brief MÃ©todo para obtener un EthGameObject hijo mediante su nombre.
        *
        *   @param name Nombre del EthGameObject hijo a buscar.
        *
        *   @return El EthGameObject hijo que se encuentra, o nulo si no se encuentra.
        */
        public EthGameObject GetChildByName(string name)
        {

            Transform[] trans = gameObject.transform.GetComponentsInChildren<Transform>();

            foreach (Transform trAct in trans)
            {
                if (trAct.gameObject.name == name)
                {
                    return new EthGameObject(trAct.gameObject);
                }
            }

            return null;
        }

        /**
        *   @brief MÃ©todo para obtener un EthGameObject hijo mediante su path.
        *
        *   @param path Path del EthGameObject hijo a buscar.
        *
        *   @return El EthGameObject hijo que se encuentra, o nulo si no se encuentra.
        */
        public EthGameObject GetChildByPath(string path)
        {
            return GetChildByPath(path, this);
        }

        /**
        *   @brief MÃ©todo para obtener un EthGameObject hijo mediante su path y su padre.
        *
        *   @param path Path del EthGameObject hijo a buscar.
        *   @param obj  Padre del EthGameObject a buscar.
        *
        *   @return El EthGameObject hijo que se encuentra, o nulo si no se encuentra.
        */
        public EthGameObject GetChildByPath(string path, EthGameObject obj)
        {

            string[] partesPath = path.Split('/');

            if (partesPath.Length == 1)
            {
                return obj.GetChildByName(path);
            }
            else
            {

                string partesNuevoPath = partesPath[1];
                for (int i = 2; i < partesPath.Length - 1; i++)
                {
                    partesNuevoPath = partesNuevoPath + "," + partesPath[i];
                }

                return GetChildByPath(partesNuevoPath, obj.GetChildByName(partesPath[0]));
            }
        }

        /**
        *   @brief MÃ©todo para Cambiar la visibilidad de un EthGameObject hijo del EthGameObject actual.
        *
        *   @param value True si el EthGameObject se pondrÃ¡ visible, false en caso contrario.
        */
        public void SetChildrenVisibility(bool value)
        {
            SetChildrenVisibility(gameObject, value);
        }

        /**
        *   @brief MÃ©todo para Cambiar la visibilidad de un EthGameObject hijo a partir de su padre.
        *
        *   @param objAct Padre del EthGameObject al cual s ele va a modificar la visibilidad.
        *   @param value True si el EthGameObject hijo se pondrÃ¡ visible, false en caso contrario.
        */
        public void SetChildrenVisibility(GameObject objAct, bool value)
        {

            if (objAct.GetComponent<Renderer>() != null)
            {
                objAct.GetComponent<Renderer>().enabled = value;
            }

            foreach (Transform trAct in objAct.transform)
            {
                if (trAct.GetComponent<Renderer>() != null)
                {
                    trAct.GetComponent<Renderer>().enabled = value;
                }

                if (trAct.childCount > 0)
                {
                    foreach (Transform trAct2 in trAct)
                    {
                        if (trAct2.gameObject.GetComponent<Renderer>() != null)
                        {
                            trAct2.gameObject.GetComponent<Renderer>().enabled = value;
                        }
                        SetChildrenVisibility(trAct2.gameObject, value);
                    }
                }
                else
                {
                    SetChildrenVisibility(trAct.gameObject, value);
                }
            }
        }

        /**
        *   @brief Método para cambiar la visibilidad del EthGameObject actual.
        *
        *   @param value True sí el EthGameObject se pondrá visible, false en caso contrario.
        */
        public void SetVisible(bool valVisible)
        {
            if (gameObject.GetComponent<Renderer>() != null)
            {
                gameObject.GetComponent<Renderer>().enabled = valVisible;
            }
            SetChildrenVisibility(valVisible);

            if (valVisible)
            {

                if (backUpScale.x != 0 && backUpScale.y != 0 && backUpScale.z != 0)
                {
                    gameObject.transform.localScale = backUpScale;
                }
            }
            else
            {
                Vector3 vr = gameObject.transform.localScale;
                if (vr.x == 0 && vr.y == 0 && vr.z == 0)
                {
                    return;
                }
                backUpScale = new Vector3(vr.x, vr.y, vr.z);
                gameObject.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        /**
        *   @brief Método para eliminar los hijos de un EthGameObject.
        */
        public void RemoveChildren()
        {

            Transform[] trans = gameObject.transform.GetComponentsInChildren<Transform>();

            foreach (Transform trAct in trans)
            {
                if (trAct.gameObject != gameObject)
                {
                    GameObject.Destroy(trAct.gameObject);
                }
            }
        }

        /**
        *   @brief Método para agregar un hijo al EthGameObject ingresando un GameObject.
        *   
        *   @param go GameObject hijo que se le agregara al EthGameObject.
        */
        public void AddChild(GameObject go)
        {
            go.transform.parent = gameObject.transform;
        }

        /**
        *   @brief Método para agregar un hijo al EthGameObject ingresando un EthGameObject.
        *   
        *   @param go EthGameObject hijo que se le agregara al EthGameObject.
        */
        public void AddChild(EthGameObject go)
        {
            AddChild(go.gameObject);
        }

        /**
        *   @brief Método para agregar un hijo al EthGameObject ingresando un MonoBehaviour.
        *   
        *   @param go MonoBehaviour al que se le optiene el GameObject para ser agregado como hijo al EthGameObject.
        */
        public void AddChild(MonoBehaviour go)
        {
            AddChild(go.gameObject);
        }

        /**
        *   @brief Método para obtener el Component de tipo T del EthGameObject.
        *   
        *   @return El Component de tipo T encontrado o nulo.
        */
        public T GetComponent<T>() where T : Component
        {
            T obj = gameObject.GetComponent<T>();
            return obj;
        }

        /**
        *	@brief Método ToString que retorna el nombre de la clase y el nombre del objeto.
        *
        *	@return Nombre de la clase concatenado con el nombre del objeto.
        */
        public override string ToString()
        {
            if (gameObject != null)
            {
                return gameObject.name;
            }

            return "Empty EthGameObject";
        }
    }
}
