using UnityEngine;

namespace Assets.Scripts.com.ethereal.display.easing {

		/** 
    	*	@author    EtherealGF <www.etherealgf.com> 
    	* 	@version   1.0 
   		* 	@date      Octubre 16 2014
    	* 
    	*	@class 	functions
    	*   @brief 	Esta clase representa el tipo de animación que un objeto animado realiza.
    	*
    	*/
		public class functions {
		
				/**
       			*	@brief Método para aplicar una función de animación.
       			*
       			*	@param typeFun 	Tipo de animación
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del tipo de animación y el porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float applyFunction (string typeFun, float start, float end, float value) {
						switch (typeFun)
						{
								case "linear":
										return linear (start, end, value);
								case "clerp":
										return clerp (start, end, value);
								case "spring":
										return spring (start, end, value);
								case "easeInQuad":
										return easeInQuad (start, end, value);
								case "easeOutQuad":
										return easeOutQuad (start, end, value);
								case "easeInOutQuad":
										return easeInOutQuad (start, end, value);
								case "easeInCubic":
										return easeInCubic (start, end, value);
								case "easeOutCubic":
										return easeOutCubic (start, end, value);
								case "easeInOutCubic":
										return easeInOutCubic (start, end, value);
								case "easeInQuart":
										return easeInQuart (start, end, value);
								case "easeOutQuart":
										return easeOutQuart (start, end, value);
								case "easeInOutQuart":
										return easeInOutQuart (start, end, value);
								case "easeInQuint":
										return easeInQuint (start, end, value);
								case "easeOutQuint":
										return easeOutQuint (start, end, value);
								case "easeInOutQuint":
										return easeInOutQuint (start, end, value);
								case "easeInSine":
										return easeInSine (start, end, value);
								case "easeOutSine":
										return easeOutSine (start, end, value);
								case "easeInOutSine":
										return easeInOutSine (start, end, value);
								case "easeInExpo":
										return easeInExpo (start, end, value);
								case "easeOutExpo":
										return easeOutExpo (start, end, value);
								case "easeInOutExpo":
										return easeInOutExpo (start, end, value);
								case "easeInCirc":
										return easeInCirc (start, end, value);
								case "easeOutCirc":
										return easeOutCirc (start, end, value);
								case "easeInOutCirc":
										return easeInOutCirc (start, end, value);
								case "easeInBounce":
										return easeInBounce (start, end, value);
								case "easeOutBounce":
										return easeOutBounce (start, end, value);
								case "easeInOutBounce":
										return easeInOutBounce (start, end, value);
								case "easeInBack":
										return easeInBack (start, end, value);
								case "easeOutBack":
										return easeOutBack (start, end, value);
								case "easeInOutBack":
										return easeInOutBack (start, end, value);
						//case "punch": return punch(start,end,value);
								case "easeInElastic":
										return easeInElastic (start, end, value);
						//case "elastic": return elastic(start,end,value);
								case "easeOutElastic":
										return easeOutElastic (start, end, value);
								case "easeInOutElastic	":
										return easeInOutElastic (start, end, value);
						}

						return linear (start, end, value);
				}

				/**
       			*	@brief Método para obtener un punto de una animación constante, sin aceleración
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float linear (float start, float end, float value) {
						return Mathf.Lerp (start, end, value);
				}
		
				/**
       			*	@brief Se comporta como el linear, pero esté se encarga de la envolvente de 0 a 360.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float clerp (float start, float end, float value) {
						float min = 0.0f;
						float max = 360.0f;
						float half = Mathf.Abs ((max - min) / 2.0f);
						float retval = 0.0f;
						float diff = 0.0f;
						if ((end - start) < -half) {
								diff = ((max - start) + end) * value;
								retval = start + diff;
						} else if ((end - start) > half) {
								diff = -((max - end) + start) * value;
								retval = start + diff;
						} else
								retval = start + (end - start) * value;
						return retval;
				}

				/**
       			*	@brief Al final da una sensación de rebote.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float spring (float start, float end, float value) {
						value = Mathf.Clamp01 (value);
						value = (Mathf.Sin (value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow (1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
						return start + (end - start) * value;
				}

				/**
       			*	@brief Da efecto de aceleración desde cero, se hace al cuadrado.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInQuad (float start, float end, float value) {
						end -= start;
						return end * value * value + start;
				}

				/**
       			*	@brief Da efecto de desaceleración hacia cero, se hace al cuadrado.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeOutQuad (float start, float end, float value) {
						end -= start;
						return -end * value * (value - 2) + start;
				}

				/**
       			*	@brief Da efecto de aceleración hasta la mitad, luego desaceleración, se hace al cuadrado.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInOutQuad (float start, float end, float value) {
						value /= .5f;
						end -= start;
						if (value < 1)
								return end / 2 * value * value + start;
						value--;
						return -end / 2 * (value * (value - 2) - 1) + start;
				}

				/**
       			*	@brief Da efecto de aceleración desde cero, se hace al cubo.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInCubic (float start, float end, float value) {
						end -= start;
						return end * value * value * value + start;
				}

				/**
       			*	@brief Da efecto de desaceleración hacia cero, se hace al cubo.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeOutCubic (float start, float end, float value) {
						value--;
						end -= start;
						return end * (value * value * value + 1) + start;
				}

				/**
       			*	@brief Da efecto de aceleración hasta la mitad, luego desaceleración, se hace al cubo.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInOutCubic (float start, float end, float value) {
						value /= .5f;
						end -= start;
						if (value < 1)
								return end / 2 * value * value * value + start;
						value -= 2;
						return end / 2 * (value * value * value + 2) + start;
				}

				/**
       			*	@brief Da efecto de aceleración desde cero, se hace a la cuatro.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInQuart (float start, float end, float value) {
						end -= start;
						return end * value * value * value * value + start;
				}

				/**
       			*	@brief Da efecto de desaceleración hacia cero, se hace a la cuatro.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeOutQuart (float start, float end, float value) {
						value--;
						end -= start;
						return -end * (value * value * value * value - 1) + start;
				}

				/**
       			*	@brief Da efecto de aceleración hasta la mitad, luego desaceleración, se hace a la cuatro.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInOutQuart (float start, float end, float value) {
						value /= .5f;
						end -= start;
						if (value < 1)
								return end / 2 * value * value * value * value + start;
						value -= 2;
						return -end / 2 * (value * value * value * value - 2) + start;
				}

				/**
       			*	@brief Da efecto de aceleración desde cero, se hace a la cinco.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInQuint (float start, float end, float value) {
						end -= start;
						return end * value * value * value * value * value + start;
				}

				/**
       			*	@brief Da efecto de desaceleración hacia cero, se hace a la cinco.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeOutQuint (float start, float end, float value) {
						value--;
						end -= start;
						return end * (value * value * value * value * value + 1) + start;
				}

				/**
       			*	@brief Da efecto de aceleración hasta la mitad, luego desaceleración, se hace a la cinco.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInOutQuint (float start, float end, float value) {
						value /= .5f;
						end -= start;
						if (value < 1)
								return end / 2 * value * value * value * value * value + start;
						value -= 2;
						return end / 2 * (value * value * value * value * value + 2) + start;
				}

				/**
       			*	@brief Da efecto de aceleracion desde cero, se hace de manera sinusoidal.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInSine (float start, float end, float value) {
						end -= start;
						return -end * Mathf.Cos (value / 1 * (Mathf.PI / 2)) + end + start;
				}

				/**
       			*	@brief Da efecto de desaceleracion hacia cero, se hace de manera sinusoidal.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeOutSine (float start, float end, float value) {
						end -= start;
						return end * Mathf.Sin (value / 1 * (Mathf.PI / 2)) + start;
				}

				/**
       			*	@brief Da efecto de aceleración hasta la mitad, luego desaceleración, se hace de manera sinusoidal.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInOutSine (float start, float end, float value) {
						end -= start;
						return -end / 2 * (Mathf.Cos (Mathf.PI * value / 1) - 1) + start;
				}

				/**
       			*	@brief Da efecto de aceleracion desde cero, se hace de manera exponencial.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInExpo (float start, float end, float value) {
						end -= start;
						return end * Mathf.Pow (2, 10 * (value / 1 - 1)) + start;
				}

				/**
       			*	@brief Da efecto de desaceleracion hacia cero, se hace de manera exponencial.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeOutExpo (float start, float end, float value) {
						end -= start;
						return end * (-Mathf.Pow (2, -10 * value / 1) + 1) + start;
				}

				/**
       			*	@brief Da efecto de aceleración hasta la mitad, luego desaceleración, se hace de manera exponencial.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInOutExpo (float start, float end, float value) {
						value /= .5f;
						end -= start;
						if (value < 1)
								return end / 2 * Mathf.Pow (2, 10 * (value - 1)) + start;
						value--;
						return end / 2 * (-Mathf.Pow (2, -10 * value) + 2) + start;
				}

				/**
       			*	@brief Da efecto de aceleracion desde cero, se hace en forma del cuarto inferior derecho de una circunferencia.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInCirc (float start, float end, float value) {
						end -= start;
						return -end * (Mathf.Sqrt (1 - value * value) - 1) + start;
				}

				/**
       			*	@brief Da efecto de desaceleracion hacia cero, se hace en forma del cuarto superior izquierdo de una circunferencia.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeOutCirc (float start, float end, float value) {
						value--;
						end -= start;
						return end * Mathf.Sqrt (1 - value * value) + start;
				}

				/**
       			*	@brief Da efecto de aceleración hasta la mitad, luego desaceleración, se hace en forma de easeInCirc y luego de easeOutCirc.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInOutCirc (float start, float end, float value) {
						value /= .5f;
						end -= start;
						if (value < 1)
								return -end / 2 * (Mathf.Sqrt (1 - value * value) - 1) + start;
						value -= 2;
						return end / 2 * (Mathf.Sqrt (1 - value * value) + 1) + start;
				}

				/**
       			*	@brief Da efecto de un leve decaimiento exponencial con un rebote parabólico.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				/* GFX47 MOD START */
				public static float easeInBounce (float start, float end, float value) {
						end -= start;
						float d = 1f;
						return end - easeOutBounce (0, end, d - value) + start;
				}
				/* GFX47 MOD END */

				/**
       			*	@brief Es el inverso del easeInBounce, inicia con el crecimiento parabólico y finaliza con un pequeño descenso exponencial.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				/* GFX47 MOD START */
				//public static float bounce(float start, float end, float value){
				public static float easeOutBounce (float start, float end, float value) {
						value /= 1f;
						end -= start;
						if (value < (1 / 2.75f)) {
								return end * (7.5625f * value * value) + start;
						} else if (value < (2 / 2.75f)) {
								value -= (1.5f / 2.75f);
								return end * (7.5625f * (value) * value + .75f) + start;
						} else if (value < (2.5 / 2.75)) {
								value -= (2.25f / 2.75f);
								return end * (7.5625f * (value) * value + .9375f) + start;
						} else {
								value -= (2.625f / 2.75f);
								return end * (7.5625f * (value) * value + .984375f) + start;
						}
				}
				/* GFX47 MOD END */

				/**
       			*	@brief Es una combinación de easeInBounce y easeOutBounce, inicia con un pequeño decaimiento exponencial, luego crece parabólicamente
       			*	y culmina con un pequeño decaimiento exponencial.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				/* GFX47 MOD START */
				public static float easeInOutBounce (float start, float end, float value) {
						end -= start;
						float d = 1f;
						if (value < d / 2)
								return easeInBounce (0, end, value * 2) * 0.5f + start;
						else
								return easeOutBounce (0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
				}
				/* GFX47 MOD END */

				/**
       			*	@brief Inicia con un decaimiento y luego sube, todo esto de emanera parabólica.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInBack (float start, float end, float value) {
						end -= start;
						value /= 1;
						float s = 1.70158f;
						return end * (value) * value * ((s + 1) * value - s) + start;
				}

				/**
       			*	@brief Inicia con un alto crecimiento y finaliza con un pequeño decaimiento, todo esto de emanera parabólica.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeOutBack (float start, float end, float value) {
						float s = 1.70158f;
						end -= start;
						value = (value / 1) - 1;
						return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
				}

				/**
       			*	@brief Inicia con un decaimiento leve y luego crece en forma de parabóla positiva, en la mitad cambia a parabola negativa y culmina
       			*	con un leve descenso siguiendo la forma de parabóla negativa.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				public static float easeInOutBack (float start, float end, float value) {
						float s = 1.70158f;
						end -= start;
						value /= .5f;
						if ((value) < 1) {
								s *= (1.525f);
								return end / 2 * (value * value * (((s) + 1) * value - s)) + start;
						}
						value -= 2;
						s *= (1.525f);
						return end / 2 * ((value) * value * (((s) + 1) * value + s) + 2) + start;
				}

				/*public static float punch(float amplitude, float value){
			float s = 9;
			if (value == 0){
				return 0;
			}
			if (value == 1){
				return 0;
			}
			float period = 1 * 0.3f;
			s = period / (2 * Mathf.PI) * Mathf.Asin(0);
			return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
	    }*/
		
				/**
       			*	@brief Inicia con un efecto de rebote mínimo, y cada rebote aumenta el doble del anterior hasta llegar al final (end).
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				/* GFX47 MOD START */
				public static float easeInElastic (float start, float end, float value) {
						end -= start;
			
						float d = 1f;
						float p = d * .3f;
						float s = 0;
						float a = 0;
			
						if (value == 0)
								return start;
			
						if ((value /= d) == 1)
								return start + end;
			
						if (a == 0f || a < Mathf.Abs (end)) {
								a = end;
								s = p / 4;
						} else {
								s = p / (2 * Mathf.PI) * Mathf.Asin (end / a);
						}
			
						return -(a * Mathf.Pow (2, 10 * (value -= 1)) * Mathf.Sin ((value * d - s) * (2 * Mathf.PI) / p)) + start;
				}		
				/* GFX47 MOD END */

				/**
       			*	@brief Es totalmente inverso al easeInElastic, inicia en el punto máximo y finaliza en el mínimo.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				/* GFX47 MOD START */
				//public static float elastic(float start, float end, float value){
				public static float easeOutElastic (float start, float end, float value) {
						/* GFX47 MOD END */
						//Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
						end -= start;
			
						float d = 1f;
						float p = d * .3f;
						float s = 0;
						float a = 0;
			
						if (value == 0)
								return start;
			
						if ((value /= d) == 1)
								return start + end;
			
						if (a == 0f || a < Mathf.Abs (end)) {
								a = end;
								s = p / 4;
						} else {
								s = p / (2 * Mathf.PI) * Mathf.Asin (end / a);
						}
			
						return (a * Mathf.Pow (2, -10 * value) * Mathf.Sin ((value * d - s) * (2 * Mathf.PI) / p) + end + start);
				}		
		
				/**
       			*	@brief Es una combinacion de easeInElastic y de easeOutElastic, lo cual indica que hasta la mitad el rebote crece, y despues el rebote decrece.
       			*
       			*	@param start 	Inicio de la animación.
       			*	@param end 		Final de la animación.
       			*	@param value 	Porcentaje de la duración transcurrido (0 - 1).
       			*
       			*	@return Pocisión dependiendo del porcentaje transcurrido, punto inicial y punto final.
        		*/
				/* GFX47 MOD START */
				public static float easeInOutElastic (float start, float end, float value) {
						end -= start;
			
						float d = 1f;
						float p = d * .3f;
						float s = 0;
						float a = 0;
			
						if (value == 0)
								return start;
			
						if ((value /= d / 2) == 2)
								return start + end;
			
						if (a == 0f || a < Mathf.Abs (end)) {
								a = end;
								s = p / 4;
						} else {
								s = p / (2 * Mathf.PI) * Mathf.Asin (end / a);
						}
			
						if (value < 1)
								return -0.5f * (a * Mathf.Pow (2, 10 * (value -= 1)) * Mathf.Sin ((value * d - s) * (2 * Mathf.PI) / p)) + start;
						return a * Mathf.Pow (2, -10 * (value -= 1)) * Mathf.Sin ((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
				}				
		}
}
