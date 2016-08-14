using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts.com.ethereal.util;

/*
Ejemplo de uso:
											x  y
EthTexture textur = gui.AddTexture("Textu",10,10,"img:fondoDialog,w:500,h:300"); 

useLangImgs -> indica si para la imagen se usa lang o no (true o false), se utiliza en ese caso img_es o img_en
url->se envia la url donde esta el video sin la extension, para web es ogg y movil es mp4, si es web no hay que enviar http://
w-> Ancho del cuadro
h-> Alto del cuadro
*/

namespace Assets.Scripts.com.ethereal.display.components
{

    /** 
    *   @author    EtherealGF <www.etherealgf.com> 
    *   @version   1.0 
    *   @date      Octubre 16 2014
    * 
    *   @class  EthVideo
    *   @brief  Esta clase se encarga de crear la ventana para reproducir un video.
    *
    */
    public class EthVideo : EthComponent
    {

        /**
        *   @brief Estilo del EthVideo.
        */
        protected GUIStyle guiBot;

        // Use this for initialization
        /**
        *   @brief URL para cargar el video.
        */
        public WWW wwwData;

#if UNITY_WEBPLAYER || UNITY_EDITOR || UNITY_STANDALONE_WIN
        /**
        *   @brief Textura sobre la cual el video va a ser reproducido, esta variable se inicializa solo si el navegador es web.
        */
        private MovieTexture m;
#endif

        /**
        *   @brief Modo de escala para dibujar texturas.
        */
        private ScaleMode mode;

        /**
        *   @brief Textura para el marco.
        */
        private Texture2D marco;

        /**
        *   @brief Estilo del fondo.
        */
        protected GUIStyle guiBack;

        /**
        *   @brief Testura de fondo.
        */
        private Texture2D textureBack;

        /**
        *   @brief Nombre del EthVideo.
        */
        private string _videoName;

        /**
        *   @brief GUI padre de la reproducciÃ³n del video.
        */
        EthGUIScreen parent;

        /**
        *   @brief Lenguaje del video, dependiendo del dispositivo.
        */
        private string compLanguage = "";

        /**
        *   @brief Variable que define si el video se cierra con un clic en navegadores.
        */
        private bool webOutOnClic = false;

        /**
        *   @brief Variable que define sÃ­ el video se esta reproduciendo.
        */
        private bool playing;

        /**
        *   @brief Vector que describe el ultimo movimiento.
        */
        private Vector2 lastOffset;

        /**
        *   @brief Textura con caracterÃ­sticas de transparencia.
        */
        private Texture2D transparent;

        /**
        *	@brief Constructor de la clase EthVideo.
        *
        *	Este mÃ©todo permite crear una instancia de la clase EthVideo.
        *
        *	@param parent 		EthGUIScreen en la cual se mostrara el video.
        *	@param args 		Parametros con los cuales se crearÃ¡ el objeto.
        *	@param parentGUI 	EthComponentManager al cual pertenecerÃ¡ el objeto.
        */
        public EthVideo(EthGUIScreen parent, EthArguments args, EthComponentManager parentGUI)
            : base(args, parentGUI)
        {
            this.parent = parent;

            guiBack = new GUIStyle();

            //Crea la textura para el back o cortina
            textureBack = new Texture2D(1, 1);
            textureBack.SetPixel(0, 0, new Color(0, 0, 0, 0.8f));
            textureBack.Apply();

            playing = false;

            guiBack.normal.background = textureBack;
            ReloadArguments(args);
        }

        /**
        *	@brief MÃ©todo para recargar los argumentos del componente.
        *
        *	@param args Argumentos que tendrÃ¡ el componente.
        */
        public override void ReloadArguments(EthArguments args)
        {
            if (args["url"] != null)
            {
#if UNITY_WEBPLAYER || UNITY_EDITOR

                if (_useLang)
                {
                    compLanguage = EthLang.LangAct;
                }
                wwwData = new WWW(string.Format("https://{0}{1}.ogv", args["url"], compLanguage));
                Debug.Log(string.Format("url: https://{0}{1}.ogv", args["url"], compLanguage));

                m = wwwData.movie;

                this.mode = ScaleMode.StretchToFill;

                if (args["scaleMode"] != null)
                {
                    switch (args["scaleMode"])
                    {
                        case "ScaleAndCrop": this.mode = ScaleMode.ScaleAndCrop;
                            break;

                        case "ScaleToFit": this.mode = ScaleMode.ScaleToFit;
                            break;
                    }
                }

                if (args["webOutOnClic"] != null)
                {
                    transparent = new Texture2D(1, 1);
                    transparent.SetPixel(0, 0, Color.clear);
                    transparent.Apply();
                    webOutOnClic = bool.Parse(args["webOutOnClic"]);
                }


                Wid=m.width;
                Hei=m.height;

                if (args["h"] != null) Hei=float.Parse(args["h"]);
                if (args["w"] != null) Wid=float.Parse(args["w"]);

                if (args["marco"] != null)
                {
                    marco = Resources.Load(args["marco"]) as Texture2D;

                }

#endif

#if UNITY_STANDALONE_WIN 

				 	if ( _useLang ) {
						compLanguage = EthLang.LangAct;
					} 
					string[] arrUrl =  args["url"].Split('/');
				 	_videoName = arrUrl[arrUrl.Length - 1] + compLanguage + ".ogv";

				 	wwwData = new WWW( "file://" +Application.streamingAssetsPath + "/" +  _videoName );
				 					 	
				 	m = wwwData.movie;

				    this.mode = ScaleMode.StretchToFill;

				    if ( args["scaleMode"] != null ) {
				    	switch( args["scaleMode"] ) {
				    		case "ScaleAndCrop": this.mode = ScaleMode.ScaleAndCrop;
				    			break;

				    		case "ScaleToFit": this.mode = ScaleMode.ScaleToFit;
				    			break;
				    	}
				    }

				    if(args["webOutOnClic"] != null) {
				    	transparent = new Texture2D(1,1);
						transparent.SetPixel(0,0, Color.clear);
						transparent.Apply();
						webOutOnClic = bool.Parse(args["webOutOnClic"]);
				    } 


				    Wid=m.width;
					Hei=m.height;

					if(args["h"] != null) {
						Hei=float.Parse(args["h"]);
					}
					if(args["w"] != null) {
						Wid= float.Parse(args["w"]);
					}

				 	if(args["marco"] != null) {
						marco = Resources.Load(args["marco"]) as Texture2D;
					}

#endif

#if UNITY_IPHONE || UNITY_ANDROID
                if (_useLang)
                {
                    compLanguage = EthLang.LangAct;
                }
                string[] arrUrl = args["url"].Split('/');
                _videoName = arrUrl[arrUrl.Length - 1] + compLanguage + ".mp4";
                parent.StartCoroutine(PlayVideo());
#endif

            }
        }

        /**
        *	@brief MÃ©todo para reproducir el video en plataformas android o IOS.
        */
#if UNITY_IPHONE || UNITY_ANDROID
        protected IEnumerator PlayVideo()
        {

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                // TODO Arreglar esto que causaba problemas en unity 5
                // iPhoneUtils.PlayMovie(_videoName, Color.black, FullScreenMovieControlMode.CancelOnTouch, FullScreenMovieScalingMode.AspectFit);
            }
            else
            {
                Handheld.PlayFullScreenMovie(_videoName, Color.black, FullScreenMovieControlMode.CancelOnInput);
            }
            yield return new WaitForEndOfFrame();
        }
#endif

        /**
		*	@brief MÃ©todo para mostrar un EthVideo.
		*
		*	@param offset ParÃ¡metro de tipo Vector2 para ser mostrado, el cual tÃ¡mbien puede ser nulo.
		*
		*	@see com.ethereal.display.components.EthComponent
		*/
        public override void Draw(Vector2? offset = null)
        {
#if UNITY_WEBPLAYER || UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (!m.isPlaying && m.isReadyToPlay)
            {
                playing = true;
                m.Play();
            }

#endif

            if (!_visible)
            {
                return;
            }

            lastOffset = offset ?? Vector2.zero;

#if UNITY_WEBPLAYER || UNITY_EDITOR || UNITY_STANDALONE_WIN
            GUI.ModalWindow(3, new Rect(0, 0, Screen.width, Screen.height), DoMyWindow, "", guiBack);
#endif
        }

        /**
        *	@brief MÃ©todo para reproducir el video en navegadores.
        *
        *	@param windowID ID de la ventana en que se reproducirÃ¡ el video.
        */
#if UNITY_WEBPLAYER || UNITY_EDITOR || UNITY_STANDALONE_WIN
        void DoMyWindow(int windowID)
        {

            float xTemp = X + lastOffset.x;
            float yTemp = Y + lastOffset.y;

            if (webOutOnClic)
            {
                if (GUI.Button(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), transparent))
                {
                    m.Stop();
                    Remove();
                }
            }

            if (m != null)
            {
                GUI.DrawTexture(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.WRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), m, mode);
            }

            if (marco != null)
            {
                GUI.DrawTexture(new Rect((xTemp * _gui.WRatio) + _gui.WOffset, (yTemp * _gui.HRatio) + _gui.HOffset, Wid * _gui.WRatio, Hei * _gui.HRatio), marco);
            }

        }
#endif

        /**
		*	@brief MÃ©todo ToString que retorna el nombre de la clase y el nombre del objeto.
		*
		*	@return Nombre de la clase concatenado con el nombre del objeto.
		*/
        public override string ToString()
        {
            return "EthTexture (" + Name + ")";
        }
    }
}
