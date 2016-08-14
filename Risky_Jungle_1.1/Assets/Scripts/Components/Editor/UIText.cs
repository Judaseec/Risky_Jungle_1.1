using UnityEngine;
using UnityEditor;

/** 
*   @author    EtherealGF <www.etherealgf.com> 
*   @version   1.0 
*   @date      Octubre 14 2015
* 
*   @class  UIText
*   @brief  Esta interfaz establece el método OnInspectorGUI para todos los elementos que sean de tipo texto.
*/
[CustomEditor(typeof(EthText))]
public class UIText : Editor
{
  /**
  *   @brief Método para definir las acciones del evento OnInspectorGUI.
  */
  public override void OnInspectorGUI()
  {
       base.OnInspectorGUI();

       EthText t = (EthText)target;
  }
}
