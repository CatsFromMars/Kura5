using UnityEngine;
using System.Collections;
using System;

public class Element : IComparable<Element> {

	public int id;
	public string name;
	public string status;
	public string opposite;

	public Element(int elemID, string elemName, string elemStatus, string elemOpposite) {
		id = elemID;
		name = elemName;
		status = elemStatus;
		opposite = elemOpposite;
	}

	public int CompareTo(Element other)
	{
		if(other == null)
		{
			return 1;
			
		}
		
		//SORTS BY ID NUMBER
		return id - other.id;
		
	}
}
