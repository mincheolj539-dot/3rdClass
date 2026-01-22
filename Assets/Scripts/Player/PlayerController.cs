using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float JoystickRadius = 100f; 

    private Vector2 _startTouchPos;
    private Vector2 _currentTouchPos;
    private Vector2 _moveDir;
    private bool _isTouching = false;

    private void Update()
    {
        _moveDir = Vector2.zero;

#if UNITY_EDITOR || UNITY_STANDALONE
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0 || v != 0)
        {
            _moveDir = new Vector2(h, v).normalized;
        }
#endif

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startTouchPos = touch.position;
                _isTouching = true;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                HandleInput(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _isTouching = false;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startTouchPos = Input.mousePosition;
                _isTouching = true;
            }
            else
            {
                HandleInput(Input.mousePosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isTouching = false;
        }

        transform.Translate(_moveDir * MoveSpeed * Time.deltaTime);
    }

    private void HandleInput(Vector2 currentPos)
    {
        _currentTouchPos = currentPos;
        Vector2 diff = _currentTouchPos - _startTouchPos;
        
        if (diff.magnitude > JoystickRadius)
        {
            diff = diff.normalized * JoystickRadius;
            _startTouchPos = _currentTouchPos - diff;
        }
        
        _moveDir = diff.normalized;
    }
}
