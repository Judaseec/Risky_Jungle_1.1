using UnityEngine;
 using System.Collections;
 
 /** 
*   @author    EtherealGF <www.etherealgf.com> 
*   @version   1.0 
*   @date      Octubre 16 2015
* 
*   @class  TextMeshOutline
*   @brief  Esta clase representa una interfaz para el componente text mesh.
*/
 public class TextMeshOutline : MonoBehaviour {
 
    /**
    *  @brief Tamaño del pixel.
    */
     public float pixelSize = 1;

    /**
    *  @brief Color del texto.
    */
     public Color outlineColor = Color.black;

    /**
    *  @brief Variable que define si la resolución es dependiente.
    */
     public bool resolutionDependant = false;

    /**
    *  @brief Resolución del text mesh.
    */
     public int doubleResolution = 1024;
 
    /**
    *  @brief Text mesh.
    */
     private TextMesh _textMesh;

    /**
    *  @brief Renderizador del mesh.
    */
     private MeshRenderer _meshRenderer;
 
    /**
    *  @brief Método que inicializa las variables.
    */
     void Start() {
         _textMesh = GetComponent<TextMesh>();    
         _meshRenderer = GetComponent<MeshRenderer>();
 
         for (int i = 0; i < 8; i++) {
             GameObject outline = new GameObject("outline", typeof(TextMesh));
             outline.transform.parent = transform;
             outline.transform.localScale = new Vector3(1, 1, 1);
 
             MeshRenderer otherMeshRenderer = outline.GetComponent<MeshRenderer>();
             otherMeshRenderer.material = new Material(_meshRenderer.material);
             otherMeshRenderer.castShadows = false;
             otherMeshRenderer.receiveShadows = false;
             otherMeshRenderer.sortingLayerID = _meshRenderer.sortingLayerID;
             otherMeshRenderer.sortingLayerName = _meshRenderer.sortingLayerName;
         }
     }
     
    /**
    *  @brief Método para actualizar.
    */
     void LateUpdate() {
         Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
 
         outlineColor.a = _textMesh.color.a * _textMesh.color.a;
 
         // copy attributes
         for (int i = 0; i < transform.childCount; i++) {
 
             TextMesh other = transform.GetChild(i).GetComponent<TextMesh>();
             other.color = outlineColor;
             other.text = _textMesh.text;
             other.alignment = _textMesh.alignment;
             other.anchor = _textMesh.anchor;
             other.characterSize = _textMesh.characterSize;
             other.font = _textMesh.font;
             other.fontSize = _textMesh.fontSize;
             other.fontStyle = _textMesh.fontStyle;
             other.richText = _textMesh.richText;
             other.tabSize = _textMesh.tabSize;
             other.lineSpacing = _textMesh.lineSpacing;
             other.offsetZ = _textMesh.offsetZ;
 
             bool doublePixel = resolutionDependant && (Screen.width > doubleResolution || Screen.height > doubleResolution);
             Vector3 pixelOffset = GetOffset(i) * (doublePixel ? 2.0f * pixelSize : pixelSize);
             Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint + pixelOffset);
             other.transform.position = worldPoint;
 
             MeshRenderer otherMeshRenderer = transform.GetChild(i).GetComponent<MeshRenderer>();
             otherMeshRenderer.sortingLayerID = _meshRenderer.sortingLayerID;
             otherMeshRenderer.sortingLayerName = _meshRenderer.sortingLayerName;
         }
     }
 
    /**
    *  @brief Método que dado un número como parametro retorna un vector offset.
    *
    *   @param int i Número parametro.
    *
    *   @return Vector3 Vector offset.
    */
     Vector3 GetOffset(int i) {
         switch (i % 8) {
         case 0: return new Vector3(0, 1, 0);
         case 1: return new Vector3(1, 1, 0);
         case 2: return new Vector3(1, 0, 0);
         case 3: return new Vector3(1, -1, 0);
         case 4: return new Vector3(0, -1, 0);
         case 5: return new Vector3(-1, -1, 0);
         case 6: return new Vector3(-1, 0, 0);
         case 7: return new Vector3(-1, 1, 0);
         default: return Vector3.zero;
         }
     }
 }