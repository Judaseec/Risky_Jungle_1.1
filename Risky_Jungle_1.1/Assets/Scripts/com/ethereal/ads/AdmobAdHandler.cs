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
    *	@class 	AdmobAdHandler
    *   @brief 	Clase que se encarga de Agregar los videos de publicidad para plataformas mobiles.
    *	
    *	Esta clase hereda de la Interface IAdhandler.
    */
    public class AdmobAdHandler : IAdHandler
    {

        /**
        *   @brief Constante para identificar un banner Android.
        */
        private const string ID_BANNER_ANDROID = "idBannerAndroid";

        /**
        *   @brief Constante para identificar un interstitial android.
        */
        private const string ID_INTERSTITIAL_ANDROID = "idInterstitialAndroid";

        /**
        *   @brief Constante para identificar un banner IOS.
        */
        private const string ID_BANNER_IOS = "idBannerIOS";

        /**
        *   @brief Constante para identificar un interstitial IOS.
        */
        private const string ID_INTERSTITIAL_IOS = "idInterstitialIOS";

        /**
        *   @brief Constatnte a usar para notificar cuando un evento de recepción de publicidad ha fallado.
        */
        private const String LOG_FAILED_TO_RECEIVE_AD_EVENT = "failedToReceiveAdEvent: ";

        /**
        *   @brief Constatnte a usar para notificar un evento de descarte de pantalla de un interstitial.
        */
        private const String LOG_INTERSTITIAL_DISMISSING_SCREEN_EVENT = "interstitialDismissingScreenEvent";

        /**
        *   @brief Constatnte a usar para notificar cuando un evento de recepción de un interstitial ha fallado.
        */
        private const String LOG_INTERSTITIAL_FAILED_TO_RECEIVE_AD_EVENT = "interstitialFailedToReceiveAdEvent: ";

        /**
        *	@brief Patrón Singleton para mantener la misma instancia de AdmobAdHandler en todo el juego.
        */
        public static AdmobAdHandler instance;

        /**
        *	@brief id del banner de la publicidad en un dispositivo Android.
        */
        private string _idBannerAndroid;

        /**
        *	@brief id del banner de la publicidad en un dispositivo IOS.
        */
        private string _idBannerIOS;

        /**
        *	@brief id del intersitial en un dispositivo Android.
        */
        private string _idInterstitialAndroid;

        /**
        *	@brief id del intersitial en un dispositivo IOS.
        */
        private string _idInterstitialIOS;

        /**
        *	@brief Método para Instanciar un AdmobAdHandler.
        *	
        *	Este método es el encargado de crear un nuevo AdmobAdHandler.
        */
        public AdmobAdHandler()
        {
        }

        /**
        *	@brief Método para definir el AdmobAdHandler a usar.
        *	
        *	Este método es el encargado de que cuando no haya alguna instancia de AdmobAdHandler cree una nueva, 
        *	de lo contrario si ya hay un AdmobAdHandler en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de AdmobAdHandler. 
        */
        public static AdmobAdHandler GetInstance()
        {
            if (AdmobAdHandler.instance == null)
            {
                AdmobAdHandler.instance = new AdmobAdHandler();
            }
            return AdmobAdHandler.instance;
        }

        /**
        *	@brief Método para obtener el banner de la publicidad.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
		*   @param parent location con la ubicacion.
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void GetAdBanner(MonoBehaviour parent, BannerLocation location)
        {
            #if UNITY_IPHONE || UNITY_ANDROID
                AdMob.createBanner(_idBannerIOS, _idBannerAndroid, AdMobBanner.SmartBanner, (AdMobLocation)location);
            #endif    
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
            if (AdMob.isInterstitalReady())
            {
                AdMob.displayInterstital();
                AdMob.requestInterstital(_idInterstitialAndroid, _idInterstitialIOS);
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
            #if UNITY_IPHONE || UNITY_ANDROID
            AdMob.destroyBanner();
            #endif    
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
        *	@param parent MonoBehaviour que llamara al AdmobAdHandler. 
        *
        *	@see  <namespace.IAdHandler>.		
        */
        public override void Init(Dictionary<string, string> data, MonoBehaviour parent)
        {
            #if UNITY_IPHONE || UNITY_ANDROID
            this._idBannerAndroid = data[ID_BANNER_ANDROID];
            this._idInterstitialAndroid = data[ID_INTERSTITIAL_ANDROID];
            this._idBannerIOS = data[ID_BANNER_IOS];
            this._idInterstitialIOS = data[ID_INTERSTITIAL_IOS];
            SetListeners();


            AdMob.requestInterstital(_idInterstitialAndroid, _idInterstitialIOS);
            #endif    
        }

        /**
        *	@brief Método para asignar los listener que seran usados en la publicidad de los videos.
        *	
        */
        public void SetListeners()
        {
            #if UNITY_IPHONE || UNITY_ANDROID
            AdMob.failedToReceiveAdEvent += FailedToReceiveAdEvent;
            AdMob.interstitialFailedToReceiveAdEvent += InterstitialFailedToReceiveAdEvent;
            #endif    
        }

        /**
        *	@brief Evento que se ejecuta cuando ha fallado la recepcion de una publicidad.
        *	
        *	@param error Error presentado en la recepcion de la publicidad
        *	
        */
        public void FailedToReceiveAdEvent(string error)
        {
            Debug.Log(LOG_FAILED_TO_RECEIVE_AD_EVENT + error);
            OnFailedBannerEvt();
        }

        /**
        *	@brief Evento para descartar la pantalla de un interstitial.
        *	
        */
        public void InterstitialDismissingScreenEvent()
        {
            Debug.Log(LOG_INTERSTITIAL_DISMISSING_SCREEN_EVENT);
             #if UNITY_IPHONE || UNITY_ANDROID
            AdMob.requestInterstital(_idInterstitialAndroid, _idInterstitialIOS);
            #endif    
        }

        /**
        *	@brief Evento que se ejecuta cuando ha fallado la recepcion de un Intersitial.
        *	
        *	@param error Error presentado en la recepcion de la publicidad
        *	
        */
        public void InterstitialFailedToReceiveAdEvent(string error)
        {
            Debug.Log(LOG_INTERSTITIAL_FAILED_TO_RECEIVE_AD_EVENT + error);
             #if UNITY_IPHONE || UNITY_ANDROID
            AdMob.requestInterstital(_idInterstitialAndroid, _idInterstitialIOS);
            #endif    
        }
    }

}
