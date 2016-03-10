using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ListUtil : MonoBehaviour {

	public static void SwapConsumables(IList<Consumable> list, int indexA, int indexB)
	{
		Consumable tmp = list[indexA];
		list[indexA] = list[indexB];
		list[indexB] = tmp;
	}

	public static void SwapValuables(IList<KeyItem> list, int indexA, int indexB)
	{
		KeyItem tmp = list[indexA];
		list[indexA] = list[indexB];
		list[indexB] = tmp;
	}
}
