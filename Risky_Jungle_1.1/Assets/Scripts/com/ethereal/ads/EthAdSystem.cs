using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.com.ethereal.ads
{
	/**
    *	@brief define la enumeración de la ubicación del banner
    */
	public enum BannerLocation
	{
		TopLeft,
		TopCenter,
		TopRight,
		Centered,
		BottomLeft,
		BottomCenter,
		BottomRight
	}

    /** 
    *	@author    Carlos Andres Carvajal <andcarva@gmail.com> 
    * 	@version   1.0 
    * 	@date      Noviembre 7 2014
    * 
    *	@class 	EthAdSystem
    *   @brief 	Clase que sirve para organizar la publicidad, asi como sus recompensas con las monedas virtuales 
    *	obtenidas en el juego.
    *	
    */
    public class EthAdSystem
    {

        /**
        *	@brief Patrón Singleton para mantener la misma instancia de EthAdSystem en todo el juego.
        */
        public static EthAdSystem instance;

        /**
        *	@brief Método que se ejecuta cuando el usuario gana virtual currency, que luego sera modificado con 
        *	respecto a como se desee utilizar manteniendo su estructura.
        *
        *	Si el valor es 0, la app decide cuanto dar, si es -1 es un fallo y si es otro valor, es lo que se definio en la red.
        */
        public delegate void OnVirtualCurrencyWonEvent(int virtualCurrency);

        /**
        *	@brief Evento llamado cuando el usuario gana virtual currency.
        */
        public event OnVirtualCurrencyWonEvent OnVirtualCurrencyWon;

        /**
        *	@brief Método que se ejecuta para agregar virtual currency al usuario, que luego sera modificado con 
        *	respecto a como se desee utilizar manteniendo su estructura.
        */
        public delegate void OnAddVirtualCurrencyEvent(int virtualCurrency);

        /**
        *	@brief Evento llamado para agregar virtual currency al usuario.
        */
        public event OnAddVirtualCurrencyEvent OnAddVirtualCurrency;

        /**
        *	@brief Constante que sirve para iniciar la red de publicidad tipo ADMOB.
        */
        public const string ADMOB = "ADMOB";

        /**
        *	@brief Constante que sirve para iniciar la red de publicidad tipo TAPJOY.
        */
        public const string TAPJOY = "TAPJOY";

        /**
        *	@brief Constante que sirve para iniciar la red de publicidad tipo CHARTBOOST.
        */
        public const string CHARTBOOST = "CHARTBOOST";

        /**
        *	@brief Constante que sirve para iniciar la red de publicidad tipo VUNGLE.
        */
        public const string VUNGLE = "VUNGLE";

        /**
        *	@brief Constante que sirve para iniciar la red de publicidad tipo ADCOLONY.
        */
        public const string ADCOLONY = "ADCOLONY";

        /**
        *   @brief Constante que sirve para iniciar la red de publicidad tipo Unity.
        */
        public const string UNITYADS = "UNITYADS";

        /**
        *	@brief parent MonoBehaviour que llamara al EthAdSystem.
        */
        public MonoBehaviour parent;

        /**
        *	@brief Lista de banners publicitarios.
        */
        private ArrayList _bannerList;

        /**
        *	@brief Lista de videos publicitarios.
        */
        private ArrayList _videoList;

        /**
        *	@brief Lista de interstitials publicitarios. 
        */
        private ArrayList _interstitialList;

        /**
        *	@brief Lista de tienda de publicidad. 
        */
        private ArrayList _adstoreList;

        /**
        *	@brief Índice de la lista de banners. 
        */
        private int _indexBanner;

        /**
        *	@brief Índice de la lista de interstitials. 
        */
        private int _indexInterstitial;

        /**
        *	@brief Índice de la lista de tienda de publicidad. 
        */
        private int _indexAdstore;

        /**
        *	@brief Variable que indica si el sistema de publicidad esta habilitado. 
        */
        private bool _isEnabled;

        /**
        *   @brief Asignación de propiedades de lectura y escritura a la variable _isEnabled.
        *
        *   @return El valor de la variable _isEnabled.
        */
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        /**
        *	@brief Diccionario de redes.
        */
        private Dictionary<string, IAdHandler> _networks;

		/**
        *	@brief define la ubicación
        */
		private BannerLocation location;
		
        /**
        *	@brief Constructor de la clase EthAdSystem.
        */
        public EthAdSystem()
        {
            _networks = new Dictionary<string, IAdHandler>();
            _isEnabled = true;
        }

        /**
        *	@brief Método para definir el EthAdSystem a usar.
        *	
        *	Este método es el encargado de que cuando no haya alguna instancia de EthAdSystem cree una nueva, 
        *	de lo contrario si ya hay un EthAdSystem en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de EthAdSystem. 
        */
        public static EthAdSystem GetInstance()
        {
            if (EthAdSystem.instance == null)
            {
                EthAdSystem.instance = new EthAdSystem();
            }

            return EthAdSystem.instance;
        }

        /**
        *	@brief Método para definir la prioridad de los banners.
        *
        *	@param networks Diccionario de redes.
        */
        public void SetPriorityBanner(ArrayList networks)
        {
            _bannerList = networks;
        }

        /**
        *	@brief Método para definir la prioridad de los videos.
        *
        *	@param networks Diccionario de redes.
        */
        public void SetPriorityVideo(ArrayList networks)
        {
            _videoList = networks;
        }

        /**
        *	@brief Método para definir la prioridad de los interstitial.
        *
        *	@param networks Diccionario de redes.
        */
        public void SetPriorityInterstitial(ArrayList networks)
        {
            _interstitialList = networks;
        }


        /**
        *	@brief Método para definir la prioridad de la tienda de publicidad.
        *
        *	@param networks Diccionario de redes.
        */
        public void SetPriorityAdstore(ArrayList networks)
        {
            _adstoreList = networks;
        }

        /**
        *	@brief Método para iniciar la red de publicidad.
        *	
        *	@param nameNetwork
        *	@param data Diccionario de datos .
        *	@param parent MonoBehaviour que llamara al EthAdSystem.		 
        */
        public void InitNetwork(string nameNetwork, Dictionary<string, string> data, MonoBehaviour parent)
        {
            this.parent = parent;
            IAdHandler network = null;

            if (!_networks.ContainsKey(nameNetwork))
            {
                switch (nameNetwork)
                {
                    case ADMOB:
                        network = AdmobAdHandler.GetInstance();
                        break;

                    case VUNGLE:
                        network = VungleAdHandler.GetInstance();
                        break;

                    case ADCOLONY:
                        network = AdcolonyAdHandler.GetInstance();
                        break;

                    case UNITYADS:
                        network = UnityAdHandler.GetInstance();
                        break;
                }

                if (network != null)
                {

                    network.Init(data, parent);
                    _networks.Add(nameNetwork, network);

                    network.OnFailedBanner += OnFailedBanner;
                    network.OnFailedInterstitial += OnFailedInterstitial;
                    network.OnSuccessVideo += OnSuccessVideo;
                    network.OnFailedStore += OnFailedStore;
                    network.OnVirtualCurrencyWon += OnVirtualCurrencyWonCB;
                    network.OnAddVirtualCurrency += OnAddVirtualCurrencyCB;
                }
            }

        }

        /*
        *	@brief Método para inicialicar un banner recibiendo un MonoBehaviour como parametro.
        *
        *	@param parent MonoBehaviour que llamara al EthAdSystem.
		*
		*	@param location con la ubicación del banner
        */
        public void InitBanner(MonoBehaviour parent, BannerLocation location = BannerLocation.BottomCenter)
        {
            if (_isEnabled)
            {
                this.location = location;
				this.parent = parent;
                _indexBanner = 0;

                InitBanner();
            }
        }

        /**
        *	@brief Método para iniciar un banner.
        */
        private void InitBanner()
        {
            if (_isEnabled)
            {
                if (_bannerList != null)
                {
                    if (_bannerList[_indexBanner] != null)
                    {
                        _networks[(string)_bannerList[_indexBanner]].GetAdBanner(parent, location);
                    }
                }
            }
        }

        /**
        *	@brief Método para iniciar un banner luego de una falla.
        */
        public void OnFailedBanner()
        {
            if (_bannerList.Count > 1)
            {
                _indexBanner = (_indexBanner + 1) % _bannerList.Count;
                InitBanner();
            }
        }

        /**
        *	@brief Método para ocultar un banner.
        */
        public void HideBanner()
        {
            if (_bannerList != null)
            {
                if (_bannerList[_indexBanner] != null)
                {
                    _networks[(string)_bannerList[_indexBanner]].HideAdBanner(parent);
                }
            }
        }

		/**
        *	@brief Método para verificar si hay video disponible
		*
		*	@return bool, con el resultado
        */
		public bool IsVideoAvailable()
		{
			for ( int i = 0; i < _videoList.Count; i ++ )
			{
				if ( _videoList[i] != null )
				{
					if ( _networks[ (string)_videoList[i] ].IsVideoAvailable( parent ) )
					{
						return true;
					}

				}
			}
			return false;
		}
		
        /*
        *	@brief Método que inicializa un video de publicidad recibiendo un Monobehaviour que llama al EthAdSystem
        *
        *	@param parent MonoBehaviour que llamara al EthAdSystem.
        *	@return true si se inicia el video, de lo contrario false.
        */
        public bool InitVideo(MonoBehaviour parent)
        {
            this.parent = parent;

            if (_videoList != null)
            {
                for (int i = 0; i < _videoList.Count; i++)
                {
                    if (_videoList[i] != null)
                    {
                        if (_networks[(string)_videoList[i]].GetVideo(parent))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /*
        *	@brief Método ejecutado cuando se termina de reproducir un video.
        */
        public void OnSuccessVideo()
        {
            //Puede que se necesite luego, por ahora cuando se termina un video se llama OnVirtualCurrencyWon
        }

        /*
        *	@brief Método que se encarga de iniciar del intersitial.
        *
        *	@param parent MonoBehaviour que llamara al EthAdSystem.
        */
        public void InitIntersitial(MonoBehaviour parent)
        {
            if (_isEnabled)
            {
                this.parent = parent;
                InitIntersitial();
            }
        }

        /**
        *	@brief Método para iniciar un intersitial.
        */
        private void InitIntersitial()
        {
            if (_isEnabled)
            {
                if (_interstitialList != null)
                {
                    try
                    {
                        if (_interstitialList[_indexInterstitial] != null)
                        {
                            _networks[(string)_interstitialList[_indexInterstitial]].GetIntersitial(parent);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.Log("error iniciando initIntersitial");
                    }

                }
            }
        }

        /**
        *	@brief Método para iniciar un intersitial luego de una falla.
        */
        public void OnFailedInterstitial()
        {
            if (_interstitialList.Count > 1)
            {
                _indexInterstitial = (_indexInterstitial + 1) % _interstitialList.Count;
                InitIntersitial();
            }
        }


        /*
        *	@brief Inicializa la publicidad para obtener monedas gratis para el juego.
        *	
        *	@Monobehaviour parent MonoBehaviour que llamara al EthAdSystem.
        */
        public void InitFreeCoins(MonoBehaviour parent)
        {
            this.parent = parent;
            InitFreeCoins();
        }

        /**
        *	@brief Método para iniciar monedas gratis.
        */
        private void InitFreeCoins()
        {

            if (_adstoreList != null)
            {
                if (_adstoreList[_indexAdstore] != null)
                {
                    _networks[(string)_adstoreList[_indexAdstore]].ShowAdStore(parent);
                }
            }

        }

        /*
        *	@brief Método ejecutado cuando falla la tienda
        *
        */
        public void OnFailedStore()
        {
            if (_adstoreList.Count > 1)
            {
                _indexAdstore = (_indexAdstore + 1) % _adstoreList.Count;
                InitFreeCoins();
            }
        }

        /**
        *	@brief Método para agregar virtual currency ganada por un usuario.
        *
        *	@param virtualCurrency Cantidad de virtual currency.
        */
        public void OnVirtualCurrencyWonCB(int virtualCurrency)
        {
            if (OnVirtualCurrencyWon != null)
            {
                OnVirtualCurrencyWon(virtualCurrency);
            }
        }

        /**
        *	@brief Método para agregar virtual currency a un usuario.
        *
        *	@param virtualCurrency Cantidad de virtual currency.
        */
        public void OnAddVirtualCurrencyCB(int virtualCurrency)
        {
            if (OnAddVirtualCurrency != null)
            {
                OnAddVirtualCurrency(virtualCurrency);
            }
        }

        /*
        *	@brief Sobreescritura del Tostring de la clase EthAdSystem
        *	
        *	@return toString de la clase.
        */
        public override string ToString()
        {
            return "ethAddSystem";
        }
    }
}
