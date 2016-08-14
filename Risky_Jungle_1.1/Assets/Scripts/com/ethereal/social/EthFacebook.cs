using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

namespace com.ethereal.social{

	/** 
	*	@author    Carlos Andres Carvajal <ccarvajal@etherealgf.com>
	* 	@version   1.0 
	* 	@date      Diciembre 16 2014
	* 
	*	@class 	EthFacebook
	*   @brief 	Esta clase se encarga de realizar la integración con Facebook, permite postear en el Timeline del usuario
	*
	*/
	public class EthFacebook{
		/**
		*	@brief Instancia estatica de la clase
		*/
		private static EthFacebook _instance;

		public delegate void OnFacebookReady();
		public static event OnFacebookReady OnReady;
		
		/**
		*	@brief Define si Facebook ya se pudo inicializar
		*/
		private bool _enable;
		public bool Enable {
            get { return _enable; }
        }

        /**
		*	@brief nombre del usuario logueado
		*/
		private string _name;
		public string Name {
            get { return _name; }
            set { _name = value; }
        }

        /**
		*	@brief Birthday del usuario logueado
		*/
		private string _birthday;
		public string Birthday {
            get { return _birthday; }
            set { _birthday = value; }
        }

        /**
		*	@brief genero del usuario logueado
		*/
		private string _gender;
		public string Gender {
            get { return _gender; }
            set { _gender = value; }
        }

        /**
		*	@brief Define si Facebook ya se pudo inicializar
		*/
		private string _email;
		public string Email {
            get { return _email; }
            set { _email = value; }
        }

        /**
		*	@brief se coloca cual fue la ultima accion que se intentó hacer cuando no estaba logueado
		*/
        private int _lastAction;

        /**
		*	@brief Diccionario con la información que se envia para postear en el Timeline del usuario
		*/
		private Dictionary <string, string> _dictPost;

		private MonoBehaviour _parent;

		/**
		*	@brief Constantes de respuesta para el post
		*/
		public const string POST_MSG_NO_INIT = "No inicializado";
		public const string POST_MSG_NO_LOGIN = "No se pudo realizar login";
		public const string POST_MSG_LOGIN_CANCELED = "El usuario canceló el login";
		public const string POST_MSG_POST_ERROR = "No se pudo realizar el post";
		public const string POST_MSG_POST_OK = "Post realizado con exito";
		public const string POST_MSG_POST_CANCELED = "Post cancelado";
		public const string POST_MSG_POST_EMPTY_RESPONSE = "Post recibió respuesta vacía";

		/**
		*	@brief constructor de la clase EthFacebook.
		*
		*	este método permite crear una instancia de la clase EthFacebook
		*/
		private EthFacebook()
		{
			#if UNITY_IPHONE || UNITY_ANDROID
				CallFBInit();
			#endif
		}

		/**
		*	@brief obtiene una instancia de la clase EthFacebook.
		*
		*	obtiene una instancia de la clase EthFacebook, preferiblemente se debe llamar esta funcion al inicio del juego para que
		*	inicialice todo
		*/
		public static EthFacebook GetInstance()
		{
			if ( EthFacebook._instance == null )
			{
				EthFacebook._instance = new EthFacebook();	
			}
			
			return EthFacebook._instance;
		}

		/**
		*	@brief Inicializa el componente de Facebook.
		*
		*	Inicializa el componente de Facebook
		*/
		private void CallFBInit(){
			_enable = false;
	        FB.Init(OnInitComplete, OnHideUnity);
	    }

	    /**
		*	@brief Se lanza cuando se inicializa el componente de Facebook.
		*
		*	Se lanza cuando se inicializa el componente de Facebook
		*/
	    private void OnInitComplete(){
	        Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
	        if ( FB.IsLoggedIn )
			{
	        	GetUserInfo();	
	        }
	        _enable = true;
	        if ( OnReady != null )
			{
	            OnReady( );
	        }
	    }

		/**
		*	@brief Método para verificar si está logueado
		*
		* 	@return bool en caso de estar logueado
		*/
	    public bool IsLoggedIn()
		{
			if ( _enable ){
	    		return FB.IsLoggedIn;
			}

			return false;
	    }

		/**
		*	@brief Método para obtener el id del usuario
		*
		* 	@return string con el id del usuario
		*/
	    public string IdUser()
		{
	    	return FB.UserId;
	    }

	    /**
		*	@brief Se lanza cuando Facebook intenta mostrar contenido html, usado principalmente en web.
		*
		*	Se lanza cuando Facebook intenta mostrar contenido html, usado principalmente en web.
		*
		* 	@param isGameShown dice si el juego tiene el foco o no
		*/
	    private void OnHideUnity(bool isGameShown)
		{
	        Debug.Log("Is game showing? " + isGameShown);
	    }

	    /**
		*	@brief Permite solicitar un fb Login
		*
		* 	@param onPost evento cuando se tenga la respuesta del login
		*/
	    public void LoginFB( OnPostEvent onPost )
		{
	    	OnPost = onPost;
	    	if ( _enable )
			{
	    		_lastAction = 2;
		    	CallFBLogin();
		    }
			else
			{
	    		SendFBResult( false, POST_MSG_NO_INIT);
	    	}
	    }

	    /**
		*	@brief Permite solicitar un fb Logout
		*
		*/
	    public void LogoutFB( )
		{
	    	if ( _enable )
			{
	    		FB.Logout();
		    }
	    }

	    /**
		*	@brief llama el componente de Facebook para hacer login.
		*
		*	llama el componente de Facebook para hacer login
		*/
	    private void CallFBLogin()
	    {
	    	if ( _enable )
			{
	        	FB.Login("email,publish_actions", LoginCallback);
	    	}
	    }

	    /**
		*	@brief Se lanza cuando se recibe una respuesta del login de Facebook.
		*
		*	Se lanza cuando se recibe una respuesta del login de Facebook
		*
		* 	@param result respuesta del login
		*/
	    private void LoginCallback(FBResult result)
		{
	        if (result.Error != null)
			{
	        	Debug.Log( "Error Response:\n" + result.Error);
	        	SendFBResult( false, POST_MSG_NO_LOGIN );
	        }
			else if (!FB.IsLoggedIn)
			{
	            Debug.Log( "Login cancelled by Player");
	            SendFBResult( false, POST_MSG_LOGIN_CANCELED );
	        }
			else
			{
	            Debug.Log( "Login was successful!");
	            switch( _lastAction )
				{
	            	case 0:  SendPost();
	            		break;

	            	case 1:  SendRequest();
	            		break;

	            	case 2: SendFBResult( true, POST_MSG_POST_OK );
	            		break;
	            }

	            GetUserInfo();	           
	        }
	    }

		 /**
		*	@brief Método para obtener la información del usuario
		*/
	    private void GetUserInfo()
		{
	    	FB.API("/me?fields=name,birthday,email,gender", Facebook.HttpMethod.GET, UserInfoCallback);
	    }

		 /**
		*	@brief Método para hacer un llamado a la información del usuario
		*
		* 	@param result, con el resultado
		*/
	    private void UserInfoCallback( FBResult result )
		{
	    	if (result.Error != null)
			{                                                                      
				Debug.Log("Error obteniendo info del usuario: " + result.Text );
			}
			else
			{
				Debug.Log("Info del usuario: " + result.Text );
				JSONObjectBoom json = JSONObjectBoom.Parse( result.Text );
				Name = json.ContainsKey("name")?json.GetString("name"):"-";
				Birthday = json.ContainsKey("birthday")?json.GetString("birthday"):"-";
				Email = json.ContainsKey("email")?json.GetString("email"):"-";
				Gender = json.ContainsKey("gender")?json.GetString("gender"):"-";
			}
	    }

	    /**
		*	@brief Envía una peticion de post en el Timeline del usuario con los datos que se encuentran en _dictPost.
		*
		*	Envía una peticion de post en el Timeline del usuario con los datos que se encuentran en _dictPost
		*/
	    private void SendPost( )
		{
	    	FB.Feed(
	    	    link: _dictPost["link"],
	    	    linkName: _dictPost["linkName"],
	    	    linkCaption: _dictPost["linkCaption"],
	    	    linkDescription: _dictPost["linkDescription"],
	    	    picture: _dictPost["picture"],
	    	    callback: Callback
	    	);
	    }

	    /**
		*	@brief Permite enviar una peticion de post en el Timeline del usuario.
		*
		*	Permite enviar una peticion de post en el Timeline del usuario
		*
		* 	@param urlLink la url al que redireccionara el post
		* 	@param name el nombre del post
		* 	@param urlCaption url del caption, el caption se ve al final del post pero no es "clickeable"
		* 	@param desc el texto del post
		* 	@param urlImg la url de la imagen
		* 	@param onPost evento cuando se tenga la respuesta del post
		*/
	    public void PostFB( string urlLink, string name, string urlCaption, string desc, string urlImg, OnPostEvent onPost )
		{
	    	OnPost = onPost;
	    	if ( _enable ){
	    		//se guarda un diccionario por si no se ha logueado, envie la misma informacion despues de loguearse
	    		_dictPost = new Dictionary<string, string>();
	    		_dictPost.Add( "link", urlLink );
	    		_dictPost.Add( "linkName", name );
	    		_dictPost.Add( "linkCaption", urlCaption );
	    		_dictPost.Add( "linkDescription", desc );
	    		_dictPost.Add( "picture", urlImg );

				if ( FB.IsLoggedIn ){
		    		SendPost();
		    	}else{
		    		_lastAction = 0;
		    		CallFBLogin();
		    	}
	    	}else{
	    		SendFBResult( false, POST_MSG_NO_INIT);
	    	}
	    }

	    /**
		*	@brief Se lanza cuando se recibe una respuesta del post de Facebook.
		*
		*	Se lanza cuando se recibe una respuesta del post de Facebook
		*
		* 	@param result respuesta del post
		*/
	    private void Callback(FBResult result)
		{
	       	// Some platforms return the empty string instead of null.
	        if (!String.IsNullOrEmpty (result.Error))
			{
	            SendFBResult( false, POST_MSG_POST_ERROR );
	        }
			else if (!String.IsNullOrEmpty (result.Text))
			{
	            if ( result.Text.IndexOf( "cancelled\":true") != -1 )
				{
	            	SendFBResult( false, POST_MSG_POST_CANCELED );
	            }
	            else
				{
	            	SendFBResult( true, POST_MSG_POST_OK );
	            }	            
	        }
			else
			{
	            SendFBResult( true, POST_MSG_POST_EMPTY_RESPONSE );
	        }

	    }

		/**
		*	@brief Método de envío de resultado de FB
		*
		*	@param success, en caso de ser exitoso
		*
		*	@param msg, con el mensaje
		*/
	    private void SendFBResult( bool success, string msg )
		{
	    	if( OnPost != null ) 
			{
				OnPost(success, msg);
			}
	    }

		/**
		*	@brief Método de petición
		*/
	    public void Request( string message, string data, string title, string objectText, OnPostEvent onPost, MonoBehaviour parent)
		{
	    	OnPost = onPost;
	    	_parent = parent;
	    	if ( _enable )
			{
	    		//se guarda un diccionario por si no se ha logueado, envie la misma informacion despues de loguearse
	    		_dictPost = new Dictionary<string, string>();
	    		_dictPost.Add( "message", message );
	    		_dictPost.Add( "data", data );
	    		_dictPost.Add( "title", title );
	    		_dictPost.Add( "object", objectText );

				if ( FB.IsLoggedIn )
				{
		    		SendRequest();
		    	}
				else
				{
		    		_lastAction = 1;
		    		CallFBLogin();
		    	}
	    	}
			else
			{
	    		SendFBResult( false, POST_MSG_NO_INIT);
	    	}
	    }

	    /**
		*	@brief Envía un apprequest con los datos que se encuentran en _dictPost.
		*
		*	Envía un apprequest con los datos que se encuentran en _dictPost
		*/
	    private void SendRequest( )
		{
	    	FB.AppRequest(
	    		_dictPost["message"],
	    		 null,
            	 "",
            	 null,
            	 _dictPost["data"],
            	 _dictPost["title"],
    			 CallbackRequest
	    	);
	    }

	    /**
		*	@brief Se lanza cuando se recibe una respuesta del AppRequest de Facebook.
		*
		*	Se lanza cuando se recibe una respuesta del AppRequest de Facebook
		*
		* 	@param result respuesta del AppRequest
		*/
	    private void CallbackRequest(FBResult result)
		{
	    	Debug.Log("answer: " + result.Text);

	    	if (!String.IsNullOrEmpty (result.Error))
			{
	            SendFBResult( false, POST_MSG_POST_ERROR );
	        }
			else if (!String.IsNullOrEmpty (result.Text))
			{
	            if ( result.Text.IndexOf( "cancelled\":true") != -1 )
				{
	            	SendFBResult( false, POST_MSG_POST_CANCELED );
	            }
	            else
				{
	            	DataManager.GetInstance().SaveRequest( result.Text, _dictPost["object"], _parent, OnSavedRequest ); 
				}	            
	        }else{
	            SendFBResult( false, POST_MSG_POST_EMPTY_RESPONSE );
	        }	    	
	    }

		/**
		*	@brief Método cuando se realiza solicitud
		*
		* 	@param success, en caso de ser exitoso
		*/
	    public void OnSavedRequest( bool success  )
		{
	    	if ( success )
			{
	    		SendFBResult( true, POST_MSG_POST_OK );
	    	}
	    	else
			{
	    		SendFBResult( false, POST_MSG_POST_OK );
	    	}
	    }

		/**
		*	@brief Método para obtener foto del FB
		*
		* 	@param parent, con la clase padre
		*
		* 	@param idUser, con el id del usuario que se desea
		*
		* 	@param onPhoto, con el evento asociado
		*
		* 	@param info, con la información
		*/
	    public void GetFBPhoto( MonoBehaviour parent, string idUser, OnPhotoEvent onPhoto, string info = "")
		{
			OnPhoto = onPhoto;
			parent.StartCoroutine( GetPhoto( idUser, info ) );
	    }

		/**
		*	@brief Método para obtener foto del FB
		*
		* 	@param idUser, con el id del usuario que se desea
		*
		* 	@param info, con la información
		*
		* 	@return IEnumerator, con la enumeración
		*/
	    private IEnumerator GetPhoto( string idUser, string info = "" )
		{
	        WWW url = new WWW("https://graph.facebook.com/" + idUser + "/picture?type=large"); //+ "?access_token=" + FB.AccessToken);

	        Texture2D textFb2 = new Texture2D(78, 87, TextureFormat.RGB24, false); //TextureFormat must be DXT5

	        yield return url;
	        
	        url.LoadImageIntoTexture(textFb2);
	        if ( OnPhoto != null )
			{
	        	OnPhoto( textFb2, info );
	        }
	    }

	    /**
		*	@brief Metodo toString de la clase.
		*
		*	Retorna el nombre de la clase como una cadena de caracteres.
		*
		*	@return la clase representada en un string.
		*/
		public override string ToString()
		{
			return "EthFacebook";
		}

		/**
		*	@brief delegate para enviar la respuesta cuando se obtiene la respuesta del post
		*
		* 	@param success si fue exitoso el post
		* 	@param msg si hubo error envia el texto del error
		*/
		public delegate void OnPostEvent (bool success, string msg );

		/**
		*	@brief delegate para enviar la respuesta cuando se obtiene la respuesta de obtener foto
		*
		* 	@param photo el texture2d de la foto
		* 	@param info informacion adicional que puede ser enviada y que se retornará cuando se obtenga la foto
		*/
		public delegate void OnPhotoEvent ( Texture2D photo, string info );
		
		/**
		*	@brief evento para enviar la respuesta cuando se obtiene la respuesta del post
		*/
		public event OnPostEvent OnPost;

		/**
		*	@brief evento para enviar la respuesta cuando se obtiene la respuesta de obtener foto
		*/
		public event OnPhotoEvent OnPhoto;
	}
}