using UnityEngine;
using Boomlagoon.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.com.ethereal.util;


namespace Assets.Scripts.com.ethereal.display
{
    /** 
    *   @author    EtherealGF <www.etherealgf.com> 
    *   @version   1.0 
    *   @date      Abril 24 2015
    * 
    *   @class  EthAnimation.cs
    *   @brief  Esta clase esta encargada del control de las animaciones en el juego
    *
    */
    public class EthAnimation : EthGUIScreen
    {
        /**
        *   @brief Diccionario que contiene la informacion de los valores anteriores.
        */
        private Dictionary<string, Dictionary<string, float>> valoresAnteriores = new Dictionary<string, Dictionary<string, float>>();

        /**
        *   @brief Array de texturas a aplicar a la animaci�n.
        */
        private ArrayList textures = new ArrayList();

        /**
        *   @brief Frame actual en la animaci�n.
        */
        private int _actualFrame = 0;

        /**
        *   @brief Control para el frame actual de la animaci�n.
        */
        private int _actualFrameCtrl = 0;

        /**
        *   @brief Controlador de la velocidad con la que pasan los frames.
        */
        private int _speedCtrl = 5;

        /**
        *   @brief HashMap que contiene efectos de animaci�n.
        */
        private IEnumerator<KeyValuePair<string, JSONValue>> todos;

        /**
        *   @brief Array que contiene todos los frames de la animaci�n.
        */
        private ArrayList frames = new ArrayList();

        /**
        *   @brief Cantidad de Frames.
        */
        private int cantFrames = 0;

        /**
        *   @brief Atributo booleano para indetificar si la animacion esta siendo reproducida o no.
        */
        private bool running = false;

        /**
        *   @brief M�todo para Instanciar un EthAnimation.
        *   
        *   Este m�todo es el encargado de crear un nuevo EthAnimation asignando todos los atributos que requiere esta clase.
        */
        public EthAnimation()
        {
        }

        /**
        *   @brief M�todo para Abrir una animaci�n que llega por par�metro.
        *   
        *   @param url Direcci�n en donde se encuentra la animaci�n a abrir
        */
        public void OpenAnimation(string url)
        {
            OpenAnimation(url, "");
        }

        /**
        *   @brief M�todo para Abrir una animaci�n que llega por par�metro.
        *   
        *   @param url Direcci�n en donde se encuentra la animaci�n a abrir.
        *   @param par argumentos requeridos para abrir o restringir una animaci�n.
        */
        public void OpenAnimation(string url, string par)
        {

            if (par != "")
            {
                EthArguments args = new EthArguments(par);

                if (args["center"] == "true")
                {
                    float wParam = float.Parse((string)Eth.GetVal(args["wScene"], 698));
                    float hParam = float.Parse((string)Eth.GetVal(args["hScene"], 500));
                    CenterScreen(wParam, hParam);
                }
                else if (args["subScreen"] == "true")
                {
                    float wParam = float.Parse((string)Eth.GetVal(args["wScene"], 698));
                    float hParam = float.Parse((string)Eth.GetVal(args["hScene"], 500));
                    float xPos = float.Parse((string)Eth.GetVal(args["xPos"], 0));
                    float yPos = float.Parse((string)Eth.GetVal(args["yPos"], 0));
                    UseAsSubScreen(wParam, hParam, xPos, yPos);
                }
            }

            TextAsset text = (TextAsset)Resources.Load(url, typeof(TextAsset));

            string texto = text.text;
            texto = texto.Substring(texto.IndexOf("{"));
            string objetosStr = texto.Substring(0, texto.IndexOf("var framesAnim ={")).Trim();
            string framesStr = texto.Substring(texto.IndexOf("var framesAnim ={") + 16).Trim();

            JSONObjectBoom json = JSONObjectBoom.Parse(objetosStr);
            JSONObjectBoom json2 = JSONObjectBoom.Parse(framesStr);

            todos = json.GetEnumerator();
            IEnumerator<KeyValuePair<string, JSONValue>> framesObj = json2.GetEnumerator();

            while (todos.MoveNext())
            {
                JSONValue obj = todos.Current.Value;

                string nombreTextura = obj.Obj.GetString("img");
                nombreTextura = nombreTextura.Substring(0, nombreTextura.IndexOf(".png"));
                textures.Add(Resources.Load(nombreTextura) as Texture);
            }

            while (framesObj.MoveNext())
            {
                JSONValue obj = framesObj.Current.Value;
                frames.Add(obj);
                cantFrames++;
            }

            framesObj.Dispose();

            running = true;
        }

        /**
        *   @brief Override del m�todo EthOnGui en el que se especifica que hacer cuando la animaci�n se encuentra en la GUI.
        */
        override protected void EthOnGUI()
        {
            if (!running)
            {
                return;
            }

            todos.Reset();

            int i = 0;

            while (todos.MoveNext())
            {
                JSONValue obj = todos.Current.Value;
                string objName = todos.Current.Key;

                float sxObj = 1;
                float syObj = 1;
                float wObj = float.Parse(obj.Obj.GetString("ancho"));
                float hObj = float.Parse(obj.Obj.GetString("alto"));
                float xObj = float.Parse(obj.Obj.GetString("x"));
                float yObj = float.Parse(obj.Obj.GetString("y"));
                float rObj = 0;
                float alpha = 1;

                JSONValue frActual = (JSONValue)frames[_actualFrame];

                Dictionary<string, float> valorAnteriorInfo = new Dictionary<string, float>();

                if (valoresAnteriores.ContainsKey(objName))
                {
                    valorAnteriorInfo = valoresAnteriores[objName];
                }
                else
                {
                    valorAnteriorInfo["x"] = xObj;
                    valorAnteriorInfo["y"] = yObj;
                    valorAnteriorInfo["scaleX"] = sxObj;
                    valorAnteriorInfo["scaleY"] = syObj;
                    valorAnteriorInfo["rotation"] = rObj;
                    valorAnteriorInfo["a"] = alpha;

                    valoresAnteriores[objName] = valorAnteriorInfo;
                }

                //Si tiene algo para modificar el objeto en el frame especifico, si no toma los valores anteriores
                if (frActual.Obj.GetValue(objName) != null)
                {
                    JSONObjectBoom dataActual = frActual.Obj.GetValue(objName).Obj.GetValue("css").Obj;

                    xObj = GetData("x", dataActual, valorAnteriorInfo["x"]);
                    yObj = GetData("y", dataActual, valorAnteriorInfo["y"]);
                    sxObj = GetData("scaleX", dataActual, valorAnteriorInfo["scaleX"]);
                    syObj = GetData("scaleY", dataActual, valorAnteriorInfo["scaleY"]);
                    rObj = GetData("rotation", dataActual, valorAnteriorInfo["rotation"]);
                    alpha = GetData("alpha", dataActual, valorAnteriorInfo["a"]);
                }
                else
                {
                    valorAnteriorInfo = valoresAnteriores[objName];

                    xObj = valorAnteriorInfo["x"];
                    yObj = valorAnteriorInfo["y"];
                    sxObj = valorAnteriorInfo["scaleX"];
                    syObj = valorAnteriorInfo["scaleY"];
                    rObj = valorAnteriorInfo["rotation"];
                    alpha = valorAnteriorInfo["a"];
                }

                DrawTexture(xObj - (wObj / 2), yObj - (hObj / 2), wObj * sxObj, hObj * syObj, alpha, (Texture)textures[i]);

                valorAnteriorInfo["x"] = xObj;
                valorAnteriorInfo["y"] = yObj;
                valorAnteriorInfo["scaleX"] = sxObj;
                valorAnteriorInfo["scaleY"] = syObj;
                valorAnteriorInfo["rotation"] = rObj;
                valorAnteriorInfo["a"] = alpha;

                valoresAnteriores[objName] = valorAnteriorInfo;

                i++;
            }

            _actualFrameCtrl++;
            _actualFrame = (int)(_actualFrameCtrl / _speedCtrl);

            if (_actualFrame >= cantFrames)
            {

                //Lanza el evento OnFinish
                if (OnFinish != null)
                {
                    OnFinish();
                }
                freezeGUI();
                running = false;

            }
        }

        /**
         *   @brief M�todo para obtener la informacion almacenada en un JSONObject.
         *
         *   @param dataType  Tipo de dato que se desea obtener el JSONObject.
         *   @param data      JSON Object del que se obtendr� la informaci�n requerida. 
         *   @param altData   Valor por defecto a ser devuelto de no ser encontrado un tipo de dato.
         *   
         *   @return En caso de ser encontrado el tipo de dato se retorna la informaci�n de este tipo convertido en float
         *   de lo contrario se retorna el mismo valor altData recibido por parametro
         */
        public float GetData(string dataType, JSONObjectBoom data, float altData)
        {
            if (data.ContainsKey(dataType))
            {
                return float.Parse(data.GetString(dataType));
            }
            return altData;
        }

        // Delegate
        /**
         * @brief M�todo para invocar el evento Onfinish
         */
        public delegate void OnFinishEvent();

        // The event
        /**
         * @brief Evento para detectar un cambio al finalizar la animaci�n.
         */
        public event OnFinishEvent OnFinish;
    }
}
