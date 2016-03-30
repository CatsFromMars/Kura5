using UnityEngine;
using System.Collections;

public struct SafeInt {
	private int offset;
	private int value;
	
	public SafeInt (int value = 0) {
		offset = SimpleXOREncryption.EncryptorDecryptor.SafeIntKey();
		this.value = value + offset;
	}
	
	public int GetValue ()
	{
		return value - offset;
	}
	
	public void Dispose ()
	{
		offset = 0;
		value = 0;
	}
	
	public override string ToString()
	{
		return GetValue().ToString();
	}
	
	public static SafeInt operator +(SafeInt f1, SafeInt f2) {
		return new SafeInt(f1.GetValue() + f2.GetValue());
	}

	public static bool operator <=(SafeInt f1, SafeInt f2) {
		return (f1.GetValue() <= f2.GetValue());
	}

	public static bool operator >=(SafeInt f1, SafeInt f2) {
		return (f1.GetValue() >= f2.GetValue());
	}

	public static bool operator <(SafeInt f1, SafeInt f2) {
		return (f1.GetValue() < f2.GetValue());
	}
	
	public static bool operator >(SafeInt f1, SafeInt f2) {
		return (f1.GetValue() > f2.GetValue());
	}

	public static SafeInt operator -(SafeInt f1, SafeInt f2) {
		return new SafeInt(f1.GetValue() - f2.GetValue());
	}

	public static float operator /(float f1, SafeInt f2) {
		return (f1 / f2.GetValue());
	}

	public static float operator *(SafeInt f1, float f2) {
		return (f1.GetValue() * f2);
	}

	public static bool operator >(SafeInt f1, int f2) {
		return f1.GetValue() > f2;
	}

	public static bool operator <(SafeInt f1, int f2) {
		return (f1.GetValue() < f2);
	}
	// ...the same for the other operators
}