using UnityEngine;
using System.Collections;

public class FloorCamera : MonoBehaviour {
	public GameObject floorGenerator;
	public float zoom;
	public float maxZoom = 4;
	public float minZoom = 1;
	public Vector3 targetBase;
	public Vector3 targetOffset;

	private float boardSize;
	private Camera cam;


	void Start() {
		cam = this.GetComponent<Camera>();
		zoom = minZoom;

		FloorGenerator gen = floorGenerator.GetComponent<FloorGenerator>();
		targetBase = gen.transform.position + new Vector3(-0.5f, -0.5f, -10);
		targetOffset = new Vector3(gen.width, gen.height) / 2f;
		boardSize = ((float)Mathf.Max(gen.width, gen.height)) / 2f;

		this.transform.position = Target();
	}

	void Update() {
		if (Input.GetKey("[")) {
			zoom -= Time.deltaTime * 2;
			if (zoom < minZoom) {
				zoom = minZoom;
			}
		} else if (Input.GetKey("]")) {
			zoom += Time.deltaTime * 2;
			if (zoom > maxZoom) {
				zoom = maxZoom;
			}
		}

		if (Input.GetKeyDown("left")) {
			this.targetOffset += new Vector3(-1, 0);
		}
		if (Input.GetKeyDown("right")) {
			this.targetOffset += new Vector3(1, 0);
		}
		if (Input.GetKeyDown("up")) {
			this.targetOffset += new Vector3(0, 1);
		}
		if (Input.GetKeyDown("down")) {
			this.targetOffset += new Vector3(0, -1);
		}
			
		cam.orthographicSize = boardSize / zoom;
		Vector3 target = Target();
		this.transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 10);
	}

	Vector3 Target() {
		return targetBase + targetOffset;
	}
}
