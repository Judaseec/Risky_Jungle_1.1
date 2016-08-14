using UnityEngine;
using UnityEditor;

/** 
*   @author    EtherealGF <www.etherealgf.com> 
*   @version   1.0 
*   @date      Octubre 14 2015
* 
*   @class  UIImage
*   @brief  Esta interfaz establece el método OnInspectorGUI para todos los elementos que sean de tipo imagen.
*/
[CustomEditor(typeof(EthImage))]
public class UIImage : Editor
{
  /**
  *   @brief Método para definir las acciones del evento OnInspectorGUI.
  */
  public override void OnInspectorGUI()
  {
       base.OnInspectorGUI();

       EthImage t = (EthImage)target;
  }
}
