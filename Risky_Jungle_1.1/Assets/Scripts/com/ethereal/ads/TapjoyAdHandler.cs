using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.com.ethereal.util;


namespace Assets.Scripts.com.ethereal.ads
{

    /** 
    *	@author    Carlos Andres Carvajal <andcarva@gmail.com> 
    * 	@version   1.0 
    * 	@date      Noviembre 7 2014
    * 
    *	@class 	TapjoyAdHandler
    *   @brief 	Esta clase es el manejador de publicidad del sistema tapjoy.
    *	
    *	Esta clase hereda de la Interface IAdhandler.
    */
    public class TapjoyAdHandler : IAdHandler
    {

        /**
        *	@brief Variable estática para permitir el logging en plataforma IOS.
        */
        private static string ENABLE_LOGGING_IOS = "TJC_OPTION_ENABLE_LOGGING";

        /**
        *	@brief Variable estática para permitir el logging en plataforma android.
        */
        private static string ENABLE_LOGGING_ANDROID = "enable_logging";

        /**
        *   @brief Constante para Ingresar como parametro al crear una instancia de la clase TapjoyAdHandler.
        */
        private const String TAPJOY_OBJECT_NAME = "TapjoyObject";

        /**
        *   @brief Constante para Ingresar como parametro al crear una instancia de la clase TapjoyAdHandler.
        */
        private const String TAPJOY_OBJECT_PLUGIN_NAME = "TapjoyPlugin";

        /**
        *   @brief Constante para mostrar en el log cuando se ejecuta el método init.
        */
        private const String START_TAPJOY_DEBUG_MSG = "va a iniciar tapjoy";

        /**
        *   @brief Constante que representa el ID de la aplicación en plataforma android.
        */
        private const String ID_APP_ANDROID_DATA_NAME = "idAppAndroid";

        /**
        *   @brief Constante que representa la llave secreta para hacer la conección mediante android a tapjoy. 
        */
        private const String KEY_APP_ANDROID_DATA_NAME = "keyAppAndroid";

        /**
        *   @brief Constante que se utiliza para crear una bandera de conexión en sistema IOS. 
        */
        private const String TJC_OPTION_COLLECT_MAC_ADDRESS_MSG = "TJC_OPTION_COLLECT_MAC_ADDRESS";

        /**
        *   @brief Constante que representa el ID de la aplicación en plataforma IOS.
        */
        private const String ID_APP_IOS_DATA_NAME = "idAppIos";

        /**
        *   @brief Constante que representa la llave secreta para hacer la conección mediante IOS a tapjoy. 
        */
        private const String KEY_APP_IOS_DATA_NAME = "keyAppIos";

        /**
        *   @brief Constante que se muestra en el log cuando se ejecuta el método setListeners.
        */
        private const String SET_LISTENERS_DEBUG_MSG = "C#: Awaking and adding Tapjoy Events";

        /**
        *   @brief Constante que se muestra en el log cuando la conección a tapjoy es exitosa.
        */
        private const String HANDLE_TAPJOY_CONNECT_SUCCESS_DEBUG_MSG = "C#: HandleTapjoyConnectSuccess";

        /**
        *   @brief Constante que se muestra en el log cuando la conección a tapjoy falla.
        */
        private const String HANDLE_TAPJOY_CONNECT_FAILED_DEBUG_MSG = "C#: HandleTapjoyConnectFailed";

        /**
        *   @brief Constante que se muestra en el log para indicar que se muestran los puntos acertados.
        */
        private const String HANDLE_GET_TAP_POINTS_SUCCEEDED_DEBUG_MSG = "C#: HandleGetTapPointsSucceeded: ";

        /**
        *   @brief Constante que se muestra en el log para indicar que se muestran los puntos no acertados.
        */
        private const String HANDLE_GET_TAP_POINTS_FAILED_DEBUG_MSG = "C#: HandleGetTapPointsFailed";

        /**
        *   @brief Constante que se muestra en el log para indicar que se muestran los puntos acertados gastados.
        */
        private const String HANDLE_SPEND_TAP_POINTS_SUCCEEDED_DEBUG_MSG = "C#: HandleSpendTapPointsSucceeded: ";

        /**
        *   @brief Constante que se muestra en el log para indicar que se muestran los puntos no acertados gastados.
        */
        private const String HANDLE_SPEND_TAP_POINTS_FAILED_DEBUG_MSG = "C#: HandleSpendTapPointsFailed";

        /**
        *   @brief Constante que se muestra en el log para indicar que se muestra la premiación de los puntos acertados.
        */
        private const String HANDLE_AWARD_TAP_POINTS_SUCCEEDED_DEBUG_MSG = "C#: HandleAwardTapPointsSucceeded";

        /**
        *   @brief Constante que se muestra en el log para indicar que se muestra la premiación de los puntos no acertados.
        */
        private const String HANDLE_AWARD_TAP_POINTS_FAILED_DEBUG_MSG = "C#: HandleAwardTapPointsFailed";

        /**
        *   @brief Constante que se muestra en el log para indicar que se muestran los puntos ganados.
        */
        private const String HANDLE_TAP_PONTS_EARNED_DEBUG_MSG = "C#: CurrencyEarned: ";

        /**
        *   @brief Constante que se muestra en el log para indicar que se está mostrando publicidad en pantalla completa.
        */
        private const String HANDLE_GET_FULL_SCREEN_AD_SUCCEEDED_DEBUG_MSG = "C#: HandleGetFullScreenAdSucceeded";

        /**
        *   @brief Constante que se muestra en el log para indicar que no se pudo mostrar publicidad en pantalla completa.
        */
        private const String HANDLE_GET_FULL_SCREEN_AD_FAILED_DEBUG_MSG = "C#: HandleGetFullScreenAdFailed";

        /**
        *   @brief Constante que se muestra en el log para indicar que el display de la publicidad se desplego exitosamente.
        */
        private const String HANDLE_GET_DISPLAY_AD_SUCCEEDED_DEBUG_MSG = "C#: HandleGetDisplayAdSucceeded";

        /**
        *   @brief Constante que se muestra en el log para indicar que el display de la publicidad no se desplego exitosamente.
        */
        private const String HANDLE_GET_DISPLAY_AD_FAILED_DEBUG_MSG = "C#: HandleGetDisplayAdFailed";

        /**
        *   @brief Constante que se muestra en el log cuando un video publicitario inicia.
        */
        private const String HANDLE_VIDEO_AD_STARTED_DEBUG_MSG = "C#: HandleVideoAdStarted";

        /**
        *   @brief Constante que se muestra en el log cuando un video publicitario falla al iniciar.
        */
        private const String HANDLE_VIDEO_AD_FAILED_DEBUG_MSG = "C#: HandleVideoAdFailed";

        /**
        *   @brief Constante que se muestra en el log cuando un video publicitario termina.
        */
        private const String HANDLE_VIDEO_AD_COMPLETED_DEBUG_MSG = "C#: HandleVideoAdCompleted";

        /**
        *   @brief Constante que se muestra en el log cuando la vista del tapjoy se muestra.
        */
        private const String HANDLE_VIEW_OPENED_DEBUG_MSG = "C#: HandleViewOpened";

        /**
        *   @brief Constante que se muestra en el log cuando la vista del tapjoy se cierra.
        */
        private const String HANDLE_VIEW_CLOSED_DEBUG_MSG = "C#: HandleViewClosed";

        /**
        *   @brief Constante que se muestra en el log cuando una oferta de publicidad no se puede mostrar.
        */
        private const String HANDLE_SHOW_OFFERS_FAILED_DEBUG_MSG = "C#: HandleShowOffersFailed";

        /**
        *	@brief Variable para indicar si se muestra la vista.
        */
        private bool _viewIsShowing = false;

        /**
        *	@brief Instancia de la clase.
        */
        public static TapjoyAdHandler instance;

        /**
        *	@brief parent MonoBehaviour que llamara al AdcolonyAdHandler.
        */
        public MonoBehaviour parent;

        /**
        *	@brief Variable para indicar si se esta conectado.
        */
        private bool _isConnected = false;

        /**
        *	@brief Método para Instanciar un TapjoyAdHandler.
        *	
        *	Este método es el encargado de crear un nuevo TapjoyAdHandler creando un nuevo objeto de tipo TapjoyObject con el
        *	componente de TapjoyPlugin.
        */
        public TapjoyAdHandler()
        {
            ObjectFactory.CreateObject<TapjoyPlugin>(TAPJOY_OBJECT_NAME);
        }

        /**
        *	@brief Método para definir el TapjoyAdHandler a usar.
        *	
        *	Este método es el encargado de que cuando no haya alguna instancia de TapjoyAdHandler cree una nueva, 
        *	de lo contrario si ya hay un TapjoyAdHandler en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de TapjoyAdHandler. 
        */
        public static TapjoyAdHandler GetInstance()
        {
            if (TapjoyAdHandler.instance == null)
            {
                TapjoyAdHandler.instance = new TapjoyAdHandler();
            }
            return TapjoyAdHandler.instance;
        }

        /**
        *	@brief Método para obtener el banner de la publicidad.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
		public override void GetAdBanner(MonoBehaviour parent, BannerLocation location)
        {
            // Get a banner ad
            TapjoyPlugin.GetDisplayAd();
        }

        /**
        *	@brief Método para obtener el Intersitial.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void GetIntersitial(MonoBehaviour parent)
        {
            OnFailedInterstitialEvt();
        }

        /**
        *	@brief Método para saber si el video se esta reproduciendo o no.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
        *
        *	@see <namespace.IAdHandler>
        *	
        *	@return True si se esta ejecutando el video de la publicidad, de lo contrario false.
        *
        */
        public override bool GetVideo(MonoBehaviour parent)
        {
            return false;
        }

        /**
        *	@brief Método para mostrar la tienda de publicidad.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void ShowAdStore(MonoBehaviour parent)
        {
            if (_isConnected)
            {
                TapjoyPlugin.ShowOffers();
            }
            else
            {
                OnFailedStoreEvt();
            }
        }

        /**
        *	@brief Método para ocultar el banner de la publicidad.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void HideAdBanner(MonoBehaviour parent)
        {
            if (_isConnected)
            {
                TapjoyPlugin.HideDisplayAd();
            }
        }

        /**
        *	@brief Método para ocultar el intersitial.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void HideIntersitial(MonoBehaviour parent)
        {

        }

        /**
        *	@brief Método para ocultar el video.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void HideVideo(MonoBehaviour parent)
        {

        }

        /**
        *	@brief Método para ocultar la tienda de publicidad.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void HideAdStore(MonoBehaviour parent)
        {

        }

        /**
        *	@brief Método para mostrar la publicidad de la tienda.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void SetAutoBanner(MonoBehaviour parent)
        {

        }

        /**
        *	@brief Método para iniciar los Id respectivos de los videos de publicidad.
        *	
        *	En los id respectivos se indican si se inicializa desde un dispositivo android o un Iphone. Además se setean 
        *	los listener que se van a utilizar. 
        *
        *	@param data Diccionario de datos .
        *	@param parent MonoBehaviour que llamara al TapjoyAdHandler. 
        *
        *	@see  <namespace.IAdHandler>.		
        */
        public override void Init(Dictionary<string, string> data, MonoBehaviour parent)
        {
            SetListeners();
            #if UNITY_ANDROID
                // Attach our thread to the java vm; obviously the main thread is already attached but this is good practice..
                if (Application.platform == RuntimePlatform.Android)
                    UnityEngine.AndroidJNI.AttachCurrentThread();
            #endif

            Dictionary<String, System.Object> connectFlags = new Dictionary<String, System.Object>();

            Debug.Log(START_TAPJOY_DEBUG_MSG);

            // Connect to the Tapjoy servers.
            if (Application.platform == RuntimePlatform.Android)
            {
                // Enable logging
                connectFlags.Add(ENABLE_LOGGING_ANDROID, true);

                // If you are not using Tapjoy Managed currency, you would set your own user ID here.
                //  connectFlags.Add("user_id", "A_UNIQUE_USER_ID");

                // You can also set your event segmentation parameters here.
                //  Dictionary<String, System.Object> segmentationParams = new Dictionary<String, System.Object>();
                //  segmentationParams.Add("iap", true);
                //  connectFlags.Add("segmentation_params", segmentationParams);

                TapjoyPlugin.RequestTapjoyConnect(data[ID_APP_ANDROID_DATA_NAME],			// YOUR APP ID GOES HERE
                                                   data[KEY_APP_ANDROID_DATA_NAME],							// YOUR SECRET KEY GOES HERE
                                                   connectFlags);                					// YOUR CONNECT FLAGS
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {

                // Enable logging
                connectFlags.Add(ENABLE_LOGGING_IOS, true);

                // Add other connect flags                
                connectFlags.Add(TJC_OPTION_COLLECT_MAC_ADDRESS_MSG, TapjoyPlugin.MacAddressOptionOffWithVersionCheck);

                // If you are not using Tapjoy Managed currency, you would set your own user ID here.
                // connectFlags.Add("TJC_OPTION_USER_ID", "A_UNIQUE_USER_ID");

                // You can also set your event segmentation parameters here.
                //  Dictionary<String, System.Object> segmentationParams = new Dictionary<String, System.Object>();
                //  segmentationParams.Add("iap", true);
                //  connectFlags.Add("TJC_OPTION_SEGMENTATION_PARAMS", segmentationParams);
                                
                TapjoyPlugin.RequestTapjoyConnect(data[ID_APP_IOS_DATA_NAME], data[KEY_APP_IOS_DATA_NAME],// YOUR SECRET KEY GOES HERE
                                                        connectFlags);								// YOUR CONNECT FLAGS
            }

        }

        /**
        *	@brief Método para asignar los listener que seran usados en la publicidad de los videos.
        *	
        */
        private void SetListeners()
        {            
            Debug.Log(SET_LISTENERS_DEBUG_MSG);
            // Tapjoy Connect Events
            TapjoyPlugin.connectCallSucceeded += HandleTapjoyConnectSuccess;
            TapjoyPlugin.connectCallFailed += HandleTapjoyConnectFailed;

            // Tapjoy Virtual Currency Events
            TapjoyPlugin.getTapPointsSucceeded += HandleGetTapPointsSucceeded;
            TapjoyPlugin.getTapPointsFailed += HandleGetTapPointsFailed;
            TapjoyPlugin.spendTapPointsSucceeded += HandleSpendTapPointsSucceeded;
            TapjoyPlugin.spendTapPointsFailed += HandleSpendTapPointsFailed;
            TapjoyPlugin.awardTapPointsSucceeded += HandleAwardTapPointsSucceeded;
            TapjoyPlugin.awardTapPointsFailed += HandleAwardTapPointsFailed;
            TapjoyPlugin.tapPointsEarned += HandleTapPointsEarned;

            // Tapjoy Full Screen Ad Events
            //TODO esto cambio y hay que actualizarlo luego TapjoyPlugin.getFullScreenAdSucceeded += HandleGetFullScreenAdSucceeded;
            TapjoyPlugin.getFullScreenAdFailed += HandleGetFullScreenAdFailed;

            // Tapjoy Display Ad Events
            TapjoyPlugin.getDisplayAdSucceeded += HandleGetDisplayAdSucceeded;
            TapjoyPlugin.getDisplayAdFailed += HandleGetDisplayAdFailed;

            // Tapjoy Video Ad Events
            TapjoyPlugin.videoAdStarted += HandleVideoAdStarted;
            TapjoyPlugin.videoAdFailed += HandleVideoAdFailed;
            TapjoyPlugin.videoAdCompleted += HandleVideoAdCompleted;

            // Tapjoy Ad View Closed Events
            TapjoyPlugin.viewOpened += HandleViewOpened;
            TapjoyPlugin.viewClosed += HandleViewClosed;

            // Tapjoy Show Offers Events
            TapjoyPlugin.showOffersFailed += HandleShowOffersFailed;
        }

        /**
        *	@brief Método para realizar la conexión y notificarlo. CONNECT
        *	
        */
        public void HandleTapjoyConnectSuccess()
        {            
            Debug.Log(HANDLE_TAPJOY_CONNECT_SUCCESS_DEBUG_MSG);
            _isConnected = true;
        }

        /**
        *	@brief Método para notificar un fallo en la conexión y cancelarla.
        *	
        */
        public void HandleTapjoyConnectFailed()
        {            
            Debug.Log(HANDLE_TAPJOY_CONNECT_FAILED_DEBUG_MSG);
            _isConnected = false;
        }

        /**
        *	@brief Método para obtener y mostrar los puntos exitosos. VIRTUAL CURRENCY
        *	
        *	@param points puntos exitosos a mostrar. 	
        */
        void HandleGetTapPointsSucceeded(int points)
        {            
            Debug.Log(HANDLE_GET_TAP_POINTS_SUCCEEDED_DEBUG_MSG + points);
        }

        /**
        *	@brief Método para obtener y mostrar los puntos fallidos.
        *	
        *	@param points puntos fallidos a mostrar. 	
        */
        public void HandleGetTapPointsFailed()
        {            
            Debug.Log(HANDLE_GET_TAP_POINTS_FAILED_DEBUG_MSG);
        }

        /**
        *	@brief Método para gastar y mostrar los puntos exitosos.
        *	
        *	@param points puntos exitosos a gastar y mostrar. 	
        */
        public void HandleSpendTapPointsSucceeded(int points)
        {            
            Debug.Log(HANDLE_SPEND_TAP_POINTS_SUCCEEDED_DEBUG_MSG + points);
        }

        /**
        *	@brief Método para gastar y mostrar los puntos fallidos.
        *	
        *	@param points puntos fallidos a gastar y mostrar. 	
        */
        public void HandleSpendTapPointsFailed()
        {            
            Debug.Log(HANDLE_SPEND_TAP_POINTS_FAILED_DEBUG_MSG);
        }

        /**
        *	@brief Método para premiar y mostrar los puntos exitosos.
        *	
        *	@param points puntos exitosos a premiar y mostrar. 	
        */
        public void HandleAwardTapPointsSucceeded()
        {            
            Debug.Log(HANDLE_AWARD_TAP_POINTS_SUCCEEDED_DEBUG_MSG);
        }

        /**
        *	@brief Método para gastar y mostrar los puntos fallidos.
        *	
        *	@param points puntos fallidos a gastar y mostrar. 	
        */
        public void HandleAwardTapPointsFailed()
        {            
            Debug.Log(HANDLE_AWARD_TAP_POINTS_FAILED_DEBUG_MSG);
        }

        /**
        *	@brief Método para gastar y mostrar los puntos exitosos.
        *	
        *	@param points puntos exitosos a gastar y mostrar. 	
        */
        public void HandleTapPointsEarned(int points)
        {            
            Debug.Log(HANDLE_TAP_PONTS_EARNED_DEBUG_MSG + points);
            TapjoyPlugin.ShowDefaultEarnedCurrencyAlert();
            OnAddVirtualCurrencyEvt(points);
        }

        /**
        *	@brief Método para notificar publicidad en pantalla completa exitosamente.
        *
        */
        public void HandleGetFullScreenAdSucceeded()
        {            
            Debug.Log(HANDLE_GET_FULL_SCREEN_AD_SUCCEEDED_DEBUG_MSG);
        }

        /**
        *	@brief Método para notificar publicidad en pantalla completa fallida.
        *
        */
        public void HandleGetFullScreenAdFailed()
        {            
            Debug.Log(HANDLE_GET_FULL_SCREEN_AD_FAILED_DEBUG_MSG);
            OnFailedInterstitialEvt();
        }

        /**
        *	@brief Método para notificar y mostrar el despliegue fallido de publicidad. DISPLAY ADS
        *
        */
        public void HandleGetDisplayAdSucceeded()
        {            
            Debug.Log(HANDLE_GET_DISPLAY_AD_SUCCEEDED_DEBUG_MSG);

            if (!_viewIsShowing)
            {
                TapjoyPlugin.ShowDisplayAd();
            }
        }

        /**
        *	@brief Método para notificar el despliegue fallido de publicidad.
        *
        */
        public void HandleGetDisplayAdFailed()
        {            
            Debug.Log(HANDLE_GET_DISPLAY_AD_FAILED_DEBUG_MSG);
            OnFailedBannerEvt();
        }

        /**
        *	@brief Método para notificar el inicio exitoso de un video publicitario. VIDEO
        *
        */
        public void HandleVideoAdStarted()
        {            
            Debug.Log(HANDLE_VIDEO_AD_STARTED_DEBUG_MSG);
        }

        /**
        *	@brief Método para notificar el inicio fallido de un video publicitario.
        *
        */
        public void HandleVideoAdFailed()
        {            
            Debug.Log(HANDLE_VIDEO_AD_FAILED_DEBUG_MSG);
        }

        /**
        *	@brief Método para notificar el final exitoso de un video publicitario.
        *
        */
        public void HandleVideoAdCompleted()
        {            
            Debug.Log(HANDLE_VIDEO_AD_COMPLETED_DEBUG_MSG);
        }

        /**
        *	@brief Método para notificar y mostrar una vista.
        *	
        *	@param viewType representa el tapjoy a mostrar. 	
        */
        public void HandleViewOpened(TapjoyViewType viewType)
        {            
            Debug.Log(HANDLE_VIEW_OPENED_DEBUG_MSG);
            _viewIsShowing = true;
        }

        /**
        *	@brief Método para notificar y cerrar una vista. VIEW CLOSED
        *	
        *	@param viewType representa el tapjoy a cerrar. 	
        */
        public void HandleViewClosed(TapjoyViewType viewType)
        {            
            Debug.Log(HANDLE_VIEW_CLOSED_DEBUG_MSG);
            _viewIsShowing = false;
        }

        /**
        *	@brief Método para notificar una falla al mostrar una oferta de publicidad.
        *
        */
        public void HandleShowOffersFailed()
        {            
            Debug.Log(HANDLE_SHOW_OFFERS_FAILED_DEBUG_MSG);
            OnFailedStoreEvt();
        }

    }
}