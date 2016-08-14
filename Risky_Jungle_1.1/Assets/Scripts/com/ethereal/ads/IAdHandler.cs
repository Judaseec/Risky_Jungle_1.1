using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.com.ethereal.ads;

/** 
*	@author    Carlos Andres Carvajal <andcarva@gmail.com> 
* 	@version   1.0 
* 	@date      Noviembre 7 2014
* 
*	@class 	IAdHandler
*   @brief 	Interface que establece los métodos y eventos para manejar el sistema de publicidad.
*	
*/
public class IAdHandler
{

    /**
    *	@brief	Evento llamado cuando ha fallado la carga del banner, que luego sera modificado.
    */
    public event OnFailedBannerEvent OnFailedBanner;

    /**
    *	@brief	Evento llamado cuando ha fallado la carga del Intersitial, que luego sera modificado.
    */
    public event OnFailedInterstitialEvent OnFailedInterstitial;

    /**
    *	@brief	Evento llamado cuando se ha completado el video, que luego sera modificado.
    */
    public event OnSuccessVideoEvent OnSuccessVideo;

    /**
    *	@brief	Evento llamado cuando ha fallado la carga de la tienda, que luego sera modificado.
    */
    public event OnFailedStoreEvent OnFailedStore;

    /**
    *	@brief	Evento llamado cuando el usuario gana virtual currency, que luego sera modificado.
    */
    public event OnVirtualCurrencyWonEvent OnVirtualCurrencyWon;

    /**
    *	@brief	Evento llamado cuando el usuario adquiere virtual currency, que luego sera modificado.
    */
    public event OnAddVirtualCurrencyEvent OnAddVirtualCurrency;


    /**
    *	@brief Método para obtener el banner de la publicidad.
    *	
    *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
	*	
    *	@param location BannerLocation con la unbicación del banner
    */
    public virtual void GetAdBanner(MonoBehaviour parent, BannerLocation location) { }

    /**
    *	@brief Método para obtener el Intersitial.
    *	
    *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
    */
    public virtual void GetIntersitial(MonoBehaviour parent) { }

    /**
    *	@brief Método para saber si el video se esta reproduciendo o no.
    *	
    *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
    *	
    *	@return True si se esta ejecutando el video de la publicidad, de lo contrario false.
    *
    */
    public virtual bool GetVideo(MonoBehaviour parent) { return false; }

	/**
    *	@brief Método para saber si el video esta disponible o no.
    *	
    *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
    *	
    *	@return True si esta disponible el video, de lo contrario false.
    *
    */
	public virtual bool IsVideoAvailable(MonoBehaviour parent){ return false; }
	
    /**
    *	@brief Método para mostrar la tienda de publicidad.
    *	
    *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
    */
    public virtual void ShowAdStore(MonoBehaviour parent) { }

    /**
    *	@brief Método para ocultar el banner de la publicidad.
    *	
    *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
    */
    public virtual void HideAdBanner(MonoBehaviour parent) { }

    /**
    *	@brief Método para ocultar el intersitial.
    *	
    *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
    */
    public virtual void HideIntersitial(MonoBehaviour parent) { }

    /**
    *	@brief Método para ocultar el video.
    *	
    *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
    *
    */
    public virtual void HideVideo(MonoBehaviour parent) { }

    /**
    *	@brief Método para ocultar la tienda de publicidad.
    *	
    *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
    */
    public virtual void HideAdStore(MonoBehaviour parent) { }

    /**
    *	@brief Método para mostrar la publicidad de la tienda.
    *	
    *	@param parent MonoBehaviour que llamara al AdcolonyAdHandler.
    *
    *	@see  <namespace.IAdHandler>.
    */
    public virtual void SetAutoBanner(MonoBehaviour parent) { }

    /**
    *	@brief Método para iniciar los Id respectivos de los videos de publicidad.
    *
    *	@param data Diccionario de datos .
    *	@param parent MonoBehaviour que llamara al AdHandler respectivo.  
    */
    public virtual void Init(Dictionary<string, string> data, MonoBehaviour parent) { Debug.Log(" inicio en padre "); }

    /**
    *	@brief Método ejecutado cuando falle la carga de un banner, que luego sera modificado con 
    *	respecto a como se desee utilizar manteniendo su estructura.
    *	
    */
    public delegate void OnFailedBannerEvent();

    /**
    *	@brief Método ejecutado cuando falle la carga de un Intersitial, que luego sera modificado con 
    *	respecto a como se desee utilizar manteniendo su estructura.
    *	
    */
    public delegate void OnFailedInterstitialEvent();

    /**
    *	@brief Método ejecutado cuando sea completado un video, que luego sera modificado con 
    *	respecto a como se desee utilizar manteniendo su estructura.
    *	
    */
    public delegate void OnSuccessVideoEvent();

    /**
    *	@brief Método ejecutado cuando falle la carga de la tienda, que luego sera modificado con 
    *	respecto a como se desee utilizar manteniendo su estructura.
    *	
    */
    public delegate void OnFailedStoreEvent();

    /**
    *	@brief Método ejecutado cuando el usuario gane virtual currency.
    *	
    *	@param virtualCurrency cantidad de virtual currency.
    */
    public delegate void OnVirtualCurrencyWonEvent(int virtualCurrency);

    /**
    *	@brief Método ejecutado para agregar virtual currency al usuario.
    *	
    *	@param virtualCurrency cantidad de virtual currency.
    */
    public delegate void OnAddVirtualCurrencyEvent(int virtualCurrency);

    /**
    *	@brief Método ejecutado cuando falle la carga de un Intersitial, que luego sera modificado con 
    *	respecto a como se desee utilizar manteniendo su estructura.  
    *	
    */
    public void OnFailedInterstitialEvt()
    {
        if (OnFailedInterstitial != null)
        {
            OnFailedInterstitial();
        }
    }

    /**
    *	@brief Método ejecutado cuando falle la carga de un banner, que luego sera modificado con 
    *	respecto a como se desee utilizar manteniendo su estructura.
    *	
    */
    public void OnFailedBannerEvt()
    {
        if (OnFailedBanner != null)
        {
            OnFailedBanner();
        }
    }

    /**
    *	@brief Método ejecutado cuando sea completado un video, que luego sera modificado con 
    *	respecto a como se desee utilizar manteniendo su estructura.
    *	
    */
    public void OnSuccessVideoEvt()
    {
        if (OnSuccessVideo != null)
        {
            OnSuccessVideo();
        }
    }

    /**
    *	@brief Método ejecutado cuando falle la carga de la tienda, que luego sera modificado con 
    *	respecto a como se desee utilizar manteniendo su estructura.
    *	
    */
    public void OnFailedStoreEvt()
    {
        if (OnFailedStore != null)
        {
            OnFailedStore();
        }
    }

    /**
    *	@brief Método ejecutado cuando el usuario gane virtual currency.
    *	
    *	@param virtualCurrency cantidad de virtual currency.
    */
    public void OnVirtualCurrencyWonEvt(int virtualCurrency)
    {
        if (OnVirtualCurrencyWon != null)
        {
            OnVirtualCurrencyWon(virtualCurrency);
        }
    }

    /**
    *	@brief Método ejecutado para agregar virtual currency al usuario.
    *	
    *	@param virtualCurrency cantidad de virtual currency.
    */
    public void OnAddVirtualCurrencyEvt(int virtualCurrency)
    {
        if (OnAddVirtualCurrency != null)
        {
            OnAddVirtualCurrency(virtualCurrency);
        }
    }
}
