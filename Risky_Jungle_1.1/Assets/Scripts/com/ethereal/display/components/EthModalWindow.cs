using UnityEngine;
using System;
using Assets.Scripts.com.ethereal.util;

namespace Assets.Scripts.com.ethereal.display.components
{

    /** 
    *   @author    EtherealGF <www.etherealgf.com> 
    *   @version   1.0 
    *   @date      Octubre 16 2014
    * 
    *   @class  EthModalWindow
    *   @brief  Esta clase se encarga de crear una ventana modal que puede ser usada para mostrar información.
    *
    */
    public class EthModalWindow : EthComponent
    {

        /**
        *   @brief EthComponentManager al que pertenece la EthModalWindow.
        */
        new public EthComponentManager gui;

        /**
        *   @brief Estilo de la EthModalWindow.
        */
        protected GUIStyle guiBack;
        
        /**
        *   @brief Textura de fondo de la EthModalWindow.
        */
        private Texture2D _textureBack;

        /**
        *   @brief Asignación de las propiedades de lectura y escritura de la variable _textureBack.
        *
        *   @return El valor de la variable _textureBack.
        */
        public UnityEngine.Texture2D TextureBack
        {
            get { return _textureBack; }
            set { _textureBack = value; }
        }
        
        /**
        *   @brief Variable que define sí existe una ventana modál.
        */
        private static bool _hasModalWindow = false;

        /**
        *   @brief Asignación de las propiedades de lectura y escritura de la variable _hasModalWindow.
        *
        *   @return El valor de la variable _hasModalWindow.
        */
        public static bool HasModalWindow
        {
            get { return _hasModalWindow; }
            set { _hasModalWindow = value; }
        }
        
        /**
        *   @brief Variable que define sí la EthModalWindow tiene imagen de fondo.
        */
        private bool _hasTextureBack = false;

        /**
        *   @brief Asignación de las propiedades de lectura y escritura de la variable _hasTextureBack.
        *
        *   @return El valor de la variable _hasTextureBack.
        */
        public bool HasTextureBack
        {
            get { return _hasTextureBack; }
            set { _hasTextureBack = value; }
        }
        
        /**
        *   @brief Vector que define el movimiento de la EthModalWindow en (0,0).
        */
        private Vector2 offsetScreen = Vector2.zero;

        /**
        *	@brief constructor de la clase EthModalWindow.
        *
        *	este método permite crear una instancia de la clase EthModalWindow
        *
        *	@param parentGUI
        *	@param args
        *	@param imagenBack
        *	@param nomTextura
        */
        public EthModalWindow(EthComponentManager parentGUI, bool imagenBack = false, string nomTextura = "", bool hasBackground = true)
            : base(parentGUI)
        {

            gui = new EthComponentManager(parentGUI);
            guiBack = new GUIStyle();

            _hasTextureBack = imagenBack;

            //Crea la textura para el back o cortina
            if (hasBackground)
            {
                if (imagenBack)
                {
                    _textureBack = Resources.Load(nomTextura) as Texture2D;

                    if (_textureBack == null)
                    {
                        throw new System.ArgumentException("No se encuentra la imagen " + nomTextura, Eth.IMG);
                    }

                    offsetScreen = new Vector2(((854 * gui.WRatio) - Screen.width) / 2, -((480 * gui.HRatio) - Screen.height) / 2);
                }
                else
                {
                    _textureBack = new Texture2D(1, 1);
                    _textureBack.SetPixel(0, 0, new Color(0, 0, 0, 0.3f));
                    _textureBack.Apply();
                }

                guiBack.normal.background = _textureBack;
            }
            _hasModalWindow = true;
        }

        /**
        *	@brief Método para dibujar una EthModalWindow.
        *
        *	@param offset Parámetro de tipo Vector2 para ser dibujado, el cual támbien puede ser nulo.
        *
        *	@see com.ethereal.display.components.EthComponent
        */
        public override void Draw(Vector2? offset = null)
        {

            if (!_hasTextureBack)
            {
                GUI.ModalWindow(0, new Rect(0, 0, Screen.width, Screen.height), DoMyWindow, "", guiBack);
            }
            else
            {
                GUI.ModalWindow(0, new Rect(-offsetScreen.x, -offsetScreen.y, 854 * gui.WRatio, 480 * gui.HRatio), DoMyWindow, "", guiBack);
            }
        }

        /**
        *	@brief Método para modificar la variable textureBack y si esta variable es nula, se arroja una excepción.
        *
        *	@param path Trayectoria de textureBack (Texture2D).
        */
        public void SetTextureBack(string path)
        {
            _textureBack = Resources.Load(path) as Texture2D;
            if (_textureBack == null)
            {
                throw new System.ArgumentException("No se encuentra la imagen " + path, Eth.IMG);
            }
        }

        /**
        *	@brief Método para dibujar la ventana modál.
        *
        *	@param windowID Id de la ventana modál.
        */
        public void DoMyWindow(int windowID)
        {
            gui.Draw(new Vector2(offsetScreen.x / gui.WRatio, offsetScreen.y / gui.HRatio));
        }

        /**
        *	@brief Método ToString que retorna el nombre de la clase.
        *
        *	@return Nombre de la clase, EthModalWindow.
        */
        public override string ToString()
        {
            return string.Format("EthModalWindow ({0})", Name);
        }

        /**
        *	@brief Método para eliminar un EthMoalWindow de su EthComponentManager.
        *
        *	@param callParent Parámetro que define si se remueve el EthComponent, si no se ingresa se tomará true por defecto.
        *
        *	@see com.ethereal.display.components.EthComponent
        */
        public override void Remove(bool callParent = true)
        {
            if (callParent)
            {
                gui.RemoveComponent(this);
            }
            _hasModalWindow = false;
        }
    }
}
