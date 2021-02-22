using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AudioHelm;

public class ShitImSchritt : MonoBehaviour
{
    public float xFactor = 7f;
    public float yFactor = 5f;
    public float zFactor = 3f;
    public float vMax = 2f;
    public float vMin = 0f;
    [Range(0f, 100f)] public float speed = 1f;
    [Range(0f, 100f)] public float forcePower = 5f;

    public AudioHelm.HelmController helmController;

    private float v;
    private float curSpeed;
    private Rigidbody rb = null;

    
    

    IEnumerator Start()
    {
        rb = this.GetComponent<Rigidbody>();


        yield return new WaitForSeconds(1f);

        helmController.NoteOn(60, 1f);

        
    }

    // Update is called once per frame
    void Update()
    {
        CalcV();
        MakeMusic();
    }


    private void MakeMusic()
    {
        float posX = Mathf.Sin( Mathf.Sin(v) * xFactor * Mathf.PI);
        float posY = Mathf.Cos(Mathf.Cos(v) * yFactor * Mathf.PI);
        float posZ = Mathf.Sin(Mathf.Cos(v) * zFactor * Mathf.PI);
        
        Vector3 pos = new Vector3(posX, posY, posZ);
        //this.transform.position = pos; // steuert die Position im Raum
        rb.AddForce(pos * forcePower); // steuert die Force des Rigidbodys

        Debug.DrawLine(pos, Vector3.zero);

        float speedRemapped = rb.velocity.magnitude.Remap(0, 2f, -1, 1f); // Remap(rb.velocity.magnitude, 
        //float formant = rb.velocity.magnitude.Remap(0, 2, 0, 1f);
        float formant = posX.Remap(-1, 1, 0, 1f);
        float filter = posX.Remap(-1, 1, 0, 1f);

        helmController.SetPitchWheel(speedRemapped);
        helmController.SetParameterValue(AudioHelm.CommonParam.kFormantX, formant);
        helmController.SetParameterValue(AudioHelm.CommonParam.kFormantY, formant);
        helmController.SetParameterPercent(AudioHelm.CommonParam.kFilterCutoff, filter);
    }

    private void CalcV()
    {
        v += Time.deltaTime * curSpeed;

        if (v >= vMax)
            curSpeed = -speed;
        if (v <= vMin)
            curSpeed = speed;
    }

    
}

public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return from2 + (value - from1) * (to2 - from2) / (to1 - from1);
    }
} 
