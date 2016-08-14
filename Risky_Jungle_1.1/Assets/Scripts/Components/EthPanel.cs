using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Assets.Scripts.com.ethereal.util;
using UnityStandardAssets.ImageEffects;
/**
 * EthPanel panel = new EthPanel( GameObject.Find("Canvas"), 
                    iTween.Hash("bg", "aparato",
                    "botAmount", 2,
                    "botWidth", 100,
                    "botHeight", 100,
                    "botSeparator", 10,
                    "bot1Text", "OK",
                    "bot2Text", "CANCEL",
                    "bot1Img", "Screens/General/chulo",
                    "bot2Img", "Screens/General/x",
                    "font", "Fonts/Windlass",
                    "fontColor", Color.black,
                    "fontSize", 20,
                    "useLang", false,
                    "title", "este es un titulo",
                    "titleColor", Color.white,
                    "titleSize", 40,
                    "titleWidth", 400,
                    "titleHeight", 100,
                    "titleY", 200 ) );
        panel.OnButtonClickEvt += clicOnDialog;
 */

/** 
*   @author    EtherealGF <www.etherealgf.com> 
*   @version   1.0 
*   @date      Octubre 16 2015
* 
*   @class  EthPanel
*   @brief  Esta clase se encarga de administrar las propiedades de los elementos de tipo panel.
*/
public class EthPanel{
    
    /**
    *   @brief Método ejecutado cuando se ejecute un evento de clic en un botón.
    *   
    *   @param index índice del boton.
    */
    public delegate void OnButtonClick(int index);
    
    /**
    *   @brief  Evento llamado cuando se efectue un evento de clic en un botón, que luego será modificado.
    */
    public event OnButtonClick OnButtonClickEvt;

    /**
    *   @brief  Contador auxiliar para identificar si el panel ha sido creado.
    */
    public static int count = 0;

    /**
    *   @brief  Objeto que representa el canvas del panel.
    */
    private GameObject canvas;

    /**
    *   @brief  Objeto que representa el panel.
    */
    private GameObject panel;

    /**
    *   @brief  Objeto que representa el loader del contenido del panel.
    */
    private static GameObject loader;

    /**
    *   @brief  Fuente del panel.
    */
    private Font font;

    /**
    *   @brief  Efecto optimizado de blur a una imagen.
    */
    private BlurOptimized blur = null;

    /**
    *   @brief  Panel actual.
    */
    public static EthPanel currentPanel;

    /**
    *   @brief  Argumentos del panel.
    */
    private Hashtable args;

    /**
    *   @brief Método constructor del EthPanel.
    *
    *   @param GameObject canvas    Borde del panel.
    *   @param Hashtable args       Argumentos del panel a crear.
    *   @param bool isOk            Define si el panel esta listo.
    */
    public EthPanel( GameObject canvas, Hashtable args, out bool isOk ){
        if ( count == 0 ){
            count ++;
            this.canvas = canvas;
            this.args = args;
            InitPanel( );
            isOk = true;
            currentPanel = this;
        }
        else{
            isOk = false;
        }

    }

    /**
    *   @brief  Método que inicia un panel, coloca los objetos de acuerdo a los argumentos enviados en el constructor.
    */
    private void InitPanel( ){
        panel = (GameObject)GameObject.Instantiate( Resources.Load("EthPanel") );
        panel.transform.SetParent( canvas.transform );

        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector2.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        if ( args.Contains("bg") ){
            SetImage( (string)args["bg"] );
        }

        if ( args.Contains("font") ){
            font = Resources.Load( (string)args["font"] ) as Font;
        }

        if ( args.Contains("botAmount") ){
            int botAmount = (int)args["botAmount"];
            if ( botAmount > 0 ){
                SetButtons( botAmount );
            }
        }

        if ( args.Contains("title") ){
            SetTitle( );
        }

        if ( args.Contains("blur") ){
            blur = (BlurOptimized)args["blur"];
            blur.enabled = true;
        }
    }

    /**
    *   @brief  Método que se encarga de colocar un titulo en el panel.
    */
    private void SetTitle( ){
        GameObject titleGO = new GameObject();
        titleGO.transform.SetParent( panel.transform, false );
        EthText text = titleGO.AddComponent<EthText>();
        titleGO.name = "title";
        
        text.text = (string)args["title"];   
        text.alignment = TextAnchor.MiddleCenter;   
        text.font = font; 
        //text.font.lineHeight = 2;
        text.color = (Color)args["titleColor"];
        text.fontSize = (int)args["titleSize"];
        text.languageSupport = (bool)args["useLang"];
        if ( text.languageSupport ){
            text.textLanguage = (string)args["title"];
            text.text = EthLang.GetEntry( text.textLanguage, true );
        }

        RectTransform rectTransform = titleGO.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector2( 0, (int)args["titleY"] );
        //este es top center
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.sizeDelta = new Vector2( (int)args["titleWidth"], (int)args["titleHeight"] );

    }

    /**
    *   @brief  Método que se encarga de colocar los botones definidos en los argumentos enviados en el constructor.
    *
    *   @param int botAmount Cantidad de botones.
    */
    private void SetButtons( int botAmount ){
        int botWidth = (int)args["botWidth"];
        int botHeight = (int)args["botHeight"];
        int botSeparator = ( args.Contains("botSeparator") )?(int)args["botSeparator"]:0;
        int initX = -((botAmount+ botSeparator)*(botAmount-1) )/2 - botWidth / 2;
        
        for( int i = 0; i < botAmount; i ++ ){
            GameObject button = (GameObject)GameObject.Instantiate( Resources.Load("EthButton") );
            
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            //este es top center
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.sizeDelta = new Vector2( botWidth, botHeight );
            rectTransform.localPosition = new Vector2( initX + botWidth * i + botSeparator*i, (int)args["botY"] );

            button.transform.SetParent( panel.transform, false );
            string argText = "bot" + (i+1) + "Text";

            EthText text = button.GetComponentInChildren<EthText>();
            if ( args.Contains(argText) ){
                text.text = (string)args[argText];   
                text.alignment = TextAnchor.MiddleCenter;   
                text.font = font; 
                text.color = (Color)args["fontColor"];
                text.fontSize = (int)args["fontSize"];
                text.languageSupport = (bool)args["useLang"];
                if ( text.languageSupport ){
                    text.textLanguage = (string)args[argText];
                    text.text = EthLang.GetEntry( text.textLanguage, true );
                }
            }
            else{
                text.text = "";
            }
            
            string argImg = "bot" + (i+1) + "Img";
            Sprite img = Resources.Load<Sprite>( (string)args[argImg] );
            button.GetComponent<Image>().sprite = img;

            Button b = button.GetComponent<Button>();
            AddListener( b, (i+1) );
        }
    }

    /**
    *   @brief  Método que se encarga de agregar el listener al boton del panel y el valor que se envia cuando se da click.
    *
    *   @param Button b     Botón al cual se le agregará el listener.
    *   @param int value    Valor que será enviado cuando se haga clic en el botón.
    */
    private void AddListener( Button b, int value ){
        b.onClick.AddListener(() => ClickOnButton(value) );
    }

    /**
    *   @brief  Método que se llama cuando se da clic a un botón del panel.
    *
    *   @param int index Índice del evento.
    */
    private void ClickOnButton( int index ){
        if ( OnButtonClickEvt != null ){
            OnButtonClickEvt( index );
        }

        Destroy();
    }

    /**
    *   @brief  Método que destruye el panel.
    */
    public void Destroy(){
        count --;
        if ( blur != null ){
            blur.enabled = false;
        }
        GameObject.Destroy(panel);
        currentPanel = null;
    }

    /**
    *   @brief  Método que agrega una imagen al panel, la imagen tiene que ser un sprite y estar en el directorio Resources.
    *
    *   @param String url   URL de la imagen a agregar.
    *   @param int w        Ancho de la imagen.
    *   @param int h        Altura de la imagen.
    *   @param int posX     Posición en el eje X donde se ubicará la imagen.
    *   @param int posY     Posición en el eje Y donde se ubicará la imagen.
    *   @param String name  Nombre de la imagen.
    */
    public void AddImage( string url, int w, int h, int posX, int posY, string name = "img" ){
        Sprite img = Resources.Load<Sprite>( url );
        GameObject imgGO = new GameObject();
        imgGO.transform.SetParent( panel.transform, false );
        Image image = imgGO.AddComponent<Image>();
        image.sprite = img;
        image.preserveAspect = true;

        imgGO.name = name;

        RectTransform rectTransform = imgGO.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector2.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        //este es top center
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.sizeDelta = new Vector2( w, h );
        rectTransform.anchoredPosition = new Vector2( 0.5f + posX, posY );
    }

    /**
    *   @brief  Método que coloca una imagen al panel.
    *
    *   @param string urlImg URL de la imagen.
    */
    private void SetImage( string urlImg ){
        int widthImg = (int)args["bgWidth"];
        int heightImg = (int)args["bgHeight"];
        AddImage( urlImg , widthImg, heightImg, 0, -heightImg/2, "BG") ;
        /*Sprite img = Resources.Load<Sprite>( urlImg );
        GameObject imgGO = new GameObject();
        imgGO.transform.SetParent( panel.transform, false );
        Image image = imgGO.AddComponent<Image>();
        image.sprite = img;
        image.preserveAspect = true;

        
        
        imgGO.name = "BG";

        RectTransform rectTransform = imgGO.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector2.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        //este es top center
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.sizeDelta = new Vector2( widthImg, heightImg );
        rectTransform.anchoredPosition = new Vector2( 0.5f, -heightImg/2 );*/

    }

    /**
    *   @brief  Método que muestra un cargador, para ello tiene que haber un prefab que se llame Loader en Resources.
    *
    *   @param GameObject canvasLoader Loader del canvas.
    */
    public static void ShowLoader( GameObject canvasLoader ){
        loader = (GameObject)GameObject.Instantiate( Resources.Load("Loader") );
        loader.transform.SetParent( canvasLoader.transform, false );
    } 

    /**
    *   @brief  Método que oculta el cargador.
    */
    public static void HideLoader( ){
        if ( loader != null ){
            GameObject.Destroy( loader );
            loader = null;
        }        
    } 
}
