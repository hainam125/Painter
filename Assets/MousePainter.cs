using UnityEngine;

public class MousePainter : MonoBehaviour {
    [SerializeField] private Camera cam;
    [SerializeField] private Color paintColor;
    [SerializeField] private float radius = 1;
    [SerializeField] private float strength = 1;
    [SerializeField] private float hardness = 1;

    private void Update() {
        if (Input.GetMouseButton(0)) {
            var position = Input.mousePosition;
            var ray = cam.ScreenPointToRay(position);

            if(Physics.Raycast(ray, out var hit, 100f)) {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                transform.position = hit.point;
                var p = hit.collider.GetComponent<Paintable>();
                if (p != null) {
                    PaintManager.Instance.Paint(p, hit.point, radius, hardness, strength, paintColor);
                }
            }
        }
    }
}
