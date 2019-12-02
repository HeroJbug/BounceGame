using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : StateMachineBehaviour
{
	public string str;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		Debug.Log(str);
	}
}
