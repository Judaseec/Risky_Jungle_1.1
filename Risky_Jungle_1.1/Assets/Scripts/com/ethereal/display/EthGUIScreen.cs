using UnityEngine;
using Boomlagoon.JSON;
using System;
using System.Collections;
using System.Collections.Generic;

using com.ethereal.display.components;
using Assets.Scripts.com.ethereal.display.easing.time;
using Assets.Scripts.com.ethereal.display.components;
using Assets.Scripts.com.ethereal.audio;
using Assets.Scripts.com.ethereal.util;



/**
 * Tener en cuenta que una clase que herede de esta debe tener constructor vacio, ya que si no lo tiene el objeto no 
 * se inicializa correctamente.
 */
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
    public class EthGUIScreen : MonoBehaviour
    {
        /**
        *   @brief ProporciÃ³n del ancho.
        */
        public float wRatio = 1f;

        /**
        *   @brief Proporción del alto.
        */
        public float hRatio = 1f;

        /**
         *  @brief Desplazamiento horizontal
         */
        public float wOffset = 0f;

        /**
         *  @brief Desplazamiento vertical
         */
        public float hOffset = 0f;

        /**
         *  @brief Desplazamiento de los componentes en un vector
         */
        private Vector2 offsetComponents = new Vector2(0, 0);

        /**
         *  @brief Array que contiene los items de la pantalla.
         */
        private ArrayList arrayItemsScreen;

        /**
         *  @brief Variable pra definir si se usa una animación de entrada.
         */
        private bool animateEntry = false;

        /**
         *  @brief Variable para definir si se toma la primera pantalla.
         */
        private bool firstScreenTaken = false;

        /**
         *  @brief Variable que identifica cuando la pantalla se queda congelada.
         */
        private bool frozen = false;


        private ScreenTimeEase animat;

        /**
         *  @brief EthComponentManager de la pantalla.
         */
        private EthComponentManager _gui;

        /**
         *  @brief Diccionario qeu contiene la informacion de la visibilidad de los botones que contiene el nombre y su estado.
         */
        private Dictionary<string, bool> buttonsVisibility = new Dictionary<string, bool>();

        /**
         *  @brief Escena a ser cargada.
         */
        private string sceneToLoad = "";

        /**
         *  @brief Instancia de la clase EthGameObject 
         */
        public EthGameObject obj;

        /**
         *  @brief Variable para definir si ha sido inicializada la pantalla
         */
        public bool initialized = false;

        /**
         *  @brief Variable para saber si se encuentra activa la tecla de retorno
         */
        protected bool _activeReturnKey = false;

        /**
         *  @brief variable que indica si la pantalla soporta los controles del vita.
         */
        private bool _vitaSupport = false;

        /**
         *   @brief MÃ©todo para Instanciar un EthGUIScreen.
         *   
         *   Este mÃ©todo es el encargado de crear un nuevo EthGUIScreen asignando todos los atributos que requiere esta clase.
         */
        public EthGUIScreen()
        {
        }

        /**
		*	@brief AsignaciÃ³n de las propiedades de lectura y escritura de la variable _gui.
        *  
        *   @return El valor de la variable _gui.
		*/
        public EthComponentManager gui
        {
            get
            {
                if (_gui == null)
                {
                    StartData();
                }
                return _gui;
            }

            private set
            {
            }
        }

        /**
        *	@brief Propiedades de lectura y escritura de la clase EthGameObject.
        *	
        *   @param index Ãndice de la clase.
        *
        *	@return El objeto EthGameObject.
        */
        public EthGameObject this[string index]
        {
            get
            {
                if (obj == null)
                {
                    obj = new EthGameObject(gameObject);
                }
                return obj.GetChildByName(index);
            }
        }

        /**
		*	@brief Metodo para centrar el screen en un objeto, ingresando las dimensiones de este por parametro.
        *  
        *   @param objectWidth  Ancho del objeto.
        *   @param objectHeight Alto del objeto.
		*/
        public void CenterScreen(float objectWidth, float objectHeight)
        {

            EthAudio.GetInstance(this).RefreshPos();

            if (!initialized)
            {
                this.StartData();
            }

            wRatio = Screen.width / objectWidth;
            hRatio = Screen.height / objectHeight;

            if (wRatio < hRatio)
            {
                hOffset = (Screen.height - (objectHeight * wRatio)) / 2;
                hRatio = wRatio;
            }
            else
            {
                wOffset = (Screen.width - (objectWidth * hRatio)) / 2;
                wRatio = hRatio;
            }

            gui.CenterScreen(objectWidth, objectHeight);
        }

        /**
		 *	@brief AsignaciÃ³n una pantalla como una sub pantalla
         *  
         *   @param objectWidth Anchura del objeto.
         *   @param wDesired    Ancho deseado.
         *   @param posX        Posicion en X que quedara la pantalla.
         *   @param posY        Posicion en Y que quedara la pantalla.
		 */
        protected void UseAsSubScreen(float objectWidth, float wDesired, float posX, float posY)
        {
            wRatio = wDesired / objectWidth;

            hOffset = posY;
            wOffset = posX;
            hRatio = wRatio;
        }

        /**
		 *	 @brief MÃ©todo usado para agregar una nueva animaciÃ³n.
         *  
         *   @param typeAnim Tipo de animaciÃ³n.
		 */
        public void EntryAnimation(string typeAnim)
        {
            animateEntry = true;
            animat = new ScreenTimeEase(Screen.width, Screen.height, 1500, "bottom", "easeOutElastic");

            animat.OnFinish += OnFinishAnimEntry;
        }

        /**
         *	@brief Metodo ejecutado al finalizaciÃ³n una animaciÃ³n entrante en donde se pone el estado en false.
         *	
         */
        public void OnFinishAnimEntry()
        {
            animateEntry = false;
        }

        /**
         *	@brief MÃ©todo usado para inicializar la informaciÃ³n y la ventana en donde irÃ¡ el screen.
         *	
         */
        public void StartData()
        {
            initialized = true;
            ManejadorVentanas.getInstance();
            _gui = new EthComponentManager(null);
            OnClick += OnButtonClick;
        }

        /**
        *	@brief MÃ©todo para manejar la carga de ventanas por nivel.
        *	
        *   @param level.
        */
        public void OnLevelWasLoaded(int level)
        {

            if (ManejadorVentanas.getInstance().OnLevelWasLoadedInvocado)
            {
                return;
            }
            ManejadorVentanas.getInstance().esceneLoaded();
        }

        /**
        *	@brief MÃ©todo usado para cargar una escena en la pantalla.
        *	
        *   @param scene Escena a cargar sobre la pantalla.
        */
        public void loadScene(string scene)
        {
            sceneToLoad = scene;
        }

        /**
        *	@brief MÃ©todo para actualizar la ventana.
        */
        protected virtual void Update()
        {
            if (sceneToLoad != null && sceneToLoad.Length > 0)
            {
                if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
                {
                    ManejadorVentanas.getInstance().OnLevelWasLoadedInvocado = false;
                    Application.LoadLevel(sceneToLoad);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_activeReturnKey)
                {
                    BackButtonPressed();
                }
            }
        }

        /**
         *  @brief Metodo ejecutado cuando se presiona el boton atras y esto hace que se devuelva a la ventana
         *  anterior.
         */
        protected virtual void BackButtonPressed()
        {
            ManejadorVentanas.getInstance().returnWindow();
        }

        /**
        *	@brief MÃ©todo para activar o desactivar la llave o tecla de retorno.
        *	
        *   @param activeReturnKey ParÃ¡metro que define la activaciÃ³n de la tecla de retorno.
        */
        protected void ActivateReturnKey(bool activeReturnKey)
        {
            this._activeReturnKey = activeReturnKey;
        }

        /**
         *  @brief MÃ©todo para cambiar el background de la ventana.
         *
         *  @param newBackGround background indicado en una ruta para ser obtenido.
         */
        protected void ChangeBackGround(string newBackGround)
        {
            Material matAct = GameObject.Find("BackVision").GetComponent<Renderer>().material;

            Texture2D textAct = (Texture2D)Resources.Load(newBackGround);

            matAct.SetTexture("_MainTex", textAct);
        }

        /**
         * Codigo agregado por Andres herrera .
         */
         /**
         *  @brief MÃ©todo para cambiar el background de la ventana.
         *
         *  @param newBackGround background indicado como un Texture2D.
         */
        protected void ChangeBackGround(Texture2D newBackGround)
        {
            Material matAct = GameObject.Find("BackVision").GetComponent<Renderer>().material;

            matAct.SetTexture("_MainTex", newBackGround);
        }

        /**
         *  @brief MÃ©todo para cargar la ventana.
         */
        public virtual void OnGUI()
        {
            if (animateEntry || frozen)
            {
                if (!firstScreenTaken)
                {
                    arrayItemsScreen = new ArrayList();
                    EthOnGUI();
                    firstScreenTaken = true;
                    animat.Start();
                }
                else
                {
                    float[] dataStep = animat.GetStep();

                    IEnumerator itemsGUI = arrayItemsScreen.GetEnumerator();

                    while (itemsGUI.MoveNext())
                    {
                        string tipoObj = (string)itemsGUI.Current;
                        itemsGUI.MoveNext();
                        ArrayList paramObj = (ArrayList)itemsGUI.Current;

                        switch (tipoObj)
                        {
                            case "DrawTexture_texture":
                                DrawTextureReal(dataStep[0] + (float)paramObj[0], dataStep[1] + (float)paramObj[1], dataStep[2] * (float)paramObj[2], dataStep[3] * (float)paramObj[3], (float)paramObj[4], (Texture)paramObj[5]);
                                break;
                        }
                    }
                }
            }
            else
            {
                arrayItemsScreen = new ArrayList();
                if (gui != null)
                {
                    gui.Draw(offsetComponents);
                }
                EthOnGUI();
                if (_vitaSupport)
                {
                    EthVitaInput.checkInputs();
                }
            }
        }

        /**
         *  @brief MÃ©todo para congelar la GUI
         */
        public void freezeGUI()
        {
            frozen = true;
        }

        protected virtual void EthOnGUI()
        {
        }

        /**
         *  @brief MÃ©todo para dibujar una textura en la ventana.
         *
         *  @param x TamaÃ±o en x de la textura
         *  @param y TamaÃ±o en y de la textura
         *  @param w ancho de la textura
         *  @param h alto de la textura
         *  @param alpha Transparencia de la textura
         *  @param texture Textura a dibujar
         */
        protected void DrawTexture(float x, float y, float w, float h, float alpha, Texture texture)
        {

            ArrayList objParams = new ArrayList();
            Eth.AddObjects(objParams, x, y, w, h, alpha, texture);

            arrayItemsScreen.Add("DrawTexture_texture");
            arrayItemsScreen.Add(objParams);

            DrawTextureReal(x, y, w, h, alpha, texture);
        }

        /**
         *  @brief MÃ©todo para dibujar un boton en la ventana.
         *
         *  @param x TamaÃ±o en x del botÃ³n.
         *  @param y TamaÃ±o en y del botÃ³n.
         *  @param w ancho del botÃ³n.
         *  @param h alto del botÃ³n.
         *  @param alpha Transparencia del boton
         *  @param texture Textura del boton a dibujar
         */
        protected void Button(string botName, float x, float y, float w, float h, float alpha, Texture texture)
        {

            ArrayList objParams = new ArrayList();
            Eth.AddObjects(objParams, x, y, w, h, alpha, texture);

            arrayItemsScreen.Add("DrawTexture_texture");
            arrayItemsScreen.Add(objParams);

            ButtonReal(botName, x, y, w, h, alpha, texture);
        }

        /**
         *  @brief Método ejecutado cuando ocurre un click sobre un botón.
         *
         *  @param botName Nombre del botón.
         */
        protected virtual void OnButtonClick(string botName)
        {
        }

        /**
         *  @brief Método para .
         *
         *  @param name 
         */
        public void DispatchClickEvent(string name)
        {
            if (OnClick != null)
            {
                OnClick(name);
            }
        }

        /**
         *  @brief MÃ©todo para dibujar una textura mas real.
         *
         *  @param x TamaÃ±o en x de la textura
         *  @param y TamaÃ±o en y de la textura
         *  @param w ancho de la textura
         *  @param h alto de la textura
         *  @param alpha Transparencia de la textura
         *  @param texture Textura a dibujar
         */
        private void DrawTextureReal(float x, float y, float w, float h, float alpha, Texture texture)
        {

            Color colPreviousGUIColor = GUI.color;
            GUI.color = new Color(colPreviousGUIColor.r, colPreviousGUIColor.g, colPreviousGUIColor.b, alpha);

            GUI.DrawTexture(new Rect((x * wRatio) + wOffset, (y * hRatio) + hOffset, w * wRatio, h * hRatio), texture);

            GUI.color = colPreviousGUIColor;
        }

        /**
         *  @brief MÃ©todo para dibujar un boton de textura mas real.
         *
         *  @param x TamaÃ±o en x de la textura
         *  @param y TamaÃ±o en y de la textura
         *  @param w ancho de la textura
         *  @param h alto de la textura
         *  @param alpha Transparencia de la textura
         *  @param texture Textura a dibujar
         */
        private void ButtonReal(string botName, float x, float y, float w, float h, float alpha, Texture texture)
        {

            Color colPreviousGUIColor = GUI.color;
            GUI.color = new Color(colPreviousGUIColor.r, colPreviousGUIColor.g, colPreviousGUIColor.b, alpha);

            bool ret = GUI.Button(new Rect((x * wRatio) + wOffset, (y * hRatio) + hOffset, w * wRatio, h * hRatio), texture, GUIStyle.none);

            GUI.color = colPreviousGUIColor;

            if (ret && OnClick != null)
            {
                OnClick(botName);
            }
        }

        /**
         *  @brief Método para remover todos los componentes.
         *
         */
        public void Remove()
        {
            if (gui != null)
            {
                gui.RemoveAllComponents();
            }
        }

        /**
         *  @brief Método para cambiar la visibilidad de un botón.
         *
         *  @param botName Nombre del boton
         *  @param value Valor true o false de la visibilidad
         */
        public void SetButtonVisibility(string botName, bool value)
        {
            buttonsVisibility[botName] = value;
        }

        /**
         *  @brief Método para obtener un componente por el nombre y el tipo de este.
         *
         *  @param name Nombre del componente.
         *  @param objectType Tipo del componente a obtener.
         *  
         *  @return Componente obtenido por el tipo.
         */
        public static Component GetClass(string name, Type objectType)
        {
            GameObject gameObject = GameObject.Find(name);
            return gameObject.GetComponent(objectType);
        }

        /**
         *  @brief Método para obtener el nombre y el tipo de un componente.
         *
         *  @param name Nombre del componente.
         *  @param objectType Tipo del componente a obtener.
         *  
         *  @return Componente obtenido por el tipo.
         */
        public static T GetClass<T>(string name) where T : Component
        {
            return (T)GetClass(name, typeof(T));
        }

        /**
         *  @brief Método para adicionar un prefab para reutilizar un elemento.
         *
         *  @param path direccion donde esta el elemento a usar nuevamente.
         *  
         *  @return Objeto gameobject de unity a ser reusado.
         */
        public static GameObject AddPrefab(string path)
        {
            GameObject obj = (GameObject)Instantiate(Resources.Load(path));
            return obj;
        }

        /**
         *  @brief Método para adicionar un prefab para reutilizar un elemento.
         *
         *  @param path dirección donde esta el elemento a usar nuevamente.
         *  @param objectType tipo del componente que se desea obtener a ser reusado
         *  
         *  @return Componente de unity a ser reusado.
         */
        public static Component AddPrefab(string path, Type objectType)
        {
            GameObject gameObject = (GameObject)Instantiate(Resources.Load(path));
            return gameObject.GetComponent(objectType);
        }

        /**
         *  @brief Método para adicionar un prefab para reutilizar un elemento.
         *
         *  @param path direccion donde esta el elemento a usar nuevamente.
         *  
         *  @return Objeto que hereda de un componente de unity a ser reusado.
         */
        public static T AddPrefab<T>(string path) where T : Component
        {
            return (T)AddPrefab(path, typeof(T));
        }

        public virtual void OnSessionEAS()
        {
        }

        /**
         *  @brief Método para setear el espacio entre los elementos.
         *
         *  @param vector de ecpacio entre elementos.
         */
        public void setOffsetComponents(Vector2 newOffset)
        {
            this.offsetComponents = newOffset;
        }

        /**
         *  @brief Método para setear si se usa el soporte del vita.
         *
         *  @param valor true o false para saber si se usa el support del vita.
         */
        public void setVitaSupport(bool value)
        {
            _vitaSupport = value;
            EthVitaInput.setScreen(this);
        }

        // Delegate
        /**
         * @brief Método para invocar un evento cuando se hace un click.
         * 
         * @param botName Nombre del botón.
         */
        public delegate void OnClickEvent(string botName);

        // The event
        /**
         * @brief Evento para detectar un cambio al darle click a un botón.
         */
        public event OnClickEvent OnClick;
    }
}
