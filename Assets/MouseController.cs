using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{

    Human hoveredOverHuman;

    Human draggingHuman;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void unHover(Human human)
    {
        human.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("OutlineWidth", 0);
    }

    void hover(Human human)
    {
        human.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("OutlineWidth", 8);
        hoveredOverHuman = human;
    }

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!draggingHuman)
            {
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                LayerMask mask = (LayerMask.GetMask("human"));

                RaycastHit2D humanHit = Physics2D.Raycast(mousePos2D, Vector2.zero, 100, mask);
                if (humanHit.collider)
                {

                    var newHuman = humanHit.collider.GetComponentInParent<Human>();
                    if (newHuman != hoveredOverHuman)
                    {
                        if (hoveredOverHuman)
                        {
                            unHover(hoveredOverHuman);
                        }
                        hover(newHuman);
                    }
                }
                else
                {
                    if (hoveredOverHuman)
                    {
                        unHover(hoveredOverHuman);
                    }
                }

            }



            if (Input.GetMouseButtonDown(0))
            {
                if (hoveredOverHuman)
                {
                    draggingHuman = hoveredOverHuman;
                    hoveredOverHuman = null;
                }
            }
        }
    }
}
