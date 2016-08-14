using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.com.ethereal.util;

namespace Assets.Scripts.com.ethereal.display.components3D
{
    /** 
    *   @author    EtherealGF <www.etherealgf.com> 
    *   @version   1.0 
    *   @date      Abril 11 2015
    * 
    *   @class  EthButton3D
    *   @brief  Esta clase esta encargada de la configuracion del scroll 3D.
    *
    */
    public class EthScroll3D : EthComponent3D
    {

        /**
        *   @brief Valor mÃ­nimo del scroll en Y.
        */
        private float minY;

        /**
        *   @brief Valor mÃ¡ximo del scroll en X.
        */
        private float maxY;

        /**
        *   @brief Valor de tolerancia (lÃ­mite) de y.
        */
        private float offSetY = 2f;

        /**
        *   @brief AsignaciÃ³n de propiedades de lectura y escritura a la variable offSetY.
        */
        public float OffSetY
        {
            get { return offSetY; }
            set { offSetY = value; }
        }

        /**
        *   @brief Ultima posiciÃ³n en y.
        */
        protected float lastPosY;

        /**
        *   @brief Variable que representa la posiciÃ³n actual del EthScroll3D en el eje Y.
        */
        private float scrollYValAct;

        /**
        *   @brief Variable que permite identificar si el EthScroll3D ya fue inicializado.
        */
        private bool compInitialized = false;

        /**
        *   @brief Velocidad con la que se mueve el scroll.
        */
        private float velocityButton = 2f;

        /**
        *   @brief Esto ya que un avance en un pixel no corresponde a un avance de una unidad en pantalla, eso depende de los objetos y se 
        *   debe calcular y setear.
        */
        private float desfasY = 0.7f;

        /**
        *   @brief EthButton3D para controlar el movimiento hacia abajo del EthScroll3D.
        */
        EthButton3D botAbajo;

        /**
        *   @brief EthButton3D para controlar el movimiento hacia arriba del EthScroll3D.
        */
        EthButton3D botArriba;

        /**
        *   @brief EthButton3D principal.
        */
        EthButton3D botPpal;

        /**
        *   @brief EthButton3D del scroll.
        */
        EthButton3D Barra;

        /**
        *   @brief EthButton3D dentro de la barra scroll.
        */
        GameObject goBotBarra;

        /**
        *   @brief Vector que indica donde estarÃ¡ ubicado inicialmente el EthScroll3D.
        */
        Vector3 valIniBarra;

        /**
        *   @brief Diccionario que cntiene las posiciones iniciales del scroll
        */
        private Dictionary<GameObject, float[]> posicionesIniciales;

        /**
        *   @brief Constante con el nombre de la clase.
        */
        private const String ETH_BUTTON_3D_CLASS_NAME = "EthButton3D";

        /**
        *   @brief Constructor de la clase EthScroll3D.
        *
        *   Este mÃ©todo permite crear una instancia de la clase EthScroll3D.
        *
        */
        public EthScroll3D()
            : base()
        {

        }

        /**
        *   @brief  MÃ©todo para asignar argumentos.
        *   
        *   @param args Argunmentos a ser asignados
        *   
        */
        public override void SetArgs(EthArguments args)
        {
        }

        /**
        *   @brief  MÃ©todo para Incializar los componentes que tendra el Scroll.
        *   
        */
        public void initComponents()
        {
            ethGameObject["BotAbajo"].gameObject.AddComponent<EthButton3D>();
            ethGameObject["BotArriba"].gameObject.AddComponent<EthButton3D>();
            ethGameObject["BotPpal"].gameObject.AddComponent<EthButton3D>();
            ethGameObject["Barra"].gameObject.AddComponent<EthButton3D>();

            goBotBarra = ethGameObject["BotPpal"].gameObject;
            valIniBarra = new Vector3(goBotBarra.transform.localPosition.x, goBotBarra.transform.localPosition.y, 0);


            botArriba = (EthButton3D)ethGameObject["BotArriba"].gameObject.GetComponent(ETH_BUTTON_3D_CLASS_NAME);
            botAbajo = (EthButton3D)ethGameObject["BotAbajo"].gameObject.GetComponent(ETH_BUTTON_3D_CLASS_NAME);
            botPpal = (EthButton3D)ethGameObject["BotPpal"].gameObject.GetComponent(ETH_BUTTON_3D_CLASS_NAME);
            Barra = (EthButton3D)ethGameObject["Barra"].gameObject.GetComponent(ETH_BUTTON_3D_CLASS_NAME);

            botPpal.SetFunctionEvents(ClickBot);
            botPpal.preventScroll = true;

        }

        /**
        *   @brief MÃ©todo para Mover el boton de scroll cuando se le da click.
        *
        *   @param eventName    Nombre del evento a suceder.
        *   @param nomBot       Nombre del botÃ³n.
        */
        public void ClickBot(string eventName, string nomBot)
        {

            float newPos = -(Input.mousePosition.y - (Screen.height * 0.6232f)) * 100 / (Screen.height * 0.3996f);

            if (newPos < 0)
            {
                newPos = 0;
            }
            if (newPos > 100)
            {
                newPos = 100;
            }

            ScrollTo(newPos);
        }

        /**
        *   @brief MÃ©todo par aactualizar en pantalla la posicion del scroll actualmente.
        */
        void Update()
        {

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                scrollYValAct += -Input.GetAxis("Mouse ScrollWheel") * 30;
                if (scrollYValAct < 0)
                {
                    scrollYValAct = 0;
                }
                if (scrollYValAct > 100)
                {
                    scrollYValAct = 100;
                }
                ScrollTo(scrollYValAct);
            }
            else if (botAbajo != null && botAbajo.pressed)
            {
                scrollYValAct += velocityButton;
                if (scrollYValAct > 100)
                {
                    scrollYValAct = 100;
                }
                ScrollTo(scrollYValAct);
            }
            else if (botArriba != null && botArriba.pressed)
            {
                scrollYValAct -= velocityButton;
                if (scrollYValAct < 0)
                {
                    scrollYValAct = 0;
                }
                ScrollTo(scrollYValAct);
            }
            else if (Barra != null && Barra.pressed)
            {
                Debug.Log(string.Format("{0} , {1} , {2}", Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
                float newPos = -(Input.mousePosition.y - 224f) * 100 / 144;
                if (newPos < 0)
                {
                    newPos = 0;
                }
                if (newPos > 100)
                {
                    newPos = 100;
                }
                ScrollTo(newPos);
            }
        }

        /**
        *   @brief MÃ©todo para calcular los limites del scroll.
        */
        public void calculateLimits()
        {

            if (!compInitialized)
            {
                initComponents();
                compInitialized = true;
            }

            scrollYValAct = 0;
            posicionesIniciales = new Dictionary<GameObject, float[]>();
            Transform[] trans = gameObject.transform.GetComponentsInChildren<Transform>();

            bool firstMeasure = true;

            foreach (Transform trAct in trans)
            {
                if (trAct.gameObject == this.gameObject || trAct.parent != this.transform || !trAct.gameObject.GetComponent<Renderer>().enabled)
                {
                    continue;
                }
                if (trAct.gameObject.name == "BotArriba" || trAct.gameObject.name == "Barra" || trAct.gameObject.name == "BotAbajo" || trAct.gameObject.name == "BotPpal")
                {
                    continue;
                }

                posicionesIniciales[trAct.gameObject] = new float[3] {
										trAct.localPosition.x,
										trAct.localPosition.y,
										trAct.localPosition.z
								};

                if (firstMeasure)
                {
                    minY = trAct.localPosition.z;
                    maxY = trAct.localPosition.z;
                }
                else
                {

                    if (minY > trAct.localPosition.z)
                    {
                        minY = trAct.localPosition.z;
                    }

                    if (maxY < trAct.localPosition.z)
                    {
                        maxY = trAct.localPosition.z;
                    }
                }

                firstMeasure = false;
            }

            ScrollTo(0);
        }

        /**
        *   @brief MÃ©todo para mover el scroll en Y.
        *
        *   @param valMov Cantidad a mover en Y.
        */
        public void moveScrollY(float valMov)
        {
            float newVal = scrollYValAct + (valMov * desfasY);

            if (newVal < 0)
            {
                newVal = 0;
            }
            else if (newVal > 100)
            {
                newVal = 100;
            }

            ScrollTo(newVal);
        }

        /**
        *   @brief MÃ©todo asignar una nueva posicion en Y al scroll.
        *
        *   @param valMov Valor en Y a ser asignado como nueva posiciÃ³n del scroll.
        */
        public void SetNewPositionY(float valMov)
        {

            float newVal = scrollYValAct + (valMov * desfasY);

            if (newVal < 0)
            {
                newVal = 0;
            }
            else if (newVal > 100)
            {
                newVal = 100;
            }

            scrollYValAct = newVal;
            ScrollTo(newVal);
        }

        /**
        *   @brief MÃ©todo para mover el scroll a una posicion determinada que llega por parametro.
        *
        *   @param valScroll Cantidad que indica a donde se movera el scroll.
        */
        public void ScrollTo(float valScroll)
        {
            if (goBotBarra == null)
            {
                return;
            }

            float posBarraBot = -3.3f + (7.3f * valScroll / 100);
            goBotBarra.transform.localPosition = new Vector3(valIniBarra.x, valIniBarra.y, posBarraBot);

            Transform[] trans = gameObject.transform.GetComponentsInChildren<Transform>();

            foreach (Transform trAct in trans)
            {
                if (trAct.gameObject == this.gameObject || trAct.parent != this.transform || !trAct.gameObject.GetComponent<Renderer>().enabled)
                {
                    continue;
                }
                if (trAct.gameObject.name == "BotArriba" || trAct.gameObject.name == "Barra" || trAct.gameObject.name == "BotAbajo" || trAct.gameObject.name == "BotPpal")
                {
                    continue;
                }

                float[] posIniciales;

                if (posicionesIniciales.TryGetValue(trAct.gameObject, out posIniciales))
                {
                    Vector3 vec = new Vector3(posIniciales[0], posIniciales[1], posIniciales[2]);
                    vec.z += ((offSetY + minY - maxY) * valScroll / 100);
                    trAct.localPosition = vec;
                }
            }
        }

        /**
        *   @brief MÃ©todo para la llamada del metodo asigna la ultima posicion en y cuando se da click sobre el scroll.
        */
        public void OnMouseDown()
        {
            lastPosY = Input.mousePosition.y;
        }

        /**
        *   @brief MÃ©todo para la llamada del metodo que mueve el scroll cuando se arrastra este scroll.
        */
        public void OnMouseDrag()
        {
            moveScrollY(Input.mousePosition.y - lastPosY);
        }

        /**
        *   @brief MÃ©todo para la llamada del metodo que asigna la nueva posicion del scroll cuando se suelta el click del scroll.
        */
        public void OnMouseUp()
        {
            SetNewPositionY(Input.mousePosition.y - lastPosY);
        }

        /**
        *   @brief  Sobreescritura del mÃ©todo toString().
        *   
        *   MÃ©todo encargado de representar la clase en forma de texto de una manera coherente. 
        *   
        *   @return Cadena de caracteres representando la clase actual.
        */
        public override string ToString()
        {
            return string.Format("EthScroll3D ({0})", name);
        }
    }
}
