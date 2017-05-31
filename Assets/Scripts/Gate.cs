using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Gate : MonoBehaviour {

    public List<Bar> bars = new List<Bar>();
    public float barDelay;
    public float barDuration;

	void Start () {
		
	}

    public void Open()
    {
        StartCoroutine(_Open());
    }

    IEnumerator _Open()
    {
        foreach (var bar in bars)
        {
            bar.Lower(barDuration);
            yield return new WaitForSeconds(barDelay);
        }
    }

    public void Close()
    {
        StartCoroutine(_Close());
    }

    IEnumerator _Close()
    {
        for (var i = bars.Count - 1; i >= 0; i--)
        {
            bars[i].Raise(barDuration);
            yield return new WaitForSeconds(barDelay);
        }
    }
}
