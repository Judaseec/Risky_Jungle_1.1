using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.com.ethereal.util;

namespace Assets.Scripts.com.ethereal.audio
{

    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Julio 1 2014
    * 
    *	@class 	EthAudio
    *   @brief 	Esta clase se encarga del manejo de Audio dentro de la Arquitectura.
    *
    *	El audio se maneja colocando un GameObject en la misma posicion de la cÃ¡mara, con un componente AudioSource.
    *
    *	Se manejan 2 canales, uno de MÃºsica, que solo reproduce un audio al tiempo  (musicSource) de manera cÃ­clica y uno de efectos que 
    *	reproduce un audio una vez (effectSource).
    *
    */
    public class EthAudio
    {
        /**
        *	@brief PatrÃ³n Singleton para mantener la misma instancia de EthAudio en todo el juego.
        */
        private static EthAudio _instance;

        /**
        *	@brief AsignaciÃ³n de las propiedades de lectura y escritura de la variable _instance.
        *	
        *   @return El objeto de la variable _instance, que viene  siendo Ã©l mismo, actuando como patrÃ³n singleton.
        */
        public static EthAudio Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }
        /**
        *	@brief GameObject que se agrega en la posicion de la camara.
        */
        private GameObject _gameObject;

        /**
        *	@brief AudioSource que reproduce un audio al tiempo de manera ciclica.
        */
        private AudioSource _musicSource;

        /**
         * @brief List con los AudioSource que reproducen un audio una vez
         */
        private List<AudioSource> _arrayEffectSource;

        /**
         *	@brief Ruta en donde se encuentra la musica de fondo
         */
        private string _mainLoopPath;

        /**
         * @brief float con el valor del volumen (valor de 1 a 100) para los efectos
         */
        private float _effVolume = 100;

        /**
        *	@brief Constante para indicar la fuente del audio.
        */
        private const string AUDIO_SOURCE_COMPONENT_NAME = "AudioSource";

        /**
        *	@brief Constante que se usa en el mÃ©todo ToString para retornar el nombre de la clase.
        */
        private const string CLASS_NAME = "EthAudio";

        /**
        *   @brief string que se usa para definir el audio por defecto caundo se le da clic a un boton
        */
        private string _audioButtonDefault = "";

        /**
        *	@brief MÃ©todo para definir el EthAudio a usar.
        *	
        *	Este mÃ©todo es el encargado de que cuando no haya alguna instancia de EthAudio cree una nueva, 
        *	de lo contrario si ya hay un EthAudio en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de audio. 
        */
        public static EthAudio GetInstance(MonoBehaviour refParent)
        {
            if (EthAudio._instance == null)
            {
                EthAudio._instance = new EthAudio();
            }

            return EthAudio._instance;
        }

        /**
        *	@brief MÃ©todo para Instanciar un EthAudio.
        *	
        *	Este mÃ©todo es el encargado de crear un nuevo EthAudio asignando todos los atributos que requiere esta clase.
        */
        private EthAudio()
        {
            _arrayEffectSource = new List<AudioSource>();
            _gameObject = new GameObject();            
            _gameObject.name = CLASS_NAME;
            MonoBehaviour.DontDestroyOnLoad(_gameObject);

            _musicSource = _gameObject.AddComponent<AudioSource>() as AudioSource;
            _musicSource.loop = true;
        }

        /**
        *	@brief MÃ©todo para Reproducir la mÃºsica.
        *	
        *	Este mÃ©todo es el encargado de reproducir la mÃºsica de fondo si no existe aÃºn que se desea colocar para el juego .
        *
        *	@param path Ruta donde se encuentra la mÃºsica a reproducir.
        */
        public void PlayMusic(string path)
        {
            // Si de hecho es el audio que esta sonando entonces lo deja continuar sonando.
            if (path == _mainLoopPath)
            {
                return;
            }
            _mainLoopPath = path;
            AudioClip mainLoop = (AudioClip)Resources.Load(path);
            _musicSource.clip = mainLoop;
            _musicSource.Play();
        }

        /**
        *	@brief MÃ©todo para Reproducir un efecto.
        *	
        *	Este mÃ©todo es el encargado de reproducir los efectos, cargando el audio desigando por la ruta que entra por parametro.
        *
        *	@param path Ruta donde se encuentra el efecto a reproducir.
        */
        public void PlayEffect(string path)
        {            
            AudioSource effectSource = _gameObject.AddComponent<AudioSource>() as AudioSource;
            effectSource.loop = false;

            AudioClip efecto = (AudioClip)Resources.Load(path);
            effectSource.clip = efecto;
            effectSource.volume = _effVolume / 100;
            effectSource.Play();
            _arrayEffectSource.Add(effectSource);
            new EthTimer((int)((efecto.length * 1000) + 500), RemoveEffect, effectSource, 1);
        }
		
		/**
        *	@brief MÃ©todo para Reproducir un efecto repetido.
        *	
        *	Este mÃ©todo es el encargado de reproducir los efectos, cargando el audio desigando por la ruta que entra por parametro.
        *
        *	@param path Ruta donde se encuentra el efecto a reproducir.
        */
		public AudioSource PlayEffectRepeated (string path)
		{
			AudioSource effectSource = _gameObject.AddComponent<AudioSource>() as AudioSource;
 			effectSource.loop = true;

 			AudioClip efecto = (AudioClip)Resources.Load(path);
			effectSource.clip = efecto;
 			effectSource.volume = _effVolume/100;
 			effectSource.Play();
 			_arrayEffectSource.Add(effectSource);
 			return effectSource;
		}

        /**
        *	@brief MÃ©todo para remover un efecto de audio que se este reproduciendo.
        *	
        *	Este mÃ©todo es el encargado de remover los efectos, encargandose primero de detener el efecto determinado que entra por parametro, para
        *	luego removerlo del array de efectos y destruirlo.
        *
        *	@param effectSource Efecto que se desea remover del juego.
        */
        public void RemoveEffect(object effectSourceObj)
        {
            AudioSource effectSource = (AudioSource)effectSourceObj;
            effectSource.Stop();
            _arrayEffectSource.Remove(effectSource);
            GameObject.Destroy(effectSource);
        }

        /**
        *	@brief MÃ©todo para Asignar el volumen de la mÃºsica.
        *	
        *	Este mÃ©todo es el encargado de ajustar el volumen de la mÃºsica que se esta reproduciendo
        *	con respecto al valor que le entra por parametro.
        *
        *	@param valVolume Valor de 1 a 100 que indica el nuevo volumen de la musica.
        */
        public void SetMusicVolume(float valVolume)
        {
            _musicSource.volume = valVolume / 100;
        }

        /**
        *	@brief MÃ©todo para llevar el GameObject a la posicion de la cÃ¡mara.
        *	
        *	Este mÃ©todo es el encargado de asignar la posicion de la cÃ¡mara al gameObject.
        *
        */
        public void RefreshPos()
        {
            _gameObject.transform.position = Camera.main.transform.position;
        }

        /**
        *	@brief MÃ©todo para Asignar el volumen de los efectos.
        *	
        *	Este mÃ©todo es el encargado de ajustar el volumen de los efectos con respecto al valor que le entra por parametro.
        *
        *	@param valVolume Valor de 1 a 100 que indica el nuevo volumen del efecto.
        */
        public void SetEffectsVolume(float valVolume)
        {
            _effVolume = valVolume;

            for (int i = 0; i < _arrayEffectSource.Count; i++)
            {
                _arrayEffectSource[i].volume = _effVolume / 100;
            }
        }
		
		/**
        *	@brief MÃ©todo para obtener el volumen de los efectos.
        *	
        *	Este mÃ©todo es el encargado de obtener el volumen de los efectos.
        *
        *	@return effVolume, valor que indica el volumen de los efectos.
        */
		public float GetEffectsVolume() 
		{
 			return _effVolume;
 		}

        public void SetAudioButtonDefault( string audioDefault ){
            this._audioButtonDefault = audioDefault;
        }

        public string GetAudioButtonDefault(){
            return _audioButtonDefault;
        }
    }
}
