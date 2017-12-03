using System;

[System.Serializable]
public class Pair<T, U> {

	public T first { get; private set; }
	public U second { get; private set; }

	public Pair(T t, U u) {
		this.first = t;
		this.second = u;
	}

	public static Pair<A, B> Create<A, B>(A first, B second) {
		return new Pair<A, B>(first, second);
	}
}