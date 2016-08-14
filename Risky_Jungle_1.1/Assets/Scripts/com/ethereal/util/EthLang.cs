using UnityEngine;
using System.Text;
using System.Collections.Generic;
using Assets.Scripts.com.ethereal.data;

namespace Assets.Scripts.com.ethereal.util
{

    /** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Julio 24 2014
    * 
    *	@class 	EthLang
    *   @brief 	Esta clase esta encargada de manejar los idiomas, es decir, la internacionalización de las palabras en el juego.
    *
    */
    public class EthLang
    {

        /**
        *	@brief Variable que dicta si el diccionario está activo o no.
        */
        private static bool _langActivated = false;

        /**
        *	@brief Asignación de las propiedades de lectura y escritura para la variable _langActivated.
         *	
         *  @return El valor d ela variable _langActivated.
        */
        public static bool LangActivated
        {
            get { return _langActivated; }
            set { _langActivated = value; }
        }

        /**
        *	@brief Idioma del diccionario a usar.
        */
        private static string _langAct = null;

        /**
        *	@brief Asignación de las propiedades de lectura y escritura para la variable _langAct.
         *	
         *  @return El valor d ela variable _langAct.
        */
        public static string LangAct
        {
            get { return _langAct; }
            set { _langAct = value; }
        }

        /**
        *	@brief Diccionario de datos en el que se encuentran todas las palbras a usar.
        */
        public static Dictionary<string, Dictionary<string, string>> labelsList;

        /**
        *	@brief Variable que dicta si un archivo en el que esta el diccionario ha sido leido o no.
        */
        public static bool langFileReaded = false;

        /**
        *	@brief Método encargado de crear un EthLang.
        *	
        */
        public EthLang()
        {

        }

        /**
        *	@brief Método usado para indicar la palabra a usar en el diccionario.
        *
        *	Por medio del type se identifican las palabras a usar en el cambio de idiomas ya que es un identificador de estas. 
        *
        *	@param type 			Tipo identificador de las palabras a usar.
        *	@param useDictionary	Variable que especifica si se va a usar el diccionario o no.
        *
        *	@return  la palabra deseada en el idioma actual.
        */
        public static string GetEntry(string type, bool useDictionary)
        {

            if (!useDictionary || !_langActivated)
            {
                return type;
            }

            type = type.ToLower();

            if (labelsList[_langAct].ContainsKey(type))
            {
                return labelsList[_langAct][type];
            }
            else
            {
                Debug.Log(string.Format("WARNING: Missing input {0} in the dictionary {1}", type, _langAct));
                return null;
            }
        }

        /**
        *	@brief Método usado para establecer un diccionario a usar.
        *
        *	Éste metodo verifica si el diccionario especificado por la url tiene todas las palabras requeridas para los idiomas, y además 
        *	no posee palabras repetidas. 
        *
        *	@param urlDict 		Url en el que se encuentra el diccionario.
        *	@param langToUse	Idioma que sera usado.
        *
        */
        public static void ActiveLangs(string urlDict, string langToUse)
        {

            if (langFileReaded)
            {
                return;
            }

            langFileReaded = true;

            EthXML ethXmlLangs = EthXML.ReadXML(urlDict);

            int cantLangs = ethXmlLangs.GetAmountChildren();

            labelsList = new Dictionary<string, Dictionary<string, string>>();

            Dictionary<string, bool> allWords = new Dictionary<string, bool>();

            for (int i = 0; i < cantLangs; i++)
            {
                Dictionary<string, string> lang = new Dictionary<string, string>();

                EthXML ethXmlEntries = ethXmlLangs.GetChildAt(i);
                int cantEntries = ethXmlEntries.GetAmountChildren();

                for (int j = 0; j < cantEntries; j++)
                {
                    if(ethXmlEntries.GetChildAt(j).GetNodeType() == "Comment") continue;
					
					string entry = ethXmlEntries.GetChildAt(j)["name"].ToLower();

                    if (i == 0 && allWords.ContainsKey(entry))
                    {
                        Debug.Log(string.Format("The input '{0}' is repeated in the dictionary ", entry));
                    }

                    allWords[entry] = true;
                    lang[entry] = ethXmlEntries.GetChildAt(j)["value"];
                }

                labelsList[ethXmlLangs.GetChildAt(i)["name"]] = lang;
                if (_langAct == null)
                {
                    _langAct = ethXmlLangs.GetChildAt(i)["name"];
                }
            }


            //Verifica que todas las cadenas se encuentren en todos los idiomas
            //Verify that all strings are in all languages

            foreach (KeyValuePair<string, bool> words in allWords)
            {
                string word = words.Key;

                foreach (KeyValuePair<string, Dictionary<string, string>> dicts in labelsList)
                {
                    if (!dicts.Value.ContainsKey(word))
                    {
                        Debug.Log(string.Format("ERROR EN EL DICTIONARIO ({0}) FALTA LA PALABRA ({1})", dicts.Key, word));
                    }
                }
            }

            _langActivated = true;

            LangAct = langToUse;
        }

    }
}