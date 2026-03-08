using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public BottleController FirstBottle, FirstBottle1;
    public BottleController SecondBottle;
    bool control = false;
    
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                //return;
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            
            //if (hit.collider == null) return;
            
            if (hit.collider.GetComponent<BottleController>() != null)
            {
                AudioManager.Instance.PlaySFX(AudioPaths.SELECT, loop: false, isPitchShift: true);
                
                if (FirstBottle == null)
                {
                    FirstBottle = hit.collider.GetComponent<BottleController>();
                    FirstBottle1 = FirstBottle;
                    StartCoroutine(LerpMove(new Vector3(FirstBottle1.transform.position.x, FirstBottle1.transform.position.y+0.2f, FirstBottle1.transform.position.z)));
                }

                else if (SecondBottle == null)
                {
                    if (FirstBottle != hit.collider.GetComponent<BottleController>())
                    {
                        AudioManager.Instance.PlaySFX(AudioPaths.PLACE, loop: false, isPitchShift: true);
                        
                        SecondBottle = hit.collider.GetComponent<BottleController>();
                        FirstBottle.bottleControlRef = SecondBottle;
                        FirstBottle.UpdateTopColorValues();
                        SecondBottle.UpdateTopColorValues();

                        if (SecondBottle.FillBottleCheck(FirstBottle.topColor) == true)
                        {
                            FirstBottle.StartColorTransfer();
                            FirstBottle = null;
                            SecondBottle = null;
                        }
                        else
                        {
                            FirstBottle = null;
                            SecondBottle = null;
                        }
                    }
                    else
                    {
                        FirstBottle = null;
                    }
                    FirstBottle1.transform.position = new Vector3(FirstBottle1.transform.position.x, FirstBottle1.transform.position.y - 0.2f, FirstBottle1.transform.position.z);
                }
            }
        }
    }
    
    IEnumerator LerpMove(Vector3 vector3)
    {
        var t = 0f;
        var start = FirstBottle1.transform.position;
        var target = vector3;

        while (t < 1)
        {
            t += Time.deltaTime*20;

            if (t > 1) t = 1;

            FirstBottle1.transform.position = Vector3.Lerp(start, target, t);

            yield return null;
        }
    }
}
