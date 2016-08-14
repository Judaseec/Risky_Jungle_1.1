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
    *	@class 	AdcolonyAdHandler
    *   @brief 	Clase que se encarga de Agregar los videos de publicidad.
    *	
    *	Esta clase hereda de la Interface IAdhandler.
    */
    public class AdcolonyAdHandler : IAdHandler
    {
        /**
         * @brief Constatnte a usar para notificar cuando un video ha finalizado.
         */
        private const String LOG_ON_VIDEO_FINISHED = "OnVideoFinished";

        /**
         * @brief Constatnte a usar para notificar cuando un video ha iniciado.
         */
        private const String LOG_ON_VIDEO_STARTED = "OnVideoStarted";

        /**
         * @brief Constatnte a usar para notificar cuando la disponibilidad de publicidad cambie.
         */
        private const String LOG_ON_AD_AVAILABILITY_CHANGE = "OnAdAvailabilityChange: {0} zone: {1}";

        /**
        *	@brief Patrón Singleton para mantener la misma instancia de AdcolonyAdHandler en todo el juego.
        */
        public static AdcolonyAdHandler instance;

        /**
        *	@brief Version arbitraria de la app.
        */
        private string _appVersion = "1";   // Arbitrary app version
        
        /**
        *   @brief Asignación de propiedades de lectura y escritura para la variable _appVersion.
        *
        *   @return El valor de la variable _appVersion.
        */
        public string AppVersion
        {
            get { return _appVersion; }
            set { _appVersion = value; }
        }

        /**
        *	@brief Id de la publicidad que sera mostrada.
        */
        private string _appId = "appc392faca95824dea95";     // ADC App ID from adcolony.com
        
        /**
        *   @brief Asignación de propiedades de lectura y escritura para la variable _appId.
        *
        *   @return El valor de la variable _appId.
        */
        public string AppId
        {
            get { return _appId; }
            set { _appId = value; }
        }
        
        /**
        *	@brief Id de los videos para obtener monedas virtuales (Videos-For-Virtual-Currency).
        */
        private string _v4vcId = "vz80de44fda4ef4f5588";
        
        public string V4vcId
        {
            get { return _v4vcId; }
            set { _v4vcId = value; }
        }
        
        /**
        *	@brief Método para Instanciar un AdcolonyAdHandler.
        *	
        *	Este método es el encargado de crear un nuevo AdcolonyAdHandler asignando los videos de publicidad que seran colocados.
        */
        public AdcolonyAdHandler()
        {
            AdColony.OnVideoStarted = this.OnVideoStarted;
            AdColony.OnVideoFinished = this.OnVideoFinished;
            AdColony.OnV4VCResult = this.OnV4VCResult;
            AdColony.OnAdAvailabilityChange = this.OnAdAvailabilityChange;
        }

        /**
        *	@brief Método para definir el AdcolonyAdHandler a usar.
        *	
        *	Este método es el encargado de que cuando no haya alguna instancia de AdcolonyAdHandler cree una nueva, 
        *	de lo contrario si ya hay un AdcolonyAdHandler en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de AdcolonyAdHandler. 
        */
        public static AdcolonyAdHandler GetInstance()
        {
            if (AdcolonyAdHandler.instance == null)
            {
                AdcolonyAdHandler.instance = new AdcolonyAdHandler();
            }
            return AdcolonyAdHandler.instance;
        }

        /**
        *	@brief Método para obtener el banner de la publicidad.
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
        *	@param location BannerLocation con la ubicacion del banner.
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
        *	@brief Método para verificar si hay video disponible
        *	
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
		public override bool IsVideoAvailable(MonoBehaviour parent)
		{
			return AdColony.IsV4VCAvailable(this._v4vcId);              
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
            if (AdColony.IsV4VCAvailable(this.V4vcId))
            {
                AdColony.ShowV4VC(false, this.V4vcId);
                return true;
            }
            else
            {
                return false;
            }
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
        *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler. 
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void Init(Dictionary<string, string> data, MonoBehaviour parent)
        {
            // Initialize the Chartboost plugin
            #if UNITY_ANDROID
                this._appId = data["idAppAndroid"];
                this._v4vcId = data["zoneAppAndroid"];
            #elif UNITY_IPHONE
				this._appId = data["idAppIos"];
				this._v4vcId = data["zoneAppIos"];
            #endif

            AdColony.Configure(
                    this.AppVersion,   // Arbitrary app version
                    this.AppId,     // ADC App ID from adcolony.com		            
                    this.V4vcId);

            SetListeners();
        }

        /**
        *	@brief Método para asignar los listener que seran usados en la publicidad de los videos.
        *	
        */
        public void SetListeners()
        {
            AdColony.OnVideoStarted = this.OnVideoStarted;
            AdColony.OnVideoFinished = this.OnVideoFinished;
            AdColony.OnV4VCResult = this.OnV4VCResult;
            AdColony.OnAdAvailabilityChange = this.OnAdAvailabilityChange;
        }

        /**
        *	@brief Método para dar una notificacion cuando el video se inicia.
        *	
        */
        private void OnVideoStarted()
        {        
            Debug.Log(LOG_ON_VIDEO_STARTED);
        }

        /**
        *	@brief Método para dar una notificacion cuando el video termina.
        *	
        *	@param adWasShown	Identifica si la publicidad ya fue mostrada.
        */
        private void OnVideoFinished(bool adWasShown)
        {            
            Debug.Log(LOG_ON_VIDEO_FINISHED);
        }

        /**
        *	@brief Método para llevar a cabo los videos para obtener monedas virtuales.
        *	
        *   The V4VCResult Delegate assigned in Start, AdColony calls this after confirming V4VC transactions with your server
        *   success - true: transaction completed, virtual currency awarded by your server - false: transaction failed, no virtual currency awarded
        *   name - The name of your virtual currency, defined in your AdColony account
        *   amount - The amount of virtual currency awarded for watching the video, defined in your AdColony account
        *	
        *	El V4VC es asignado al inicio, el AdColony llama a este confirmando las transacciones de V4VC con el servidor
        *
        *	@param success  True: transsacion completada, monedas virtuales otorgadas por el servidos- False: la transaccion ha fallado, no hay monedas virtuales otorgadas.
        *	@param name     Nombre de las moneda virtual, definida en la cuenta AdColony.
        *	@param amount	Cantidad de monedas a ser reclamada.
        */
        private void OnV4VCResult(bool success, string name, int amount)
        {
            if(success)
		    {
		       OnVirtualCurrencyWonEvt( amount );
		    }
		    else
		    {
		        OnVirtualCurrencyWonEvt( -1 );
		    }
        }

        /**
        *	@brief Método para notificar un cambio en la disponibilidad de la publicidad.
        *	
        *	@param avail 	Disponibilidad de la publicidad.
        *	@param zone_id 	ID de la zona.
        */
        private void OnAdAvailabilityChange(bool avail, string zoneId)
        {        
            Debug.Log(string.Format(LOG_ON_AD_AVAILABILITY_CHANGE, avail, zoneId));
        }

    }

}
