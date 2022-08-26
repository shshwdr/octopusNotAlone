using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{

    Human hoveredOverHuman;

    Human draggingHuman;
    Vector3 dragOriginalPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void unHover(Human human)
    {
       // human.spriteRender.sharedMaterial.SetFloat("OutlineWidth", 0);
        human.spriteRender.sharedMaterial.SetFloat("_FillPhase", 0);
        hoveredOverHuman = null;
    }

    void hover(Human human)
    {
        //MaterialPropertyBlock block;
       // block = new MaterialPropertyBlock();
        //meshRenderer.SetPropertyBlock(block);
        // human.spriteRender.sharedMaterial.SetFloat("OutlineWidth", 8);
        human.spriteRender.material.SetFloat("_FillPhase", 0.15f);
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
            else
            {
                draggingHuman.transform.position = new Vector3(mousePos.x, mousePos.y, dragOriginalPosition.z);
            }



            if (Input.GetMouseButtonDown(0))
            {
                if (hoveredOverHuman)
                {
                    dragOriginalPosition = hoveredOverHuman.transform.position;
                    draggingHuman = hoveredOverHuman;
                    hoveredOverHuman = null;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (draggingHuman)
                {
                    RoomArea inRoom = null;
                    foreach(var room in RoomsAndHumanManager.Instance.allRooms)
                    {
                        var pos = room.GetComponentInChildren<Collider2D>().ClosestPoint(mousePos);
                        if (pos == (Vector2) mousePos)
                        {
                            inRoom = room;
                            break;
                        }
                    }

                    if (inRoom)//overlap with room
                    {
                        draggingHuman.currentArea.removeHuman(draggingHuman);
                        inRoom.GetComponent<AreaBase>().addHuman(draggingHuman);
                        draggingHuman = null;

                    }
                    else
                    {


                        draggingHuman.transform.position = dragOriginalPosition;
                        draggingHuman = null;
                    }

                }
            }
        }
    }
}
