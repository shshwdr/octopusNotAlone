using DG.Tweening;
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

    void setAcceptableColor(Human human)
    {

        human.spriteRender.sharedMaterial.SetColor("_FillColor", Color.green);
    }
    void setNotAcceptableColor(Human human)
    {

        human.spriteRender.sharedMaterial.SetColor("_FillColor", Color.red);
    }

    void hover(Human human)
    {
        //MaterialPropertyBlock block;
        // block = new MaterialPropertyBlock();
        //meshRenderer.SetPropertyBlock(block);
        // human.spriteRender.sharedMaterial.SetFloat("OutlineWidth", 8);
        human.spriteRender.material.SetFloat("_FillPhase", 0.15f);
        hoveredOverHuman = human;
        shake(hoveredOverHuman.transform);

        setAcceptableColor(human);
    }

    void shake(Transform trans)
    {

        trans.DOKill();
        trans.localScale = Vector3.one;
        trans.DOShakeScale(0.3f, 0.5f);
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



            // check building

            {
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                LayerMask mask = (LayerMask.GetMask("building"));
                RaycastHit2D humanHit = Physics2D.Raycast(mousePos2D, Vector2.zero, 100, mask);
                if (humanHit.collider)
                {

                    var room = humanHit.collider.GetComponentInParent<RoomArea>();

                    BuildingMenu.Instance.show(RoomsAndHumanManager.Instance.getRoomByName(room.workType));
                }
                else
                {
                    BuildingMenu.Instance.hide();
                }
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

            if (draggingHuman)
            {
                RoomArea inRoom = null;
                foreach (var room in RoomsAndHumanManager.Instance.allRooms)
                {
                    var pos = room.GetComponentInChildren<Collider2D>().ClosestPoint(mousePos);
                    if (pos == (Vector2)mousePos)
                    {
                        inRoom = room;
                        break;
                    }
                }
                bool isRoomAcceptable = false;
                if (inRoom && (inRoom == draggingHuman.currentArea.room || inRoom.GetComponent<AreaBase>().canAddHuman()))//overlap with room
                {

                    isRoomAcceptable = true;
                    setAcceptableColor(draggingHuman);

                }
                else
                {
                    //warning on the room
                    if (inRoom)
                    {
                        inRoom.warnHumanCount();

                    }

                    setNotAcceptableColor(draggingHuman);
                }


                if (Input.GetMouseButtonUp(0))
                {
                    shake(draggingHuman.transform);
                    if (isRoomAcceptable)
                    {

                        draggingHuman.currentArea.removeHuman(draggingHuman);
                        inRoom.GetComponent<AreaBase>().addHuman(draggingHuman);
                    }
                    else
                    {

                        draggingHuman.transform.position = dragOriginalPosition;
                    }
                    draggingHuman = null;
                }

            }
        }
    }
}
