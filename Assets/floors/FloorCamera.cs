using UnityEngine;

public class FloorCamera : MonoBehaviour {
    private float boardSize;
    private Camera cam;
    public GameObject floorGenerator;
    public float maxZoom = 4;
    public float minZoom = 1;
    public Vector3 targetBase;
    public Vector3 targetOffset;
    public float zoom;
    private GameState state;


    private void Start() {
        state = GameState.FindInScene();
        zoom = minZoom;
    }

    public void UpdateCamera() {
        if (state == null) {
            state = GameState.FindInScene();
        }
        cam = GetComponent<Camera>();
        zoom = minZoom;

        targetBase = new Vector3(-0.5f, -0.5f, -10);
        targetOffset = new Vector3(state.map.width, state.map.height) / 2f;
        boardSize = Mathf.Max(state.map.width, state.map.height) / 2f;
        cam.orthographicSize = boardSize / zoom;

        transform.position = Target();
    }

    private void Update() {
        if (Input.GetKey("[")) {
            zoom -= Time.deltaTime * 2;
            if (zoom < minZoom) {
                zoom = minZoom;
            }
        }
        else if (Input.GetKey("]")) {
            zoom += Time.deltaTime * 2;
            if (zoom > maxZoom) {
                zoom = maxZoom;
            }
        }

        if (Input.GetKeyDown("left")) {
            targetOffset += new Vector3(-1, 0);
        }
        if (Input.GetKeyDown("right")) {
            targetOffset += new Vector3(1, 0);
        }
        if (Input.GetKeyDown("up")) {
            targetOffset += new Vector3(0, 1);
        }
        if (Input.GetKeyDown("down")) {
            targetOffset += new Vector3(0, -1);
        }

        cam.orthographicSize = boardSize / zoom;
        var target = Target();
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 10);
    }

    private Vector3 Target() {
        return targetBase + targetOffset;
    }
}