using UnityEngine;
using System.Collections.Generic;
using System;
using Chartboost;
using Assets.Scripts.com.ethereal.util;

namespace Assets.Scripts.com.ethereal.ads
{

    /** 
    *	@author    Carlos Andres Carvajal <andcarva@gmail.com> 
    * 	@version   1.0 
    * 	@date      Noviembre 7 2014
    * 
    *	@class 	ChartboostAdHandler
    *   @brief 	Esta clase es la que se encarga de administrar el sistema Chartboost.
    *	
    *	Esta clase hereda de la Interface IAdhandler.
    */
    public class ChartboostAdHandler : IAdHandler
    {

        /**
        *   @brief Constante que se usa en el constructor para indicar que se esta creando un objeto Chartboost.
        */
        private const String CHARTBOOST_OBJECT = "ChartboostObject";
        
        /**
        *   @brief Constante que se usa en el constructor para indicar que se esta creando un administrador de Chartboost.
        */
        private const String CB_MANAGER = "CBManager";

        /**
        *   @brief Constante que se usa para notificar que un interstitial dejó de cargar.
        */
        private const String LOG_DID_FAIL_TO_LOAD_INTERSTITIAL_EVENT = "didFailToLoadInterstitialEvent: ";

        /**
        *   @brief Constante que se usa para notificar que un interstitial es descartado.
        */
        private const String LOG_DID_DISMISS_INTERSTITIAL_EVENT = "didDismissInterstitialEvent: ";

        /**
        *   @brief Constante que se usa para indicar que un interstitial se cerró.
        */
        private const String LOG_DID_CLOSE_INTERSTITIAL_EVENT = "didCloseInterstitialEvent: ";

        /**
        *   @brief Constante que se usa para indicar que se hizo click en un interstitial.
        */
        private const String LOG_DID_CLICK_INTERSTITIAL_EVENT = "didClickInterstitialEvent: ";

        /**
        *   @brief Constante que se usa para indicar que un interstitial se mostró.
        */
        private const String LOG_DID_SHOW_INTERSTITIAL_EVENT = "didShowInterstitialEvent: ";

        /**
        *	@brief Patrón Singleton para mantener la misma instancia de ChartboostAdHandler en todo el juego.
        */
        public static ChartboostAdHandler instance;

        /**
        *	@brief Método para Instanciar un ChartboostAdHandler.
        *	
        *	Este método es el encargado de crear un nuevo ChartboostAdHandler creando un nuevo objeto de tipo ChartboostObject con el
        *	componente de CBManager
        */
        public ChartboostAdHandler()
        {
            ObjectFactory.CreateObject<CBManager>(CHARTBOOST_OBJECT);
        }

        /**
        *	@brief Método para definir el ChartboostAdHandler a usar.
        *	
        *	Este método es el encargado de que cuando no haya alguna instancia de ChartboostAdHandler cree una nueva, 
        *	de lo contrario si ya hay un ChartboostAdHandler en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de AdmobAdHandler. 
        */
        public static ChartboostAdHandler GetInstance()
        {
            if (ChartboostAdHandler.instance == null)
            {
                ChartboostAdHandler.instance = new ChartboostAdHandler();
            }
            return ChartboostAdHandler.instance;
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
            #if UNITY_IPHONE || UNITY_ANDROID
                if (CBBinding.hasCachedInterstitial(null))
                {
                    CBBinding.showInterstitial(null);
                }
                else
                {
                    OnFailedInterstitialEvt();
                }
            #endif 
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
        *	@param parent MonoBehaviour que llamara al ChartboostAdHandler.
        *
        *	@see  <namespace.IAdHandler>.		 
        */
        public override void Init(Dictionary<string, string> data, MonoBehaviour parent)
        {
            // Initialize the Chartboost plugin
            #if UNITY_ANDROID
                // Remember to set the Android app ID and signature in the file `/Plugins/Android/res/values/strings.xml`
                CBBinding.init();
            #elif UNITY_IPHONE
				// Replace these with your own app ID and signature from the Chartboost web portal				
            #endif

            SetListeners();

            #if UNITY_IPHONE || UNITY_ANDROID
                CBBinding.cacheInterstitial(null);
            #endif
        }

        /**
        *	@brief Método para asignar los listener que seran usados en la publicidad de los videos.
        *	
        */
        public void SetListeners()
        {
            #if UNITY_IPHONE || UNITY_ANDROID

            // Listen to all impression-related events
            CBManager.didFailToLoadInterstitialEvent += DidFailToLoadInterstitialEvent;
            CBManager.didDismissInterstitialEvent += DidDismissInterstitialEvent;
            CBManager.didCloseInterstitialEvent += DidCloseInterstitialEvent;
            CBManager.didClickInterstitialEvent += DidClickInterstitialEvent;

            CBManager.didShowInterstitialEvent += DidShowInterstitialEvent;
            #endif
        }

        /**
        *	@brief Evento ejecutado cuando deja de cargar un Intersitial.
        *	
        *	@param location Lugar en donde iba a ser cargado el Intersitial.
        */
        public void DidFailToLoadInterstitialEvent(string location)
        {
            Debug.Log(LOG_DID_FAIL_TO_LOAD_INTERSTITIAL_EVENT + location);
            OnFailedInterstitialEvt();
        }

        /**
        *   @brief Evento ejecutado cuando se descarta un Intersitial.
        *   
        *   @param location Lugar en donde se encuentra ubicado el Intersitial.
        */
        public void DidDismissInterstitialEvent(string location)
        {
            Debug.Log(LOG_DID_DISMISS_INTERSTITIAL_EVENT + location);
        }

        /**
        *	@brief Evento ejecutado cuando se cierra un Intersitial.
        *	
        *	@param location Lugar en donde se encuentra ubicado el Intersitial.
        */
        public void DidCloseInterstitialEvent(string location)
        {
            Debug.Log(LOG_DID_CLOSE_INTERSTITIAL_EVENT + location);
            #if UNITY_IPHONE || UNITY_ANDROID
            CBBinding.cacheInterstitial(null);
            #endif
        }

        /**
        *	@brief Evento ejecutado cuando se da click sobre un Intersitial.
        *	
        *	@param location Lugar en donde se encuentra ubicado el Intersitial.
        */
        public void DidClickInterstitialEvent(string location)
        {
            Debug.Log(LOG_DID_CLICK_INTERSTITIAL_EVENT + location);
        }

        /**
        *	@brief Evento ejecutado cuando se muestra el Intersitial.
        *	
        *	@param location Lugar en donde se encuentra ubicado el intersitial.
        */
        public void DidShowInterstitialEvent(string location)
        {
            Debug.Log(LOG_DID_SHOW_INTERSTITIAL_EVENT + location);
        }

    }

}
