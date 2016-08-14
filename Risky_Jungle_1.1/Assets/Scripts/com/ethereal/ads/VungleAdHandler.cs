using UnityEngine;
using System.Collections.Generic;
using System;
using Prime31;

namespace Assets.Scripts.com.ethereal.ads
{

    /** 
    *	@author    Carlos Andres Carvajal <andcarva@gmail.com> 
    * 	@version   1.0 
    * 	@date      Noviembre 7 2014
    * 
    *	@class 	VungleAdHandler
    *   @brief 	Esta clase es el manejador de videos del sistema tapjoy.
    *	
    *	Esta clase hereda de la Interface IAdhandler.
    */
    public class VungleAdHandler : IAdHandler
    {

        /**
        *	@brief Patrón Singleton para mantener la misma instancia de VungleAdHandler en todo el juego.
        */
        public static VungleAdHandler instance;

        /**
        *	@brief Constante para ingresar como parametro al obtener un video.
        */
        private const String USER_NAME = "user-name";

        /**
        *	@brief Constante para indicar que se está sobre una plataforma android.
        */
        private const String ID_APP_ANDROID_DATA_NAME = "idAppAndroid";

        /**
        *	@brief Constante para indicar que se está sobre una plataforma IOS.
        */
        private const String ID_APP_IOS_DATA_NAME = "idAppIos";

        /**
        *	@brief Constante para mostrar en el log cuando se inicia el sistema Vungle.
        */
        private const String ON_AD_STARTED_EVENT_DEBUG_MSG = "onAdStartedEvent";

        /**
        *	@brief Constante para mostrar en el log cuando se finaliza el sistema Vungle.
        */
        private const String ON_AD_ENDED_EVENT_DEBUG_MSG = "onAdEndedEvent";

        /**
        *	@brief Constante para mostrar en el log que un video ya fue mostrado.
        */
        private const String ON_AD_VIEWED_EVENT_DEBUG_MSG = "onAdViewedEvent. watched: {0}, length: {1}";

        /**
        *	@brief Método para Instanciar un VungleAdHandler.
        *	
        *	Este método es el encargado de crear un nuevo VungleAdHandler.
        */
        public VungleAdHandler()
        {

        }

        /**
        *	@brief Método para definir el VungleAdHandler a usar.
        *	
        *	Este método es el encargado de que cuando no haya alguna instancia de VungleAdHandler cree una nueva, 
        *	de lo contrario si ya hay un VungleAdHandler en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de VungleAdHandler. 
        */
        public static VungleAdHandler GetInstance()
        {
            if (VungleAdHandler.instance == null)
            {
                VungleAdHandler.instance = new VungleAdHandler();
            }
            return VungleAdHandler.instance;
        }

        /**
        *	@brief Método para obtener el banner de la publicidad.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
		*   @param location BannerLocation, con la ubicacion
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void GetAdBanner(MonoBehaviour parent, BannerLocation location)
        {
            OnFailedBannerEvt();
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
        *	@brief Método que define si el video esta o no disponible.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
        *
        */
		public override bool IsVideoAvailable(MonoBehaviour parent)
		{
            #if UNITY_IPHONE || UNITY_ANDROID
			     return Vungle.isAdvertAvailable();  
            #endif 

            return false;            
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
            #if UNITY_IPHONE || UNITY_ANDROID
            if (Vungle.isAdvertAvailable())
            {
                Vungle.playAd();
                return true;
            }
            else
            {
                return false;
            }
            #endif 

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
            OnFailedStoreEvt();
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
        *	@param parent MonoBehaviour que llamara al VungleAdHandler. 
        *
        *	@see  <namespace.IAdHandler>.		
        */
        public override void Init(Dictionary<string, string> data, MonoBehaviour parent)
        {
            #if UNITY_IPHONE || UNITY_ANDROID
            Vungle.init(data[ID_APP_ANDROID_DATA_NAME], data[ID_APP_IOS_DATA_NAME]);
            SetListeners();
            #endif 
        }

        /**
        *	@brief Método para asignar los listener que seran usados en la publicidad de los videos.
        *	
        */
        public void SetListeners()
        {
            #if UNITY_IPHONE || UNITY_ANDROID
            Vungle.onAdStartedEvent += OnAdStartedEvent;
            Vungle.onAdEndedEvent += OnAdEndedEvent;
            Vungle.onAdViewedEvent += OnAdViewedEvent;
            #endif 
        }

        /**
        *	@brief Evento a ejecutar cuando se inicie una publicidad.
        *	
        */
        public void OnAdStartedEvent()
        {
            Debug.Log(ON_AD_STARTED_EVENT_DEBUG_MSG);
        }

        /**
        *	@brief Evento a ejecutar cuando se termine una publicidad.
        *	
        */
        public void OnAdEndedEvent()
        {
            Debug.Log(ON_AD_ENDED_EVENT_DEBUG_MSG);
            OnVirtualCurrencyWonEvt(0);
        }

        /**
        *	@brief Evento a ejecutar cuando se este viendo una publicidad.
        *	
        *	@param	watched Tiempo transcurrido viendo la publicidad
        *	@param	length Tiempo de duracion de la publicidad
        */
        public void OnAdViewedEvent(double watched, double length)
        {
            Debug.Log(string.Format(ON_AD_VIEWED_EVENT_DEBUG_MSG, watched, length));
        }

    }
}
