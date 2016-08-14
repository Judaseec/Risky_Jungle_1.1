using UnityEngine;
//using UnityEngine.Advertisements;
using System.Collections.Generic;

namespace Assets.Scripts.com.ethereal.ads
{
    
    /** 
    *	@author    Carlos Andres Carvajal <andcarva@gmail.com> 
    * 	@version   1.0 
    * 	@date      Noviembre 7 2014
    * 
    *	@class 	UnityAdHandler
    *   @brief 	Clase que se encarga de Agregar los videos de publicidad.
    *	
    *	Esta clase hereda de la Interface IAdhandler.
    */
    public class UnityAdHandler : IAdHandler
    {
        /**
        *	@brief Patrón Singleton para mantener la misma instancia de UnityAdHandler en todo el juego.
        */
        public static UnityAdHandler instance;

        /**
        *	@brief Método para Instanciar un UnityAdHandler.
        *	
        *	Este método es el encargado de crear un nuevo UnityAdHandler asignando los videos de publicidad que seran colocados.
        */
        public UnityAdHandler()
        {
            
        }

        /**
        *	@brief Método para definir el UnityAdHandler a usar.
        *	
        *	Este método es el encargado de que cuando no haya alguna instancia de UnityAdHandler cree una nueva, 
        *	de lo contrario si ya hay un UnityAdHandler en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de UnityAdHandler. 
        */
        public static UnityAdHandler GetInstance()
        {
            if (UnityAdHandler.instance == null)
            {
                UnityAdHandler.instance = new UnityAdHandler();
            }
            return UnityAdHandler.instance;
        }

        /**
        *	@brief Método para obtener el banner de la publicidad.
        *	
        *	@param parent MonoBehaviour que llamara al UnityAdHandler.
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
        *	@param parent MonoBehaviour que llamara al UnityAdHandler.
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
        *	@param parent MonoBehaviour que llamara al UnityAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
		public override bool IsVideoAvailable(MonoBehaviour parent)
		{
            return false;
		    //return Advertisement.IsReady("rewardedVideo");//Advertisement.IsReady()	            
		}

        /**
        *	@brief Método para saber si el video se esta reproduciendo o no.
        *	
        *	@param parent MonoBehaviour que llamara al UnityAdHandler.
        *
        *	@see <namespace.IAdHandler>
        *	
        *	@return True si se esta ejecutando el video de la publicidad, de lo contrario false.
        *
        */
        public override bool GetVideo(MonoBehaviour parent)
        {
            /*if (Advertisement.IsReady("rewardedVideo"))//if (Advertisement.IsReady()) { Advertisement.Show(); }
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show("rewardedVideo", options);
                return true;
            }
            else{*/
                return false;
            //}
        }

        /*private void HandleShowResult(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    Debug.Log("The ad was successfully shown.");
                    OnVirtualCurrencyWonEvt( 0 );
                break;
                case ShowResult.Skipped:
                    Debug.Log("The ad was skipped before reaching the end.");
                    OnVirtualCurrencyWonEvt( -1 );
                break;
                case ShowResult.Failed:
                    Debug.LogError("The ad failed to be shown.");
                    OnVirtualCurrencyWonEvt( -1 );
                break;
            }
        }*/

        /**
        *	@brief Método para mostrar la tienda de publicidad.
        *	
        *	@param parent MonoBehaviour que llamara al UnityAdHandler.
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
        *	@param parent MonoBehaviour que llamara al UnityAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void HideAdBanner(MonoBehaviour parent)
        {

        }

        /**
        *	@brief Método para ocultar el intersitial.
        *	
        *	@param parent MonoBehaviour que llamara al UnityAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void HideIntersitial(MonoBehaviour parent)
        {

        }

        /**
        *	@brief Método para ocultar el video.
        *	
        *	@param parent MonoBehaviour que llamara al UnityAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void HideVideo(MonoBehaviour parent)
        {

        }

        /**
        *	@brief Método para ocultar la tienda de publicidad.
        *	
        *	@param parent MonoBehaviour que llamara al UnityAdHandler.
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void HideAdStore(MonoBehaviour parent)
        {

        }

        /**
        *	@brief Método para mostrar la publicidad de la tienda.
        *	
        *	@param parent MonoBehaviour que llamara al UnityAdHandler.
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
        *	@param parent MonoBehaviour que llamara al UnityAdHandler. 
        *
        *	@see  <namespace.IAdHandler>.
        */
        public override void Init(Dictionary<string, string> data, MonoBehaviour parent)
        {
           
        }
     
    }

}
