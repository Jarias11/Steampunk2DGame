using UnityEngine;
public class NPCInteractable: MonoBehaviour
{
    public NPCData npcData;
    public GameObject namePopupUI;
    public TMPro.TextMeshProUGUI nameText;
    public SpriteRenderer portraitRenderer;

    private void OnMouseEnter(){
        nameText.text = npcData.npcName;
        namePopupUI.SetActive(true);
    }
    private void OnMouseExit(){
        namePopupUI.SetActive(false);
    }
    void start(){
        if (npcData.portrait != null){
            portraitRenderer.sprite = npcData.portrait;
        }else{
            portraitRenderer.sprite = GameManager.Instance.defaultNPCPortrait;
        }
    }
}
