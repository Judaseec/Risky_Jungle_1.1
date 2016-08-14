using UnityEngine;
using UnityEditor;

/** 
*   @author    EtherealGF <www.etherealgf.com> 
*   @version   1.0 
*   @date      Octubre 14 2015
* 
*   @class  UIToggle
*   @brief  Esta interfaz establece el método OnInspectorGUI para todos los elementos que sean de tipo toggle.
*/
[CustomEditor(typeof(EthToggle))]
public class UIToggle : Editor
{
  /**
  *   @brief Método para definir las acciones del evento OnInspectorGUI.
  */
  public override void OnInspectorGUI()
  {
       base.OnInspectorGUI();

       EthToggle t = (EthToggle)target;
  }
}
