using UnityEngine;



using System;
using Assets.Scripts.com.ethereal.util;

namespace Assets.Scripts.com.ethereal.display.components3D
{

    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Marzo 25 2015
    * 
    *	@class 	EthButton3D
    *   @brief 	Esta clase esta encargada de generar los botones en 3D.
    *
    */
    public class EthButton3D : EthComponent3D
    {

        /*
        *	MÃ©todo ejecutado para detectar los eventos de un click, que luego sera modificado con 
        *	respecto a como se desee utilizar manteniendo su estructura.
        */
        public delegate void OnClickEvent(string name);

        /*
        *	Evento llamado cuando se da click sobre el botÃ³n.
        */
        public event OnClickEvent OnClick;

        /*
        *	MÃ©todo ejecutado para detectar los eventos del mouse, que luego sera modificado con 
        *	respecto a como se desee utilizar manteniendo su estructura.
        */
        public delegate void OnMouseEvent(string eventName, string name);

        /*
        *	Evento llamado para los movimientos del mouse.
        */
        public event OnMouseEvent OnMouse;

        /*
        *	@brief Efecto que se le aplica por programaciÃ³n a un Texto 3D para cambiar la apariencia.
        */
        protected Shader shaderTextAct;

        /*
        *	@brief Material o base de un objeto 3d que define la apariencia de este.
        */
        protected Material matAct;

        /*
        *	@brief Booleano para saber si ya se creÃ³ el shader.
        */
        protected bool shaderCreated = false;

        /*
        *	@brief Url de las textura que tendrÃ­a el botÃ³n en el estado normal.
        */
        protected string urlNormal;

        /*
        *	@brief Url de las textura que tendrÃ­a el botÃ³n en el estado hover.
        */
        protected string urlHover;

        /*
        *	@brief Ultima posicion en X del botÃ³n.
        */
        protected float lastPosX;

        /*
        *	@brief Ultima posicion en Y del botÃ³n.
        */
        protected float lastPosY;

        /*
        *	@brief Estado del click del botÃ³n.
        */
        protected bool clickState;

        /*
        *	@brief Espacio entre botones.
        */
        protected int threshold = 5;

        /*
        *	@brief Valor para indicar si el click esta presionado o no.
        */
        public bool pressed = false;

        /*
        *	@brief Scroll 3D en el cual podria ir un boton 3D para este scroll.
        */
        protected EthScroll3D scroll;

        /*
        *	@brief Previene que se muestre el scroll.
        */
        public bool preventScroll = false;

        /*
        *	@brief Booleano para indicar si los click se estan ignorando o no.
        */
        public bool ignoreClicks = false;

        /**
        *	@brief Constructor de la clase EthButton3D.
        *
        *	Este mÃ©todo permite crear una instancia de la clase ethButton3D
        *
        */
        public EthButton3D()
            : base()
        {
        }

        /**
        *	@brief MÃ©todo para asignar los argumentos a un boton3D
        *
        *	Este mÃ©todo permite la asignaciÃ³n de todos los argumentos que contienen los datos con la informaciÃ³n que llevaran 
        *	los atributos de la clase buton3D.
        *	
        *	@param args objeto de la clase EthArguments, el cual posee el diccionario de datos con la informaciÃ³n de los 
        *	atributos de la clase.
        */
        public override void SetArgs(EthArguments args)
        {

            if (args["img"] != null)
            {

                if (!shaderCreated)
                {
                    shaderTextAct = Shader.Find("Transparent/Cutout/Soft Edge Unlit");
                    matAct = new Material(shaderTextAct);
                    shaderCreated = true;
                }

                urlNormal = args["img"];
                SetTexture("normal");
                urlHover = urlNormal + "_pressed";
                Texture2D textAct = (Texture2D)Resources.Load(urlHover);

                if (textAct == null)
                {
                    urlHover = urlNormal;
                }
            }

            //Variaciones de posicion
            float x = Convert.ToSingle((string)Eth.GetVal(args["x"], "" + transform.position.x));
            float y = Convert.ToSingle((string)Eth.GetVal(args["y"], "" + transform.position.y));
            transform.position = new Vector3(x, y, transform.position.z);

            //Variaciones de escala
            float wid = Convert.ToSingle((string)Eth.GetVal(args["width"], "" + transform.localScale.x));
            float hei = Convert.ToSingle((string)Eth.GetVal(args["height"], "" + transform.localScale.y));
            transform.localScale = new Vector3(wid, hei, transform.localScale.z);

        }

        /**
        *	@brief MÃ©todo para asignar la textura encontrada en el url especificado.
        *
        *	Este mÃ©todo permite la asignacion de la textura que tendra el botÃ³n, siendo esta la que se encuentra en la direccion 
        *	especificada por la url que entra por parÃ¡metro.
        *	
        *	@param url Direccion en donde se especifica la ruta en donde se aloja la textura que tendra el botÃ³n
        */
        public virtual void SetTexture(string url)
        {

            if (shaderTextAct == null)
            {
                return;
            }

            url = url == "normal" ? urlNormal : urlHover;

            Material[] materiales = (Material[])Eth.GetVal(GetComponent<Renderer>().materials, new Material[1]);

            Texture2D textAct = (Texture2D)Resources.Load(url);
            matAct = new Material(shaderTextAct);
            matAct.SetTexture("_MainTex", textAct);
            materiales[0] = matAct;

            GetComponent<Renderer>().materials = materiales;
        }

        /**
        *	@brief MÃ©todo para establecer el evento a suceder al cabo de ser soltado el botÃ³n.
        *
        *	Este mÃ©todo permite volver a establecer la textura del botÃ³n como normalmente debe ser una ves se deje de presionar el boton, ademÃ¡s de 
        *	ejecutar un evento determinado.
        *	
        */
        public override void _EthOnMouseUp()
        {
            if (clickState)
            {
                clickState = false;
                EthOnMouseUp();
                SetTexture("normal");
                mouseEvent("mouseUp");
            }

            pressed = false;

            if (scroll != null)
            {
                scroll.SetNewPositionY(Input.mousePosition.y - lastPosY);
            }
        }

        /**
        *	@brief MÃ©todo para establecer el evento a suceder al cabo de ser presionado el botÃ³n.
        *
        *	Este mÃ©todo permite establecer la textura determinada del botÃ³n que debe tener cuando se presion, ademÃ¡s de 
        *	ejecutar un evento determinado y guardar la ultima posiciÃ³n del mouse.
        *	
        */
        public override void _EthOnMouseDown()
        {

            lastPosX = Input.mousePosition.x;
            lastPosY = Input.mousePosition.y;

            if (ignoreClicks)
            {
                return;
            }

            SetTexture("hover");
            EthOnMouseDown();
            clickState = true;
            mouseEvent("mouseDown");
            pressed = true;
        }

        /**
        *	@brief MÃ©todo para llamar a la funciÃ³n que se debe ejecutar cuando se presiona un botÃ³n o el mouse actua como botÃ³n.
        *
        *	Si el objeto esta renderizado acciona el click de este al momento del mouse presionarlo.
        *	
        */
        public override void _EthOnMouseUpAsButton()
        {
            if (clickState)
            {
                _EthOnMouseUp();
                if (gameObject.GetComponent<Renderer>().enabled)
                {
                    click();
                }
            }
        }

        /**
        *	@brief MÃ©todo para establecer lo que debe suceder al arrastrar el mouse.
        *
        */
        public override void _EthOnMouseDrag()
        {
            //if(ignoreDrags)return;

            if (!clickState)
            {
                mouseEvent("drag");

                if (preventScroll)
                {
                    return;
                }

                if (scroll == null)
                {
                    scroll = (components3D.EthScroll3D)getEthParent(typeof(components3D.EthScroll3D));
                }

                //si tiene scroll
                if (scroll != null)
                {
                    //Debug.Log(lastPosY);
                    scroll.moveScrollY(Input.mousePosition.y - lastPosY);
                }
            }
            else if (Input.mousePosition.x < lastPosX - threshold || Input.mousePosition.x > lastPosX + threshold || Input.mousePosition.y < lastPosY - threshold || Input.mousePosition.y > lastPosY + threshold)
            {
                _EthOnMouseUp();
            }
        }

        /*
		*	@brief MÃ©todo para llamar al evento _EthOnMouseDown
		*/
        public void OnMouseDown()
        {
            _EthOnMouseDown();
        }

        /*
		*	@brief MÃ©todo para llamar al evento _EthOnMouseUpAsButton
		*/
        public void OnMouseUpAsButton()
        {
            _EthOnMouseUpAsButton();
        }

        /*
		*	@brief MÃ©todo para llamar al evento _EthOnMouseUp
		*/
        public void OnMouseUp()
        {
            _EthOnMouseUp();
        }

        /*
		*	@brief MÃ©todo para llamar al evento _EthOnMouseDrag
		*/
        public void OnMouseDrag()
        {
            _EthOnMouseDrag();
        }

        /**
        *	@brief MÃ©todo para asignar la evento.
        *
        *	Asigna un evento cuando se da click.
        */
        public void setFunction(OnClickEvent fn)
        {
            OnClick += fn;
        }

        /**
        *	@brief MÃ©todo para asignar las funciones de los eventos del mouse.
        *
        */
        public void SetFunctionEvents(OnMouseEvent fn)
        {
            OnMouse += fn;
        }

        /**
        *	@brief MÃ©todo para asignar los eventos del click.
        */
        public virtual void click()
        {
            if (OnClick != null)
            {
                OnClick(name);
            }
            mouseEvent("click");
        }

        /**
        *	@brief MÃ©todo para asignar los eventos del mouse.
        */
        public virtual void mouseEvent(string eventName)
        {
            if (OnMouse != null)
            {
                OnMouse(eventName, name);
            }
        }

        /**
        *	@brief	Sobreescritura del mÃ©todo toString().
        *	
        *	MÃ©todo encargado de representar la clase en forma de texto de una manera coherente. 
        *	
        *	@return Cadena de caracteres representando la clase actual.
        */
        public override string ToString()
        {
            return "EthButton3D (" + name + ")";
        }

        
    }
}
