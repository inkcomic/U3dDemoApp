using UnityEngine;
using System.Collections;
using PathologicalGames;
using DG.Tweening;
public class DOTweenTest : MonoBehaviour
{

	// Use this for initialization
	void Start () {
        Sequence sq = DOTween.Sequence();
        Tweener tw =  DOTween.To((x) => { }, 0, 0, 3);
        sq.Append(tw);
        tw =  transform.DOMove(Vector3.zero,1.0f);
        tw.SetEase(Ease.OutExpo);
        tw.OnComplete(() => { Debug.Log("tw.OnComplete"); });
        tw.SetLoops(2,LoopType.Yoyo);
        sq.Append(tw);
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

   
}
