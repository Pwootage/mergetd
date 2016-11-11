using UnityEngine;
using System.Collections;

public class PopUp : MonoBehaviour {
    public void Show() {
        transform.SetAsLastSibling();
    }

    public void Hide() {
        transform.SetAsFirstSibling();
    }
}
