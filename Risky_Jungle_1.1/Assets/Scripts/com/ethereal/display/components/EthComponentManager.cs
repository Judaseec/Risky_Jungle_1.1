using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts.com.ethereal.util;
using Assets.Scripts.com.ethereal.display.easing.time;

namespace Assets.Scripts.com.ethereal.display.components
{

    /** 
    *   @author    EtherealGF <www.etherealgf.com> 
    *   @version   1.0 
    *   @date      Octubre 16 2014
    * 
    *   @class  EthComponentManager
    *   @brief  Esta clase se encarga de administrar los componentes que pertenescan a ella.
    *
    */
    public class EthComponentManager : EthComponent
    {

        /**
        *   @brief Lista de todos los EthComponents pertenecientes al EthComponentManager.
        */
        private ArrayList _allComponents = new ArrayList();

        /**
        *   @brief Tasa de crecimiento para el ancho del EthComponentManager.
        */
        private float _wRatio = 1f;

        /**
        *   @brief Asignación de la propiedad de lectura para la variable _wRatio.
        *   
        *   @return El valor de la variable _wRatio.
        */
        public float WRatio
        {
            get
            {
                return _wRatio;
            }
            set
            {
                _wRatio = value;
            }
        }
        /**
        *   @brief Tasa de crecimiento para la altura del EthComponentManager.
        */
        private float _hRatio = 1f;

        /**
        *   @brief Asignación de la propiedad de lectura para la variable _hRatio.
        *   
        *   @return El valor de la variable _hRatio.
        */
        public float HRatio
        {
            get
            {
                return _hRatio;
            }
            set
            {
                _hRatio = value;
            }
        }

        /**
        *   @brief desplazamiento en el anchor del EthComponentManager.
        */
        private float _wOffset = 0f;

        /**
        *   @brief Asignación de las propiedades de lectura y escritura de la variable _wOffset
        *   
        *   @return El valor de la variable _wOffset.
        */
        public float WOffset
        {
            get
            {
                return _wOffset;
            }
            set
            {
                _wOffset = value;
            }
        }

        /**
        *   @brief desplazamiento en la altura del EthComponentManager.
        */
        private float _hOffset = 0f;

        /**
        *   @brief Asignación de las propiedades de lectura y escritura de la variable _hOffset
        *   
        *   @return El valor de la variable _hOffset.
        */
        public float HOffset
        {
            get
            {
                return _hOffset;
            }
            set
            {
                _hOffset = value;
            }
        }

        /**
        *   @brief Anchor del objeto.
        */
        private float _objectWidth;

        /**
        *   @brief Asignación de las propiedades de lectura y escritura para la variable _objectWidth.
        *   
        *   @return El valor de la variable _objectWidth.
        */
        public float ObjectWidth
        {
            get
            {
                return _objectWidth;
            }
            set
            {
                _objectWidth = value;
            }
        }

        /**
        *   @brief Altura del objeto.
        */
        private float _objectHeight;

        /**
        *   @brief Asignación de las propiedades de lectura y escritura para la variable _objectHeight.
        *   
        *   @return El valor de la variable _objectHeight.
        */
        public float ObjectHeight
        {
            get
            {
                return _objectHeight;
            }
            set
            {
                _objectHeight = value;
            }
        }

        /**
        *   @brief Punto mínimo en el eje Y que se ha elongado el EthComponentManager.
        */
        private float _minY = 0;

        /**
        *   @brief Punto máximo en el eje Y que se ha elongado el EthComponentManager.
        */
        private float _maxY = 0;

        /**
        *   @brief Asignación de las propiedades de lectura y escritura para la variable _maxY.
        *   
        *   @return El valor de la variable _maxY.
        */
        public float MaxY
        {
            get
            {
                return _maxY;
            }
            set
            {
                _maxY = value;
            }
        }

        /**
        *   @brief Indíca la cantidad de veces que los valores de la pantalla han sido copiados de otra.
        */
        private static int _val = 0;

        /**
        *   @brief Indíca sí el EthComponentManager es el primer nivel de su árbol.
        */
        private bool _firstLevel = true;

        /**
        *   @brief Indíca sí el EthComponentManager usa un grupo.
        */
        private bool _shouldUseGroup = false;

        /**
        *   @brief Asignación de las propiedades de lectura y escritura para la variable _shouldUseGroup.
        *   
        *   @return El valor de la variable _shouldUseGroup.
        */
        public bool ShouldUseGroup
        {
            get
            {
                return _shouldUseGroup;
            }
            set
            {
                _shouldUseGroup = value;
            }
        }

        /**
        *   @brief EthComponentManager padre del actual EthComponentManager.
        */
        private EthComponentManager _guiParent;

        /**
        *   @brief Asignación de las propiedades de lectura y escritura para la variable _guiParent.
        *   
        *   @return El valor de la variable _guiParent.
        */
        public EthComponentManager GuiParent
        {
            get
            {
                return _guiParent;
            }
            set
            {
                _guiParent = value;
            }
        }

        /**
        *   @brief Variable que define si el EthComponentManager se está animando.
        */
        private bool _isAnimating = false;

        /**
        *   @brief Animación del EthComponentManager.
        */
        private AnimationEase _animat;

        /**
        *	@brief Constructor de la clase EthComponentManager.
        *
        *	Este método permite crear una instancia de la clase EthComponentManager.
        *
        *	@param guiParent GUI sobre la cual se creará el component manager.
        *
        */
        public EthComponentManager(EthComponentManager guiParent)
        {
            if (guiParent != null)
            {
                this._guiParent = guiParent;
                CenterScreen(guiParent._objectWidth, guiParent._objectHeight);
            }
        }

        /**
        *	@brief Método para definir que el EthComponentManager use un grupo.
        *
        *	@param x 	Pocisión en x del EthComponentManager.
        *	@param y 	Pocisión en x del EthComponentManager.
        *	@param wid 	Anchor del EthComponentManager.
        *	@param hei 	Altura del EthComponentManager.
        *
        */
        public void UseGroup(float x, float y, float wid, float hei)
        {
            _shouldUseGroup = true;
            X = x;
            Y = y;
            Wid=wid;
            Hei=hei;

            _wOffset = 0;
            _hOffset = 0;
        }

        /**
        *	@brief Método para copiar los valores de pantalla de otro component manager.
        *
        *	@param guiParent Parent del cual se copiarán los valores.
        *
        */
        public void CopyScreenValues(EthComponentManager guiParent)
        {
            Name = "" + _val;
            _val++;

            _wRatio = guiParent._wRatio;
            _hRatio = guiParent._hRatio;
            _hOffset = guiParent._hOffset;
            _wOffset = guiParent._wOffset;
        }

        /**
        *	@brief Método para eliminar un EthComponentManager de un EthComponentManager padre.
        *
        *	@param callParent Parámetro que define si se remueve el EthComponentManager, si no se ingresa se tomara true por defecto.
        *
        */
        public override void Remove(bool callParent = true)
        {
            if (callParent)
            {
                _guiParent.RemoveComponent(this);
            }
        }

        /**
        *	@brief Método para centrar la pantalla.
        *
        *	@param objectWidth	Altura de la pantalla.
        *	@param objectHeight	Anchor de la pantalla.
        *
        */
        public void CenterScreen(float objectWidth, float objectHeight)
        {
            this._objectWidth = objectWidth;
            this._objectHeight = objectHeight;

            _wRatio = Screen.width / objectWidth;
            _hRatio = Screen.height / objectHeight;

            if (_wRatio < _hRatio)
            {
                _hOffset = (Screen.height - (objectHeight * _wRatio)) / 2;
                _hRatio = _wRatio;
            }
            else
            {
                _wOffset = (Screen.width - (objectWidth * _hRatio)) / 2;
                _wRatio = _hRatio;
            }
        }

        /**
        *	@brief Método para agregar una ventana.
        *
        *	@param ethWindow Ventana a agregar.
        *
        *	@return La ventana agregada.
        */
        public EthComponentManager AddWindow(EthComponentManager ethWindow)
        {

            ethWindow._firstLevel = false;

            if (!this._firstLevel)
            {
                ethWindow._wOffset = 0;
                ethWindow._hOffset = 0;
            }

            _allComponents.Add(ethWindow);
            return ethWindow;
        }

        /**
        *	@brief Método para agregar un submenú.
        *
        *	Este método recibe un solo parámetro string, y llama al metodo que recibe dos parámetros, mandando un string vacío en el segundo.
        *
        *	@param name Nombre del submenú.
        *
        *	@return El submenú creado, únicamente con nombre.
        */
        public EthTabSheet AddTabSheet(string name)
        {
            return AddTabSheet(name, "");
        }

        /**
        *	@brief Método para agregar un submenú.
        *
        *	Este método recibe dos parámetros string, y llama al metodo que recibe un parámetro string y uno EthArguments.
        *	Crea un EthArguments a partir del segunda parametro string (par).
        *
        *	@param name Nombre del submenú.
        *	@param par 	Parametros del submenú.
        *
        *	@return El submenú creado con nombre y argumentos.
        */
        public EthTabSheet AddTabSheet(string name, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddTabSheet(name, args);
        }

        /**
        *	@brief Método para agregar un submenú.
        *
        *	Este método recibe dos parámetros, un string y un EthArguments, los cuales utiliza para crear el componente.
        *
        *	@param name Nombre del submenú.
        *	@param args Argumentos del submenú.
        *
        *	@return El submenú creado con sus respectivas propiedades.
        */
        public EthTabSheet AddTabSheet(string name, EthArguments args)
        {
            args["name"] = name;
            EthTabSheet tempComponent = new EthTabSheet(args, this);
            _allComponents.Add(tempComponent);
            return tempComponent;
        }


        /**
        *	@brief Método para agregar un scroll.
        *
        *	Este método recibe los parámetros name, x, y, width y height del scroll, llama al método que recibe estos mismos más otro string que
        *	corresponde a lo argumentos, pero este lo ingresa vacío.
        *
        *	@param name 	Nombre del scroll.
        *	@param x 		Posición en x del scroll.
        *	@param y 		Posición en y del scroll.
        *	@param width 	Anchor del scroll.
        *	@param height 	Altura del scroll.
        *
        *	@return El scroll con los parámetros ingresados.
        */
        public EthScroll AddScroll(string name, float x, float y, float width, float height)
        {
            return AddScroll(name, x, y, width, height, "");
        }

        /**
        *	@brief Método para agregar un scroll.
        *
        *	Este método recibe los parámetros name, x, y, width y height del scroll, además de un string (par) para crear los EthArguments.
        *
        *	@param name 	Nombre del scroll.
        *	@param x 		Posición en x del scroll.
        *	@param y 		Posición en y del scroll.
        *	@param width 	Anchor del scroll.
        *	@param height 	Altura del scroll.
        *	@param par 		Propiedades para crear los EthArguments del scroll.
        *
        *	@return El scroll con los parámetros ingresados.
        */
        public EthScroll AddScroll(string name, float x, float y, float width, float height, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddScroll(name, x, y, width, height, args);
        }

        /**
        *	@brief Método para agregar un scroll.
        *
        *	Este método recibe los parámetros name, x, y, width, height y los EthArguments del scroll.
        *
        *	@param name 	Nombre del scroll.
        *	@param x 		Posición en x del scroll.
        *	@param y 		Posición en y del scroll.
        *	@param width 	Anchor del scroll.
        *	@param height 	Altura del scroll.
        *	@param args 	EthArguments del scroll.
        *
        *	@return El scroll con los parámetros ingresados.
        */
        public EthScroll AddScroll(string name, float x, float y, float width, float height, EthArguments args)
        {
            args["name"] = name;

            EthScroll tempScroll = new EthScroll(args, this);
            tempScroll.X = x;
            tempScroll.Y=y;
            tempScroll.Wid=width;
            tempScroll.Hei=height;

            tempScroll.gui.CenterScreen(_objectWidth, _objectHeight);
            if (_shouldUseGroup)
            {
                tempScroll.gui._wOffset = 0;
            }
            _allComponents.Add(tempScroll);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempScroll.Hei) * _hRatio)
            {
                _maxY = (y + tempScroll.Hei) * _hRatio;
            }

            return tempScroll;
        }

        /**
        *	@brief Método para evaluar si hay un nuevo minimo o maximo, sí lo hay, cambia los valores.
        *	
        */
        public void EvaluateMinAndMax(float y, float hei)
        {
            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + hei) * _hRatio)
            {
                _maxY = (y + hei) * _hRatio;
            }
        }

        /**
        *	@brief Método para restablecer a cero los valores de minY y maxY.	
        */
        public void ResetMinAndMax()
        {
            _minY = 0;
            _maxY = 0;
        }

        /**
        *	@brief Método para agregar una textura cruda.
        *
        *	Este método recibe los parámetros name, x, y y text de la textura, además de un string (par) para crear los EthArguments.
        *
        *	@param name 	Nombre de la textura.
        *	@param x 		Posición en x de la textura.
        *	@param y 		Posición en y de la textura.
        *	@param par 		Propiedades para crear los EthArguments de la textura.
        *	@param text 	Atributo Texture2D para manipular la textura.
        *
        *	@return La textura cruda con los parámetros ingresados.
        */
        public EthTexture AddRawTexture(string name, float x, float y, string par, Texture2D text)
        {
            EthArguments args = new EthArguments(par);
            return AddRawTexture(name, x, y, args, text);
        }

        /**
        *	@brief Método para agregar una textura cruda.
        *
        *	Este método recibe los parámetros name, x, y y los EthArguments de la textura, también evalúa el minY y maxY.
        *
        *	@param name 	Nombre de la textura.
        *	@param x 		Posición en x de la textura.
        *	@param y 		Posición en y de la textura.
        *	@param args 	EthArguments de la textura.
        *	@param text 	Atributo Texture2D para manipular la textura.
        *
        *	@return La textura cruda con los parámetros ingresados.
        */
        public EthTexture AddRawTexture(string name, float x, float y, EthArguments args, Texture2D text)
        {

            //addListeners();
            args["name"] = name;
            args["x"] = "" + x;
            args["y"] = "" + y;

            EthTexture tempButton = new EthTexture(args, this);
            tempButton.SetRawTexture(args, text);
            _allComponents.Add(tempButton);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempButton.Hei) * _hRatio)
            {
                _maxY = (y + tempButton.Hei) * _hRatio;
            }

            return tempButton;
        }

        /**
        *	@brief Método para agregar una textura render.
        *
        *	Este método recibe los parámetros name, x, y y text de la textura, además de un string (par) para crear los EthArguments.
        *	Este tipo de textura puede ser utilizada para implementar imagenes basadas en efectos de renderizado, sombras dinámicas,
        *	proyectores, reflexiones o cámaras de vigilancia.
        *
        *	@param name 	Nombre de la textura.
        *	@param x 		Posición en x de la textura.
        *	@param y 		Posición en y de la textura.
        *	@param par 		Propiedades para crear los EthArguments de la textura.
        *	@param text 	Atributo Texture2D para manipular la textura.
        *
        *	@return La textura render con los parámetros ingresados.
        */
        public EthTexture AddRenderTexture(string name, float x, float y, string par, RenderTexture text)
        {
            EthArguments args = new EthArguments(par);
            return AddRenderTexture(name, x, y, args, text);
        }

        /**
        *	@brief Método para agregar una textura render.
        *
        *	Este método recibe los parámetros name, x, y y los EthArguments de la textura, también evalúa el minY y maxY.
        *
        *	@param name 	Nombre de la textura.
        *	@param x 		Posición en x de la textura.
        *	@param y 		Posición en y de la textura.
        *	@param args 	EthArguments de la textura.
        *	@param text 	Atributo Texture2D para manipular la textura.
        *
        *	@return La textura render con los parámetros ingresados.
        */
        public EthTexture AddRenderTexture(string name, float x, float y, EthArguments args, RenderTexture text)
        {

            //addListeners();
            args["name"] = name;
            args["x"] = "" + x;
            args["y"] = "" + y;

            EthTexture tempButton = new EthTexture(args, this);
            tempButton.SetRenderTexture(args, text);
            _allComponents.Add(tempButton);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempButton.Hei) * _hRatio)
            {
                _maxY = (y + tempButton.Hei) * _hRatio;
            }

            return tempButton;
        }

        /**
        *	@brief Método para agregar una línea.
        *
        *	Este método recibe el parámetro name, además de un string (par) para crear los EthArguments.
        *
        *	@param name 	Nombre de la línea.
        *	@param par 		Propiedades para crear los EthArguments de la línea.
        *
        *	@return La línea con los parámetros ingresados.
        */
        public EthLine AddLine(string name, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddLine(name, args);
        }

        /**
        *	@brief Método para agregar una línea.
        *
        *	Este método recibe el parámetro name y los EthArguments de la textura
        *
        *	@param name 	Nombre de la línea.
        *	@param args 	EthArguments de la textura.
        *
        *	@return La línea con los parámetros ingresados.
        */
        public EthLine AddLine(string name, EthArguments args)
        {
            args["name"] = name;

            EthLine tempComponent = new EthLine(args, this);
            _allComponents.Add(tempComponent);

            return tempComponent;
        }

        /**
        *	@brief Método para agregar una textura.
        *
        *	Este método recibe los parámetros name, x e y de la textura, además de un string (par) para crear los EthArguments.
        *
        *	@param name 	Nombre de la textura.
        *	@param x 		Posición en x de la textura.
        *	@param y 		Posición en y de la textura.
        *	@param par 		Propiedades para crear los EthArguments de la textura.
        *
        *	@return La textura con los parámetros ingresados.
        */
        public EthTexture AddTexture(string name, float x, float y, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddTexture(name, x, y, args);
        }

        /**
        *	@brief Método para agregar una textura.
        *
        *	Este método recibe los parámetros name, x, y y los EthArguments de la textura, también evalúa el minY y maxY.
        *
        *	@param name 	Nombre de la textura.
        *	@param x 		Posición en x de la textura.
        *	@param y 		Posición en y de la textura.
        *	@param args 	EthArguments de la textura.
        *
        *	@return La textura con los parámetros ingresados.
        */
        public EthTexture AddTexture(string name, float x, float y, EthArguments args)
        {
            args["name"] = name;
            args["x"] = "" + x;
            args["y"] = "" + y;

            EthTexture tempButton = new EthTexture(args, this);
            _allComponents.Add(tempButton);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempButton.Hei) * _hRatio)
            {
                _maxY = (y + tempButton.Hei) * _hRatio;
            }

            return tempButton;
        }

        /**
        *	@brief Método para agregar un video.
        *
        *	Este método recibe los parámetros name, x e y del video, además de un string (par) para crear los EthArguments.
        *
        *	@param name 	Nombre del video.
        *	@param x 		Posición en x del video.
        *	@param y 		Posición en y del video.
        *	@param par 		Propiedades para crear los EthArguments del video.
        *
        *	@return El video con los parámetros ingresados.
        */
        public EthVideo AddVideo(EthGUIScreen parent, string name, float x, float y, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddVideo(parent, name, x, y, args);
        }

        /**
        *	@brief Método para agregar un video.
        *
        *	Este método recibe los parámetros name, x, y y los EthArguments del video, también evalúa el minY y maxY.
        *
        *	@param name 	Nombre del video.
        *	@param x 		Posición en x del video.
        *	@param y 		Posición en y del video.
        *	@param args 	EthArguments del video.
        *
        *	@return El video con los parámetros ingresados.
        */
        public EthVideo AddVideo(EthGUIScreen parent, string name, float x, float y, EthArguments args)
        {

            //addListeners();
            args["name"] = name;
            args["x"] = "" + x;
            args["y"] = "" + y;

            EthVideo tempButton = new EthVideo(parent, args, this);
            _allComponents.Add(tempButton);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempButton.Hei) * _hRatio)
            {
                _maxY = (y + tempButton.Hei) * _hRatio;
            }

            return tempButton;
        }

        /**
        *	@brief Método para agregar un sprite.
        *
        *	Este método recibe los parámetros name, x e y del sprite, además de un string (par) para crear los EthArguments.
        *	Un sprite es un componente gráfico 2D utilizado para los personajes, proyectiles, y otros elementos.
        *
        *	@param name 	Nombre del sprite.
        *	@param x 		Posición en x del sprite.
        *	@param y 		Posición en y del sprite.
        *	@param par 		Propiedades para crear los EthArguments del sprite.
        *
        *	@return El sprite con los parámetros ingresados.
        */
        public EthSprite AddSprite(string name, float x, float y, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddSprite(name, x, y, args);
        }

        /**
        *	@brief Método para agregar un sprite.
        *
        *	Este método recibe los parámetros name, x, y y los EthArguments del sprite, también evalúa el minY y maxY.
        *
        *	@param name 	Nombre del sprite.
        *	@param x 		Posición en x del sprite.
        *	@param y 		Posición en y del sprite.
        *	@param args 	EthArguments del sprite.
        *
        *	@return El sprite con los parámetros ingresados.
        */
        public EthSprite AddSprite(string name, float x, float y, EthArguments args)
        {

            //addListeners();
            args["name"] = name;
            args["x"] = "" + x;
            args["y"] = "" + y;

            EthSprite tempComp = new EthSprite(args, this);
            _allComponents.Add(tempComp);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempComp.Hei) * _hRatio)
            {
                _maxY = (y + tempComp.Hei) * _hRatio;
            }

            return tempComp;
        }

        /**
        *	@brief Método para agregar una EthModalWindow.
        *
        *	Este método recibe los parámetros name, imgBack e imagen, de los cuales, sí imgBack no es ingresado se toma como false y sí
        *	imagen no es ingresado se toma como cadena vacía.
        *
        *	@param name 	Nombre de la EthModalWindow.
        *	@param imgBack 	Define si la EthModalWindow tendra imagen de fondo.
        *	@param imagen 	Ruta de la imagen de fondo.
        *
        *	@return La EthModalWindow con los parámetros ingresados.
        */
        public EthModalWindow AddModalWindow(string name, bool imgBack = false, string imagen = "", bool hasBackground = true)
        {
            EthModalWindow tempComponent = new EthModalWindow(this, imgBack, imagen, hasBackground);
            tempComponent.Name = name;
            _allComponents.Insert(0, tempComponent);

            return tempComponent;
        }

        /**
        *	@brief Método para agregar un cuadro de diálogo.
        *
        *	Este método recibe los parámetros name y text, además de un string (par) para crear los EthArguments.
        *
        *	@param name 	Nombre del cuadro de diálogo.
        *	@param text 	Texto que va a aparecer en el diálogo.
        *	@param par 		Propiedades para crear los EthArguments del cuadro de diálogo.
        *
        *	@return El cuadro de diálogo con los parámetros ingresados.
        */
        public EthDialog AddDialog(string name, string text, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddDialog(name, text, args);
        }

        /**
        *	@brief Método para agregar un cuadro de diálogo.
        *
        *	Este método recibe los parámetros name, text y los EthArguments del cuadro de diálogo.
        *
        *	@param name 	Nombre del cuadro de diálogo.
        *	@param text 	Texto que va a aparecer en el diálogo.
        *	@param args 	EthArguments del cuadro de diálogo.
        *
        *	@return El cuadro de diálogo con los parámetros ingresados.
        */
        public EthDialog AddDialog(string name, string text, EthArguments args)
        {
            args["name"] = name;
            args["text"] = text;

            EthDialog tempComponent = new EthDialog(args, this);
            _allComponents.Add(tempComponent);

            return tempComponent;
        }

        /**
        *	@brief Método para agregar un slider.
        *
        *	Este método recibe los parámetros name, x e y del slider, además de un string (par) para crear los EthArguments.
        *
        *	@param name 	Nombre del slider.
        *	@param x 		Posición en x del slider.
        *	@param y 		Posición en y del slider.
        *	@param par 		Propiedades para crear los EthArguments del slider.
        *
        *	@return El slider con los parámetros ingresados.
        */
        public EthSlider AddSlider(string name, float x, float y, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddSlider(name, x, y, args);
        }

        /**
        *	@brief Método para agregar un slider.
        *
        *	Este método recibe los parámetros name, x, y y los EthArguments del slider, también evalúa el minY y maxY.
        *
        *	@param name 	Nombre del slider.
        *	@param x 		Posición en x del slider.
        *	@param y 		Posición en y del slider.
        *	@param args 	EthArguments del slider.
        *
        *	@return El slider con los parámetros ingresados.
        */
        public EthSlider AddSlider(string name, float x, float y, EthArguments args)
        {
            args["name"] = name;
            args["x"] = "" + x;
            args["y"] = "" + y;

            EthSlider tempComponent = new EthSlider(args, this);
            _allComponents.Add(tempComponent);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempComponent.Hei) * _hRatio)
            {
                _maxY = (y + tempComponent.Hei) * _hRatio;
            }

            return tempComponent;
        }

        /**
        *	@brief Método para agregar una barra de progreso.
        *
        *	Este método recibe los parámetros name, x e y de la barra de progreso, además de un string (par) para crear los EthArguments.
        *
        *	@param name 	Nombre de la barra de progreso.
        *	@param x 		Posición en x de la barra de progreso.
        *	@param y 		Posición en y de la barra de progreso.
        *	@param par 		Propiedades para crear los EthArguments de la barra de progreso.
        *
        *	@return La barra de progreso con los parámetros ingresados.
        */
        public EthProgressBar AddProgressBar(string name, float x, float y, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddProgressBar(name, x, y, args);
        }

        /**
        *	@brief Método para agregar una barra de progreso.
        *
        *	Este método recibe los parámetros name, x, y y los EthArguments de la barra de progreso, también evalúa el minY y maxY.
        *
        *	@param name 	Nombre de la barra de progreso.
        *	@param x 		Posición en x de la barra de progreso.
        *	@param y 		Posición en y de la barra de progreso.
        *	@param args 	EthArguments de la barra de progreso.
        *
        *	@return La barra de progreso con los parámetros ingresados.
        */
        public EthProgressBar AddProgressBar(string name, float x, float y, EthArguments args)
        {
            args["name"] = name;
            args["x"] = "" + x;
            args["y"] = "" + y;

            EthProgressBar tempComponent = new EthProgressBar(args, this);
            _allComponents.Add(tempComponent);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempComponent.Hei) * _hRatio)
            {
                _maxY = (y + tempComponent.Hei) * _hRatio;
            }

            return tempComponent;
        }

        /**
        *	@brief Método para agregar un botón.
        *
        *	Este método recibe los parámetros name, x e y, más no recibe altura y anchor.
        *
        *	@param name 	Nombre del botón.
        *	@param x 		Posición en x del botón.
        *	@param y 		Posición en y del botón.
        *
        *	@return El botón con los parámetros ingresados.
        */
        public EthButton AddButton(string name, float x, float y)
        {
            return AddButton(name, x, y, "");
        }

        /**
        *	@brief Método para agregar un botón.
        *
        *	Este método recibe los parámetros name, x e y del botón, además de un string (par) para crear los EthArguments.
        *	La altura y el anchor (h y w) solo se pueden omitir si al botón se le agrega una textura.
        *
        *	@param name 	Nombre del botón.
        *	@param x 		Posición en x del botón.
        *	@param y 		Posición en y del botón.
        *	@param par 		Propiedades para crear los EthArguments del botón.
        *
        *	@return El botón con los parámetros ingresados.
        */
        public EthButton AddButton(string name, float x, float y, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddButton(name, x, y, args);
        }

        /**
        *	@brief Método para agregar un botón.
        *
        *	Este método recibe los parámetros name, x, y y los EthArguments del botón, también evalúa el minY y maxY.
        *
        *	@param name 	Nombre del botón.
        *	@param x 		Posición en x del botón.
        *	@param y 		Posición en y del botón.
        *	@param args 	EthArguments del botón.
        *
        *	@return El botón con los parámetros ingresados.
        */
        public EthButton AddButton(string name, float x, float y, EthArguments args)
        {

            //addListeners();
            args["name"] = name;
            args["x"] = "" + x;
            args["y"] = "" + y;

            EthButton tempButton = new EthButton(args, this);
            _allComponents.Add(tempButton);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempButton.Hei) * _hRatio)
            {
                _maxY = (y + tempButton.Hei) * _hRatio;
            }

            return tempButton;
        }

        /**
        *	@brief Método para agregar un combo box.
        *
        *	Este método recibe los parámetros name, x e y del combo box, además de un string (par) para crear los EthArguments.
        *
        *	@param name 	Nombre del combo box.
        *	@param x 		Posición en x del combo box.
        *	@param y 		Posición en y del combo box.
        *	@param par 		Propiedades para crear los EthArguments del combo box.
        *
        *	@return El combo box con los parámetros ingresados.
        */
        public EthComboBox AddComboBox(string name, float x, float y, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddComboBox(name, x, y, args);
        }

        /**
        *	@brief Método para agregar un combo box.
        *
        *	Este método recibe los parámetros name, x, y y los EthArguments del combo box, también evalúa el minY y maxY.
        *
        *	@param name 	Nombre del combo box.
        *	@param x 		Posición en x del combo box.
        *	@param y 		Posición en y del combo box.
        *	@param args 	EthArguments del combo box.
        *
        *	@return El combo box con los parámetros ingresados.
        */
        public EthComboBox AddComboBox(string name, float x, float y, EthArguments args)
        {
            //addListeners();
            args["name"] = name;
            args["x"] = "" + x;
            args["y"] = "" + y;

            EthComboBox tempButton = new EthComboBox(args, this);
            _allComponents.Add(tempButton);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempButton.Hei) * _hRatio)
            {
                _maxY = (y + tempButton.Hei) * _hRatio;
            }

            return tempButton;
        }

        /**
        *	@brief Método para agregar un label.
        *
        *	Este método recibe los parámetros text, x e y del label, además de un string (par) para crear los EthArguments.
        *
        *	@param text 	Texto del label.
        *	@param x 		Posición en x del label.
        *	@param y 		Posición en y del label.
        *	@param par 		Propiedades para crear los EthArguments del label.
        *
        *	@return El label con los parámetros ingresados.
        */
        public EthLabel AddLabel(string text, float x, float y, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddLabel(text, x, y, args);
        }

        /**
        *	@brief Método para agregar un label.
        *
        *	Este método recibe los parámetros name, x, y y los EthArguments del label, también evalúa el minY y maxY.
        *
        *	@param text 	Texto del label.
        *	@param x 		Posición en x del label.
        *	@param y 		Posición en y del label.
        *	@param args 	EthArguments del label.
        *
        *	@return El label con los parámetros ingresados.
        */
        public EthLabel AddLabel(string text, float x, float y, EthArguments args)
        {

            //addListeners();
            args["text"] = text;
            args["x"] = "" + x;
            args["y"] = "" + y;

            EthLabel tempLabel = new EthLabel(args, this);
            _allComponents.Add(tempLabel);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempLabel.Hei) * _hRatio)
            {
                _maxY = (y + tempLabel.Hei) * _hRatio;
            }

            return tempLabel;
        }

        /**
        *	@brief Método para agregar un campo de texto.
        *
        *	Este método recibe los parámetros text, x e y del campo de texto, además de un string (par) para crear los EthArguments.
        *
        *	@param text 	Texto del campo de texto.
        *	@param name 	Nombre del campo de texto.
        *	@param x 		Posición en x del campo de texto.
        *	@param y 		Posición en y del campo de texto.
        *	@param par 		Propiedades para crear los EthArguments del campo de texto.
        *
        *	@return El campo de texto con los parámetros ingresados.
        */
        public EthTextField AddTextField(string text, string name, float x, float y, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddTextField(text, name, x, y, args);
        }

        /**
        *	@brief Método para agregar un campo de texto.
        *
        *	Este método recibe los parámetros name, x, y y los EthArguments del campo de texto, también evalúa el minY y maxY.
        *
        *	@param text 	Texto del campo de texto.
        *	@param name 	Nombre del campo de texto.
        *	@param x 		Posición en x del campo de texto.
        *	@param y 		Posición en y del campo de texto.
        *	@param args 	EthArguments del campo de texto.
        *
        *	@return El campo de texto con los parámetros ingresados.
        */
        public EthTextField AddTextField(string text, string name, float x, float y, EthArguments args)
        {

            //addListeners();
            args["text"] = text;
            args["name"] = name;
            args["x"] = "" + x;
            args["y"] = "" + y;

            EthTextField tempText = new EthTextField(args, this);
            _allComponents.Add(tempText);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempText.Hei) * _hRatio)
            {
                _maxY = (y + tempText.Hei) * _hRatio;
            }

            return tempText;
        }

        /**
        *	@brief Método para agregar un toggleButton.
        *
        *	Este método recibe los parámetros name, x e y, más no recibe altura y anchor.
        *
        *	@param name 	Nombre del toggleButton.
        *	@param x 		Posición en x del toggleButton.
        *	@param y 		Posición en y del toggleButton.
        *
        *	@return El toggleButton con los parámetros ingresados.
        */
        public EthToggleButton AddToggleButton(string name, float x, float y)
        {
            return AddToggleButton(name, x, y, "");
        }

        /**
        *	@brief Método para agregar un toggleButton.
        *
        *	Este método recibe los parámetros name, x e y del toggleButton, además de un string (par) para crear los EthArguments.
        *	La altura y el anchor (h y w) solo se pueden omitir si al toggleButton se le agrega una textura.
        *
        *	@param name 	Nombre del toggleButton.
        *	@param x 		Posición en x del toggleButton.
        *	@param y 		Posición en y del toggleButton.
        *	@param par 		Propiedades para crear los EthArguments del toggleButton.
        *
        *	@return El toggleButton con los parámetros ingresados.
        */
        public EthToggleButton AddToggleButton(string name, float x, float y, string par)
        {
            EthArguments args = new EthArguments(par);
            return AddToggleButton(name, x, y, args);
        }

        /**
        *	@brief Método para agregar un toggleButton.
        *
        *	Este método recibe los parámetros name, x, y y los EthArguments del toggleButton, también evalúa el minY y maxY.
        *
        *	@param name 	Nombre del toggleButton.
        *	@param x 		Posición en x del toggleButton.
        *	@param y 		Posición en y del toggleButton.
        *	@param args 	EthArguments del toggleButton.
        *
        *	@return El toggleButton con los parámetros ingresados.
        */
        public EthToggleButton AddToggleButton(string name, float x, float y, EthArguments args)
        {

            args["name"] = name;
            args["x"] = "" + x;
            args["y"] = "" + y;

            EthToggleButton tempButton = new EthToggleButton(args, this);
            _allComponents.Add(tempButton);

            if (_minY > y)
            {
                _minY = y;
            }
            if (_maxY < (y + tempButton.Hei) * _hRatio)
            {
                _maxY = (y + tempButton.Hei) * _hRatio;
            }

            return tempButton;
        }

        /**
        *	@brief Método para borrar un componente de la lista de componentes desplegados.
        *
        *	@param comp Componente a ser borrado.
        */
        public void RemoveComponent(EthComponent comp)
        {
            comp.Remove(false);
            _allComponents.Remove(comp);
        }

        /**
        *	@brief Método para borrar todos los componentes de la lista de componentes desplegados.
        */
        public void RemoveAllComponents()
        {
            _minY = 0;
            _maxY = 0;
            _allComponents = new ArrayList();
        }

        /**
        *	@brief Método para mover un componente al final de la lista de componentes desplegados.
        *
        *	@param comp Componente a ser movido.
        */
        public void ToBack(EthComponent comp)
        {
            _allComponents.Remove(comp);
            _allComponents.Insert(0, comp);
        }

        /**
        *	@brief Método para mover un componente al inicio de la lista de componentes desplegados.
        *
        *	@param comp Componente a ser movido.
        */
        public void ToFront(EthComponent comp)
        {
            _allComponents.Remove(comp);
            _allComponents.Add(comp);
        }

        /**
        *	@brief Método para desplazar la vista hasta un objeto en algun punto del escenario.
        *
        *	@param go GameObject para obtener las coordenadas del objeto y convertirlas a un Vector2.
        *
        *	@return Vector2 con las coordenadas.
        */
        public Vector2 WorldToGUI(GameObject go)
        {
            Vector3 screenPos3 = Camera.main.WorldToViewportPoint(go.transform.position);
            return new Vector2(-_wOffset + (screenPos3.x * (_objectWidth + (_wOffset * 2))), _objectHeight - (screenPos3.y * _objectHeight));
        }

        /**
        *	@brief Método para dibujar un EthComponentManager.
        *
        *	@param offset Parámetro de tipo Vector2 para ser dibujado, el cual tambien puede ser nulo.
        *
        *	@see com.ethereal.display.components.EthComponent
        */
        public override void Draw(Vector2? offset = null)
        {

            if (!Visible)
            {
                return;
            }

            if (_isAnimating)
            {
                float[] dataStep = _animat.GetStep();
                X = dataStep[0];
                Y = dataStep[1];
                Wid=dataStep[2];
                Hei=dataStep[3];

                if (_animat.isEnded)
                {
                    _isAnimating = false;
                }
            }

            if (_shouldUseGroup)
            {
                Rect groupRect = new Rect((X * _wRatio) + _guiParent._wOffset, (Y * _hRatio) + _guiParent._hOffset, Wid * _wRatio, Hei * _hRatio);
                GUI.BeginGroup(groupRect);
            }

            try
            {
                foreach (object arr in _allComponents)
                {
                    ((EthComponent)arr).Draw(offset);
                }
            }
            catch (InvalidOperationException)
            {
            }

            if (_shouldUseGroup)
            {
                GUI.EndGroup();
            }
        }

        /**
        *	@brief Método para buscar un componente por su nombre en la lista de componentes desplegados.
        *
        *	@param name nombre del componente a buscar.
        *
        *	@return Componente en la lista de componentes desplegados o null.
        */
        public EthComponent GetComponentByName(string name)
        {
            foreach (object arr in _allComponents)
            {
                if (((EthComponent)arr).Name == name)
                {
                    return (EthComponent)arr;
                }
            }

            return null;
        }

        /**
        *	@brief Método para agregar una animación.
        *
        *	@param anim animación a agregar.
        *
        */
        public void AddAnimation(AnimationEase anim)
        {
            _isAnimating = true;
            this._animat = anim;
        }
    }
}
