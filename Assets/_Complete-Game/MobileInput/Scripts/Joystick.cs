using UnityEngine;
using UnityEngine.EventSystems;
using UnitySampleAssets.CrossPlatformInput;

public class Joystick : MonoBehaviour , IPointerUpHandler , IPointerDownHandler , IDragHandler {

    public int MovementRange = 100;

    public enum AxisOption
    {                                                    
        Both,                                                                   
        OnlyHorizontal,                                                         
        OnlyVertical                                                            
    }

    public AxisOption axesToUse = AxisOption.Both;   
    public string horizontalAxisName = "Horizontal";
    public string verticalAxisName = "Vertical";    

    private Vector3 startPos;
    private bool useX;                                                          
    private bool useY;                                                          
    private CrossPlatformInputManager.VirtualAxis horizontalVirtualAxis;               
    private CrossPlatformInputManager.VirtualAxis verticalVirtualAxis;                 
      
    void OnEnable () {

        startPos = transform.position;
        CreateVirtualAxes ();
    }

    private void UpdateVirtualAxes (Vector3 value) {

        var delta = startPos - value;
        delta.y = -delta.y;
        delta /= MovementRange;
        if(useX)
        horizontalVirtualAxis.Update (-delta.x);

        if(useY)
        verticalVirtualAxis.Update (delta.y);

    }

    private void CreateVirtualAxes()
    {
        
        useX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
        useY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

        
        if (useX)
            horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
        if (useY)
            verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
    }


    public  void OnDrag(PointerEventData data) {

        Vector3 newPos = Vector3.zero;

        if (useX) {
            int delta = (int) (data.position.x - startPos.x);
            delta = Mathf.Clamp(delta,  - MovementRange,  MovementRange);
            newPos.x = delta;
        }

        if (useY)
        {
            int delta = (int)(data.position.y - startPos.y);
            delta = Mathf.Clamp(delta, -MovementRange,  MovementRange);
            newPos.y = delta;
        }
        transform.position = new Vector3(startPos.x + newPos.x , startPos.y + newPos.y , startPos.z + newPos.z);
        UpdateVirtualAxes (transform.position);
    }


    public  void OnPointerUp(PointerEventData data)
    {
        transform.position = startPos;
        UpdateVirtualAxes (startPos);
    }


    public  void OnPointerDown (PointerEventData data) {
    }

    void OnDisable () {
        
        if (useX)
        {
            horizontalVirtualAxis.Remove();
        }
        if (useY)
        {
            verticalVirtualAxis.Remove();
        }
    }
}
