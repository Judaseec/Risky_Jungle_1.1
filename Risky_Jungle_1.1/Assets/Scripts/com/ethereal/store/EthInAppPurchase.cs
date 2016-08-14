using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;
using Assets.Scripts.com.ethereal.util;


namespace com.ethereal.ethInAppPurchase
{
    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Septiembre 5 2014
    * 
    *	@class 	EthInAppPurchase
    *   @brief 	Clase encargada de manejar las compras en el juego con dinero real.
    *
    */
    public class EthInAppPurchase
    {
        /**
        *	@brief Patrón Singleton para mantener la misma instancia de EthInAppPurchase en todo el juego.
        */
        public static EthInAppPurchase instance;

        /**
        *	@brief Instancia de los elementos de la tienda.
        */
        private EthStoreAssets store;

        /**
        *	@brief Método ejecutado cuando se ejecute un evento de compra en la tienda, que luego sera modificado con 
        *	respecto a como se desee utilizar manteniendo su estructura.
        *	
        *	@param id Item a ser adquirido
        */
        public delegate void OnMarketPurchaseEvent(string id);

        /**
        *	@brief Método ejecutado cuando se ejecute un evento de reembolso de un item en la tienda, que luego sera modificado con 
        *	respecto a como se desee utilizar manteniendo su estructura.
        *	
        *	@param id Item a ser reembolsado
        */
        public delegate void OnMarketRefundEvent(string id);

        /**
        *	@brief Método ejecutado cuando se ejecute un evento de cancelación de una compra en la tienda, que luego sera modificado 
        *	con respecto a como se desee utilizar manteniendo su estructura.
        *	
        *	@param id Item de la compra a ser cancelada
        */
        public delegate void OnMarketCancelledEvent(string id);

        /**
        *	@brief	Evento llamado cuando se efectue una compra de un item, que luego sera modificado.
        */
        public event OnMarketPurchaseEvent OnMarketPurchaseEvt;

        /**
        *	@brief	Evento llamado cuando se efectue el reembolso de una compra, que luego sera modificado.
        */
        public event OnMarketRefundEvent OnMarketRefundEvt;

        /**
        *	@brief	Evento llamado cuando se cancele la compra de un item, que luego sera modificado.
        */
        public event OnMarketCancelledEvent OnMarketCancelledEvt;

        /**
        *	@brief Método para instanciar un EthInAppPurchase.
        *	
        *	Este método es el encargado de crear un nuevo EthAudio asignando todos los atributos que requiere esta clase.
        *	Envia a crear un gameObject llamado SoomlaEvents por medio del metodo de la clase ObjectFactory con el método createObject,
        *	además se le agrega un componente StoreEvents encargado de los eventos en la tienda. Posteriormente se le agregan los listeners
        *	de los eventos posibles en la tienda.
        *
        */
        public EthInAppPurchase()
        {
            ObjectFactory.CreateObject<StoreEvents>("StoreEvents");
            store = new EthStoreAssets();

            SetListeners();
        }

        /**
        *	@brief Método para definir el EthInAppPurchase a usar.
        *	
        *	Este método es el encargado de que cuando no haya alguna instancia de EthInAppPurchase cree una nueva, 
        *	de lo contrario si ya hay un EthInAppPurchase en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de EthInAppPurchase. 
        */
        public static EthInAppPurchase GetInstance()
        {
            if (EthInAppPurchase.instance == null)
            {
                EthInAppPurchase.instance = new EthInAppPurchase();
            }

            return EthInAppPurchase.instance;
        }

        /**
        *	@brief	Método usado para Inicializar el controlador que ejecuta la tienda en el juego.
        *	
        */
        public void Start()
        {
            SoomlaStore.Initialize(store);
        }

        /**
        *	@brief	Método usado para agregar un itetm al paquete de la tienda.
        *	
        *	@param idItem		id del item a ser agregado al paquete.
        *	@param idAndroid	id del dispositivo android del cual se agregará el item al paquete de compra.
        *	@param idIOS		id del dispositivo IOS del cual se asgregará el item al paqute de compra. 
        *
        */
        public void AddItem(string idItem, string idAndroid, string idIOS)
        {
            store.AddCurrencyPack(idItem, idAndroid, idIOS);
        }

        /**
        *	@brief	Método usado para abrir la tienda del juego en el dispositivo android.
        *	
        */
        public void StoreOpened()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
				SoomlaStore.StartIabServiceInBg();
#endif
        }

        /**
        *	@brief	Método usado para cerrar la tienda del juego en el dispositivo android.
        *	
        */
        public void StoreClosed()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
						SoomlaStore.StopIabServiceInBg();
#endif
        }

        /**
        *	@brief	Método usado para Comprar un item del inventario.
        *	
        *	@param idItem item que se desea comprar.
        */
        public void BuyItem(string idItem)
        {
            StoreInventory.BuyItem(idItem);
        }

        /**
        *	@brief	Método usado para agregar los listener que seran utilizados al momento de la adquisicion de items.
        *	
        */
        public void SetListeners()
        {
            StoreEvents.OnMarketPurchase += OnMarketPurchase;
            StoreEvents.OnMarketRefund += OnMarketRefund;
            /*Events.OnItemPurchased += onItemPurchased;
            Events.OnGoodEquipped += onGoodEquipped;
            Events.OnGoodUnEquipped += onGoodUnequipped;
            Events.OnGoodUpgrade += onGoodUpgrade;
            Events.OnBillingSupported += onBillingSupported;
            Events.OnBillingNotSupported += onBillingNotSupported;*/
            StoreEvents.OnMarketPurchaseStarted += OnMarketPurchaseStarted;
            /*Events.OnItemPurchaseStarted += onItemPurchaseStarted;
            Events.OnUnexpectedErrorInStore += onUnexpectedErrorInStore;
            Events.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
            Events.OnGoodBalanceChanged += onGoodBalanceChanged;*/
            StoreEvents.OnMarketPurchaseCancelled += OnMarketPurchaseCancelled;
            /*Events.OnRestoreTransactionsStarted += onRestoreTransactionsStarted;
            Events.OnRestoreTransactions += onRestoreTransactions;*/
            StoreEvents.OnSoomlaStoreInitialized += OnStoreControllerInitialized;
#if UNITY_ANDROID && !UNITY_EDITOR
				StoreEvents.OnIabServiceStarted += onIabServiceStarted;
				StoreEvents.OnIabServiceStopped += onIabServiceStopped;
#endif
        }

        /**
        *	@brief	Método realizado una vez se haya comprado un item.
        *	
        *	Éste se especificará de acuerdo al juego que lo ejecute.
        *	
        *	@param pvi Item virtual que sera comprado.
        */
        public void OnMarketPurchase(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> extra)
        {
            Debug.Log("market purchase");
            if (OnMarketPurchaseEvt != null)
            {
                OnMarketPurchaseEvt(pvi.ItemId);
            }
        }

        /**
        *	@brief	Método realizado una vez se haya reembolsado la compra de un item.
        *	
        *	Éste se especificará de acuerdo al juego que lo ejecute.
        *	
        *	@param pvi Item el cual se requiere ser reembolsado.
        */
        public void OnMarketRefund(PurchasableVirtualItem pvi)
        {
            Debug.Log("market purchase Refund");
            if (OnMarketRefundEvt != null)
            {
                OnMarketRefundEvt(pvi.ItemId);
            }
        }

        /**
        *	@brief	Método usado una vez se inicie la compra de un item.
        *	
        *	@param pvi Item a ser adquirido.
        */
        public void OnMarketPurchaseStarted(PurchasableVirtualItem pvi)
        {
            Debug.Log("onMarketPurchaseStarted");
        }

        /**
        *	@brief	Método usado una vez se cancele la compra de un item.
        *	
        *	@param pvi Item del que sera cancelado la compra.
        */
        public void OnMarketPurchaseCancelled(PurchasableVirtualItem pvi)
        {
            Debug.Log("onMarketPurchaseCancelled");
            if (OnMarketCancelledEvt != null)
            {
                OnMarketCancelledEvt(pvi.ItemId);
            }
        }
       
        /**
        *	@brief	Método usado para imprimir si la compra es soportada.
        *	
        */
        public void OnBillingSupported()
        {
            Debug.Log("onBillingSupported");
        }

        /**
        *	@brief	Método usado para imprimir si la compra no es soportada.
        *	
        */
        public void OnBillingNotSupported()
        {
            Debug.Log("onBillingNotSupported");
        }
        
        /**
        *	@brief	Método usado para Inicializar el controlador encargado de  la tienda virtual.
        *	
        */
        public void OnStoreControllerInitialized()
        {

        }


        /**
        *	@brief	Método usado para inicializar el servicio en una dispositivo android.
        *	
        */
#if UNITY_ANDROID && !UNITY_EDITOR
			public void onIabServiceStarted() {
				Debug.Log("onIabServiceStarted");
			}
			public void onIabServiceStopped() {
				Debug.Log("onIabServiceStopped");
			}
#endif

        /**
	    *	@brief	Método usado para renombrar el toString de esta clase.
	    *	
	    *	@return To String de la clase.
	    */
        public override string ToString()
        {
            return "EthInAppPurchase";
        }
    }
}
