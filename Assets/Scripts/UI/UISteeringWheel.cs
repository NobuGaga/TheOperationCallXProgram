using UnityEngine;
using UnityEngine.EventSystems;

public class UISteeringWheel:UIPrefab, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private GameObject m_steeringWheel;
    private RectTransform m_steeringWheelRT;
    private float m_steeringWheelHalfWidth;
    private float m_steeringWheelHalfHeight;
    private GameObject m_steeringWheelPoint;
    private RectTransform m_steeringWheelPointRT;
    private float m_maxWheelPosX;
    private float m_maxWheelPosY;
    private float m_minPointPosX;
    private float m_minPointPosY;
    private float m_maxPointPosX;
    private float m_maxPointPosY;
    private Vector2 m_dragDelta = Vector2.zero;
    public Vector2 DragDelta => m_dragDelta;

    protected override void Awake() {
        base.Awake();
        m_steeringWheel = GetNode("steeringWheel");
        m_steeringWheelPoint = GetNode("steeringWheelPoint");
        m_steeringWheel.SetActive(false);
        m_steeringWheelPoint.SetActive(false);
        m_steeringWheelRT = m_steeringWheel.GetComponent<RectTransform>();
        m_steeringWheelPointRT = m_steeringWheelPoint.GetComponent<RectTransform>();
        Rect rect = GetComponent<RectTransform>().rect;
        m_maxWheelPosX = rect.width;
        m_maxWheelPosY = rect.height;
        m_steeringWheelHalfWidth = m_steeringWheelRT.rect.width / 2;
        m_steeringWheelHalfHeight = m_steeringWheelRT.rect.height / 2;
    }

    private void SetPointPosition(Vector2 position, Vector2 originPos) {
        float x = position.x; 
        if (x > m_maxPointPosX)
            x = m_maxPointPosX;
        else if (x < m_minPointPosX)
            x = m_minPointPosX;
        float y = position.y;
        if (y > m_maxPointPosY)
            y = m_maxPointPosY;
        else if (y < m_minPointPosY)
            y = m_minPointPosY;
        Vector2 pointPos = new Vector2(x, y);
        m_dragDelta = pointPos - originPos;
        m_steeringWheelPointRT.anchoredPosition = pointPos;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        float x = eventData.pressPosition.x > m_maxWheelPosX ? m_maxWheelPosX : eventData.pressPosition.x;
        float y = eventData.pressPosition.x > m_maxWheelPosY ? m_maxWheelPosY : eventData.pressPosition.y;
        m_steeringWheelRT.anchoredPosition = new Vector2(x, y);
        m_minPointPosX = x - m_steeringWheelHalfWidth;
        m_minPointPosY = y - m_steeringWheelHalfHeight;
        m_maxPointPosX = x + m_steeringWheelHalfWidth;
        m_maxPointPosY = y + m_steeringWheelHalfHeight;
        SetPointPosition(eventData.position, eventData.pressPosition);
        m_steeringWheel.SetActive(true);
        m_steeringWheelPoint.SetActive(true);
        EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.SteeringWheelDragBegin, m_dragDelta);
    }

    public void OnDrag(PointerEventData eventData) {
        SetPointPosition(eventData.position, eventData.pressPosition);
        EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.SteeringWheelDraging, m_dragDelta);
    }

    public void OnEndDrag(PointerEventData eventData) {
        m_dragDelta = Vector2.zero;
        m_steeringWheel.SetActive(false);
        m_steeringWheelPoint.SetActive(false);
        EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.SteeringWheelDragEnd, m_dragDelta);
    }
}